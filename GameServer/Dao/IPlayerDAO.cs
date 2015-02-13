using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface IPlayerDAO
    {
        /// <summary>
        /// Get players from database
        /// </summary>
        /// <returns>Return list of players</returns>
        List<Player> GetPlayers();
        /// <summary>
        /// Get player from database by id
        /// </summary>
        /// <param name="playerId">Identificator of player</param>
        /// <returns>Return object of player by id</returns>
        Player GetPlayerById(int playerId);
        /// <summary>
        /// Get player from database by player name
        /// </summary>
        /// <param name="playerName">Name of player</param>
        /// <returns>Return object of player by name</returns>
        Player GetPlayerByName(string playerName);
        /// <summary>
        /// Get player from database by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Return object of player by email</returns>
        Player GetPlayerByEmail(string email);
        /// <summary>
        /// Insert new player to database
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertPlayer(Player player);
        /// <summary>
        /// Remove exist player by id
        /// </summary>
        /// <param name="playerId">Identificator of player</param>
        /// <returns>Return true if operation is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemovePlayerById(int playerId);
        /// <summary>
        /// Update exist player by id
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdatePlayerById(Player player);
    }
}
