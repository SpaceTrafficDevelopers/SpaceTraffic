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
    public interface ISpaceShipDAO
    {
        /// <summary>
        /// Gets space ships from database.
        /// </summary>
        /// <returns>List of space ships.</returns>
        List<SpaceShip> GetSpaceShips();
        /// <summary>
        /// Gets space ships by player.
        /// </summary>
        /// <returns>List of space ships.</returns>
        List<SpaceShip> GetSpaceShipsByPlayer(int playerId);
		
		/// <summary>
		/// Gets the players ships at specific base.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <param name="baseId">The base identifier.</param>
		/// <returns></returns>
		IList<SpaceShip> GetPlayersShipsAtBase(int playerId, int baseId);
		/// <summary>
		/// Gets space ship by id.
		/// </summary>
		/// <param name="spaceShipId">The space ship id.</param>
		/// <returns>Space ship.</returns>
		SpaceShip GetSpaceShipById(int spaceShipId);

		/// <summary>
		/// Gets space ship by id. Contains details of the ship.
		/// </summary>
		/// <param name="spaceShipId">The space ship id.</param>
		/// <returns>Space ship.</returns>
		SpaceShip GetDetailedSpaceShipById(int spaceShipId);  
        /// <summary>
        /// Inserts the space ship to DB.
        /// </summary>
        /// <param name="spaceShip">The space ship.</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertSpaceShip(SpaceShip spaceShip);
        /// <summary>
        /// Removes the space ship by id from DB.
        /// </summary>
        /// <param name="spaceShipId">The space ship id.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveSpaceShipById(int spaceShipId);
        /// <summary>
        /// Updates the space ship.
        /// </summary>
        /// <param name="spaceShip">The space ship.</param>
        /// <returns>Return true if operation of update is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateSpaceShipById(SpaceShip spaceShip);

		
	}
}
