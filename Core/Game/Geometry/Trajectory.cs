using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Data;
using SpaceTraffic.Game;

namespace SpaceTraffic.Game.Geometry
{
    /// <summary>
    /// Trajectory of an object.
    /// </summary>s
    public interface Trajectory
    {
        
        /// <summary>
        /// Calculates the position of an object in given time.
        /// </summary>
        /// <param name="timeInSec">The time in sec.</param>
        /// <returns>
        /// The position of an object on the trajectory.
        /// </returns>
        Point2d CalculatePosition(double timeInSec);

        string ToString();
    }
}
