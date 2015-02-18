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

namespace SpaceTraffic.Game.Geometry
{
    /// <summary>
    /// 
    /// </summary>
    public class LensTrajectory : LinearTrajectory
    {

        /// <summary>
        /// Gets or sets radius of block zone around star.
        /// </summary>
        /// <value>
        /// Radius of block zone.
        /// </value>
        private double BlockZone { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LensTrajectory"/> class.
        /// </summary>
        /// <param name="start">Start point of a trajectory.</param>
        /// <param name="end">End point of a trajectory.</param>
        /// <param name="blockZone">Block zone around a star.</param>
        public LensTrajectory(Point2d start, Point2d end, double blockZone) : base(start, end)
        {
            this.BlockZone = blockZone;
        }
        #endregion

        /// <summary>
        /// Applies the lens effect on the linear trajectory.
        /// </summary>
        /// <param name="originalPoint">Original point of trajectory.</param>
        /// <returns>Point of trajectory after lens effect.</returns>
        public Point2d addLensEffect(Point2d originalPoint)
        {
            double centerToPointDist = Math.Sqrt(originalPoint.X * originalPoint.X + originalPoint.Y * originalPoint.Y);
            double tempX = originalPoint.X / centerToPointDist;
            double tempY = originalPoint.Y / centerToPointDist;

            StartPoint.toString();

            double relativeRadius = Math.Sin(Math.PI * centerToPointDist / this.BlockZone);

            double transformedX = tempX * relativeRadius;
            double transformedY = tempY * relativeRadius;

            return new Point2d(originalPoint.X + transformedX, originalPoint.Y + transformedY);
        }

        /// <summary>
        /// Calculates position of ship entered time.
        /// </summary>
        /// <param name="timeInSec">Time in seconds.</param>
        /// <returns>Position of ship on transformed trajectory.</returns>
        public override Point2d CalculatePosition(double timeInSec)
        {
            double lengthX = Math.Abs(this.StartPoint.X - this.EndPoint.X);
            double lengthY = Math.Abs(this.StartPoint.Y - this.EndPoint.Y);

            double positionX = StartPoint.X + lengthX * (timeInSec / this.TravelTime);
            double positionY = StartPoint.Y + lengthY * (timeInSec / this.TravelTime);

            Point2d pointOnLine = new Point2d(positionX, positionY);

            return addLensEffect(pointOnLine);
        }

    }
}
