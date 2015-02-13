using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Game
{
    public abstract class CelestialObject : VisibleObject
    {
        #region Properties
        /// <summary>
        /// Name of celestial object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alternative name of celestial object
        /// </summary>
        public string AlternativeName { get; set; }

        /// <summary>
        /// Details of celestial object.
        /// </summary>
        public CelestialObjectInfo Details { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleObject"/> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        public CelestialObject() : base ()
        {
            this.Name = "";
            this.AlternativeName = "";
            this.Details = new CelestialObjectInfo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceObject"/> class.
        /// </summary>
        /// <param name="starSystem">Star system where the object is located.</param>
        /// <param name="trajectory">Trajectory of the object.</param>
        public CelestialObject( string name, string altName, CelestialObjectInfo details, StarSystem starSystem, Trajectory trajectory )
            : base (starSystem, trajectory)
        {
            this.Name = name;
            this.AlternativeName = altName;
            this.Details = details;
        }
        #endregion
    }
}
