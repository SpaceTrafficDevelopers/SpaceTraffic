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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SpaceTraffic.Utils.Collections;
using SpaceTraffic.Utils.Debugging;
using SpaceTraffic.Entities.PublicEntities;

namespace SpaceTraffic.Game
{

    
    /// <summary>
    /// 
    /// </summary>
    public class GalaxyMap : IKeyAccessibleList<string, StarSystem>
    {

        private class StarSystemNameComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return String.Compare(x, y, true);
            }
        }

        private bool _locked = false;

        private SortedList<string, StarSystem> starSystems = new SortedList<string, StarSystem>(new StarSystemNameComparer());

        #region Properties

        public String MapName { get; set; }

        #region IKeyAccessibleList properties
        /// <summary>
        /// Gets the number of star systems in this map.
        /// </summary>
        public int Count
        {
            get { return this.starSystems.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this map is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return this._locked; }
        }
        #endregion
        #endregion

        #region IKeyAccessible indexers

        /// <summary>
        /// Indexer to get or set items within this collection using array index syntax.
        /// </summary>
        ///
        /// <value>
        /// Vrací instanci StarSystem příslušného jména. Pokud taková neexistuje, vrací null.
        /// </value>
        public StarSystem this[string key]
        {
            get
            {
                StarSystem retVal = null;
                this.starSystems.TryGetValue(key, out retVal);
                return retVal;
            }
            set
            {
                if (this.IsReadOnly)
                    throw new InvalidOperationException("GalaxyMap instance is locked, no modifications allowed.");
                
                this.starSystems[key] = value;
            }
        }

        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public StarSystem this[int index]
        {
            get
            {
                throw new InvalidOperationException("Operation not supported.");
            }
            set
            {
                throw new InvalidOperationException("Operation not supported.");
            }
        }
        #endregion

        #region IKeyAccessibleList implementation
        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public int IndexOf(StarSystem item)
        {
            throw new InvalidOperationException("Operation not supported.");
        }

        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public void Insert(int index, StarSystem item)
        {
            throw new InvalidOperationException("Operation not supported.");
        }

        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public void RemoveAt(int index)
        {
            throw new InvalidOperationException("Operation not supported.");
        }

        /// <summary>
        /// Adds StarSystem instance to this map.
        /// </summary>
        /// <param name="starSystem">StarSystem instance.</param>
        public void Add(StarSystem starSystem)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("GalaxyMap instance is locked, no modifications allowed.");

            this.starSystems.Add(starSystem.Name, starSystem);
        }

        public void AddAll(ICollection<StarSystem> starSystems)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("GalaxyMap instance is locked, no modifications allowed.");

            //
            // TODO: Capacity increase
            foreach (StarSystem starSystem in starSystems)
            {
                this.Add(starSystem);
            }
        }

        /// <summary>
        /// Clears this map.
        /// </summary>
        public void Clear()
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("GalaxyMap instance is locked, no modifications allowed.");

            this.starSystems.Clear();
        }

        /// <summary>
        /// Determines whether this map contains the specified StarSystemInstance.
        /// </summary>
        /// <param name="starSystem">StarSystem instance</param>
        /// <returns>
        ///   <c>true</c> if map contains the specified StarSystem instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(StarSystem starSystem)
        {
            return this.ContainsValue(starSystem);
        }

        /// <summary>
        /// Determines whether this map contains the specified StarSystemInstance.
        /// </summary>
        /// <param name="starSystem">StarSystem instance</param>
        /// <returns>
        ///   <c>true</c> if map contains the specified StarSystem instance; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValue(StarSystem starSystem)
        {
            return this.starSystems.ContainsValue(starSystem);
        }

        /// <summary>
        /// Determines whether map contains the specified star system.
        /// </summary>
        /// <param name="starSystemName">Name of the star system.</param>
        /// <returns>
        ///   <c>true</c> if map contains the specified star system; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(string starSystemName)
        {
            return this.starSystems.ContainsKey(starSystemName);
        }

        /// <summary>
        /// Copies all StarSystems into the specified array from given index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">Initial index of the array.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="ArgumentException">The number of elements in this map is greater than the available space from arrayIndex to the end of the destination array.</exception>
        public void CopyTo(StarSystem[] array, int arrayIndex)
        {
            if(array == null) throw new ArgumentNullException("Array is null.");

            if(arrayIndex < 0) throw new ArgumentOutOfRangeException("Value of index cannot be negative.");

            if(array.Rank > 0)
            {
                throw new ArgumentException("Array is multidimensional");
            }
            else if ((array.Length - arrayIndex) <= this.starSystems.Count)
            {
                throw new ArgumentException("Not enough space in array.");
            }

            int i = arrayIndex;
            foreach (StarSystem starSystem in this.starSystems.Values)
            {
                array[i] = starSystem;
            }
        }



        /// <summary>
        /// Removes the specified StarSystem instance from map.
        /// </summary>
        /// <param name="item">The StarSystem instance.</param>
        /// <returns>true, if StarSystem instance was successfully removed, otherwise false</returns>
        public bool Remove(StarSystem item)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("GalaxyMap instance is locked, no modifications allowed.");

            if(item == null) return false;

            StarSystem existing = this.starSystems[item.Name];

            if((existing != null) && (Object.ReferenceEquals(existing, item)))
            {
                return this.starSystems.Remove(item.Name);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the specified star system.
        /// </summary>
        /// <param name="starSystemName">Name of the star system.</param>
        /// <returns>true, if star system was successfully removed, otherwise false.</returns>
        public bool Remove(string starSystemName)
        {
            if (this.IsReadOnly)
                throw new InvalidOperationException("GalaxyMap instance is locked, no modifications allowed.");

            return this.starSystems.Remove(starSystemName);
        }
        #endregion

        #region IEnumerable implementation
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<StarSystem> GetEnumerator()
        {
            return this.starSystems.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.starSystems.Values.GetEnumerator();
        }
        #endregion

        public void Lock()
        {
            this._locked = true;
        }

        /// <summary>
        /// Gets the list of star system instances in read only collection.
        /// </summary>
        /// <returns>read-only IList of starSystems.</returns>
        public IList<StarSystem> GetStarSystems()
        {
            return new ReadOnlyCollection<StarSystem>(this.starSystems.Values);
        }
        
        /// <summary>
        /// Gets the star system connections in list of WormholeEndpointDestination instances.
        /// Thread-safety of this method is guaranteed by locking the map after it is successfuly loaded by server.
        /// </summary>
        /// <param name="starSystemName">Name of the star system.</param>
        /// <returns>List of object arrays with given</returns>
        public IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystemName)
        {
            DebugEx.Entry(starSystemName);
            StarSystem starSystem = null;
            if (this.starSystems.TryGetValue(starSystemName, out starSystem))
            {
                IList<WormholeEndpointDestination> connections = new List<WormholeEndpointDestination>(starSystem.WormholeEndpoints.Count);
                
                WormholeEndpointDestination connection;
                foreach (WormholeEndpoint wormholeEndpoint in starSystem.WormholeEndpoints)
                {
                    if (wormholeEndpoint.IsConnected)
                    {
                        connection = new WormholeEndpointDestination{
                            EndpointId = wormholeEndpoint.Id,
                            DestinationStarSystemName = wormholeEndpoint.Destination.StarSystem.Name
                        };
                
                        connections.Add(connection);
                    }
                }

                DebugEx.Exit(connections);

                return connections;
            }
            else
            {
                // StarSystem was not found, returning empty list.
                IList<WormholeEndpointDestination> emptyList = new List<WormholeEndpointDestination>(0);
                DebugEx.Exit(emptyList);
                return emptyList;
            }
        }















        /// <summary>
        /// Get planets of star system.
        /// </summary>
        /// <param name="starSystemName"></param>
        /// <returns></returns>
        public IList<Planet> GetPlanets(string starSystemName)
        {
            DebugEx.Entry(starSystemName);
            StarSystem starSystem = null;
            if (this.starSystems.TryGetValue(starSystemName, out starSystem))
            {
                IList<Planet> planets = new List<Planet>(starSystem.Planets.Count);

                foreach (Planet p in starSystem.Planets)
                {
                    if (p.StarSystem.Equals(starSystem))
                    {
                        planets.Add(p);
                    }
                }

                DebugEx.Exit(planets);

                return planets;
            }
            else
            {
                // StarSystem was not found, returning empty list.
                IList<Planet> emptyList = new List<Planet>(0);
                DebugEx.Exit(emptyList);
                return emptyList;
            }
        }
    }


}
