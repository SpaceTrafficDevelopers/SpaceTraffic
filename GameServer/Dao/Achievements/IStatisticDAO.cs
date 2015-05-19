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
    public interface IStatisticDAO
    {
        /// <summary>
        /// Gets all statistics from DB.
        /// </summary>
        /// <returns>List of Statistics.</returns>
        List<Statistic> GetStatistic();

        /// <summary>
        /// Gets all stats owned by player from DB
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        List<Statistic> GetStatisticsByPlayerId(int playerId);

        /// <summary>
        /// Gets the Statistic by id from DB.
        /// </summary>
        /// <param name="LandId">The Statistic id.</param>
        /// <returns>Statistic.</returns>
        Statistic GetStatisticById(int statisticId);

        /// <summary>
        /// Inserts the Statistic to DB.
        /// </summary>
        /// <param name="?">The Statistic.</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertStatistic(Statistic statistic);

        /// <summary>
        /// Removes the Statistic by id from DB.
        /// </summary>
        /// <param name="LandId">The Statistic id.</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveStatisticById(int statisticId);

        /// <summary>
        /// Updates the Statistic by id.
        /// </summary>
        /// <param name="bLand">The Statistic.</param>
        /// <returns>Return true if operation of update is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateStatisticById(Statistic statistic);

    }
}
