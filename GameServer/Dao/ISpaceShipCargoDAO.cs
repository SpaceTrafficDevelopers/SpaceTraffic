using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface ISpaceShipCargoDAO
    {
        /// <summary>
        /// Insert spaceShip with Cargo
        /// </summary>
        /// <param name="spaceShipId">SpaceShip id</param>
        /// <param name="CargoId">Cargo id</param>
        /// <returns>List of spaceShip with Cargo</returns>
        bool InsertSpaceShipCargo(SpaceShipCargo spaceShipCargo);

        /// <summary>
        /// Update spaceShip with Cargo
        /// </summary>
        /// <param name="spaceShipId">SpaceShip id</param>
        /// <param name="CargoId">Cargo id</param>
        /// <returns>Result of updating</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateCargoCountById(SpaceShipCargo spaceShipCargo);

        /// <summary>
        /// Remove spaceShip with Cargo
        /// </summary>
        /// <param name="spaceShipId">SpaceShip id</param>
        /// <param name="CargoId">Cargo id</param>
        /// <returns>Result of removing</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveSpaceShipCargoById(int spaceShipId, int cargoId);

        /// <summary>
        /// Return list of Cargos by spaceShip id
        /// </summary>
        /// <param name="spaceShipId">SpaceShip id</param>
        /// <returns>Cargos</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        List<SpaceShipCargo> GetSpaceShipCargoBySpaceShipId(int spaceShipId);


    }
}
