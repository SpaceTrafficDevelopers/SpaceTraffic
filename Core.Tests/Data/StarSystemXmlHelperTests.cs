using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using SpaceTraffic.Game.Geometry;
using SpaceTraffic.Data;
using SpaceTraffic.Utils;
using System.Diagnostics;

namespace Core.Tests.Data
{
    /// <summary>
    /// Summary description for StarSystemXmlHelperTests
    /// </summary>
    [TestClass]
    public class StarSystemXmlHelperTests
    {
        public StarSystemXmlHelperTests()
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
        public void ParseTrajectoryTest_Elliptic()
        {
            //string[] trajectoryParams = { "velocity", "5", "direction", "clockwise" };
            string[] ellipseParams = {"velocity", "5", "direction", "clockwise", "x", "10", "y", "20", "a", "40", "b", "30", "initialAngle", "90", "angle", "100", "period", "5"};
            EllipticOrbit expectedOrbit = new EllipticOrbit(new Point2d(0, 0), 40, 30, 100, 5, Direction.CLOCKWISE, 90);
            XmlNode trajectoryNode = this.GenerateTrajectoryNode("ellipticOrbit", ellipseParams);

            Trajectory trajectory = trajectoryNode.ParseTrajectory();

            AssertEqualsEllipticOrbit(expectedOrbit, trajectory);
        }

        [TestMethod]
        public void ParseTrajectoryTest_Circular()
        {
            //string[] trajectoryParams = {"velocity", "5", "direction", "clockwise" };
            string[] circleParams = {"velocity", "5", "direction", "clockwise", "x", "10", "y", "20", "radius", "30", "initialAngle", "90", "period", "5"};

            CircularOrbit expectedOrbit = new CircularOrbit(30, 5, Direction.CLOCKWISE, 90);


            XmlNode trajectoryNode = this.GenerateTrajectoryNode("circularOrbit", circleParams);
            
            Debug.Assert(trajectoryNode != null, "Trajectory node is null");
            Trajectory trajectory = trajectoryNode.ParseTrajectory();

            AssertEqualsCircularOrbit(expectedOrbit, trajectory);
        }

        

        [TestMethod]
        public void ParseTrajectoryTest_OrbitClockwise()
        {
            //string[] trajectoryParams = { "velocity", "0", "direction", "clockwise" };
            string[] circleParams = { "velocity", "0", "direction", "clockwise", "x", "10", "y", "20", "radius", "30", "initialAngle", "90", "period", "5"};

            XmlNode trajectoryNode = this.GenerateTrajectoryNode( "circularOrbit", circleParams);

            Debug.Assert(trajectoryNode != null, "Trajectory node is null");

            Trajectory trajectory = trajectoryNode.ParseTrajectory();

            OrbitDefinition orbit = (OrbitDefinition)trajectory;

            Assert.AreEqual(Direction.CLOCKWISE, orbit.Direction, "Invalid orbit direction");
        }

        [TestMethod]
        public void ParseTrajectoryTest_OrbitCounterclockwise()
        {
            //string[] trajectoryParams = { "velocity", "5", "direction", "counterclockwise" };
            string[] circleParams = {"velocity", "5", "direction", "counterclockwise", "x", "10", "y", "20", "radius", "30", "initialAngle", "90", "period", "5"};

            XmlNode trajectoryNode = this.GenerateTrajectoryNode("circularOrbit", circleParams);

            Debug.Assert(trajectoryNode != null, "Trajectory node is null");

            Trajectory trajectory = trajectoryNode.ParseTrajectory();

            OrbitDefinition orbit = (OrbitDefinition)trajectory;

            Assert.AreEqual(Direction.COUNTERCLOCKWISE, orbit.Direction, "Invalid orbit direction");
        }

        [TestMethod]
        public void ParseTrajectoryTest_Stacionary()
        {
            //string[] trajectoryParams = { "velocity", "5", "direction", "clockwise" };
            string[] pointParams = {"velocity", "5", "direction", "clockwise", "x", "10", "y", "20" };

            Stacionary expected = new Stacionary(10, 20);

            XmlNode trajectoryNode = this.GenerateTrajectoryNode("stacionary", pointParams);

            Trajectory trajectory = trajectoryNode.ParseTrajectory();

            Assert.AreEqual(expected, trajectory);
        }

        #region Test data generation
        private XmlDocument GenerateXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        private XmlNode GenerateTrajectoryNode(string innerTag, string[] innerTagArgs)
        {
            XmlTestDataGenerator xmlGen = new XmlTestDataGenerator();
            xmlGen.AppendOpeningTag("trajectory");
            xmlGen.AppendTag(innerTag, innerTagArgs);
            xmlGen.AppendClosingTag("trajectory");
            Debug.WriteLine("Xml:" + xmlGen.ToString());
            XmlDocument xmlDoc = GenerateXml(xmlGen.ToString());
            return xmlDoc.GetElementsByTagName("trajectory")[0];
        }
        #endregion

        #region Complex assertions
        private void AssertEqualsCircularOrbit(CircularOrbit expected, object actual)
        {
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(CircularOrbit));

            CircularOrbit circularOrbit = (CircularOrbit)actual;

            Assert.AreEqual(expected.PeriodInSec, circularOrbit.PeriodInSec);
            Assert.AreEqual(expected.Direction, circularOrbit.Direction, "Invalid orbit direction");
            Assert.AreEqual(expected.Radius, circularOrbit.Radius);
            Assert.AreEqual(expected.InitialAngleRad, circularOrbit.InitialAngleRad);
        }

        private void AssertEqualsEllipticOrbit(EllipticOrbit expected, object actual)
        {
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(EllipticOrbit));

            EllipticOrbit ellipticOrbit = (EllipticOrbit)actual;

            Assert.AreEqual(5d, ellipticOrbit.PeriodInSec);
            Assert.AreEqual(Direction.CLOCKWISE, ellipticOrbit.Direction, "Invalid orbit direction");
            Assert.AreEqual(expected.Barycenter.X, ellipticOrbit.Barycenter.X);
            Assert.AreEqual(expected.Barycenter.Y, ellipticOrbit.Barycenter.Y);
            Assert.AreEqual(expected.A, ellipticOrbit.A);
            Assert.AreEqual(expected.B, ellipticOrbit.B);
            Assert.AreEqual(expected.InitialAngleRad, ellipticOrbit.InitialAngleRad);
            Assert.AreEqual(expected.RotationAngleInRad, ellipticOrbit.RotationAngleInRad);
        }
        #endregion
    }
}
