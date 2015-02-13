

using SpaceTraffic.Game.Geometry;
namespace SpaceTraffic.Game
{
    /// <summary>
    /// This class represents space object. Every space object should be inheriated from it.
    /// </summary>
    public abstract class SpaceObject
    {
        #region Properties
        /// <summary>
        /// Name of the object.
        /// </summary>
        public string Name { get; set; }

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
        /// Initializes a new instance of the <see cref="SpaceObject"/> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        public SpaceObject(string name)
        {
            this.Name = name;
            this.StarSystem = null;
            this.Trajectory = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceObject"/> class.
        /// </summary>
        /// <param name="name">Name of the object</param>
        /// <param name="starSystem">Star system where the object is located.</param>
        /// <param name="trajectory">Trajectory of the object.</param>
        public SpaceObject( string name, StarSystem starSystem, Trajectory trajectory )
        {
            this.Name = name;
            this.StarSystem = starSystem;
            this.Trajectory = trajectory;
        }
        #endregion
    }
}
