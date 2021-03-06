﻿/**
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
        private SpaceShipCargo spaceShipCargo;

       
        [TestInitialize]
        public void Initialize()
        {  
            CargoDAO cargoDao = new CargoDAO();
            cargo1 = CreateCargo();
            cargo2 = CreateCargo();
            cargo2.Name = "AK47";
            cargoDao.InsertCargo(cargo1);
            cargoDao.InsertCargo(cargo2);

            BaseDAO bas = new BaseDAO();
            newBase = new Base();
            newBase.Planet = "Los Santos";
            bas.InsertBase(newBase);

            PlayerDAO playerDao = new PlayerDAO();
            player = CreatePlayer();
            playerDao.InsertPlayer(player);

            SpaceShipDAO shipDao = new SpaceShipDAO();
            ship = CreateSpaceShip();
            shipDao.InsertSpaceShip(ship);
        }

        [TestCleanup]
        public void CleanUp()
        {
            CargoDAO cargoDao = new CargoDAO();
            cargoDao.RemoveCargoById(cargo1.CargoId);
            cargoDao.RemoveCargoById(cargo2.CargoId);

            BaseDAO bas = new BaseDAO();
            bas.RemoveBaseById(newBase.BaseId);

            PlayerDAO playerDao = new PlayerDAO();
            playerDao.RemovePlayerById(player.PlayerId);

            SpaceShipDAO shipDao = new SpaceShipDAO();
            shipDao.RemoveSpaceShipById(ship.SpaceShipId);
            
            if(spaceShipCargo != null)
            {
                SpaceShipCargoDAO sscDAO = new SpaceShipCargoDAO();
                sscDAO.RemoveCargoById(spaceShipCargo.SpaceShipCargoId);
            }
        }

        /// <summary>
        ///A test for GetCargoByID
        ///</summary>
        [TestMethod()]
        public void GetCargoByIDTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            spaceShipCargo = CreateSpaceShipCargo();

            target.InsertCargo(spaceShipCargo);

            SpaceShipCargo ssc = target.GetCargoByID(spaceShipCargo.SpaceShipCargoId) as SpaceShipCargo;

            SpaceShipCargoTest(spaceShipCargo, ssc);
        }

        /// <summary>
        ///A test for GetCargoListByOwnerId
        ///</summary>
        [TestMethod()]
        public void GetCargoListByOwnerIdTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            spaceShipCargo = CreateSpaceShipCargo();
            target.InsertCargo(spaceShipCargo);
            spaceShipCargo.CargoCount = 20;
            target.InsertCargo(spaceShipCargo);

            List<ICargoLoadEntity> cargos = target.GetCargoListByOwnerId(spaceShipCargo.SpaceShipId);

            Assert.IsNotNull(cargos);
            Assert.IsTrue(cargos.Count == 2, "GetCargoListByOwnerIdTest: List of cargo does not have expected number of items.");
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
            spaceShipCargo = CreateSpaceShipCargo();
            bool insert = target.InsertCargo(spaceShipCargo);

            Assert.IsTrue(insert);  
        }

        /// <summary>
        ///A test for RemoveSpaceShipCargoById
        ///</summary>
        [TestMethod()]
        public void RemoveSpaceShipCargoByIdTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            spaceShipCargo = CreateSpaceShipCargo();           
             
            target.InsertCargo(spaceShipCargo);
            bool actual = target.RemoveCargoById(spaceShipCargo.SpaceShipCargoId);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for UpdateCargo
        ///</summary>
        [TestMethod()]
        public void UpdateCargoTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();
            
            spaceShipCargo = CreateSpaceShipCargo();
            target.InsertCargo(spaceShipCargo);

            spaceShipCargo.CargoId = cargo2.CargoId;
            spaceShipCargo.CargoPrice = 100;
            spaceShipCargo.CargoCount = 100;
                       
            target.UpdateCargo(spaceShipCargo);

            SpaceShipCargo ssc = target.GetCargoByID(spaceShipCargo.SpaceShipCargoId) as SpaceShipCargo;

            SpaceShipCargoTest(spaceShipCargo, ssc);
        }

        /// <summary>
        ///A test for InsertOrUpdateCargo
        ///</summary>
        [TestMethod()]
        public void InsertOrUpdateCargoTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();

            spaceShipCargo = CreateSpaceShipCargo();
            spaceShipCargo.CargoCount = 100;
            target.InsertOrUpdateCargo(spaceShipCargo);

            SpaceShipCargo ssc = target.GetCargoByID(spaceShipCargo.SpaceShipCargoId) as SpaceShipCargo;

            SpaceShipCargoTest(spaceShipCargo, ssc);
            spaceShipCargo.CargoCount = 20;

            target.InsertOrUpdateCargo(spaceShipCargo);

            ssc = target.GetCargoByID(spaceShipCargo.SpaceShipCargoId) as SpaceShipCargo;
            spaceShipCargo.CargoCount = 120;

            SpaceShipCargoTest(spaceShipCargo, ssc);
        }

        /// <summary>
        ///A test for UpdateOrRemoveCargo
        ///</summary>
        [TestMethod()]
        public void UpdateOrRemoveCargoTest()
        {
            SpaceShipCargoDAO target = new SpaceShipCargoDAO();

            spaceShipCargo = CreateSpaceShipCargo();
            spaceShipCargo.CargoCount = 100;
            target.InsertCargo(spaceShipCargo);

            spaceShipCargo.CargoCount = 50;

            target.UpdateOrRemoveCargo(spaceShipCargo);

            SpaceShipCargo ssc = target.GetCargoByID(spaceShipCargo.SpaceShipCargoId) as SpaceShipCargo;

            SpaceShipCargoTest(spaceShipCargo, ssc);

            target.UpdateOrRemoveCargo(spaceShipCargo);

            ssc = target.GetCargoByID(spaceShipCargo.SpaceShipCargoId) as SpaceShipCargo;

            Assert.IsNull(ssc);
        }

        private Cargo CreateCargo()
        {
            Cargo cargo = new Cargo();

            cargo.Name = "M4";
            cargo.Description = "Fakt dobrej kulomet.";
            cargo.Type = GoodsType.Mainstream.ToString();
            cargo.DefaultPrice = 200;
            cargo.Category = "Zbraně";
            cargo.LevelToBuy = 2;
            cargo.Volume = 100;

            return cargo;
        }

        private SpaceShip CreateSpaceShip()
        {
            SpaceShip ship = new SpaceShip();
            ship.SpaceShipId = 1;
            ship.CurrentStarSystem = "Star Wars";
            ship.SpaceShipName = "Autokár";
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
            newPlayer.PlayerName = "UplneNejvicNejbozesjiJmeno";
            newPlayer.PlayerShowName = "UplneNejvicNejbozesjiJmeno";
            newPlayer.Credit = 0;
            newPlayer.Email = "michel@párek.cz";
            newPlayer.PsswdHash = "enanTfHBOWSrAlyc5x6d2emhcmI=";
            newPlayer.PassChangeDate = DateTime.Now;
            newPlayer.AddedDate = DateTime.Now;
            newPlayer.LastVisitedDate = DateTime.Now;
            return newPlayer;
        }


        private SpaceShipCargo CreateSpaceShipCargo()
        {
            SpaceShipCargo ssc = new SpaceShipCargo();
            ssc.CargoCount = 300;
            ssc.CargoId = cargo1.CargoId;
            ssc.SpaceShipId = ship.SpaceShipId;
            ssc.CargoPrice = 200;

            return ssc;
        }

        private void SpaceShipCargoTest(SpaceShipCargo excepted, SpaceShipCargo actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.SpaceShipCargoId, actual.SpaceShipCargoId, "SpaceShipCargoID are not equal.");
            Assert.AreEqual(excepted.CargoId, actual.CargoId, "CargoID are not equal.");
            Assert.AreEqual(excepted.SpaceShipId, actual.SpaceShipId, "SpaceShipID are not equal.");
            Assert.AreEqual(excepted.CargoCount, actual.CargoCount, "CargoCount are not equal.");
            Assert.AreEqual(excepted.CargoPrice, actual.CargoPrice, "CargoPrice are not equal.");
        }


        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
