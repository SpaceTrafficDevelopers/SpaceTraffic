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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTraffic.Entities;
using SpaceTraffic.Dao;
using System.Collections.Generic;

namespace SpaceTraffic.GameServerTests.Dao
{
    /// <summary>
    ///This is a test class for TraderCargoDAOTest and is intended
    ///to contain all TraderCargoDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class TraderCargoDAOTest
    {
        private Trader trader;
        private Cargo cargo1;
        private Cargo cargo2;
        private Base newBase;
        private TraderCargo traderCargo;


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

            trader = CreateTrader();

            TraderDAO traderDao = new TraderDAO();
            traderDao.InsertTrader(trader);
        }

        [TestCleanup]
        public void CleanUp()
        {
            CargoDAO cargoDao = new CargoDAO();
            cargoDao.RemoveCargoById(cargo1.CargoId);
            cargoDao.RemoveCargoById(cargo2.CargoId);

            BaseDAO bas = new BaseDAO();
            bas.RemoveBaseById(newBase.BaseId);

            TraderDAO td = new TraderDAO();
            td.RemoveTraderById(trader.TraderId);

            if (traderCargo != null)
            {
                TraderCargoDAO tcDAO = new TraderCargoDAO();
                tcDAO.RemoveCargoById(traderCargo.TraderCargoId);
            }
        }

        /// <summary>
        ///A test for GetCargoByID
        ///</summary>
        [TestMethod()]
        public void GetCargoByIDTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();
            traderCargo = CreateTraderCargo();

            target.InsertCargo(traderCargo);

            TraderCargo tc = target.GetCargoByID(traderCargo.TraderCargoId) as TraderCargo;

            TraderCargoTest(traderCargo, tc);
        }

        /// <summary>
        ///A test for GetCargoListByOwnerId
        ///</summary>
        [TestMethod()]
        public void GetCargoListByOwnerIdTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();
            traderCargo = CreateTraderCargo();

            target.InsertCargo(traderCargo);
            traderCargo.CargoCount = 20;
            target.InsertCargo(traderCargo);

            List<ICargoLoadEntity> cargos = target.GetCargoListByOwnerId(traderCargo.TraderId);

            Assert.IsNotNull(cargos);
            Assert.IsTrue(cargos.Count == 2, "GetCargoListByOwnerIdTest: List of cargo does not have expected number of items.");
        }

        /// <summary>
        ///A test for TraderCargoDAO Constructor
        ///</summary>
        [TestMethod()]
        public void TraderCargoDAOConstructorTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for InsertTraderCargo
        ///</summary>
        [TestMethod()]
        public void InsertTraderCargoTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();
            traderCargo = CreateTraderCargo();
            bool insert = target.InsertCargo(traderCargo);

            Assert.IsTrue(insert);
        }

        /// <summary>
        ///A test for RemoveTraderCargoById
        ///</summary>
        [TestMethod()]
        public void RemoveTraderCargoByIdTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();
            traderCargo = CreateTraderCargo();

            target.InsertCargo(traderCargo);
            bool actual = target.RemoveCargoById(traderCargo.TraderCargoId);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for UpdateCargo
        ///</summary>
        [TestMethod()]
        public void UpdateCargoTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();
            traderCargo = CreateTraderCargo();

            target.InsertCargo(traderCargo);

            traderCargo.CargoId = cargo2.CargoId;
            //traderCargo.CargoPrice = 100;
            traderCargo.CargoCount = 100;

            target.UpdateCargo(traderCargo);

            TraderCargo tc = target.GetCargoByID(traderCargo.TraderCargoId) as TraderCargo;

            TraderCargoTest(traderCargo, tc);
        }

        /// <summary>
        ///A test for InsertOrUpdateCargo
        ///</summary>
        [TestMethod()]
        public void InsertOrUpdateCargoTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();

            traderCargo = CreateTraderCargo();
            traderCargo.CargoCount = 100;
            target.InsertOrUpdateCargo(traderCargo);

            TraderCargo tc = target.GetCargoByID(traderCargo.TraderCargoId) as TraderCargo;

            TraderCargoTest(traderCargo, tc);
            traderCargo.CargoCount = 20;

            target.InsertOrUpdateCargo(traderCargo);

            tc = target.GetCargoByID(traderCargo.TraderCargoId) as TraderCargo;
            traderCargo.CargoCount = 120;

            TraderCargoTest(traderCargo, tc);
        }

        /// <summary>
        ///A test for UpdateOrRemoveCargo
        ///</summary>
        [TestMethod()]
        public void UpdateOrRemoveCargoTest()
        {
            TraderCargoDAO target = new TraderCargoDAO();

            traderCargo = CreateTraderCargo();
            traderCargo.CargoCount = 100;
            target.InsertCargo(traderCargo);

            traderCargo.CargoCount = 50;

            target.UpdateOrRemoveCargo(traderCargo);

            TraderCargo tc = target.GetCargoByID(traderCargo.TraderCargoId) as TraderCargo;

            TraderCargoTest(traderCargo, tc);

            target.UpdateOrRemoveCargo(traderCargo);

            tc = target.GetCargoByID(traderCargo.TraderCargoId) as TraderCargo;

            Assert.AreEqual(tc.CargoCount, 0);
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

        private Trader CreateTrader()
        {
            Trader trader = new Trader();
            trader.BaseId = newBase.BaseId;

            return trader;
        }

        private TraderCargo CreateTraderCargo()
        {
            TraderCargo tc = new TraderCargo();
            tc.CargoCount = 300;
            tc.CargoId = cargo1.CargoId;
           // tc.CargoPrice = 200;
            tc.TraderId = trader.TraderId;

            return tc;
        }

        private void TraderCargoTest(TraderCargo excepted, TraderCargo actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.TraderCargoId, actual.TraderCargoId, "TraderCargoID are not equal.");
            Assert.AreEqual(excepted.CargoId, actual.CargoId, "CargoID are not equal.");
            Assert.AreEqual(excepted.TraderId, actual.TraderId, "TraderID are not equal.");
            Assert.AreEqual(excepted.CargoCount, actual.CargoCount, "CargoCount are not equal.");
           // Assert.AreEqual(excepted.CargoPrice, actual.CargoPrice, "CargoPrice are not equal.");
        }


        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
