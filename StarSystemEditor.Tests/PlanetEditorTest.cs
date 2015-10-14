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
    public class PlanetEditorTest
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
            Editor.PlanetEditor = new PlanetEditorEntity();
            Editor.IsLoaded = false;
            Editor.Time = 0;
            Editor.LoadGalaxy("GalaxyMap2", ".//..//..//..//StarSystemEditor.Tests//Assets");
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Editor.PlanetEditor = new PlanetEditorEntity();
        }

        [TestMethod]
        public void EditorLoadPlanetTest()
        {
            Planet planet = Editor.GalaxyMap["Vitera"].Planets["Vitera 1"];
            Editor.PlanetEditor.LoadObject(planet);

            Assert.IsNotNull(Editor.PlanetEditor.LoadedObject, "loading object failed");
        }

        [TestMethod]
        public void EditorSetPlanetNameTest()
        {
            Planet planet = Editor.GalaxyMap["Vitera"].Planets["Vitera 1"];
            Editor.PlanetEditor.LoadObject(planet);
            Editor.PlanetEditor.SetName("TestName");

            Assert.AreEqual("TestName", planet.Name);
        }
        
        [TestMethod]
        public void EditorGetPlanetInfoTest()
        {
            Planet planet = Editor.GalaxyMap["Vitera"].Planets["Vitera 1"];
            Editor.PlanetEditor.LoadObject(planet);
            string info = Editor.PlanetEditor.GetInfo();

            Assert.AreEqual("Planet: Vitera 1, StarSystem: Vitera, Trajectory: SpaceTraffic.Game.Geometry.EllipticOrbit"
                , info);
        }
        
    }
}
