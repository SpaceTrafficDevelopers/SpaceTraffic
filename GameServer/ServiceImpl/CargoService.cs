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
	public class CargoService : ICargoService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();

		

		public bool PlayerHasEnaughCreditsForCargo(int playerId, int cargoLoadEntityId, int count)
		{

			long actualMoney = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId).Credit;
			TraderCargo tc = (TraderCargo)GS.CurrentInstance.Persistence.GetTraderCargoDAO().GetCargoByID(cargoLoadEntityId);
			long price = tc.CargoPrice;
			return actualMoney >= (price * count);
		}

		public bool SpaceShipHasCargoSpace(int spaceShipId, int cargoLoadEntityId, int count)
		{
			int actualVolume = 0;
			int spaceCargo = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId).CargoSpace;

			ICargoLoadEntity tc = GS.CurrentInstance.Persistence.GetTraderCargoDAO().GetCargoByID(cargoLoadEntityId);

			int cargoVolume = GS.CurrentInstance.Persistence.GetCargoDAO().GetCargoById(tc.CargoId).Volume;

			List<ICargoLoadEntity> cargos = GS.CurrentInstance.Persistence.GetSpaceShipCargoDAO().GetCargoListByOwnerId(spaceShipId);
			foreach (ICargoLoadEntity cargo in cargos)
			{
				actualVolume += GS.CurrentInstance.Persistence.GetCargoDAO().GetCargoById(cargo.CargoId).Volume;
			}
			return (spaceCargo - actualVolume) >= (cargoVolume * count);
		}


		/// <summary>
		/// Control if player has cargo on loading place for buy or unload.
		/// </summary>
		/// <param name="loadingPlace">Loading place.</param>
		/// <param name="cargoLoadEntityId">Identification number of cargo load entity.</param>
		/// <param name="cargoCount">Count of cargo.</param>
		/// <returns>Value if player has cargo on loading place.</returns>
		public bool PlayerHasEnoughCargo(string loadingPlace, int cargoLoadEntityId, int cargoCount)
		{
			ICargoLoadDao loading = GS.CurrentInstance.Persistence.GetCargoLoadDao(loadingPlace);
			ICargoLoadEntity cargo = loading.GetCargoByID(cargoLoadEntityId);
			if (cargo == null)
				return false;
			int count = cargo.CargoCount;
			return count >= cargoCount;
		}

		/// <summary>
		/// Control if player has cargo on spaceship for buy or unload.
		/// </summary>
		/// <param name="spaceShipId">Identification number of spaceship.</param>
		/// <param name="cargoLoadEntityId">Identification number of cargo load entity.</param>
		/// <param name="cargoCount">Count of cargo.</param>
		/// <returns>Value if player has cargo on spaceship</returns>
		public bool PlayerHasEnoughCargoOnSpaceShip(int spaceShipId, int cargoLoadEntityId, int cargoCount)
		{
			SpaceShip spaceShip = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
			ICargoLoadEntity cargo = GS.CurrentInstance.Persistence.GetSpaceShipCargoDAO().GetCargoByID(cargoLoadEntityId);

			if (cargo == null || spaceShipId != cargo.CargoOwnerId)
				return false;

			int count = cargo.CargoCount;
			return count >= cargoCount;
		}

		/// <summary>
		/// Control if trader has cargo for sell.
		/// </summary>
		/// <param name="traderId">Identification number of trader.</param>
		/// <param name="cargoLoadEntityId">Identification number of cargo load entity.</param>
		/// <param name="cargoCount">Count of cargo.</param>
		/// <returns>Value if trader has enough cargo for selling or not.</returns>
		public bool TraderHasEnoughCargo(int traderId, int cargoLoadEntityId, int cargoCount)
		{
			TraderCargo tc = (TraderCargo)GS.CurrentInstance.Persistence.GetTraderCargoDAO().GetCargoByID(cargoLoadEntityId);
			if (traderId != tc.CargoOwnerId || tc == null)
				return false;
			int count = tc.CargoCount;
			return count >= cargoCount;
		}


		/// <summary>
		/// Gets the trader at base.
		/// </summary>
		/// <param name="baseId">The base identifier.</param>
		/// <returns></returns>		
		public Trader GetTraderAtBase(int baseId)
		{
			return GS.CurrentInstance.Persistence.GetTraderDAO().GetTraderByBaseId(baseId);
		}
	}
}
