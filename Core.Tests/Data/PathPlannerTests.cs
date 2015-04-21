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
using System.IO;
using System.Diagnostics;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SpaceTraffic.Data;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Game.Geometry;


namespace Core.Tests
{   


    /// <summary>
    /// Summary description for StarSystemLoaderTests
    /// </summary>
    [TestClass]
    public class PathPlannerTests
    {
        private static Spaceship testShip = null;
        private static NavPath testPath = null;
        private static GalaxyMap map = null;

        /// <summary>
        ///Expected coordinates of test path 1 and spedd 10.
        ///</summary>
        static Point2d[] pointsPath1_speed10 = new Point2d[] 
        {
            new Point2d(0, -88),
            new Point2d(132.815, 157.464), // !
            new Point2d(134.893, 159.64),
            new Point2d(129.89, 153.39),
            new Point2d(140.966, 168.904),
            new Point2d(-34.54, 41.51),
            new Point2d(133.877, 161.793),
            new Point2d(198.193, 33.476),
            new Point2d(214.468, 39.082),
            new Point2d(127.366, 154.201),
            new Point2d(-20.687, -50.961),
        };

        /// <summary>
        ///Expected coordinates of test path 2 and speed 3.
        ///</summary>
        static Point2d[] pointsPath2_speed3 = new Point2d[] 
        {
            new Point2d(-30.228, 33.571),
            new Point2d(129.244, 156.54),
            new Point2d(159.014, 187.698),
            new Point2d(131.23, 152.248),
            new Point2d(140.13, 169.598),
            new Point2d(53.575, 25.422),
            new Point2d(131.996, 163.331),
            new Point2d(198.585, 31.064),
            new Point2d(225.943, 43.011),
            new Point2d(129.842, 159.929),
            new Point2d(63.307, 24.053),
        };

        /// <summary>
        ///Expected coordinates of test path 2 and speed 150.
        ///</summary>
        static Point2d[] pointsPath2_speed150 = new Point2d[] 
        {
            new Point2d(-30.228, 33.571),
            new Point2d(130.461, 155.528),
            new Point2d(158.144, 188.432),
            new Point2d(129.241, 153.941),
            new Point2d(141.388, 168.551),
            new Point2d(-37.091, -19.537),
        };

        /// <summary>
        ///Expected times of arrival of test path 1 and speed 10.
        ///</summary>
        static double[] timesPath1_speed10 = new double[] {
            0, 27.91, 27.91, 27.91, 28.711, 50.398, 71.089, 71.089, 72.81, 72.81, 96.27
        };

        /// <summary>
        ///Expected times of arrival of test path 2 and speed 3.
        ///</summary>
        static double[] timesPath2_speed3 = new double[] {
            0, 67.126, 67.126, 82.139, 82.139, 138.193, 191.075, 191.075, 201.026, 201.026, 251.457
        };

        /// <summary>
        ///Expected times of arrival of test path 2 and speed 150.
        ///</summary>
        static double[] timesPath2_speed150 = new double[] {
            0, 1.345, 1.345, 1.645, 1.645, 3.734
        };

        public PathPlannerTests()
        {
            testShip = new Spaceship(0, "TestShip");
            testPath = new NavPath();
            TestGalaxyMapDataStreamProvider provider = new TestGalaxyMapDataStreamProvider(".//..//..//..//..//assets");
            provider.Initialize();
            GalaxyMapLoader loader = new GalaxyMapLoader();
            map = loader.LoadGalaxyMap("GalaxyMap", provider);
            Debug.Assert((map.Count > 0), "No starsystem was loaded.");
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

        /// <summary>
        /// Tests path for the specified parameters.
        ///</summary>
        /// <param name="shioSpeed">Speed of ship.</param>
        /// <param name="startTime">Start time of path.</param>
        /// <param name="testPositions">The coordinates for compare with calculated values.</param>
        /// <param name="testTimes">The times for compare with calculated values.</param>
        private void pathTesting(int shipSpeed, double startTime, Point2d[] testPositions, double[] testTimes) 
        {
            String testPathPlanerPositions = "An error in the navigation point, ";
            testShip.MaxSpeed = shipSpeed;

            PathPlanner.SolvePath(testPath, testShip, 0.0);

            bool check = false;
            double epsXY = 5.0; // 5 pixels tolerance
            double epsTime = 2.0; // 2 second tolerance, rounding error
            double diffTime = 0.0; // previous calculation error
            Debug.Print("Test for ship speed: " + shipSpeed);
            for (int i = 0; i < testPath.Count; i++)
            {
                double timeOfArrival = testPath[i].TimeOfArrival.TimeOfDay.TotalSeconds;
                Point2d position = testPath[i].Location.Trajectory.CalculatePosition(timeOfArrival);

                Debug.Print("[" + i + "] x: " + testPositions[i].X + ", y: " + testPositions[i].Y + ", time: " + timeOfArrival);

                check = Math.Abs(testTimes[i] - timeOfArrival - diffTime) < epsTime;
                Debug.Assert(check, testPathPlanerPositions + "time of arrival is wrong. Calculated: " + (timeOfArrival + diffTime) + ", expected: " + testTimes[i] + " (point " + i + ")");
                diffTime = testTimes[i] - timeOfArrival;

                check = Math.Abs(testPositions[i].X - position.X) < epsXY;
                Debug.Assert(check, testPathPlanerPositions + "X position is wrong. Calculated: " + position.X + ", expected: " + testPositions[i].X + " (point " + i + ")");

                check = Math.Abs(testPositions[i].Y - position.Y) < epsXY;
                Debug.Assert(check, testPathPlanerPositions + "Y position is wrong. Calculated: " + position.Y + ", expected: " + testPositions[i].Y + " (point " + i + ")");
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
        public void PathPlannerTest_testPath1_speed10()
        {
            testPath.Clear();
            testPath.Add(new NavPoint(map["Tertius"].Planets["Tertius 2"]));
            testPath.Add(new NavPoint(map["Tertius"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 3"]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[2]));
            testPath.Add(new NavPoint(map["Solas"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solas"].Planets["Solas 2"]));

            pathTesting(10, 0.0, pointsPath1_speed10, timesPath1_speed10); 
        }
            

        [TestMethod]
        public void PathPlannerTest_testPath2_speed3()
        {
            testPath.Clear();
            testPath.Add(new NavPoint(map["Granari"].Planets["Granari 1"]));
            testPath.Add(new NavPoint(map["Granari"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[3]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 1"]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[3]));
            testPath.Add(new NavPoint(map["Rurawua"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Rurawua"].Planets["Rurawua 2"]));

            pathTesting(3, 0.0, pointsPath2_speed3, timesPath2_speed3);
        }

        [TestMethod]
        public void PathPlannerTest_testPath2_speed150()
        {
            testPath.Clear();
            testPath.Add(new NavPoint(map["Granari"].Planets["Granari 1"]));
            testPath.Add(new NavPoint(map["Granari"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[3]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 1"]));


            pathTesting(150, 0.0, pointsPath2_speed150, timesPath2_speed150);
        }

        [TestMethod]
        public void PathPlannerTest_calculationDuration()
        {
            // 5O points
            testPath.Clear();
            testPath.Add(new NavPoint(map["Tertius"].Planets["Tertius 2"]));
            testPath.Add(new NavPoint(map["Tertius"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 3"]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[2]));
            testPath.Add(new NavPoint(map["Solas"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solas"].Planets["Solas 2"]));
            // back
            testPath.Add(new NavPoint(map["Solas"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[2]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 3"]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Tertius"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Tertius"].Planets["Tertius 2"]));
            // back
            testPath.Add(new NavPoint(map["Tertius"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 3"]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[2]));
            testPath.Add(new NavPoint(map["Solas"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solas"].Planets["Solas 2"]));
            // back
            testPath.Add(new NavPoint(map["Solas"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[2]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 3"]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Tertius"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Tertius"].Planets["Tertius 2"]));
            // back
            testPath.Add(new NavPoint(map["Tertius"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Atabea"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[1]));
            testPath.Add(new NavPoint(map["Solar system"].Planets["Sol 3"]));
            testPath.Add(new NavPoint(map["Solar system"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Proxima Centauri"].WormholeEndpointsList[2]));
            testPath.Add(new NavPoint(map["Solas"].WormholeEndpointsList[0]));
            testPath.Add(new NavPoint(map["Solas"].Planets["Solas 2"]));

            testShip.MaxSpeed = 10;

            Stopwatch stopWatch = Stopwatch.StartNew();
            PathPlanner.SolvePath(testPath, testShip, 0.0);
            stopWatch.Stop();

            double maxDuration = 10.0;
            bool check = (stopWatch.ElapsedMilliseconds < maxDuration);
            Debug.Print("Time for 50 point: " + stopWatch.ElapsedMilliseconds + " ms");
            Debug.Assert(check, "The calculation of navigation path takes too long. (" + stopWatch.ElapsedMilliseconds + "ms)");           
        }
    }
}
