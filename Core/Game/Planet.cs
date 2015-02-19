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
