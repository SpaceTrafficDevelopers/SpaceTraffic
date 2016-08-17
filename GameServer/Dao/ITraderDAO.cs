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
    public interface ITraderDAO
    {
        /// <summary>
        /// Get all traders
        /// </summary>
        /// <returns>List of traders</returns>
        List<Trader> GetTraders();

        /// <summary>
        /// Get instance of trader by ID
        /// </summary>
        /// <param name="traderId">trader ID</param>
        /// <returns>Instance of trader</returns>
        Trader GetTraderById(int traderId);

		/// <summary>
		/// Get trader by base id
		/// </summary>
		/// <returns>Trader</returns>
		Trader GetTraderByBaseId(int baseId);

		/// <summary>
		/// Get trader by base id with full info of his cargo
		/// </summary>
		/// <returns>Trader</returns>
		Trader GetTraderByBaseIdWithCargo(int baseId);

        /// <summary>
        /// Insert trader into Traders
        /// </summary>
        /// <param name="trader">instance of trader</param>
        /// <returns>Result of insert</returns>
        bool InsertTrader(Trader trader);

        /// <summary>
        /// Remove trader from Traders by trader ID
        /// </summary>
        /// <param name="traderId">trader ID</param>
        /// <returns>Result of remove</returns>
        bool RemoveTraderById(int traderId);

        /// <summary>
        /// Update trader by Trader ID 
        /// </summary>
        /// <param name="trader">instance of trader</param>
        /// <returns>Result of update</returns>
        bool UpdateTraderById(Trader trader);

		List<Trader> GetTradersWithCargo();
    }
}
