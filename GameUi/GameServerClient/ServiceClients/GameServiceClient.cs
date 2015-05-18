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
using System.ServiceModel;

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

		public bool PlayerHasEnaughCredits(int playerId, long amount)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IGameService).PlayerHasEnaughCredits(playerId, amount);
			}
		}

        public bool PlayerHasEnaughCreditsForCargo(int playerId, int cargoLoadEntityId, int count)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).PlayerHasEnaughCreditsForCargo(playerId, cargoLoadEntityId, count);
            }
        }

        public bool SpaceShipHasCargoSpace(int spaceShipId, int cargoLoadEntityId, int count)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).SpaceShipHasCargoSpace(spaceShipId, cargoLoadEntityId, count);
            }
        }

        public bool SpaceShipDockedAtBase(int spaceShipId, string starSystemName, string planetName)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).SpaceShipDockedAtBase(spaceShipId, starSystemName, planetName);
            }
        }

        public bool PlayerHasSpaceShip(int playerId, int spaceShipId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).PlayerHasSpaceShip(playerId, spaceShipId);
            }
        }

        public bool PlayerHasEnoughCargo(string buyingPlace, int cargoLoadEntityId, int cargoCount)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).PlayerHasEnoughCargo(buyingPlace, cargoLoadEntityId, cargoCount);
            }
        }

        public bool PlayerHasEnoughCargoOnSpaceShip(int spaceShipId, int cargoLoadEntityId, int cargoCount)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).PlayerHasEnoughCargoOnSpaceShip(spaceShipId, cargoLoadEntityId, cargoCount);
            }
        }

        public bool TraderHasEnoughCargo(int traderId, int cargoLoadEntityId, int cargoCount)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).TraderHasEnoughCargo(traderId, cargoLoadEntityId, cargoCount);
            }
        }

        public int CreatePathPlan(int playerId, int spaceShipId, bool isCycled)
        {
            using (var channel = this.GetClientChannel())
            {
                ((IContextChannel)channel).OperationTimeout = new TimeSpan(0, 20, 0); //testing timeout
                return (channel as IGameService).CreatePathPlan(playerId, spaceShipId, isCycled);
            }
        }

        public int AddPlanItem(int pathPlanId, string solarSystem, bool isPlanet, string index, int sequenceNumber)
        {
            using (var channel = this.GetClientChannel())
            {
                ((IContextChannel)channel).OperationTimeout = new TimeSpan(0, 20, 0); //testing timeout
                return (channel as IGameService).AddPlanItem(pathPlanId, solarSystem, isPlanet, index, sequenceNumber);
            }
        }

        public bool AddPlanAction(int planItemId, int sequenceNumber, int playerId, string actionName, params object[] actionArgs)
        {
            using (var channel = this.GetClientChannel())
            {
                ((IContextChannel)channel).OperationTimeout = new TimeSpan(0, 20, 0); //testing timeout
                return (channel as IGameService).AddPlanAction(planItemId, sequenceNumber, playerId, actionName, actionArgs);
            }
        }

        public bool StartPathPlan(int pathPlanId)
        {
            using (var channel = this.GetClientChannel())
            {
                ((IContextChannel)channel).OperationTimeout = new TimeSpan(0, 20, 0); //testing timeout
                return (channel as IGameService).StartPathPlan(pathPlanId);
            }
        }
    }
}