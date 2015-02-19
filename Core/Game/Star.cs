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
