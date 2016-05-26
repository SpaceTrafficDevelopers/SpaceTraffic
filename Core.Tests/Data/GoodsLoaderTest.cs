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

using SpaceTraffic.GameServer.Configuration;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.GameServer;
using SpaceTraffic.Data;
using SpaceTraffic.Utils.Tests;
using System.IO;

namespace Core.Tests.Data
{
    /// <summary>
    /// Summary description for GoodsLoaderTest
    /// </summary>
    [TestClass]
    public class GoodsLoaderTest
    {
        /// <summary>
        /// List of goods
        /// </summary>
        private IList<IGoods> listGoods;

        /// <summary>
        /// Test of goods loader
        /// </summary>
        public GoodsLoaderTest()
        {
            string solutionPath = TestPath.getPathToSolution();
            string assetPath = Path.Combine(solutionPath, "Assets");
            AssetManager instance = new AssetManager(assetPath);
            instance.Initialize();

            listGoods = instance.LoadGoods("Goods");
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides information about and functionality for the current test run.
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
                Assert.IsNotNull(good.Description, "Goods have not set description.");
                Assert.IsNotNull(good.ID, "Goods have not set id.");
                Assert.IsNotNull(good.LevelToBuy, "Goods have not set level to buy.");
                Assert.IsNotNull(good.Name, "Goods have not set name.");
                Assert.IsNotNull(good.Price, "Goods have not set price.");
                Assert.IsNotNull(good.Type, "Goods have not set type.");
                Assert.IsNotNull(good.Volume, "Goods have not set volume.");
                
            }
        }
    }
}
