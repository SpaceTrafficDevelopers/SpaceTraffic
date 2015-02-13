using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SpaceTraffic.Data;
using SpaceTraffic.Game;


namespace Core.Tests
{   
    /// <summary>
    /// Summary description for StarSystemLoaderTests
    /// </summary>
    [TestClass]
    public class StarSystemLoaderTests
    {
        public StarSystemLoaderTests()
        {
            //
            // TODO: Add constructor logic here
            //
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

        [TestMethod]
        public void StarSystemLoadTest()
        {
            TestGalaxyMapDataStreamProvider provider = new TestGalaxyMapDataStreamProvider(".//..//..//..//Assets");
            provider.Initialize();
            StarSystemLoader loader = new StarSystemLoader();
            StarSystem loadedSS = loader.LoadStarSystem("Sol", provider);
            Debug.Assert((loadedSS != null), "Starsystem load failed!");
            Debug.Assert((loadedSS.Name.Equals("Solar system", StringComparison.CurrentCultureIgnoreCase)), "Solar system load failed!");
            Debug.Assert((loadedSS.Star != null), "Solar system star load failed!");
            Debug.Assert((loadedSS.Planets.Count > 0), "Solar system planets load failed!");
            Debug.Assert((loadedSS.WormholeEndpoints.Count > 0), "Solar system wormholeendpoints load failed!");
            Debug.Assert((loadedSS.WormholeEndpoints.Count <= 6), "Solar system contains more then 6 allowed wormoleendpoints!");
        }
    }
}
