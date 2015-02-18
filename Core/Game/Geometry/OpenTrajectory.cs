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
    /// Represents general open trajectory of an object, defined by its start and end point
    /// and initial velocity of the movement.
    /// </summary>
    public abstract class OpenTrajectory : Trajectory
    {
        #region Properties
        /// <summary>
        /// Initial speed of an object.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public double Velocity { get; set; }

        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        /// <value>
        /// The start point.
        /// </value>
        public Point2d StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public Point2d EndPoint { get; set; }
        #endregion


        /// <summary>
        /// Calculates the position of an object in given time.
        /// </summary>
        /// <param name="timeInSec">The time in sec.</param>
        /// <returns>
        /// The position of an object on the trajectory.
        /// </returns>
        public abstract Point2d CalculatePosition(double timeInSec);
    }
}
