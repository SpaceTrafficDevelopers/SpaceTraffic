﻿/**
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

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{
	public class GameServiceClient : ServiceClientBase<IGameService>, IGameService
	{
		public IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).GetStarSystemConnections(starSystem);
			}
		}

		public IList<SpaceShip> GetPlayersShips(int playerId) {
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).GetPlayersShips(playerId);
			}
		}

		public int PerformAction(int playerId, string actionName, params object[] actionArgs)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).PerformAction(playerId, actionName, actionArgs);
			}
		}

		public object GetActionResult(int playerId, int actionCode)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).GetActionResult(playerId, actionCode);
			}
		}
	}
}