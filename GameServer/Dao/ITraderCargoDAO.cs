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
    public interface ITraderCargoDAO : ICargoLoadDao
    {
        /*/// <summary>
        /// Insert trader cargo into table
        /// </summary>
        /// <param name="traderCargo">Instance of trader cargo</param>
        /// <returns>Result of insert</returns>
        bool InsertTraderCargo(TraderCargo traderCargo);

        /// <summary>
        /// Update cargo count by trader cargo
        /// </summary>
        /// <param name="traderCargo">Instance of trader cargo</param>
        /// <returns>Result of update</returns>
        bool UpdateCargo(TraderCargo traderCargo);

        /// <summary>
        /// Update cargo price by trader cargo
        /// </summary>
        /// <param name="traderCargo">Instance of trader cargo</param>
        /// <returns>Result of update</returns>
        bool UpdateCargoPriceById(TraderCargo traderCargo);

        /// <summary>
        /// Remove trader cargo
        /// </summary>
        /// <param name="traderId">Trader ID</param>
        /// <param name="cargoId">Cargo ID</param>
        /// <returns></returns>
        bool RemoveTraderCargoById(int traderId, int cargoId);

        /// <summary>
        /// Get trader cargo list by trader ID
        /// </summary>
        /// <param name="traderId">trader ID</param>
        /// <returns>List of trader cargos</returns>
        List<TraderCargo> GetTraderCargoByTraderId(int traderId);*/
    }
}
