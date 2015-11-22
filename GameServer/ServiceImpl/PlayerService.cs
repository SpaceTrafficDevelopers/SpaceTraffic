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
	public class PlayerService : IPlayerService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();



		/// <summary>
		/// Returns bool value which decides if given player can afford given amount of expenses
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <param name="amount">The size of expense</param>
		/// <returns></returns>
		public bool PlayerHasEnaughCredits(int playerId, long amount)
		{
			long actualMoney = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId).Credit;
			return actualMoney >= amount;
		}

		/// <summary>
		/// Returns player from database.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <returns></returns>
		public Player GetPlayer(int playerId)
		{
			return GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId);
		}

		/// <summary>
		/// Control if spaceship belong to player.
		/// </summary>
		/// <param name="playerId">Identification number of player.</param>
		/// <param name="spaceShipId">Identification number of spaceship.</param>
		/// <returns></returns>
		public bool PlayerHasSpaceShip(int playerId, int spaceShipId)
		{
			SpaceShip spaceShip = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
			if (spaceShip == null)
				return false;
			return spaceShip.PlayerId == playerId;
		}

		/// <summary>
		/// Gets ships of given player.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <returns>list of all his ships</returns>
		public IList<SpaceShip> GetPlayersShips(int playerId)
		{
			return GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipsByPlayer(playerId);
		}


		/// <summary>
		/// Returns the players credits.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public int GetPlayersCredits(int playerId)
		{
			return GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId).Credit;
		}
	}
}
