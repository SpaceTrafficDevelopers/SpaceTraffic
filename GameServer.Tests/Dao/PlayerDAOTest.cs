using SpaceTraffic.Dao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SpaceTraffic.Entities;
using System.Collections.Generic;
using SpaceTraffic.Persistence;
using System.Data.Entity;

namespace SpaceTraffic.GameServerTests.Dao
{


    /// <summary>
    ///This is a test class for PlayerDAOTest and is intended
    ///to contain all PlayerDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class PlayerDAOTest
    {


        private TestContext testContextInstance;

        private Player player;


        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestInitialize()]
        public void Initializace()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<SpaceTrafficContext>());
            PlayerDAO dao = new PlayerDAO();
            player = CreatePlayer();
            dao.InsertPlayer(player);
        }

        /// <summary>
        ///A test for PlayerDAO Constructor
        ///</summary>
        [TestMethod()]
        public void PlayerDAOConstructorTest()
        {
            PlayerDAO dao = new PlayerDAO();
            Assert.IsNotNull(dao);

        }

        [TestMethod()]
        public void GetPlayerByEmailTest()
        {
            PlayerDAO dao = new PlayerDAO();
            Player returnPlayer = dao.GetPlayerByEmail("email@email.cz");
            Assert.IsTrue(returnPlayer.PlayerId == player.PlayerId);
        }

        /// <summary>
        ///A test for InsertPlayer
        ///</summary>
        [TestMethod()]
        public void InsertPlayerTest()
        {
            PlayerDAO dao = new PlayerDAO();
            Player player = CreatePlayer();
            player.PlayerName = "player2";
            bool result = dao.InsertPlayer(player);
            Assert.IsTrue(result);
            Player insertPlayer = dao.GetPlayerByName("player2");
            dao.RemovePlayerById(insertPlayer.PlayerId);
            
        }

        /// <summary>
        ///A test for GetPlayerByName
        ///</summary>
        [TestMethod()]
        public void GetPlayerByNameTest()
        {
            PlayerDAO dao = new PlayerDAO();
            Player player = dao.GetPlayerByName("player");
            Assert.IsNotNull(player);
            Assert.IsTrue(player.PlayerName == "player");            
        }


        /// <summary>
        ///A test for GetPlayerById
        ///</summary>
        [TestMethod()]
        public void GetPlayerByIdTest()
        {
            PlayerDAO dao = new PlayerDAO();
            int id = dao.GetPlayerByName("player").PlayerId;
            Player player = dao.GetPlayerById(id);
            Assert.IsTrue(player.PlayerId == id);
            Assert.IsNotNull(player);
        }

        /// <summary>
        ///A test for GetPlayers
        ///</summary>
        [TestMethod()]
        public void GetPlayersTest()
        {
            PlayerDAO dao = new PlayerDAO();
            List<Player> players = dao.GetPlayers();
            Assert.IsNotNull(players);
            Assert.IsTrue(players.Count > 0);
        }


        /// <summary>
        ///A test for UpdatePlayerById
        ///</summary>
        [TestMethod()]
        public void UpdatePlayerByIdTest()
        {
            PlayerDAO dao = new PlayerDAO();
            int id = dao.GetPlayerByName("player").PlayerId;
            Player player = dao.GetPlayerById(id);
            player.FirstName = "Lukáš";
            bool result = dao.UpdatePlayerById(player);
            Player comparePlayer = dao.GetPlayerById(id);
            Assert.IsTrue(result);
            Assert.IsTrue(comparePlayer.FirstName.Equals("Lukáš"));
        }

        /// <summary>
        ///A test for RemovePlayerById
        ///</summary>
        [TestMethod]
        public void RemovePlayerByIdTest()
        {
            PlayerDAO dao = new PlayerDAO();
            Player player = CreatePlayer();
            player.PlayerName = "player2";
            dao.InsertPlayer(player);
            int id = dao.GetPlayerByName("player2").PlayerId;
            dao.RemovePlayerById(id);
            Assert.IsNull(dao.GetPlayerById(id));
        }

        [TestCleanup]
        public void ClenUp()
        {
            PlayerDAO dao = new PlayerDAO();
            int id = dao.GetPlayerByName("player").PlayerId;
            dao.RemovePlayerById(id);
        }



        /// <summary>
        /// Create player.
        /// </summary>
        /// <returns></returns>
        private Player CreatePlayer()
        {
            Player newPlayer = new Player();
            newPlayer.FirstName = "Karel";
            newPlayer.LastName = "Malý";
            newPlayer.PlayerName = "player";
            newPlayer.CorporationName = "ZCU";
            newPlayer.Credit = 0;
            newPlayer.DateOfBirth = DateTime.Parse("16/02/2008 12:15:12");
            newPlayer.Email = "email@email.cz";
            newPlayer.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            newPlayer.PsswdSalt = "cbOpKKxb";
            newPlayer.OrionEmail = "email@students.zcu.cz";
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;
            return newPlayer;
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
