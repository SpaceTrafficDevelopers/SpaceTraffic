using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceTraffic.Tools.StarSystemEditor;
using SpaceTraffic.Game;
using System.Diagnostics;
using SpaceTraffic.Tools.StarSystemEditor.Data;
using SpaceTraffic.Tools.StarSystemEditor.Presentation;
using SpaceTraffic.Tools.StarSystemEditor.Entities;

namespace StarSystemEditor.Tests
{
    [TestClass]
    public class StarSystemEditorTest
    {

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
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Editor.Names = new Names(".//..//..//..//StarSystemEditor.Tests//Assets//names.txt");
            Editor.GalaxyMap = new GalaxyMap();
            Editor.dataPresenter = new DataPresenter();
            //create instance of all editors
            Editor.StarSystemEditor = new StarSystemEditorEntity();
            Editor.PlanetEditor = new PlanetEditorEntity();
            Editor.StarEditor = new StarEditorEntity();
            Editor.WormholeEditor = new WormholeEditorEntity();
            Editor.CircleOrbitEditor = new CircleEditorEntity();
            Editor.EllipseOrbitEditor = new EllipseEditorEntity();
            Editor.IsLoaded = false;
            Editor.Time = 0;
            Editor.LoadGalaxy("GalaxyMap2", ".//..//..//..//StarSystemEditor.Tests//Assets");
        }
        
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Editor.FlushEditors();
        }

        [TestMethod]
        public void EditorGalaxyLoadTest()
        {
            GalaxyMap map = Editor.GalaxyMap;

            Assert.IsNotNull(map, "No galaxy map loaded.");
            Debug.Assert((map.Count > 0), "No starsystem was loaded.");
        }
    }
}
