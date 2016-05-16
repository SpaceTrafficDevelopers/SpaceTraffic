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
using SpaceTraffic.Services.Contracts;
using NLog;
using SpaceTraffic.Utils.Debugging;
using GS = SpaceTraffic.GameServer.GameServer;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Actions;
using SpaceTraffic.Entities;
using SpaceTraffic.Dao;
using SpaceTraffic.Game.Planner;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Game;

namespace SpaceTraffic.GameServer.ServiceImpl
{
	public class ShipsService : IShipsService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();


		public bool SpaceShipDockedAtBase(int spaceShipId, string starSystemName, string planetName)
		{

			SpaceShip spaceShip = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
			Entities.Base dockedBase = GS.CurrentInstance.Persistence.GetBaseDAO().GetBaseById((int)spaceShip.DockedAtBaseId);
			Game.Planet planet = GS.CurrentInstance.World.Map[starSystemName].Planets[planetName];

			if (dockedBase.Planet.Equals(planet.Location))
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns single spaceship
		/// </summary>
		/// <param name="spaceShipId">The space ship identifier.</param>
		/// <returns></returns>
		public SpaceShip GetSpaceShip(int spaceShipId)
		{

			var ship = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
			return ship;
		}

		/// <summary>
		/// Returns single spaceship
		/// </summary>
		/// <param name="spaceShipId">The space ship identifier.</param>
		/// <returns></returns>
		public SpaceShip GetDetailedSpaceShip(int spaceShipId)
		{

			var ship = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetDetailedSpaceShipById(spaceShipId);
			return ship;
		}

		/// <summary>
		/// Changes info about the ship - avalibility and state
		/// </summary>
		/// <param name="shipId">The ship identifier.</param>
		/// <param name="available">if set to <c>true</c> [available].</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public SpaceShip ChangeShipState(int shipId, bool available, string message = "") {
			SpaceShip ship = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(shipId);
			ship.IsAvailable = available;
			ship.StateText = (String.IsNullOrEmpty(message)) ? SpaceShip.StateTextDefault : message;
			GS.CurrentInstance.Persistence.GetSpaceShipDAO().UpdateSpaceShip(ship);

			return ship;
		}
	}
}
