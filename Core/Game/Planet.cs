using SpaceTraffic.Game.Geometry;
namespace SpaceTraffic.Game
{

    /// <summary>
    /// This class represents planet.
    /// </summary>
    public class Planet : CelestialObject, ILocation
    {
        public string Location
        {
            get
            {
                if (this.StarSystem != null)
                {
                    return this.StarSystem.Name + "\\" + this.Name;
                }
                else
                    return "";
            }
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Planet"/> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        public Planet()
            : base()
        {
        }

        /// <summary>
        /// Constructor creates fully determined planet. 
        /// There's no need to edit the planet later.
        /// </summary>
        /// <param name="name">Program name of the planet</param>
        /// <param name="starSystem">Star system where the planet belongs to.</param>
        /// <param name="trajectory">Trajectory of the planet.</param>
        public Planet(string name, string altName, CelestialObjectInfo details, StarSystem starSystem, Trajectory trajectory)
            : base(name, altName, details, starSystem, trajectory)
        {
        }
        #endregion
    }
}
