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
    public interface ISpaceShipCargoDAO : ICargoLoadDao
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
        /// Update spaceship price by ID
        /// </summary>
        /// <param name="spaceShipCargo">Instance of pace ship cargo </param>
        /// <returns>Result of updating</returns>
        bool UpdateCargoPriceById(SpaceShipCargo spaceShipCargo);

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
