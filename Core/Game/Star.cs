using SpaceTraffic.Game.Geometry;
namespace SpaceTraffic.Game
{
    /// <summary>
    /// This class represents star.
    /// Star is 
    /// </summary>
    public class Star: CelestialObject
    {

        #region Properties
        /// <summary>
        /// Gets or sets the minimum approach distance to the star,
        /// before the space ship will definitelly melt.
        /// </summary>
        /// <value>
        /// The minimum approach distance to the star.
        /// </value>
        public int MinimumApproachDistance { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Star"/> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        public Star()
            : base()
        {
        }

        /// <summary>
        /// Constructor creates fully determined Star. 
        /// There's no need to edit the star later.
        /// </summary>
        /// <param name="name">Program name of the star</param>
        /// <param name="starSystem">Star system where the star belongs to.</param>
        /// <param name="trajectory">Trajectory of the star.</param>
        public Star( string name, string altName, CelestialObjectInfo details, StarSystem starSystem, Trajectory trajectory ): base ( name, altName, details, starSystem, trajectory )
        {
        }
        #endregion
    }
}
