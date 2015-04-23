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
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface IBaseDAO
    {
        /// <summary>
        /// Gets all bases from DB.
        /// </summary>
        /// <returns>List of bases.</returns>
        List<Base> GetBases();
        /// <summary>
        /// Gets the base by id from DB.
        /// </summary>
        /// <param name="baseId">The base id.</param>
        /// <returns>Base.</returns>
        Base GetBaseById(int baseId);

        /// <summary>
        /// Get Base by full planet name. 
        /// Full planet name (location) = "StarSystem.Name\Planet.Name"
        /// </summary>
        /// <param name="planetFullName">full planet name (location)</param>
        /// <returns>Base</returns>
        Base GetBaseByPlanetFullName(string planetFullName);

        /// <summary>
        /// Inserts the base to DB.
        /// </summary>
        /// <param name="?">The base.</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertBase(Base bbase);
        /// <summary>
        /// Removes the base by id from DB.
        /// </summary>
        /// <param name="baseId">The base id.</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveBaseById(int baseId);
        /// <summary>
        /// Updates the base by id.
        /// </summary>
        /// <param name="bbase">The base.</param>
        /// <returns>Return true if operation of update is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateBaseById(Base bbase);
    }
}
