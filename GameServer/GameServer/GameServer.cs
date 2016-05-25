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


            logger.Info("Compiling scripts:");
            ScriptManager scriptManager = new ScriptManager();
            scriptManager.CompileScripts(".\\scripts", SCRIPT_TARGET_ASSEMBLY, new string[] { "SpaceTraffic.Core.dll", "SpaceTraffic.GameServer.exe" });
            
            logger.Debug("CONFIG: Assets path: {0}", assetsPath);
            logger.Debug("CONFIG: Map name: {0}", galaxyMapName);
            logger.Debug("CONFIG: Goods filename: {0}", goodsFileName);


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
            //TODO: add this into log-on and remove into log-off
            this.worldManager.AddPlayer(1);

            // Inicializace herního světa.
            this.gameManager = new GameManager(this, this.gameStateManager);
            this.gameManager.RestoreGameState();

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
