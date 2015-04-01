using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SpaceTraffic.GameServer.Configuration;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.GameServer;
using SpaceTraffic.Data;

namespace Core.Tests.Data
{
    /// <summary>
    /// Summary description for GoodsLoaderTest
    /// </summary>
    [TestClass]
    public class GoodsLoaderTest
    {

        private IList<IGoods> listGoods;

        public GoodsLoaderTest()
        {
            AssetManager instance = new AssetManager("./../../../Assets");
            instance.Initialize();

            listGoods = instance.LoadGoods("Goods");
        }

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


        [TestMethod]
        public void TestLoadedXml()
        {
            Assert.IsNotNull(listGoods, "Goods was not loaded.");
            Assert.IsTrue(listGoods.Count > 0, "List of goods is empty.");

            foreach (IGoods good in listGoods) {
                Assert.IsNotNull(good.Description, "Goods haven not set description.");
                Assert.IsNotNull(good.ID, "Goods haven not set id.");
                Assert.IsNotNull(good.LevelToBuy, "Goods haven not set level to buy.");
                Assert.IsNotNull(good.Name, "Goods haven not set name.");
                Assert.IsNotNull(good.Price, "Goods haven not set price.");
                Assert.IsNotNull(good.type, "Goods haven not set type.");
                Assert.IsNotNull(good.Volume, "Goods haven not set volume.");
                
            }
        }
    }
}
