/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SpaceTraffic.GameServer.ServiceImpl;
using SpaceTraffic.GameServer.Configuration;
using NLog;
using SpaceTraffic.Game;
using SpaceTraffic.Data;
using System.Xml;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.Game.Actions;
using SpaceTraffic.Game.Minigame;
using System.IO;
using SpaceTraffic.Entities;

namespace SpaceTraffic.GameServer
{
    /// <summary>
    /// Game server controlls game's active server part.
    /// It manages WCF services, simulations, scripting, assets and provides access to game data.
    /// </summary>
    class GameServer : IGameServer
    {
        public const string SCRIPT_TARGET_ASSEMBLY = "SpaceTraffic.Scripts.dll";

        private Thread gameServerThread;
        
        private Logger logger = LogManager.GetCurrentClassLogger();
        private ServiceManager serviceManager;
        private AssetManager assetManager;
        private PersistenceManager persistenceManager;
        private ScriptManager scriptManager;
        private WorldManager worldManager;
        private GameStateManager gameStateManager;
        private GameManager gameManager;
        private GoodsManager goodsManager;
		private StatisticsManager statisticsManager;
        private MinigameManager minigameManager;

		const int SIMULATION_SPEED = 5 * 60;/* step in seconds for simulation run*/
		const int WRITE_EVERY = 60 * 60 * 1; /* how often simulation writes output into file */

        public volatile bool run = false;

        public volatile GameTime currentGameTime;

        #region Properties
        /// <summary>
        /// Gets the current instance of GameServer. Used by WCF services to access GameServer.
        /// </summary>
        public static GameServer CurrentInstance { get; private set; }

        public IPersistenceManager Persistence
        {
            get
            {
                return this.persistenceManager;
            }
        }

        public IAssetManager Assets
        {
            get
            {
                return this.assetManager;
            }
        }

        public IScriptManager Scripts
        {
            get
            {
                return this.scriptManager;
            }
        }

		public IStatisticsManager Statistics
		{
			get
			{
				return this.statisticsManager;
			}
		}

        public IMinigameManager Minigame
        {
            get
            {
                return this.minigameManager;
            }
        }

        public IGoodsManager Goods
        {
            get
            {
                return this.goodsManager;
            }
        }

        #endregion

        //TODO: Thread-safe state indication of GameServer instance.

        public void Initialize()
        {
            CurrentInstance = this;
            
            logger.Info("Game server initialization.");
            logger.Debug("Referential time: {0}", GameTime.REFERENTIAL_TIME);

            logger.Info("Reading configuration:");

            string assetsPath = GameServerConfiguration.GameServerConfig.Assets.Path;
            string galaxyMapName = GameServerConfiguration.GameServerConfig.Map.Name;
            string goodsFileName = GameServerConfiguration.GameServerConfig.Goods.Name;
            string economicLevelsFileName = GameServerConfiguration.GameServerConfig.EconomicLevels.Name;


            logger.Info("Compiling scripts:");
            ScriptManager scriptManager = new ScriptManager();
            scriptManager.CompileScripts(".\\scripts", SCRIPT_TARGET_ASSEMBLY, new string[] { "SpaceTraffic.Core.dll", "SpaceTraffic.GameServer.exe" });
            
            logger.Debug("CONFIG: Assets path: {0}", assetsPath);
            logger.Debug("CONFIG: Map name: {0}", galaxyMapName);
            logger.Debug("CONFIG: Goods filename: {0}", goodsFileName);
            logger.Debug("CONFIG: EconomicLevels filename: {0}", economicLevelsFileName);

            logger.Info("Initializing Asset Manager.");

            this.assetManager = new AssetManager(assetsPath);
            this.assetManager.Initialize();
            
            logger.Info("Initializing Persistence.");
            try
            {
                this.persistenceManager = new PersistenceManager();
                this.persistenceManager.Initialize();
            }
            catch (Exception ex)
            {
                logger.Fatal("Persistence initialization failed: {0}", ex.Message, ex);
                throw;
            }

			this.statisticsManager = new StatisticsManager(this);
			logger.Info("Initializing Statistics.");
			          
            
            scriptManager.RunScript("SpaceTraffic.Scripts.Testing.TestDataGenerator");

            logger.Info("Restoring game world.");
            this.worldManager = new WorldManager(this);
            GalaxyMap galaxyMap = this.assetManager.LoadGalaxyMap(galaxyMapName);
            this.worldManager.Map = galaxyMap;
			this.worldManager.Achievements = this.assetManager.LoadAchievements();
			this.worldManager.ExperienceLevels = this.assetManager.LoadExperienceLevels();

            this.worldManager.GenerateBasesAndTraders();

            this.goodsManager = new GoodsManager(this);

            this.goodsManager.GoodsList = this.assetManager.LoadGoods(goodsFileName);
            this.goodsManager.EconomicLevels = this.assetManager.LoadEconomicLevels(economicLevelsFileName);

            this.goodsManager.InsertCargoIntoDb();
            this.goodsManager.GenerateGoodsOverGalaxyMap(this.worldManager.Map);

            // Create the GameStateManager used to persist and restore state of GameManager.
            this.gameStateManager = new GameStateManager(
                this.persistenceManager.GetGameActionDao(),
                this.persistenceManager.GetGameEventDao()
            );

            this.minigameManager = new MinigameManager(this);
            this.minigameManager.loadAssets();

            Utils.EmailClient.EmailFormats = assetManager.LoadEmailTemplates();

            //for tests: add "user" player into active players
            this.worldManager.AddPlayer(1);

            // Inicializace herního světa.
            this.gameManager = new GameManager(this, this.gameStateManager);
            this.gameManager.RestoreGameState();

            //planning all economic events
            this.goodsManager.planEconomicEvents(this.worldManager.Map);

            //test minigame and start action data
            #region minigame test data
            
            StartAction testStartAction = new StartAction { ActionName = "TestAction" };
            StartAction cargoBuyStartAction = new StartAction{ ActionName = "CargoBuy" };

            this.persistenceManager.GetStartActionDAO().InsertStartAction(testStartAction);
            this.persistenceManager.GetStartActionDAO().InsertStartAction(cargoBuyStartAction);

            MinigameDescriptor md = new MinigameDescriptor
            {
                Name = "Spaceship cargo finder",
                PlayerCount = 1,
                Description = "Hra na motiva hada, kde je hlavním úkolem nasbírat alespoň 30 jednotek nákladu.",
                Controls = "Hra se ovládá šipkami.",
                StartActions = new List<StartAction>() { cargoBuyStartAction },
                RewardType = RewardType.CREDIT,
                SpecificReward = null,
                RewardAmount = 1000,
                ConditionType = ConditionType.CREDIT,
                ConditionArgs = "200",
                ExternalClient = false,
                MinigameClassFullName = "SpaceTraffic.Game.Minigame.SpaceshipCargoFinder, SpaceTraffic.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                ClientURL = "/SpaceshipCargoFinder"
            };

            this.minigameManager.registerMinigame(md);

            md.Name = "Spaceship cargo finder test";
            md.ConditionType = ConditionType.NOTHING;
            md.StartActions = new List<StartAction> { testStartAction };
            md.ConditionArgs = null;

            this.minigameManager.registerMinigame(md);

            md = new MinigameDescriptor
            {
                Name = "LogoQuiz",
                PlayerCount = 1,
                Description = "Hra, kde je hlavním úkolem uhádnout z neúplného loga o jaké logo se jedná. " +
                              "Pokud uhádneš alspoň 20 z 30 log, dostaneš odměnu 1000 kreditů.",
                Controls = "Hra se ovládá dotykem.",
                StartActions = new List<StartAction>() { testStartAction },
                RewardType = RewardType.CREDIT,
                SpecificReward = null,
                RewardAmount = 1000,
                ConditionType = ConditionType.NOTHING,
                ConditionArgs = null,
                ExternalClient = true,
                MinigameClassFullName = "SpaceTraffic.Game.Minigame.LogoQuiz, SpaceTraffic.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                ClientURL = "logo_quiz.apk"
            };

            this.minigameManager.registerMinigame(md);
            IGameAction ga = new MinigameAllGamesLifeAction();

            this.Game.PlanEvent(ga, DateTime.UtcNow);

            #endregion

            serviceManager = new ServiceManager();
            //Načíst servisy z konfigurace
			serviceManager.ServiceList = new List<Type>(new Type[] { typeof(AccountService), typeof(GameService), typeof(HelloWorldService), typeof(AchievementsService), typeof(CargoService), typeof(ShipsService), typeof(PlanningService), typeof(PlayerService), typeof(MinigameService), typeof(MailService)});
            serviceManager.Initialize();
        }

        public void Start()
        {
            this.gameServerThread = new Thread(Run);
            this.gameServerThread.Name = "MainGameServerThread";
            this.run = true;
            this.gameServerThread.Start();
        }


        public void Stop()
        {
            this.run = false;
        }
        
        private void Run()
        {
			if (Program.economicSimulation) {/* when is economy simulated, runs another method */
				simulationRun();
				return;
			}
            this.serviceManager.Start();
            Console.Write("Working");
            int counter = 0;
            this.currentGameTime = new GameTime();
            while (this.run)
            {
                this.currentGameTime.Update();
                this.gameManager.Update(this.currentGameTime);
                counter++;
                if (counter > 1000)
                {
                    Console.Write(".");
                    counter = 0;
                }
					Thread.Sleep(1);
            }
            this.serviceManager.Stop();
            this.gameManager.PersistGameState();
        }

		private void simulationRun()
		{
			this.serviceManager.Start();
			Console.Write("Simulating");
			this.currentGameTime = new GameTime();
			var startTime = currentGameTime.ValueInSeconds;
			var lastWrittenTime = currentGameTime.ValueInSeconds;
			List<SpaceTraffic.Entities.Base> planets = this.Persistence.GetBaseDAO().GetBases();
			planets.ForEach(x => prepareStatistics(x.BaseId, x.BaseName));

			startSimulatedPlayers();

			while (this.run)
			{
				this.currentGameTime.Value = this.currentGameTime.Value.AddSeconds(SIMULATION_SPEED);
				this.gameManager.Update(this.currentGameTime);
				if ((currentGameTime.ValueInSeconds - lastWrittenTime) >= WRITE_EVERY) {
					lastWrittenTime = currentGameTime.ValueInSeconds;
					planets.ForEach(x => writeStatistics(x.BaseId));
					logger.Info(currentGameTime.Value);
				}
				if ((currentGameTime.ValueInSeconds - startTime) >= Program.simulationLength) {
					foreach(StreamWriter stream in outputFiles.Values){
						stream.Flush();
					}
					break;
				}
			}
			this.serviceManager.Stop();
			this.gameManager.PersistGameState();
		}

		private void startSimulatedPlayers()
		{
			for (int i = 0; i < Program.simulatedPlayers; i++)
			{
				SimulatedPlayer simPlayer = new SimulatedPlayer(i);
				this.Game.PlanEvent(simPlayer, currentGameTime.Value.AddMinutes(1));
			}
		}

		Dictionary<int, StreamWriter> outputFiles = new Dictionary<int, StreamWriter>();
		List<SpaceTraffic.Entities.Cargo> allCargos;

		private void prepareStatistics(int index, string planetName)
		{
			 outputFiles[index] = new StreamWriter(@"planeta-" + planetName + ".csv", false, Encoding.UTF8);
			 outputFiles[index].Write("Čas;Planeta;Level;Cena paliva;Cena opravy;Nákupní daň;Prodejní daň;");

			 allCargos = this.Persistence.GetCargoDAO().GetCargos();
			foreach(Cargo cargo in allCargos){
				outputFiles[index].Write(cargo.Name + ";");
				outputFiles[index].Write(cargo.Name + " - kupní cena;");
				outputFiles[index].Write(cargo.Name + " - prodejní cena;");
				outputFiles[index].Write(cargo.Name + " - denní produkce;");
				outputFiles[index].Write(cargo.Name + " - denní spotřeba;");
				outputFiles[index].Write(cargo.Name + " - dnes vyprodukováno;");
				outputFiles[index].Write(cargo.Name + " - dnes spotřebováno;");
			}
			outputFiles[index].WriteLine("");
		}

		private void writeStatistics(int index)
		{
			SpaceTraffic.Entities.Trader trader = this.persistenceManager.GetTraderDAO().GetTraderByBaseIdWithCargo(index);


			outputFiles[index].Write(String.Format("{0};{1};{2};{3};{4};{5};{6};", currentGameTime.Value, trader.Base.BaseName, trader.EconomicLevel, trader.FuelPrice, trader.RepairPrice, trader.PurchaseTax, trader.SalesTax));
			foreach (Cargo cargo in allCargos) {
				TraderCargo traderCargo = trader.TraderCargos.FirstOrDefault(x => x.CargoId == cargo.CargoId);
				if(traderCargo != null){
					outputFiles[index].Write(traderCargo.CargoCount + ";");
					outputFiles[index].Write(traderCargo.CargoBuyPrice + ";");
					outputFiles[index].Write(traderCargo.CargoSellPrice + ";");
					outputFiles[index].Write(traderCargo.DailyProduction + ";");
					outputFiles[index].Write(traderCargo.DailyConsumption + ";");
					outputFiles[index].Write(traderCargo.TodayProduced + ";");
					outputFiles[index].Write(traderCargo.TodayConsumed + ";");
				}else{
					outputFiles[index].Write(";");
					outputFiles[index].Write(";");
					outputFiles[index].Write(";");
					outputFiles[index].Write(";");
					outputFiles[index].Write(";");
					outputFiles[index].Write(";");
					outputFiles[index].Write(";");
				}
				
			}


			outputFiles[index].WriteLine("");
		}

        protected internal void JoinThread()
        {
            this.gameServerThread.Join();
        }


        public IWorldManager World
        {
            get { return this.worldManager; }
        }

        public IGameManager Game
        {
            get { return this.gameManager; }
        }

        public GameTime CurrentGameTime
        {
            get { return this.currentGameTime; }
        }
    }

    public enum GameServerState
    {
        INITIALIZING,
        READY,
        STARTING,
        RUNNING,
        STOPPING,
        STOPPED
    }
}
