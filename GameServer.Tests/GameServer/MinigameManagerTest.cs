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

namespace SpaceTraffic.GameServerTests.GameServer
{
    /// <summary>
    /// Summary description for MinigameManagerTest
    /// </summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class MinigameManagerTest
    {
        private IGameServer gameServer = new GameServerMock();

        private StartAction startAction1;
        private StartAction startAction2;

        private MinigameDescriptor minigameDescriptor;

        private Player player1;
        private Player player2;

        [TestInitialize()]
        public void TestInitialize()
        {
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

            minigameDescriptor = CreateMinigameDescriptor();
        }

        [TestCleanup()]
        public void CleanUp()
        {
            StartActionDAO startActionDao = new StartActionDAO();
            startActionDao.RemoveStartActionById(startAction1.StartActionID);
            startActionDao.RemoveStartActionById(startAction2.StartActionID);

            PlayerDAO playerDao = new PlayerDAO();
            playerDao.RemovePlayerById(player1.PlayerId);

            if (minigameDescriptor != null)
            {
                MinigameDescriptorDAO mddao = new MinigameDescriptorDAO();
                mddao.RemoveMinigameById(minigameDescriptor.MinigameId);
            }

        }

        public MinigameManagerTest()
        {

        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }


        /// <summary>
        ///A test for MinigameManager Constructor
        ///</summary>
        [TestMethod()]
        public void MinigameManagerConstructorTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            Assert.IsNotNull(target);
        }

        [TestMethod()]
        public void RegisterMinigameTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            bool result = target.registerMinigame(this.minigameDescriptor);

            Assert.IsTrue(result, "RegisterMinigameTest: MinigameDescriptor has not been registered.");
        }


        [TestMethod()]
        public void DeregisterMinigameTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            bool result = target.deregisterMinigame(this.minigameDescriptor.MinigameId);

            Assert.IsTrue(result, "DeregisterMinigameTest: MinigameDescriptor has not been deregistered.");
            this.minigameDescriptor = null;
        }

        [TestMethod()]
        public void GetMinigameTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = target.getMinigame(startAction1.ActionName, player1.PlayerId);

            Assert.AreEqual(minigameId, this.minigameDescriptor.MinigameId, "GetMinigameTest: MinigameIds are not equal.");
        }

        [TestMethod()]
        public void GetMinigameListTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            ICollection<int> minigames = target.getMinigameList(startAction1.ActionName, player1.PlayerId);

            Assert.IsNotNull(minigames);
            Assert.AreEqual(minigames.Count, 1, "GetMinigameListTest: Unexpected number of minigames in list.");
            Assert.AreEqual(minigames.FirstOrDefault(), this.minigameDescriptor.MinigameId, "GetMinigameListTest: MinigameIds are not equal.");
        }

        [TestMethod()]
        public void AddRelationshipWithStartActionsTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            bool result = target.addRelationshipWithStartActions(this.minigameDescriptor.Name, this.startAction2.ActionName);

            int minigameId1 = target.getMinigame(startAction1.ActionName, player1.PlayerId);
            int minigameId2 = target.getMinigame(startAction2.ActionName, player1.PlayerId);

            Assert.IsTrue(result, "AddRelationshipWithStartActionsTest: Relationship has not been added.");
            Assert.AreEqual(minigameId1, this.minigameDescriptor.MinigameId, "AddRelationshipWithStartActionsTest: MinigameID is not not equal with minigameDescriptor id.");
            Assert.AreEqual(minigameId2, minigameId1, "AddRelationshipWithStartActionsTest: MinigameIds are not equal.");
        }

        [TestMethod()]
        public void RemoveRelationshipWithStartActionsTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            bool result = target.removeRelationshipWithStartActions(this.minigameDescriptor.Name, this.startAction1.ActionName);

            int minigameId = target.getMinigame(startAction1.ActionName, player1.PlayerId);

            Assert.IsTrue(result, "RemoveRelationshipWithStartActionsTest: Relationship has not been removed.");
            Assert.AreEqual(minigameId, -1, "RemoveRelationshipWithStartActionsTest: Minigame is still in relaionship with start action.");
        }

        [TestMethod()]
        public void GetStartActionsTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            List<StartAction> startActions = target.getStartActions();
            
            Assert.IsNotNull(startActions);
            Assert.AreEqual(startActions.Count, 2, "GetStartActionsTest: Unexpected number of start actions.");
        }

        [TestMethod()]
        public void CreateGameTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = target.createGame(this.minigameDescriptor.MinigameId, true);

            Assert.AreNotEqual(minigameId, -1, "CreateGameTest: Minigame has not been created.");
        }

        [TestMethod()]
        public void AddPlayerTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = target.createGame(this.minigameDescriptor.MinigameId, true);
            Result result = target.addPlayer(minigameId, player1.PlayerId);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "AddPlayerTest: " + result.Message);
        }

        [TestMethod()]
        public void StartGameTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = target.createGame(this.minigameDescriptor.MinigameId, true);
            target.addPlayer(minigameId, player1.PlayerId);
            target.addPlayer(minigameId, player2.PlayerId);

            Result result = target.startGame(minigameId);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "StartGameTest: " + result.Message);
        }

        [TestMethod()]
        public void EndGameTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = prepareMinigame(target);

            Result result = target.endGame(minigameId);
            Assert.IsTrue(result.State == ResultState.SUCCESS, "EndGameTest: " + result.Message);
        }

        [TestMethod()]
        public void RewardPlayerTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = prepareMinigame(target);
            target.endGame(minigameId);

            Result result = target.reward(minigameId, player1.PlayerId);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "RewardPlayerTest: " + result.Message);
        }

        [TestMethod()]
        public void RewardPlayersTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = prepareMinigame(target);
            target.endGame(minigameId);

            Result result = target.reward(minigameId, new int[] { player1.PlayerId, player2.PlayerId });

            Assert.IsTrue(result.State == ResultState.SUCCESS, "RewardPlayersTest: " + result.Message);
        }

        [TestMethod()]
        public void PerformActionTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = prepareMinigame(target);

            Result result = target.performAction(minigameId, "ToString", null, true);

            Assert.IsTrue(result.State == ResultState.SUCCESS, "PerformActionTest: " + result.Message);
            Assert.AreEqual(result.ReturnValue, "SpaceTraffic.Game.Minigame.Minigame", "PerformActionTest: Unexpected return value.");
        }

        [TestMethod()]
        public void RemoveGameTest()
        {
            MinigameManager target = new MinigameManager(gameServer);
            target.registerMinigame(this.minigameDescriptor);

            int minigameId = prepareMinigame(target);
            target.endGame(minigameId);

            Result result = target.removeGame(minigameId);
            Assert.IsTrue(result.State == ResultState.SUCCESS, "RemoveGameTest: " + result.Message);
        }

        private int prepareMinigame(MinigameManager manager)
        {
            int minigameId = manager.createGame(this.minigameDescriptor.MinigameId, false);
            manager.addPlayer(minigameId, player1.PlayerId);
            manager.addPlayer(minigameId, player2.PlayerId);

            manager.startGame(minigameId);

            return minigameId;
        }

        private StartAction CreateStartAction(string actionName)
        {
            StartAction sa = new StartAction();
            sa.ActionName = actionName;

            return sa;
        }

        private MinigameDescriptor CreateMinigameDescriptor()
        {
            MinigameDescriptor md = new MinigameDescriptor();
            md.Name = "SpaceTraffic";
            md.PlayerCount = 2;
            md.Description = "Popis hry.";
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
            player.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            player.PsswdSalt = "cbOpKKxb";
            player.OrionEmail = "email@students.zcu.cz";
            player.AddedDate = DateTime.Now;
            player.LastVisitedDate = DateTime.Now;
            player.ExperienceLevel = 11;
            
            return player;
        }
    }

    internal class GameServerMock : IGameServer
    {
        private IPersistenceManager persistenceManager;
        private IGameManager gameManager;

        public IPersistenceManager Persistence
        {
            get { return persistenceManager; }
        }

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
            get { throw new NotImplementedException(); }
        }

        public IGameManager Game
        {
            get { return gameManager; }
        }

        public IStatisticsManager Statistics
        {
            get { throw new NotImplementedException(); }
        }

        public IMinigameManager Minigame
        {
            get { throw new NotImplementedException(); }
        }

        public GameServerMock()
        {
            this.persistenceManager = new PersitanceManagerMock();
            this.gameManager = new GameManagerMock();
        }
    }

    internal class PersitanceManagerMock : IPersistenceManager
    {

        public SpaceTraffic.Dao.IPlayerDAO GetPlayerDAO()
        {
            return new PlayerDAO();
        }

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

        public SpaceTraffic.Dao.IMinigameDescriptorDAO GetMinigameDescriptorDAO()
        {
            return new MinigameDescriptorDAO();
        }

        public SpaceTraffic.Dao.IStartActionDAO GetStartActionDAO()
        {
            return new StartActionDAO();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

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

        public object PerformAction(IGameAction action)
        {
            throw new NotImplementedException();
        }

        public void PerformActionAsync(IGameAction action)
        {
            throw new NotImplementedException();
        }

        public void PlanEvent(IGameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        public void PlanEvent(IGameAction action, DateTime when)
        {
            throw new NotImplementedException();
        }
    }
}
