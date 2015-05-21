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
	public class PlayerServiceClient : ServiceClientBase<IPlayerService>, IPlayerService
	{

		public bool PlayerHasEnaughCredits(int playerId, long amount)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).PlayerHasEnaughCredits(playerId, amount);
			}
		}

		public Player GetPlayer(int playerId)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).GetPlayer(playerId);
			}
		}

		public bool PlayerHasSpaceShip(int playerId, int spaceShipId)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).PlayerHasSpaceShip(playerId, spaceShipId);
			}
		}

		public IList<SpaceShip> GetPlayersShips(int playerId)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).GetPlayersShips(playerId);
			}
		}
	}
}