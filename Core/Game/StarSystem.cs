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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Utils.Collections;
using SpaceTraffic.Game.Geometry;
using System.Runtime.Serialization;

namespace SpaceTraffic.Game
{   
    [DataContract(Name="StarSystem")]
    public class StarSystem : IVersionedObject
    {
        #region Properties
        /// <summary>
        /// This is a unique identifier of a system.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// These are coordinates of the system on galactic map
        /// </summary>
        /// <value>
        /// The map position.
        /// </value>
        [DataMember]
        public Point2d MapPosition { get; set; }

        /// <summary>
        /// This points to the star in the system
        /// </summary>
        /// <value>
        /// The star.
        /// </value>
        public Star Star { get; set; }

        /// <summary>
        /// This points to all planets in the system
        /// </summary>
        /// <value>
        /// The planets.
        /// </value>
        public IKeyAccessibleList<string, Planet> Planets { get; set; }
        
        /// <summary>
        /// This points to all wormhole endpoints in the system
        /// </summary>
        /// <value>
        /// The wormhole endpoints.
        /// </value>

        public IKeyAccessibleList<int, WormholeEndpoint> WormholeEndpoints { get; set; }

        [DataMember]
        public IList<WormholeEndpoint> WormholeEndpointsList { get; set; }

        public DateTime LastUpdate { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of class StarSystem
        /// </summary>
        public StarSystem()
        {
            this.Planets = new PlanetList();
            this.WormholeEndpoints = new WormholeEndpointList();
        }
        #endregion

        #region Internal classes
        private class PlanetList : KeyAccessibleSortedList<string, Planet>
        {
            public override string GetKeyForValue(Planet item)
            {
                return item.Name;
            }
        }
        [DataContract(Name="WormholeEndpoints")]
        private class WormholeEndpointList : KeyAccessibleSortedList<int, WormholeEndpoint>
        {
            public override int GetKeyForValue(WormholeEndpoint item)
            {
                return item.Id;
            }
            //TODO: test of indexer implementation.

            
        }
        #endregion
    }
}
