using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Geometry
{
    public abstract class OrbitDefinition
    {

        #region Properties
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
        #endregion
    }
}
