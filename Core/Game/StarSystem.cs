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
using SpaceTraffic.Entities;

namespace SpaceTraffic.Game
{
    public class StarSystem : IVersionedObject
    {
        public int Id { get; set; }

        #region Properties
        /// <summary>
        /// This is a unique identifier of a system.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// These are coordinates of the system on galactic map
        /// </summary>
        /// <value>
        /// The map position.
        /// </value>
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

        private class WormholeEndpointList : KeyAccessibleSortedList<int, WormholeEndpoint>
        {
            public override int GetKeyForValue(WormholeEndpoint item)
            {
                return item.Id;
            }
            //TODO: test of indexer implementation.

            
        }
        #endregion







        // seznam lodi leticich ve star systemu
        public virtual ICollection<SpaceShip> SpaceShips { get; set; }

        public void AddSpaceShip(SpaceShip ship)
        {
            SpaceShips.Add(ship);
        }


        public SpaceShip GetSpaceShip(int shipId)
        {
            //this has O(n)! Dictionary will be better, but it can not be mapped to entity framework
            foreach (SpaceShip s in SpaceShips)
            {
                if (s.SpaceShipId.Equals(shipId))
                {
                    return s;
                }
            }
            return null;
        }

        public ICollection<SpaceShip> GetSpaceShips()
        {
            return SpaceShips;
        }     

        public bool RemoveSpaceShip(SpaceShip ship)
        {
            return SpaceShips.Remove(ship);
        }
    }
}
