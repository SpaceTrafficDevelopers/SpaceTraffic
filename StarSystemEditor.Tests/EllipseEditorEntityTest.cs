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
    public class EllipseEditorEntityTest
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
            Editor.EllipseOrbitEditor = new EllipseEditorEntity();
            Editor.IsLoaded = false;
            Editor.Time = 0;
            Editor.LoadGalaxy("GalaxyMap2", ".//..//..//..//StarSystemEditor.Tests//Assets");
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Editor.EllipseOrbitEditor = new EllipseEditorEntity();
        }

        [TestMethod]
        public void EditorLoadEllipseOrbitTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 4"];
            Editor.EllipseOrbitEditor.LoadObject(planet.Trajectory);

            Assert.IsNotNull(Editor.EllipseOrbitEditor.LoadedObject, "loading object failed");
        }

        [TestMethod]
        public void EditorSetRotationAngleTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 4"];
            Editor.EllipseOrbitEditor.LoadObject(planet.Trajectory);
            Editor.EllipseOrbitEditor.SetRotationAngleInRad(Math.PI);

            Assert.AreEqual(Math.PI, ((EllipticOrbit)Editor.CircleOrbitEditor.LoadedObject).RotationAngleInRad);
        }

        [TestMethod]
        public void EditorPreviewSetWidthTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 4"];
            Editor.EllipseOrbitEditor.LoadObject(planet.Trajectory);
            Editor.EllipseOrbitEditor.PreviewSetWidth(60);

            Assert.AreEqual(60, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).A);
        }

        [TestMethod]
        public void EditorSetWidthTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 4"];
            Editor.EllipseOrbitEditor.LoadObject(planet.Trajectory);
            Editor.EllipseOrbitEditor.SetWidth(60);

            Assert.AreEqual(60, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).A);
            Assert.AreEqual(21.540659228538019, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).Cx);
            Assert.AreEqual(0.35901098714230029, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).OrbitalEccentricity);
            Assert.AreEqual(52.266666666666666, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).SemiLatusRectum);
            Assert.AreEqual(1.4560832005096076, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).Sqrt1PlusESlash1MinusE);
        }

        [TestMethod]
        public void EditorPreviewSetHeightTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 4"];
            Editor.EllipseOrbitEditor.LoadObject(planet.Trajectory);
            Editor.EllipseOrbitEditor.PreviewSetHeight(51);

            Assert.AreEqual(51, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).B);
        }

        [TestMethod]
        public void EditorSetHeightTest()
        {
            Planet planet = Editor.GalaxyMap["Solar system"].Planets["Sol 4"];
            Editor.EllipseOrbitEditor.LoadObject(planet.Trajectory);
            Editor.EllipseOrbitEditor.SetHeight(50);

            Assert.AreEqual(50, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).B);
            Assert.AreEqual(27.367864366808018, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).Cx);
            Assert.AreEqual(0.48013797134750907, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).OrbitalEccentricity);
            Assert.AreEqual(43.859649122807014, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).SemiLatusRectum);
            Assert.AreEqual(1.6873572873361604, ((EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject).Sqrt1PlusESlash1MinusE);
        }

    }
}
