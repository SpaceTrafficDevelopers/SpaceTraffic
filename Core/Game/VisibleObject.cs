using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Game
{
    public abstract class VisibleObject
    {
        #region Properties

        /// <summary>
        /// Star system where the object belongs to.
        /// </summary>
        public StarSystem StarSystem { get; set; }

        /// <summary>
        /// Trajectory of the object.
        /// </summary>
        public Trajectory Trajectory { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleObject"/> class.
        /// </summary>
        public VisibleObject()
        {
            this.StarSystem = null;
            this.Trajectory = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceObject"/> class.
        /// </summary>
        /// <param name="starSystem">Star system where the object is located.</param>
        /// <param name="trajectory">Trajectory of the object.</param>
        public VisibleObject( StarSystem starSystem, Trajectory trajectory )
        {
            this.StarSystem = starSystem;
            this.Trajectory = trajectory;
        }
        #endregion
    }
}
