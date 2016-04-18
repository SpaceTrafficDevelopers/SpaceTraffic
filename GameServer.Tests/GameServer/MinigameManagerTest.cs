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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTraffic.Engine;
using SpaceTraffic.Dao;
using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.GameServer;
using SpaceTraffic.Game.Minigame;
using SpaceTraffic.Entities;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Events;
using SpaceTraffic.Utils.Security;

namespace SpaceTraffic.GameServerTests.GameServer
{
    /// <summary>
    /// Tests for the <see cref="MinigameManager"/> class.
    /// </summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class MinigameManagerTest
    {
        /// <summary>
        /// Game server mock object
        /// </summary>
        private IGameServer gameServer;

        /// <summary>
        /// Start actions
        /// </summary>
        private StartAction startAction1;
        private StartAction startAction2;

        /// <summary>
        /// Minigame descriptor
        /// </summary>
        private MinigameDescriptor minigameDescriptor;

        /// <summary>
        /// Players.
        /// </summary>
        private Player player1;
        private Player player2;

        /// <summary>
        /// Reference on testing manager object,
        /// </summary>
        private IMinigameManager manager;

        /// <summary>
        /// Initialization method. Creating start actions, players, minigame descriptor
        /// and testing object.
        /// </summary>
        [TestInitialize()]
        public void TestInitialize()
        {
            this.gameServer  = new GameServerMock();

            startAction1 = CreateStartAction("ShipLanding");
            startAction2 = CreateStartAction("CargoBuy");

            StartActionDAO startActionDao = new StartActionDAO();
            startActionDao.InsertStartAction(startAction1);
            startActionDao.InsertStartAction(startAction2);

            player1 = CreatePlayer("Karel", "Malý", "player");
            player2 = CreatePlayer("Anakin", "Skywalker", "Darth Vader");

            PlayerDAO playerDao = new PlayerDAO();
            playerDao.InsertPlayer(player1);
            playerDao.InsertPlayer(player2);

            this.gameServer.World.AddPlayer(player1.PlayerId);
            this.gameServer.World.AddPlayer(player2.PlayerId);

            minigameDescriptor = CreateMinigameDescriptor();

            manager = this.gameServer.Minigame;
        }

        /// <summary>
        /// Cleaning method. Removing start actions, players, and minigame descriptor.
        /// </summary>
        [TestCleanup()]
        public void CleanUp()
        {
            StartActionDAO startActionDao = new StartActionDAO();
            startActionDao.RemoveStartActionById(startAction1.StartActionID);
            startActionDao.RemoveStartActionById(startAction2.StartActionID);

            PlayerDAO playerDao = new PlayerDAO();
            playerDao.RemovePlayerById(player1.PlayerId);
            playerDao.RemovePlayerById(player2.PlayerId);

            if (minigameDescriptor != null)
            {
                MinigameDescriptorDAO mddao = new MinigameDescriptorDAO();
                mddao.RemoveMinigameById(minigameDescriptor.MinigameId);
            }

        }

        public MinigameManagerTest(){ }

        /// <summary>
        /// Cleaning method. Dropping database after finished test.
        /// </summary>
        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }


        /// <summary>
        ///Test for MinigameManager Constructor
        ///</summary>
        [TestMethod()]
        public void MinigameManagerConstructorTest()
        {
            Assert.IsNotNull(manager);
        }

        /// <summary>
        /// A test for register minigame descriptor.
        /// </summary>
        [TestMethod()]
        public void RegisterMinigameTest()
        {
            bool result = manager.registerMinigame(this.minigameDescriptor);
            Assert.IsTrue(result, "RegisterMinigameTest: MinigameDescriptor has not been registered.");
        }

        /// <summary>
        /// Test for deregister minigame descriptor.
        /// </summary>
        [TestMethod()]
        public void DeregisterMinigameTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            bool result = manager.deregisterMinigame(this.minigameDescriptor.MinigameId);

            Assert.IsTrue(result, "DeregisterMinigameTest: MinigameDescriptor has not been deregistered.");
            this.minigameDescriptor = null;
        }

        /// <summary>
        /// Test for get minigame descriptor id.
        /// </summary>
        [TestMethod()]
        public void GetMinigameTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = manager.getMinigame(startAction1.ActionName, player1.PlayerId);

            Assert.AreEqual(minigameId, this.minigameDescriptor.MinigameId, "GetMinigameTest: MinigameIds are not equal.");
        }

        /// <summary>
        /// Test for get list of minigame descriptor id.
        /// </summary>
        [TestMethod()]
        public void GetMinigameListTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            ICollection<int> minigames = manager.getMinigameList(startAction1.ActionName, player1.PlayerId);

            Assert.IsNotNull(minigames);
            Assert.AreEqual(minigames.Count, 1, "GetMinigameListTest: Unexpected number of minigames in list.");
            Assert.AreEqual(minigames.FirstOrDefault(), this.minigameDescriptor.MinigameId, "GetMinigameListTest: MinigameIds are not equal.");
        }

        /// <summary>
        /// Test for get minigame descriptor.
        /// </summary>
        [TestMethod()]
        public void GetMinigameDescriptorTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            IMinigameDescriptor minigameDescriptor = manager.getMinigameDescriptorByActionName(startAction1.ActionName, player1.PlayerId);

            minigameDescriptorAssert(minigameDescriptor as MinigameDescriptor);
        }

        /// <summary>
        /// Test for get list of minigame descriptor id.
        /// </summary>
        [TestMethod()]
        public void GetMinigameDescriptorListTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            ICollection<MinigameDescriptor> minigames = manager.getMinigameDescriptorListByActionName(startAction1.ActionName, player1.PlayerId);

            Assert.IsNotNull(minigames);
            Assert.AreEqual(minigames.Count, 1, "GetMinigameListTest: Unexpected number of minigames in list.");
            minigameDescriptorAssert(minigames.FirstOrDefault());
        }

        /// <summary>
        /// Test for adding relationship between minigame descriptor and start action.
        /// </summary>
        [TestMethod()]
        public void AddRelationshipWithStartActionsTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            bool result = manager.addRelationshipWithStartActions(this.minigameDescriptor.Name, this.startAction2.ActionName);

            int minigameId1 = manager.getMinigame(startAction1.ActionName, player1.PlayerId);
            int minigameId2 = manager.getMinigame(startAction2.ActionName, player1.PlayerId);

            Assert.IsTrue(result, "AddRelationshipWithStartActionsTest: Relationship has not been added.");
            Assert.AreEqual(minigameId1, this.minigameDescriptor.MinigameId, "AddRelationshipWithStartActionsTest: MinigameID is not not equal with minigameDescriptor id.");
            Assert.AreEqual(minigameId2, minigameId1, "AddRelationshipWithStartActionsTest: MinigameIds are not equal.");
        }

        /// <summary>
        /// Test for removing relationship between minigame descriptor and start action.
        /// </summary>
        [TestMethod()]
        public void RemoveRelationshipWithStartActionsTest()
        {
            manager.registerMinigame(this.minigameDescriptor);

            bool result = manager.removeRelationshipWithStartActions(this.minigameDescriptor.Name, this.startAction1.ActionName);
            int minigameId = manager.getMinigame(startAction1.ActionName, player1.PlayerId);

            Assert.IsTrue(result, "RemoveRelationshipWithStartActionsTest: Relationship has not been removed.");
            Assert.AreEqual(minigameId, -1, "RemoveRelationshipWithStartActionsTest: Minigame is still in relaionship with start action.");
        }

        /// <summary>
        /// Test for get list of start actions.
        /// </summary>
        [TestMethod()]
        public void GetStartActionsTest()
        {
            List<StartAction> startActions = manager.getStartActions();
            
            Assert.IsNotNull(startActions);
            Assert.AreEqual(startActions.Count, 2, "GetStartActionsTest: Unexpected number of start actions.");
        }

        /// <summary>
        /// Test for creating game.
        /// </summary>
        [TestMethod()]
        public void CreateGameTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, true);

            Assert.AreNotEqual(minigameId, -1, "CreateGameTest: Minigame has not been created.");
        }

        /// <summary>
        /// Test for adding player into game.
        /// </summary>
        [TestMethod()]
        public void AddPlayerTest()
        {
            manager.registerMinigame(this.minigameDescriptor);

            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, true);
            Result result = manager.addPlayer(minigameId, player1.PlayerId);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "AddPlayerTest: " + result.Message);
        }

        /// <summary>
        /// Test for starting game.
        /// </summary>
        [TestMethod()]
        public void StartGameTest()
        {
            manager.registerMinigame(this.minigameDescriptor);

            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, true);
            manager.addPlayer(minigameId, player1.PlayerId);
            manager.addPlayer(minigameId, player2.PlayerId);

            Result result = manager.startGame(minigameId);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "StartGameTest: " + result.Message);
        }

        /// <summary>
        /// Test for ending game.
        /// </summary>
        [TestMethod()]
        public void EndGameTest()
        {
            manager.registerMinigame(this.minigameDescriptor);

            int minigameId = prepareMinigame();

            Result result = manager.endGame(minigameId);
            Assert.IsTrue(result.State == ResultState.SUCCESS, "EndGameTest: " + result.Message);
        }

        /// <summary>
        /// Test for rewarding player.
        /// </summary>
        [TestMethod()]
        public void RewardPlayerTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = prepareMinigame();

            manager.endGame(minigameId);
            Result result = manager.reward(minigameId, player1.PlayerId);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "RewardPlayerTest: " + result.Message);
        }

        /// <summary>
        /// Test for rewarding players.
        /// </summary>
        [TestMethod()]
        public void RewardPlayersTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = prepareMinigame();

            manager.endGame(minigameId);
            Result result = manager.reward(minigameId, new int[] { player1.PlayerId, player2.PlayerId });

            Assert.IsTrue(result.State == ResultState.SUCCESS, "RewardPlayersTest: " + result.Message);
        }

        /// <summary>
        /// Test for performing action.
        /// </summary>
        [TestMethod()]
        public void PerformActionTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = prepareMinigame();

            Result result = manager.performAction(minigameId, "ToString", true, null);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "PerformActionTest: " + result.Message);
            Assert.AreEqual(result.ReturnValue, "SpaceTraffic.Game.Minigame.Minigame", "PerformActionTest: Unexpected return value.");
        }

        /// <summary>
        /// Test for removing game.
        /// </summary>
        [TestMethod()]
        public void RemoveGameTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = prepareMinigame();

            manager.endGame(minigameId);
            Result result = manager.removeGame(minigameId);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "RemoveGameTest: " + result.Message);
        }

        /// <summary>
        /// Test for checking if player is playing any minigame.
        /// </summary>
        [TestMethod()]
        public void IsPlayerPlayingTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = prepareMinigame();

            //this.gameServer.World.GetPlayer(this.player1.PlayerId).MinigameId = 1;

            bool result = manager.isPlayerPlaying(this.player1.PlayerId);
            Assert.IsTrue(result, "IsPlayerPlayigTest: Unxpected result. Player has to be in playing state.");
        }

        /// <summary>
        /// Test for checking minigame life.
        /// </summary>
        [TestMethod()]
        public void CheckMinigameLifeTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, true);

            bool result = manager.checkMinigameLife(minigameId);

            Assert.IsTrue(result, "CheckMinigameLifeTest: Unxpected result. Minigame has to be alive.");
        }

        /// <summary>
        /// Test for updating  last request time.
        /// </summary>
        [TestMethod()]
        public void UpdateLastRequestTimeTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, true);
            
            manager.updateLastRequestTime(minigameId);            
            bool result = manager.checkMinigameLife(minigameId);

            Assert.IsTrue(result, "UpdateLastRequestTimeTest: Unxpected result. Minigame has to be alive.");
        }

        /// <summary>
        /// Test for checking minigame life and updating last request time.
        /// </summary>
        [TestMethod()]
        public void CheckMinigameLifeAndUpdateLastRequestTimeTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, true);

            manager.checkLifeOfAllMinigames(0);
            bool result = manager.checkMinigameLife(minigameId);

            Assert.IsTrue(result, "CheckMinigameLifeAndUpdateLastRequestTimeTest: Unxpected result. Minigame has to be alive.");
        }

        /// <summary>
        /// Test for checking actual playing minigame id.
        /// </summary>
        [TestMethod()]
        public void ActualPlayingMinigameIdTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = prepareMinigame();

            int resultId = manager.actualPlayingMinigameId(this.player1.PlayerId);

            Assert.AreEqual(minigameId, resultId, "ActualPlayingMinigameIdTest: Unxpected actual playing minigame id.");
        }

        /// <summary>
        /// Test for checking all minigame lifes.
        /// </summary>
        [TestMethod()]
        public void CheckLifeOfAllMinigamesTest()
        {
            manager.registerMinigame(this.minigameDescriptor);
            int minigameId = prepareMinigame();

            int resultId = manager.actualPlayingMinigameId(this.player1.PlayerId);

            Assert.AreEqual(minigameId, resultId, "ActualPlayingMinigameIdTest: Unxpected actual playing minigame id.");
        }

        /// <summary>
        /// Test for check authentication fo
        /// </summary>
        [TestMethod()]
        public void AuthenticatePlayerForMinigameTest()
        {
            MinigamePasswordHasher hasher = new MinigamePasswordHasher();
            string encryptPassword = hasher.getEncryptedPassword("user");
            int playerID = manager.authenticatePlayerForMinigame(this.player1.PlayerName, encryptPassword);

            Assert.AreEqual(playerID, this.player1.PlayerId, "AuthenticatePlayerForMinigameTest: Unexpected player ID.");
        }

        /// <summary>
        /// Assert method for minigame descriptor.
        /// </summary>
        /// <param name="descriptor">minigame descriptor</param>
        private void minigameDescriptorAssert(MinigameDescriptor descriptor)
        {
            Assert.AreEqual(this.minigameDescriptor.MinigameId, descriptor.MinigameId);
            Assert.AreEqual(this.minigameDescriptor.Name, descriptor.Name);
            Assert.AreEqual(this.minigameDescriptor.Controls, descriptor.Controls);
            Assert.AreEqual(this.minigameDescriptor.PlayerCount, descriptor.PlayerCount);
            Assert.AreEqual(this.minigameDescriptor.MinigameClassFullName, descriptor.MinigameClassFullName);
            Assert.AreEqual(this.minigameDescriptor.RewardAmount, descriptor.RewardAmount);
            Assert.AreEqual(this.minigameDescriptor.RewardType, descriptor.RewardType);
            Assert.AreEqual(this.minigameDescriptor.SpecificReward, descriptor.SpecificReward);
            Assert.AreEqual(this.minigameDescriptor.ExternalClient, descriptor.ExternalClient);
            Assert.AreEqual(this.minigameDescriptor.Description, descriptor.Description);
            Assert.AreEqual(this.minigameDescriptor.ConditionType, descriptor.ConditionType);
            Assert.AreEqual(this.minigameDescriptor.ConditionArgs, descriptor.ConditionArgs);
            Assert.AreEqual(this.minigameDescriptor.ClientURL, descriptor.ClientURL);
        }

        /// <summary>
        /// Method for prepare minigame. Creates game, adds players and starts game.
        /// </summary>
        /// <returns>return minigame id</returns>
        private int prepareMinigame()
        {
            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, false);
            manager.addPlayer(minigameId, player1.PlayerId);
            manager.addPlayer(minigameId, player2.PlayerId);

            manager.startGame(minigameId);

            return minigameId;
        }

        /// <summary>
        /// Method for creating start action.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <returns>return start action</returns>
        private StartAction CreateStartAction(string actionName)
        {
            StartAction sa = new StartAction();
            sa.ActionName = actionName;

            return sa;
        }

        /// <summary>
        /// Method for creating minigame descriptor.
        /// </summary>
        /// <returns>return minigame descriptor</returns>
        private MinigameDescriptor CreateMinigameDescriptor()
        {
            MinigameDescriptor md = new MinigameDescriptor();
            md.Name = "SpaceTraffic";
            md.PlayerCount = 2;
            md.Description = "Popis hry.";
            md.Controls = "Hra se ovládá lopatou.";
            md.RewardType = RewardType.CREDIT;
            md.SpecificReward = null;
            md.RewardAmount = 10;
            md.ConditionType = ConditionType.LEVEL;
            md.ConditionArgs = "10";
            md.ExternalClient = true;
            md.ClientURL = "kiv.zcu.cz";
            md.MinigameClassFullName = "SpaceTraffic.Game.Minigame.Minigame, SpaceTraffic.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            md.StartActions = new List<StartAction>();
            md.StartActions.Add(startAction1);

            return md;
        }

        /// <summary>
        /// Method for creating player.
        /// </summary>
        /// <param name="firstName">first name</param>
        /// <param name="lastName">last name</param>
        /// <param name="playerName">player name</param>
        /// <returns>return player</returns>
        private Player CreatePlayer(string firstName, string lastName, string playerName)
        {
            Player player = new Player();

            player.FirstName = firstName;
            player.LastName = lastName;
            player.PlayerName = playerName;
            player.CorporationName = "ZCU";
            player.Credit = 100;
            player.DateOfBirth = new DateTime(2008, 2, 16, 12, 15, 12);
            player.Email = "email@email.cz";
            player.PsswdHash = "o8Drx+MJghpMvCN5v0oGB1AB0m0TABBWjt+p1jFsAnvQkBWaGkqFiMo2r6fPeG5+";
            player.PsswdSalt = "";
            player.OrionEmail = "email@students.zcu.cz";
            player.AddedDate = DateTime.Now;
            player.LastVisitedDate = DateTime.Now;
            player.ExperienceLevel = 11;
            
            return player;
        }
    }

    /// <summary>
    /// Mock for game server.
    /// </summary>
    internal class GameServerMock : IGameServer
    {
        private IPersistenceManager persistenceManager;
        private IGameManager gameManager;
        private IWorldManager worldManager;
        private IMinigameManager minigameManager;

        public IPersistenceManager Persistence
        {
            get { return persistenceManager; }
        }

        public IGameManager Game
        {
            get { return gameManager; }
        }


        public IMinigameManager Minigame
        {
            get { return this.minigameManager; }
            set { this.minigameManager = value; }
        }

        #region Not Implemented Method

        public IAssetManager Assets
        {
            get { throw new NotImplementedException(); }
        }

        public IScriptManager Scripts
        {
            get { throw new NotImplementedException(); }
        }

        public IWorldManager World
        {
            get { return worldManager; }
        }

        public IStatisticsManager Statistics
        {
            get { throw new NotImplementedException(); }
        }

        #endregion Not Implemented Method
        
        public GameServerMock()
        {
            this.persistenceManager = new PersitanceManagerMock();
            this.gameManager = new GameManagerMock();
            this.worldManager = new WorldManagerMock(this);
            this.minigameManager = new MinigameManager(this);
        }
    }

    /// <summary>
    /// Mock for persistance manager.
    /// </summary>
    internal class PersitanceManagerMock : IPersistenceManager
    {

        public SpaceTraffic.Dao.IPlayerDAO GetPlayerDAO()
        {
            return new PlayerDAO();
        }

        public SpaceTraffic.Dao.IMinigameDescriptorDAO GetMinigameDescriptorDAO()
        {
            return new MinigameDescriptorDAO();
        }

        public SpaceTraffic.Dao.IStartActionDAO GetStartActionDAO()
        {
            return new StartActionDAO();
        }

        #region Not Implemented Method

        public SpaceTraffic.Dao.IMessageDAO GetMessageDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.ICargoDAO GetCargoDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.IFactoryDAO GetFactoryDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.ISpaceShipDAO GetSpaceShipDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.ISpaceShipCargoDAO GetSpaceShipCargoDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.IBaseDAO GetBaseDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.ITraderCargoDAO GetTraderCargoDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.ITraderDAO GetTraderDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.ICargoLoadDao GetCargoLoadDao(string cargoLoadName)
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.IEarnedAchievementDAO GetEarnedAchievementDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.IStatisticDAO GetStatisticsDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.IPlanActionDAO GetPlanActionDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.IPathPlanEntityDAO GetPathPlanEntityDAO()
        {
            throw new NotImplementedException();
        }

        public SpaceTraffic.Dao.IPlanItemEntityDAO GetPlanItemEntityDAO()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion Not Implemented Method
    }

    /// <summary>
    /// Mock for game manager
    /// </summary>
    internal class GameManagerMock : IGameManager
    {
        public GameTime currentGameTime
        {
            get { 
                return new GameTime(){
                    Value = DateTime.UtcNow 
                }; 
            }
        }

        private EventQueue gameEventQueue = new EventQueue();

        public void PlanEvent(IGameAction action, DateTime when)
        {
            if (action != null && when != null)
            {
                action.State = GameActionState.PREPARED;
                GeneralEvent newEvent = new GeneralEvent();
                GameTime time = new GameTime();
                time.Value = when;

                newEvent.BoundAction = action;
                newEvent.PlannedTime = time;

                PlanEvent(newEvent);
            }
        }


        public void PlanEvent(IGameEvent gameEvent)
        {
            this.gameEventQueue.Enqueue(gameEvent);
        }

        #region Not Implemented Method

        public object PerformAction(IGameAction action)
        {
            throw new NotImplementedException();
        }

        public void PerformActionAsync(IGameAction action)
        {
            throw new NotImplementedException();
        }
        #endregion Not Implemented Method


		public Services.Contracts.ICargoService GetCargoService()
		{
			throw new NotImplementedException();
		}
	}

        /// <summary>
    /// Mock for world manager
    /// </summary>
    internal class WorldManagerMock : IWorldManager
    {
        private IGameServer gameServer;
        private IDictionary<int, Game.IGamePlayer> activePlayers;

        

        public IDictionary<int, Game.IGamePlayer> ActivePlayers
        {
            get { return activePlayers; }
        }

        public WorldManagerMock(IGameServer gameServer)
        {
            this.gameServer = gameServer;
            this.activePlayers = new Dictionary<int, Game.IGamePlayer>();
        }

        #region Not Implemented Method

        public Game.GalaxyMap Map
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Game.IGamePlayer> GetActivePlayers()
        {
            throw new NotImplementedException();
        }

        public Game.IGamePlayer GetPlayer(int playerId)
        {
            return this.activePlayers[playerId];
        }

        public void ShipDock(int spaceshipId)
        {
            throw new NotImplementedException();
        }

        public void ShipTakeoff(int spaceshipId, Game.Navigation.NavPath path, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void ShipUpdateLocation(int spaceshipId, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void GenerateBasesAndTraders()
        {
            throw new NotImplementedException();
        }

        public Achievements Achievements
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ExperienceLevels ExperienceLevels
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public TAchievement GetAchievementById(int id)
        {
            throw new NotImplementedException();
        }
    
        #endregion Not Implemented Method

        public bool AddPlayer(int playerId)
        {
            Player player = this.gameServer.Persistence.GetPlayerDAO().GetPlayerById(playerId);

            if (player != null)
            {
                if (this.ActivePlayers.ContainsKey(playerId))
                    return true;

                GamePlayer gamePlayer = new GamePlayer(player);
                this.ActivePlayers.Add(playerId, gamePlayer);

                return true;
            }
            return false;
        }

        public void RemovePlayer(int playerId)
        {
            this.activePlayers.Remove(playerId);
        }
    }
}
