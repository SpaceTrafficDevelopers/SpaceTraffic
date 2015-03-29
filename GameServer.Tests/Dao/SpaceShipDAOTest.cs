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
using SpaceTraffic.Dao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SpaceTraffic.Entities;
using System.Collections.Generic;
using System.Text;

namespace SpaceTraffic.GameServerTests.Dao
{


    /// <summary>
    ///This is a test class for SpaceShipsDAOTest and is intended
    ///to contain all SpaceShipsDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class SpaceShipDAOTest
    {


        private TestContext testContextInstance;

        private Player player;

        private Base newBase;

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
            PlayerDAO dao = new PlayerDAO();
            player = CreatePlayer();
            dao.InsertPlayer(player);

            BaseDAO bas = new BaseDAO();
            newBase = new Base();
            newBase.Planet = "Země";
            bas.InsertBase(newBase);
        }

        /// <summary>
        ///A test for SpaceShipsDAO Constructor
        ///</summary>
        [TestMethod()]
        public void SpaceShipsDAOConstructorTest()
        {
            SpaceShipDAO target = new SpaceShipDAO();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for GetSpaceShipById
        ///</summary>
        [TestMethod()]
        public void GetSpaceShipByIdTest()
        {
            SpaceShipDAO target = new SpaceShipDAO();
            SpaceShip spaceShip = CreateSpaceShip();
            target.InsertSpaceShip(spaceShip);
            SpaceShip actual = target.GetSpaceShipById(spaceShip.SpaceShipId);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for GetSpaceShips
        ///</summary>
        [TestMethod()]
        public void GetSpaceShipsTest()
        {
            SpaceShipDAO target = new SpaceShipDAO();
            SpaceShip spaceShip = CreateSpaceShip();
            target.InsertSpaceShip(spaceShip);
            List<SpaceShip> spaceShips = target.GetSpaceShips();
            Assert.IsTrue(spaceShips.Count > 0);

            target.RemoveSpaceShipById(spaceShip.SpaceShipId);
        }

        /// <summary>
        ///A test for GetSpaceShipsByPlayer
        ///</summary>
        [TestMethod()]
        public void GetSpaceShipsByPlayerTest()
        {
            SpaceShipDAO target = new SpaceShipDAO();
            SpaceShip spaceShip = CreateSpaceShip();
            target.InsertSpaceShip(spaceShip);
            List<SpaceShip> spaceShips = target.GetSpaceShipsByPlayer(player.PlayerId);
            Assert.IsNotNull(spaceShips);

            target.RemoveSpaceShipById(spaceShip.SpaceShipId);
        }

        /// <summary>
        ///A test for InsertSpaceShip
        ///</summary>
        [TestMethod()]
        public void InsertSpaceShipTest()
        {
            SpaceShipDAO target = new SpaceShipDAO();
            SpaceShip spaceShip = CreateSpaceShip();
            bool result = target.InsertSpaceShip(spaceShip);
            Assert.IsTrue(result);

            SpaceShip actual = target.GetSpaceShipById(spaceShip.SpaceShipId);
            Assert.IsNotNull(actual);

            target.RemoveSpaceShipById(spaceShip.SpaceShipId);
        }

        /// <summary>
        ///A test for RemoveSpaceShipById
        ///</summary>
        [TestMethod()]
        public void RemoveSpaceShipByIdTest()
        {
            SpaceShipDAO target = new SpaceShipDAO();
            SpaceShip spaceShip = CreateSpaceShip();
            target.InsertSpaceShip(spaceShip);

            SpaceShip actual = target.GetSpaceShipById(spaceShip.SpaceShipId);
            Assert.IsNotNull(actual);

            target.RemoveSpaceShipById(spaceShip.SpaceShipId);
            Assert.IsNull(target.GetSpaceShipById(spaceShip.SpaceShipId));
        }

        /// <summary>
        ///A test for UpdateSpaceShipById
        ///</summary>
        [TestMethod()]
        public void UpdateSpaceShipByIdTest()
        {
            SpaceShipDAO target = new SpaceShipDAO();
            SpaceShip spaceShip = CreateSpaceShip();
            bool result = target.InsertSpaceShip(spaceShip);
            Assert.IsTrue(result);

            SpaceShip actual = target.GetSpaceShipById(spaceShip.SpaceShipId);

            
            spaceShip.IsFlying = false;
            spaceShip.DamagePercent = 70;
            spaceShip.CurrentStarSystem = "Mars";
            target.UpdateSpaceShipById(spaceShip);

            SpaceShip compare = target.GetSpaceShipById(spaceShip.SpaceShipId);
            Assert.IsTrue(compare.IsFlying.Equals(false) & compare.DamagePercent == 70
                & compare.CurrentStarSystem.Equals("Mars"));

            target.RemoveSpaceShipById(spaceShip.SpaceShipId);
        }

        /// <summary>
        /// Remove players from database.
        /// </summary>
        [TestCleanup]
        public void ClenUp()
        {
            PlayerDAO dao = new PlayerDAO();
            dao.RemovePlayerById(player.PlayerId);

            BaseDAO bas = new BaseDAO();
            bas.RemoveBaseById(newBase.BaseId);
        }

        private SpaceShip CreateSpaceShip()
        {
            SpaceShip ship = new SpaceShip();
            ship.CurrentStarSystem = "Země";
            ship.SpaceShipName = "Moje lod";
            ship.SpaceShipModel = "model";
            ship.DamagePercent = 40;
            ship.FuelTank = 50;
            ship.CurrentFuelTank = 34;
            ship.UserCode = "Go to ...";
            ship.TimeOfArrival = "12:00";
            ship.PlayerId = player.PlayerId;
            ship.IsFlying = true;
            ship.DockedAtBaseId = newBase.BaseId;
            return ship;
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
            newPlayer.PlayerName = RandomString(4);
            newPlayer.CorporationName = "ZCU";
            newPlayer.Credit = 0;
            newPlayer.DateOfBirth = new DateTime(2008, 2, 16, 12, 15, 12);
            newPlayer.Email = "email@email.cz";
            newPlayer.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            newPlayer.PsswdSalt = "cbOpKKxb";
            newPlayer.OrionEmail = "email@students.zcu.cz";
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;
            return newPlayer;
        }

        /// <summary>
        /// Generate random player name
        /// </summary>
        /// <param name="size">length of the string</param>
        /// <returns>string</returns>
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
