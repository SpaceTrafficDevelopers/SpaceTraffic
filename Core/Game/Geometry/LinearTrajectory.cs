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
    /// Represents general linear trajectory of an object, defined by its start and end point
    /// and initial velocity of the movement.
    /// </summary>
    public class LinearTrajectory : OpenTrajectory
    {
        protected double TravelTime;

        /// <summary>
        /// Initializes a new insance of the <see cref="LinearTrajectory"/> class.
        /// </summary>
        /// <param name="startPoint">Start point of a linear trajectory.</param>
        /// <param name="endPoint">End point of a linear trajectory.</param>
        public LinearTrajectory(Point2d startPoint, Point2d endPoint) 
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }
        /// <summary>
        /// Calculates the position of an object in given time.
        /// </summary>
        /// <param name="timeInSec">The time in sec.</param>
        /// <returns>
        /// The position of an object on the trajectory.
        /// </returns>
        public override Point2d CalculatePosition(double timeInSec) 
        {
            double ordinateX = StartPoint.X - EndPoint.X;
            double ordinateY = StartPoint.Y - EndPoint.Y;
            double length = Math.Sqrt(ordinateX * ordinateX + ordinateY * ordinateY);
            this.TravelTime = length / this.Velocity;

            double directiveX = (EndPoint.X - StartPoint.X) / this.TravelTime;
            double directiveY = (EndPoint.Y - StartPoint.Y) / this.TravelTime;

            Point2d point = new Point2d();
            point.X = StartPoint.X + directiveX * timeInSec;
            point.Y = StartPoint.Y + directiveY * timeInSec;

            return point;
        }
    }
}
