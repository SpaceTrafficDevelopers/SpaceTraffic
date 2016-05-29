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
using SpaceTraffic.Game;
using SpaceTraffic.Utils.Tests;
using System.IO;

namespace Core.Tests.Data
{
    /// <summary>
    /// Summary description for EconomicLevelLoaderTests
    /// </summary>
    [TestClass]
    public class EconomicLevelLoaderTests
    {
        /// <summary>
        /// List of economic levels
        /// </summary>
        private IList<EconomicLevel> economicLevels;

        /// <summary>
        /// Constructor of economic level loader test
        /// </summary>
        public EconomicLevelLoaderTests()
        {
            string solutionPath = TestPath.getPathToSolution();
            string assetPath = Path.Combine(solutionPath, "Assets");
            AssetManager instance = new AssetManager(assetPath);
            instance.Initialize();

            economicLevels = instance.LoadEconomicLevels("EconomicLevels");
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

        /// <summary>
        /// Test for loaded XML.
        /// </summary>
        [TestMethod]
        public void TestLoadedXml()
        {
            Assert.IsNotNull(economicLevels, "Economic levels was not loaded.");
            Assert.IsTrue(economicLevels.Count > 0, "List of economic levels is empty.");
            bool uniqueLevels = economicLevels.GroupBy(n => n.Level).Any(g => g.Count() > 1);

            Assert.IsFalse(uniqueLevels, "Levels are not unique.");

            foreach (EconomicLevel level in economicLevels)
            {
                Assert.IsTrue(level.Level >= 0, "Level is not greater or equal than 0.");
                Assert.IsTrue(level.UpgradeLevelQuantity >= 0, "Upgrade level quantity is not greater or equal than 0.");
                Assert.IsTrue(level.DowngradeLevelQuantity >= 0, "Downgrade level quantity is not greater or equal than 0.");
                Assert.IsTrue(level.DowngradeLevelQuantity >= level.UpgradeLevelQuantity, "Downgrade level is bigger or equal than upgrade level.");

                checkLevelItems(level.LevelItems);
            }
        }

        /// <summary>
        /// Method for test level items.
        /// </summary>
        /// <param name="levelItems"><level items/param>
        private void checkLevelItems(IList<EconomicLevelItem> levelItems)
        {
            Assert.IsNotNull(levelItems, "List of level items is empty.");
            Assert.IsTrue(levelItems.Count > 0, "List of level items is empty.");
            bool uniqueSequenceNumber = levelItems.GroupBy(n => n.SequenceNumber).Any(g => g.Count() > 1);

            Assert.IsFalse(uniqueSequenceNumber, "Sequence numbers are not unique.");
            Assert.IsTrue(levelItems.Last().IsDiscovered, "Last level item is not discovered.");

            foreach (EconomicLevelItem item in levelItems)
                Assert.IsTrue(item.SequenceNumber >= 0, "Sequence number is not greater or equal than 0.");
        }
    }
}
