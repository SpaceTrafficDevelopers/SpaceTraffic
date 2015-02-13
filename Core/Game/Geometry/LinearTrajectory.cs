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
