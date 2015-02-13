using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Geometry
{
    public abstract class Orbit : Trajectory
    {

        #region Properties
        /// <summary>
        /// Initial speed of an object.
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public double Velocity { get;  set; }
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the period in seconds.
        /// </summary>
        /// <value>
        /// The period in seconds.
        /// </value>
        public double PeriodInSec { get; set; }

        /// <summary>
        /// Initial angle of an object in radians.
        /// </summary>
        /// <value>
        /// The initial angle.
        /// </value>
        public double InitialAngleRad { get; set; }

        /// <summary>
        /// Gets or sets the barycenter.
        /// The barycenter is the point between two objects where they balance each other.
        /// For our model of star system the mass of planets is negligible.
        /// </summary>
        /// <value>
        /// The barycenter.
        /// </value>
        public Point2d Barycenter { get; set; }

        /// <summary>
        /// Gets or sets the orbital eccentricity.
        /// </summary>
        /// <value>
        /// The orbital eccentricity.
        /// </value>
        public double OrbitalEccentricity { get; set; }
        #endregion

        /// <summary>
        /// Calculates the position of an object in given time.
        /// </summary>
        /// <param name="timeInSec">The time in sec.</param>
        /// <returns>
        /// The position of an object on the trajectory.
        /// </returns>
        public abstract Point2d CalculatePosition(double timeInSec);

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
