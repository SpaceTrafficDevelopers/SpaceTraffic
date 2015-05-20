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
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SpaceTraffic.Data;
using SpaceTraffic.Game;

namespace Core.Tests
{
    /// <summary>
    /// Summary description for GalaxyMapLoaderTests
    /// </summary>
    [TestClass]
    public class GalaxyMapLoaderTests
    {
        public GalaxyMapLoaderTests()
        {
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private GalaxyMap map = null;

        [TestMethod]
        public void GalaxyLoadTest()
        {
            TestGalaxyMapDataStreamProvider provider = new TestGalaxyMapDataStreamProvider(".//..//..//assets");
            provider.Initialize();
            GalaxyMapLoader loader = new GalaxyMapLoader();
            map = loader.LoadGalaxyMap("GalaxyMap", provider);
            Debug.Assert((map.Count > 0), "No starsystem was loaded.");
            Debug.Assert((map.GetStarSystemConnections("Solar System").Count > 0), "Starsystem connections cannot be loaded.");
        }
    }
}
