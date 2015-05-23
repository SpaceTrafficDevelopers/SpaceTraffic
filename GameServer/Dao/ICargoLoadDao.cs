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
using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Interface for cargo load DAO
    /// </summary>
    public interface ICargoLoadDao
    {
        /// <summary>
        /// Insert cargo load entity to DB.
        /// </summary>
        /// <param name="cargoLoadEntity">Instance of cargo load entity</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        bool InsertCargo(ICargoLoadEntity cargoLoadEntity);

        /// <summary>
        /// Remove cargo load entity from DB. 
        /// </summary>
        /// <param name="cargoLoadEntityId">Instance of cargo load entity</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        bool RemoveCargoById(int cargoLoadEntityId);

        /// <summary>
        /// Update cargo load entity in DB.
        /// </summary>
        /// <param name="cargoLoadEntity">Instace of cargo load entity</param>
        /// <returns>Return true if operation of update is successful.</returns>
        bool UpdateCargo(ICargoLoadEntity cargoLoadEntity);

        /// <summary>
        /// Get cargo load entity by identification number.
        /// </summary>
        /// <param name="cargoLoadEntityId">Identification number of cargo load entity</param>
        /// <returns>Cargo load entity.</returns>
        ICargoLoadEntity GetCargoByID(int cargoLoadEntityId);

        /// <summary>
        /// Insert cargo load entity to DB if it is not in DB.
        /// Update cargo load entity count if it is in DB.
        /// </summary>
        /// <param name="cargo">Cargo load entity</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        bool InsertOrUpdateCargo(ICargoLoadEntity cargo);

        /// <summary>
        /// Update cargo load entity count if count in DB is bigger then param count.
        /// Remove cargo load entity if count in DB is same as param count.
        /// </summary>
        /// <param name="cargo">Cargo load entity</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        bool UpdateOrRemoveCargo(ICargoLoadEntity cargo);

        /// <summary>
        /// Get list of cargo load entities by owner identification number.
        /// </summary>
        /// <param name="ownerID">Owner identification number</param>
        /// <returns>List of cargo load entities.</returns>
        List<ICargoLoadEntity> GetCargoListByOwnerId(int ownerID);

    }
}
