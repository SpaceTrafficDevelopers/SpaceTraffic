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
    [TestClass()]
    [DeploymentItem("GameServer.Tests.dll.config")]
    public class TraderDAOTest
    {
        private Trader traderTest;

        private Base baseTest;

        private Base baseTest2;

        [TestInitialize()]
        public void TestInitialize()
        {

        }

        [TestCleanup()]
        public void CleanUp()
        {
            if (traderTest != null)
            {
                TraderDAO traderDAO = new TraderDAO();
                traderDAO.RemoveTraderById(traderTest.TraderId);
            }

            if (baseTest != null)
            {
                BaseDAO bd = new BaseDAO();
                bd.RemoveBaseById(baseTest.BaseId);
            }

            if (baseTest2 != null)
            {
                BaseDAO bd = new BaseDAO();
                bd.RemoveBaseById(baseTest2.BaseId);
            }
        }


        /// <summary>
        ///A test for TraderDAO Constructor
        ///</summary>
        [TestMethod()]
        public void TraderDAOConstructorTest()
        {
            TraderDAO target = new TraderDAO();
            Assert.IsNotNull(target);
        }



        /// <summary>
        ///A test for GetTraders
        ///</summary>
        [TestMethod()]
        public void GetTradersTest()
        {
            TraderDAO target = new TraderDAO();
            traderTest = CreateTrader();
            target.InsertTrader(traderTest);

            baseTest2 = new Base();
            baseTest2.BaseId = 2;
            baseTest2.Planet = "Naboo";

            BaseDAO bd = new BaseDAO();
            bd.InsertBase(baseTest2);

            traderTest.BaseId = baseTest2.BaseId;
            target.InsertTrader(traderTest);
            
            List<Trader> traders = target.GetTraders();

            Assert.IsNotNull(traders);
            Assert.IsTrue(traders.Count == 2, "GetTradersTest: List of traders does not have expected number of items.");
        }

		/// <summary>
		///A test for GetTraderByBaseId
		///</summary>
		[TestMethod()]
		public void GetTraderByBaseIdTest()
		{
			TraderDAO target = new TraderDAO();
			traderTest = CreateTrader();
			target.InsertTrader(traderTest);

			baseTest2 = new Base();
			baseTest2.BaseId = 2;
			baseTest2.Planet = "Naboo";

			BaseDAO bd = new BaseDAO();
			bd.InsertBase(baseTest2);

			traderTest.BaseId = baseTest2.BaseId;
			target.InsertTrader(traderTest);

			Trader newTrader = target.GetTraderByBaseId(traderTest.BaseId);

			TraderTest(newTrader);
		}

		/// <summary>
		///A test for GetTraderByBaseIdWithCargo
		///</summary>
		[TestMethod()]
		public void GetTraderByBaseIdWithCargoTest()
		{
			TraderDAO target = new TraderDAO();
			traderTest = CreateTrader();
			target.InsertTrader(traderTest);

			baseTest2 = new Base();
			baseTest2.BaseId = 2;
			baseTest2.Planet = "Naboo";

			BaseDAO bd = new BaseDAO();
			bd.InsertBase(baseTest2);

			traderTest.BaseId = baseTest2.BaseId;
			target.InsertTrader(traderTest);

			Trader newTrader = target.GetTraderByBaseIdWithCargo(traderTest.BaseId);

			TraderTest(newTrader);
			Assert.IsNotNull(newTrader.TraderCargos);
		}
        
        /// <summary>
        ///A test for GetTraderById
        ///</summary>
        [TestMethod()]
        public void GetTraderByIdTest()
        {
            TraderDAO target = new TraderDAO();
            traderTest = CreateTrader();
            target.InsertTrader(traderTest);
            Trader newTrader = target.GetTraderById(traderTest.TraderId);

            TraderTest(newTrader);
        }

        /// <summary>
        ///A test for InsertTrader
        ///</summary>
        [TestMethod()]
        public void InsertTraderTest()
        {
            TraderDAO target = new TraderDAO();
            traderTest = CreateTrader();
            bool insert = target.InsertTrader(traderTest);

            Assert.IsTrue(insert);

            
        }

        /// <summary>
        ///A test for RemoveTraderById
        ///</summary>
        [TestMethod()]
        public void RemoveTraderByIdTest()
        {
            TraderDAO target = new TraderDAO();
            traderTest = CreateTrader();
            target.InsertTrader(traderTest);
            bool remove = target.RemoveTraderById(traderTest.TraderId);

            Assert.IsTrue(remove);

            traderTest = null;
        }

        /// <summary>
        ///A test for UpdateTraderById
        ///</summary>
        [TestMethod()]
        public void UpdateTraderByIdTest()
        {
            TraderDAO target = new TraderDAO();
            traderTest = CreateTrader();
            target.InsertTrader(traderTest);

            baseTest2 = new Base();
            baseTest2.BaseId = 2;
            baseTest2.Planet = "Naboo";

            BaseDAO bd = new BaseDAO();
            bd.InsertBase(baseTest2);

            traderTest.BaseId = baseTest2.BaseId;

            target.UpdateTraderById(traderTest);

            Trader newTrader = target.GetTraderById(traderTest.TraderId);
            TraderTest(newTrader);
        }

        private Trader CreateTrader()
        {
            baseTest = new Base();

            baseTest.BaseId = 1;
            baseTest.Planet = "Tatooine";

            BaseDAO bd = new BaseDAO();
            bd.InsertBase(baseTest);

            Trader trader = new Trader();
            trader.BaseId = baseTest.BaseId;

            return trader;
        }

        private void TraderTest(Trader trader)
        {
            Assert.IsNotNull(trader, "Trader cannot be null.");
            Assert.AreEqual(this.traderTest.TraderId, trader.TraderId, "Trader ID are not equal.");
            Assert.AreEqual(this.traderTest.BaseId, trader.BaseId, "Trader BaseID are not equal.");
        }

        [ClassCleanup()]
        public static void DropDatabase()
        {
            System.Data.Entity.Database.Delete(System.Configuration.ConfigurationManager.ConnectionStrings["SpaceTrafficContext"].ConnectionString);
        }
    }
}
