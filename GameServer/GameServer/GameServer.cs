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

            
            
            scriptManager.RunScript("SpaceTraffic.Scripts.Testing.TestDataGenerator");

            logger.Info("Restoring game world.");
            this.worldManager = new WorldManager(this);
            GalaxyMap galaxyMap = this.assetManager.LoadGalaxyMap(galaxyMapName);
            this.worldManager.Map = galaxyMap;

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

            // Inicializace herního světa.
            this.gameManager = new GameManager(this, this.gameStateManager);
            this.gameManager.RestoreGameState();

            
            serviceManager = new ServiceManager();
            //Načíst servisy z konfigurace
            serviceManager.ServiceList = new List<Type>(new Type[] { typeof(AccountService), typeof(GameService), typeof(HelloWorldService) });
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
