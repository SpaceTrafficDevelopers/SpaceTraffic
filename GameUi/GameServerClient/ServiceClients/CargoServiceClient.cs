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
using System.Web;
using SpaceTraffic.Services.Contracts;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Entities;
using System.ServiceModel;

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{
	public class CargoServiceClient : ServiceClientBase<ICargoService>, ICargoService
	{
		public bool PlayerHasEnaughCreditsForCargo(int playerId, int cargoLoadEntityId, int count)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as ICargoService).PlayerHasEnaughCreditsForCargo(playerId, cargoLoadEntityId, count);
			}
		}

		public bool SpaceShipHasCargoSpace(int spaceShipId, int cargoLoadEntityId, int count)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as ICargoService).SpaceShipHasCargoSpace(spaceShipId, cargoLoadEntityId, count);
			}
		}

		public bool PlayerHasEnoughCargo(string buyingPlace, int cargoLoadEntityId, int cargoCount)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as ICargoService).PlayerHasEnoughCargo(buyingPlace, cargoLoadEntityId, cargoCount);
			}
		}

		public bool PlayerHasEnoughCargoOnSpaceShip(int spaceShipId, int cargoLoadEntityId, int cargoCount)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as ICargoService).PlayerHasEnoughCargoOnSpaceShip(spaceShipId, cargoLoadEntityId, cargoCount);
			}
		}

		public bool TraderHasEnoughCargo(int traderId, int cargoLoadEntityId, int cargoCount)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as ICargoService).TraderHasEnoughCargo(traderId, cargoLoadEntityId, cargoCount);
			}
		}

		public Trader GetTraderAtBase(int baseId)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as ICargoService).GetTraderAtBase(baseId);
			}
		}

		public Trader GetTraderAtBaseWithCargo(int baseId)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as ICargoService).GetTraderAtBaseWithCargo(baseId);
			}
		}

        public bool IsTraderCargoExistsForBuy(int cargoLoadEntityId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as ICargoService).IsTraderCargoExistsForBuy(cargoLoadEntityId);
            }
        }

        public bool IsTraderCargoExistsForSell(int cargoLoadEntityId, int traderId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as ICargoService).IsTraderCargoExistsForSell(cargoLoadEntityId, traderId);
            }
        }
    }
}