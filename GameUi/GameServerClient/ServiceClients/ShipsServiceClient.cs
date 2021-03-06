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
	public class ShipsServiceClient : ServiceClientBase<IShipsService>, IShipsService
	{

		public bool SpaceShipDockedAtBase(int spaceShipId, string starSystemName, string planetName)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IShipsService).SpaceShipDockedAtBase(spaceShipId, starSystemName, planetName);
			}
		}


		public SpaceShip GetSpaceShip(int spaceShipId)
		{
			using (var channel = this.GetClientChannel())
			{
				var spaceship = (channel as IShipsService).GetSpaceShip(spaceShipId);
				return spaceship;
			}
		}

		public SpaceShip GetDetailedSpaceShip(int spaceShipId)
		{
			using (var channel = this.GetClientChannel())
			{
				var spaceship = (channel as IShipsService).GetDetailedSpaceShip(spaceShipId);
				return spaceship;
			}
		}

		public SpaceShip ChangeShipState(int shipId, bool available, string message = "")
		{
			using (var channel = this.GetClientChannel())
			{
				var spaceship = (channel as IShipsService).ChangeShipState(shipId, available, message);
				return spaceship;
			}
		}
	}
}