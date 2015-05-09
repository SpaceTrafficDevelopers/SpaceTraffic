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
using SpaceTraffic.Game.Geometry;

namespace StarSystemEditor.Tests
{
    [TestClass]
    public class CircleEditorEntityTest
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
            Editor.CircleOrbitEditor = new CircleEditorEntity();
            Editor.IsLoaded = false;
            Editor.Time = 0;
            Editor.LoadGalaxy("GalaxyMap2", ".//..//..//..//StarSystemEditor.Tests//Assets");
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Editor.CircleOrbitEditor = new CircleEditorEntity();
        }

        [TestMethod]
        public void EditorLoadCircleOrbitTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 3"];
            Editor.CircleOrbitEditor.LoadObject(planet.Trajectory);

            Assert.IsNotNull(Editor.CircleOrbitEditor.LoadedObject, "loading object failed");
        }

        [TestMethod]
        public void EditorSetRadiusTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 3"];
            Editor.CircleOrbitEditor.LoadObject(planet.Trajectory);
            Editor.CircleOrbitEditor.SetRadius(100);

            Assert.AreEqual(100, ((CircularOrbit)planet.Trajectory).Radius);
        }

        [TestMethod]
        public void EditorCirclePreviewSetWidthTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 3"];
            Editor.CircleOrbitEditor.LoadObject(planet.Trajectory);
            Editor.CircleOrbitEditor.PreviewSetWidth(60);

            Assert.AreEqual(60, ((EllipticOrbit)Editor.CircleOrbitEditor.LoadedObject).A);
        }

        [TestMethod]
        public void EditorCircleSetWidthTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 3"];
            Editor.CircleOrbitEditor.LoadObject(planet.Trajectory);
            Editor.CircleOrbitEditor.SetWidth(60);

            Assert.AreEqual(60, ((EllipticOrbit)Editor.CircleOrbitEditor.LoadedObject).A);
        }

        [TestMethod]
        public void EditorCirclePreviewSetHeightTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 3"];
            Editor.CircleOrbitEditor.LoadObject(planet.Trajectory);
            Editor.CircleOrbitEditor.PreviewSetHeight(51);

            Assert.AreEqual(51, ((EllipticOrbit)Editor.CircleOrbitEditor.LoadedObject).B);
        }

        [TestMethod]
        public void EditorCircleSetHeightTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 3"];
            Editor.CircleOrbitEditor.LoadObject(planet.Trajectory);
            Editor.CircleOrbitEditor.SetHeight(50);

            Assert.AreEqual(50, ((EllipticOrbit)Editor.CircleOrbitEditor.LoadedObject).B);
        }

    }
}
