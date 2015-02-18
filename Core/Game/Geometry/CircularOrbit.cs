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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SpaceTraffic.Utils;

namespace SpaceTraffic.Game.Geometry
{
    /// <summary>
    /// Represents circular orbit for space objects.
    /// </summary>
    public class CircularOrbit : OrbitDefinition, Trajectory
    {
        #region Properties
        /// <summary>
        /// Radius of circular trajectory
        /// </summary>
        public int Radius { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularOrbit"/> class.
        /// </summary>
        /// <param name="radius">The radius of circle.</param>
        /// <param name="periodInSec">The period of an object.</param>
        /// <param name="direction">The direction of the movement.</param>
        /// <param name="initialAngleDeg">The initial angle in degree.</param>
        public CircularOrbit(int radius, int periodInSec, Direction direction, double initialAngleDeg)
        {
            Debug.Assert((periodInSec > 0), "Period is less or equal to 0");
            this.Radius = radius;
            this.PeriodInSec = periodInSec;
            this.Direction = direction;
            this.InitialAngleRad = MathUtil.DegreeToRadian(initialAngleDeg);
        }
        #endregion

        /// <summary>
        /// Calculate position in given time.
        /// </summary>
        /// <param name="timeInSec">Given time for calculate the position.</param>
        /// <returns>Position of object in given time.</returns>
        public Point2d CalculatePosition(double timeInSec)
        {
            //double realTime = timeInSec % this.PeriodInSec;
            timeInSec *= (double)this.Direction;
            double timePerRad = this.PeriodInSec / (MathUtil.TWO_PI);
            double realAngle = timeInSec / timePerRad;
            double xPos = this.Radius * Math.Cos(this.InitialAngleRad + realAngle);
            double yPos = this.Radius * Math.Sin(this.InitialAngleRad + realAngle);
            return new Point2d(xPos, yPos);
        }
    }
}
