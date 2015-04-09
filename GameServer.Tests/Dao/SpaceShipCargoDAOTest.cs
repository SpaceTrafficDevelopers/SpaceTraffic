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
using System.Text;
using System.Collections.Generic;


namespace SpaceTraffic.GameServerTests.Dao
{
    
    
    /// <summary>
    ///This is a test class for SpaceShipCargoDAOTest and is intended
    ///to contain all SpaceShipCargoDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class SpaceShipCargoDAOTest
    {

        private Player player;
        private SpaceShip ship;
        private Cargo cargo1;
        private Cargo cargo2;
        private Base newBase;

        private TestContext testContextInstance;

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

        [TestInitialize]
        public void Initialize()
        {  
            CargoDAO cargoDao = new CargoDAO();
            cargo1 = CreateCargo();
            cargo2 = CreateCargo();
            cargo2.Type = "newtype";
            cargoDao.InsertCargo(cargo1);
            cargoDao.InsertCargo(cargo2);

            BaseDAO bas = new BaseDAO();
            newBase = new Base();
            newBase.Planet = "Země";
            bas.InsertBase(newBase);

            PlayerDAO playerDao = new PlayerDAO();
            player = CreatePlayer();
            playerDao.InsertPlayer(player);

            SpaceShipDAO shipDao = new SpaceShipDAO();
            ship = CreateSpaceShip();
            shipDao.InsertSpaceShip(ship);
        }

        [TestCleanup]
        public void ClenUp()
        {
            CargoDAO cargoDao = new CargoDAO();
            cargoDao.RemoveCargoById(cargo1.Goods);
            cargoDao.RemoveCargoById(cargo2.Goods);

            BaseDAO bas = new BaseDAO();
            bas.RemoveBaseById(newBase.BaseId);

            PlayerDAO playerDao = new PlayerDAO();
            playerDao.RemovePlayerById(player.PlayerId);

            SpaceShipDAO shipDao = new SpaceShipDAO();
            shipDao.RemoveSpaceShipById(ship.SpaceShipId);
        }

        /// <summary>
        ///A test for SpaceShipCargoDAO Constructor
        ///</summary>
        [TestMethod()]
        public void SpaceShipCargoDAOConstructorTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for InsertSpaceShipCargo
        ///</summary>
        [TestMethod()]
        public void InsertSpaceShipCargoTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            SpaceShipCargo spaceShipCargo = new SpaceShipCargo();          
            spaceShipCargo.CargoId = cargo1.Goods;
            spaceShipCargo.CargoCount = 3;
            spaceShipCargo.SpaceShipId = ship.SpaceShipId;
            bool insert = target.InsertSpaceShipCargo(spaceShipCargo);
            Assert.IsTrue(insert);           
        }

        /// <summary>
        ///A test for RemoveSpaceShipCargoById
        ///</summary>
        [TestMethod()]
        public void RemoveSpaceShipCargoByIdTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            SpaceShipCargo spaceShipCargo = new SpaceShipCargo();           
            spaceShipCargo.CargoId = cargo1.Goods;
            spaceShipCargo.CargoCount = 3;
            spaceShipCargo.SpaceShipId = ship.SpaceShipId;
            bool insert = target.InsertSpaceShipCargo(spaceShipCargo);
            bool actual = target.RemoveSpaceShipCargoById(ship.SpaceShipId, cargo1.Goods);
            Assert.IsTrue(actual);

            SpaceShipCargoDAO cargo = new SpaceShipCargoDAO();
            List<SpaceShipCargo> cargos = cargo.GetSpaceShipCargoBySpaceShipId(ship.SpaceShipId);
            Assert.IsTrue(cargos != null);
        }

        /// <summary>
        ///A test for UpdateCargoCountById
        ///</summary>
        [TestMethod()]
        public void UpdateCargoCountByIdTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            SpaceShipCargo spaceShipCargo = new SpaceShipCargo();           
            spaceShipCargo.CargoId = cargo1.Goods;
            spaceShipCargo.CargoCount = 3;
            spaceShipCargo.SpaceShipId = ship.SpaceShipId;
            bool insert = target.InsertSpaceShipCargo(spaceShipCargo);
            spaceShipCargo.CargoId = cargo2.Goods;
            target.InsertSpaceShipCargo(spaceShipCargo);
            spaceShipCargo.CargoCount = 5;

            target.UpdateCargoCountById(spaceShipCargo);

            List<SpaceShipCargo> cargos = target.GetSpaceShipCargoBySpaceShipId(ship.SpaceShipId);
            Assert.IsTrue(cargos.Count == 2);
            Assert.IsTrue(cargos[1].CargoCount == 8);
            
        }

        /// <summary>
        ///A test for UpdateCargoCountById
        ///</summary>
        [TestMethod()]
        public void GetSpaceShipCargoBySpaceShipId()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            SpaceShipCargo spaceShipCargo = new SpaceShipCargo();
            spaceShipCargo.CargoId = cargo1.Goods;
            spaceShipCargo.CargoCount = 3;
            spaceShipCargo.SpaceShipId = ship.SpaceShipId;
            bool insert = target.InsertSpaceShipCargo(spaceShipCargo);
            spaceShipCargo.CargoId = cargo2.Goods;
            target.InsertSpaceShipCargo(spaceShipCargo);

            List<SpaceShipCargo> cargos = target.GetSpaceShipCargoBySpaceShipId(ship.SpaceShipId);
            Assert.IsTrue(cargos.Count == 2);
        }

        private Cargo CreateCargo()
        {
            Cargo cargo = new Cargo();
            cargo.PriceCargo = 200;
            cargo.Type = "nářadí";
            return cargo;
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

        private Player CreatePlayer()
        {
            Player newPlayer = new Player();
            newPlayer.FirstName = "Karel";
            newPlayer.LastName = "Malý";
            newPlayer.PlayerName = RandomString(4);
            newPlayer.CorporationName = "ZCU";
            newPlayer.Credit = 0;
            newPlayer.DateOfBirth = new DateTime(2008, 02, 16, 12, 15, 12);
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
