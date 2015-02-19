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
using SpaceTraffic.Game.Geometry;

namespace Core.Tests.Data
{
    [TestClass]
    public class OrbitTests
    {
        [TestMethod]
        public void EllipticOrbitTest()
        {
            int period = 150;
            bool check = false;
            int accuracy = 5;
            String testOrbitDefinition = "Rotated elliptic CCW orbit with initial angle";
            EllipticOrbit testOrbit = new EllipticOrbit(new Point2d(0, 0), 50, 40, 30, period, Direction.COUNTERCLOCKWISE, 54);
            Point2d orbitIn0 = this.RoundCoords(testOrbit.CalculatePosition(0), accuracy);
            Point2d orbitInT = this.RoundCoords(testOrbit.CalculatePosition(period), accuracy);
            Point2d orbitIn2T = this.RoundCoords(testOrbit.CalculatePosition(2*period), accuracy);
            Point2d orbitIn5T = this.RoundCoords(testOrbit.CalculatePosition(5*period), accuracy);
            Point2d orbitIn10T = this.RoundCoords(testOrbit.CalculatePosition(10*period), accuracy);

            check = Debug.Equals(orbitIn0, orbitInT);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and T");
            check = Debug.Equals(orbitIn0, orbitIn2T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 2*T");
            check = Debug.Equals(orbitIn0, orbitIn5T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 5*T");
            check = Debug.Equals(orbitIn0, orbitIn10T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 10*T");
        }

        public void EllipticOrbitRotationTest()
        {
            int period = 150;
            bool check = false;
            int accuracy = 5;
            EllipticOrbit testOrbit = new EllipticOrbit(new Point2d(0, 0), 50, 40, 30, period, Direction.COUNTERCLOCKWISE, 54);
            EllipticOrbit testOrbit2 = new EllipticOrbit(new Point2d(0, 0), 50, 40, 390, period, Direction.COUNTERCLOCKWISE, 54);
            Point2d orbitIn0 = this.RoundCoords(testOrbit.CalculatePosition(0), accuracy);
            Point2d orbit2In0 = this.RoundCoords(testOrbit2.CalculatePosition(0), accuracy);

            check = Debug.Equals(orbitIn0, orbit2In0);
            Debug.Assert(check, "Elliptic rotation test failed!");
        }

        [TestMethod]
        public void EllipticOrbit2Test()
        {
            int period = 150;
            bool check = false;
            int accuracy = 5;
            String testOrbitDefinition = "Elliptic CW orbit";
            EllipticOrbit testOrbit = new EllipticOrbit(new Point2d(0, 0), 50, 40, 0, period, Direction.CLOCKWISE, 0);
            Point2d orbitIn0 = this.RoundCoords(testOrbit.CalculatePosition(0), accuracy);
            Point2d orbitInT = this.RoundCoords(testOrbit.CalculatePosition(period), accuracy);
            Point2d orbitIn2T = this.RoundCoords(testOrbit.CalculatePosition(2 * period), accuracy);
            Point2d orbitIn5T = this.RoundCoords(testOrbit.CalculatePosition(5 * period), accuracy);
            Point2d orbitIn10T = this.RoundCoords(testOrbit.CalculatePosition(10 * period), accuracy);

            check = Debug.Equals(orbitIn0, orbitInT);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and T");
            check = Debug.Equals(orbitIn0, orbitIn2T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 2*T");
            check = Debug.Equals(orbitIn0, orbitIn5T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 5*T");
            check = Debug.Equals(orbitIn0, orbitIn10T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 10*T");
        }

        [TestMethod]
        public void EllipticOrbit3Test()
        {
            int period = 150;
            bool check = false;
            int accuracy = 5;
            String testOrbitDefinition = "Elliptic CW orbit with initial angle";
            EllipticOrbit testOrbit = new EllipticOrbit(new Point2d(0, 0), 50, 40, 0, period, Direction.CLOCKWISE, 55);
            Point2d orbitIn0 = this.RoundCoords(testOrbit.CalculatePosition(0), accuracy);
            Point2d orbitInT = this.RoundCoords(testOrbit.CalculatePosition(period), accuracy);
            Point2d orbitIn2T = this.RoundCoords(testOrbit.CalculatePosition(2 * period), accuracy);
            Point2d orbitIn5T = this.RoundCoords(testOrbit.CalculatePosition(5 * period), accuracy);
            Point2d orbitIn10T = this.RoundCoords(testOrbit.CalculatePosition(10 * period), accuracy);

            check = Debug.Equals(orbitIn0, orbitInT);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and T");
            check = Debug.Equals(orbitIn0, orbitIn2T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 2*T");
            check = Debug.Equals(orbitIn0, orbitIn5T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 5*T");
            check = Debug.Equals(orbitIn0, orbitIn10T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 10*T");
        }

        [TestMethod]
        public void CircularOrbitTest()
        {
            int period = 150;
            bool check = false;
            int accuracy = 5;
            String testOrbitDefinition = "Circular CW orbit with initial angle";
            CircularOrbit testOrbit = new CircularOrbit(50, period, Direction.CLOCKWISE, 45);
            Point2d orbitIn0 = this.RoundCoords(testOrbit.CalculatePosition(0), accuracy);
            Point2d orbitInT = this.RoundCoords(testOrbit.CalculatePosition(period), accuracy);
            Point2d orbitIn2T = this.RoundCoords(testOrbit.CalculatePosition(2 * period), accuracy);
            Point2d orbitIn5T = this.RoundCoords(testOrbit.CalculatePosition(5 * period), accuracy);
            Point2d orbitIn10T = this.RoundCoords(testOrbit.CalculatePosition(10 * period), accuracy);

            check = Debug.Equals(orbitIn0, orbitInT);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and T");
            check = Debug.Equals(orbitIn0, orbitIn2T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 2*T");
            check = Debug.Equals(orbitIn0, orbitIn5T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 5*T");
            check = Debug.Equals(orbitIn0, orbitIn10T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 10*T");
        }

        [TestMethod]
        public void CircularOrbit2Test()
        {
            int period = 150;
            bool check = false;
            int accuracy = 5;
            String testOrbitDefinition = "Circular CCW orbit";
            CircularOrbit testOrbit = new CircularOrbit(50, period, Direction.COUNTERCLOCKWISE, 0);
            Point2d orbitIn0 = this.RoundCoords(testOrbit.CalculatePosition(0), accuracy);
            Point2d orbitInT = this.RoundCoords(testOrbit.CalculatePosition(period), accuracy);
            Point2d orbitIn2T = this.RoundCoords(testOrbit.CalculatePosition(2 * period), accuracy);
            Point2d orbitIn5T = this.RoundCoords(testOrbit.CalculatePosition(5 * period), accuracy);
            Point2d orbitIn10T = this.RoundCoords(testOrbit.CalculatePosition(10 * period), accuracy);

            check = Debug.Equals(orbitIn0, orbitInT);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and T");
            check = Debug.Equals(orbitIn0, orbitIn2T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 2*T");
            check = Debug.Equals(orbitIn0, orbitIn5T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 5*T");
            check = Debug.Equals(orbitIn0, orbitIn10T);
            Debug.Assert(check, testOrbitDefinition + " calculated position isn't same in 0 and 10*T");
        }

        private Point2d RoundCoords(Point2d coord, int accuracy) 
        {
            coord.X = Math.Round(coord.X, accuracy);
            coord.Y = Math.Round(coord.Y, accuracy);
            return coord;
        }
    }
}
