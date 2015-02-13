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
        /// Gets space ship by id.
        /// </summary>
        /// <param name="spaceShipId">The space ship id.</param>
        /// <returns>Space ship.</returns>
        SpaceShip GetSpaceShipById(int spaceShipId);        
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
