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
using System.ServiceModel;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Services.Contracts
{
	[ServiceContract]
	public interface ICargoService
	{

		[OperationContract]
		bool PlayerHasEnaughCreditsForCargo(int playerId, int cargoLoadEntityId, int count);
		
		[OperationContract]
		bool SpaceShipHasCargoSpace(int spaceShipId, int cargoLoadEntityID, int count);
		
		
		[OperationContract]
		bool PlayerHasEnoughCargo(string buyingPlace, int cargoLoadEntityId, int cargoCount);

		[OperationContract]
		bool TraderHasEnoughCargo(int traderId, int cargoLoadEntityId, int cargoCount);

		[OperationContract]
		bool PlayerHasEnoughCargoOnSpaceShip(int spaceShipId, int cargoLoadEntityId, int cargoCount);

		[OperationContract]
		Trader GetTraderAtBase(int baseId);

		[OperationContract]
		Trader GetTraderAtBaseWithCargo(int baseId);

        /// <summary>
        /// Method for checking if trader cargo exists.
        /// </summary>
        /// <param name="cargoLoadEntityId">trader cargo id</param>
        /// <returns>true if cargo exists</returns>
        [OperationContract]
        bool IsTraderCargoExistsForBuy(int cargoLoadEntityId);

        /// <summary>
        /// Method for checking if trader cargo exists for sell.
        /// </summary>
        /// <param name="cargoLoadEntityId">ship cargo id</param>
        /// <param name="traderId">trader (buyer) id</param>
        /// <returns>true if cargo exists<</returns>
        [OperationContract]
        bool IsTraderCargoExistsForSell(int cargoLoadEntityId, int traderId);

	}

}
