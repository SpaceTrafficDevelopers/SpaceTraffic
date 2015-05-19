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
    public interface IEarnedAchievementDAO
    {
        /// <summary>
        /// Gets all earned achievements from DB.
        /// </summary>
        /// <returns>List of EarnedAchievement.</returns>
        List<EarnedAchievement> GetEarnedAchievements();

        /// <summary>
        /// Gets all EarnedAchievement owned by player from DB
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        List<EarnedAchievement> GetEarnedAchievementsByPlayerId(int playerId);

        /// <summary>
        /// Gets the EarnedAchievements by id from DB.
        /// </summary>
        /// <param name="LandId">The EarnedAchievement id.</param>
        /// <returns>EarnedAchievement.</returns>
        EarnedAchievement GetEarnedAchievementById(int earnedAchievementId);

        /// <summary>
        /// Inserts the EarnedAchievement to DB.
        /// </summary>
        /// <param name="?">The EarnedAchievement.</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertEarnedAchievement(EarnedAchievement earnedAchievement);

        /// <summary>
        /// Removes the EarnedAchievement by id from DB.
        /// </summary>
        /// <param name="LandId">The EarnedAchievement id.</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveEarnedAchievementById(int earnedAchievementId);

        /// <summary>
        /// Updates the EarnedAchievement by id.
        /// </summary>
        /// <param name="bLand">The EarnedAchievement.</param>
        /// <returns>Return true if operation of update is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateEarnedAchievementById(EarnedAchievement earnedAchievement);


		/// <summary>
		/// Gets the unviewed achievements by player.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <returns></returns>
		List<EarnedAchievement> GetUnviewedAchievementsByPlayerId(int playerId);


    }
}
