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

namespace SpaceTraffic.GameServer.ServiceImpl
{
	public class GameService :IGameService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();


		public IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem)
		{
			DebugEx.Entry(starSystem);

			IList<WormholeEndpointDestination> result = GS.CurrentInstance.World.Map.GetStarSystemConnections(starSystem);

			DebugEx.Exit(result);
			return result;
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

        public bool PlayerHasEnaughCreditsForCargo(int playerId, int cargoLoadEntityId, int count, int traderId)
        {
            long price = 0;
            long actualMoney = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId).Credit;
            List<ICargoLoadEntity> cargos = GS.CurrentInstance.Persistence.GetTraderCargoDAO().GetCargoListByOwnerId(traderId);

            foreach (ICargoLoadEntity ssc in cargos) 
            {
                if (ssc.CargoLoadEntityId == cargoLoadEntityId) {
                    price = ssc.CargoPrice;
                }
            }
            return actualMoney >= (price * count);
        }

        public bool SpaceShipHasCargoSpace(int spaceShipId, string buyingPlace, int cargoLoadEntityId)
        {
            long actualVolume = 0;
            long cargoSpace = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId).CargoSpace;
            List<ICargoLoadEntity> cargos = GS.CurrentInstance.Persistence.GetSpaceShipCargoDAO().GetCargoListByOwnerId(spaceShipId);
            int cargoId = GS.CurrentInstance.Persistence.GetCargoLoadDao(buyingPlace).GetCargoByID(cargoLoadEntityId).CargoId;
            int volume = GS.CurrentInstance.Persistence.GetCargoDAO().GetCargoById(cargoId).Volume;
            foreach (ICargoLoadEntity ssc in cargos)
            {
                Cargo cargo = GS.CurrentInstance.Persistence.GetCargoDAO().GetCargoById(ssc.CargoId);
                actualVolume += (cargo.Volume * ssc.CargoCount);
            }

            return cargoSpace >= (actualVolume + volume);
        }

        public bool SpaceShipDockedAtBase(int spaceShipId, string starSystemName, string planetName)
        {
           
                SpaceShip spaceShip = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
                Entities.Base dockedBase = GS.CurrentInstance.Persistence.GetBaseDAO().GetBaseById(spaceShip.DockedAtBaseId);
                Game.Planet planet = GS.CurrentInstance.World.Map[starSystemName].Planets[planetName];

                if (dockedBase.Planet.Equals(planet))
                {
                    return true;
                }
                return false;
        }

        public bool PlayerHasSpaceShip(int playerId, int spaceShipId) 
        {
            SpaceShip spaceShip = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
            return spaceShip.PlayerId == playerId;
        }

        public bool PlayerHasEnoughCargoOnSpaceShip(int spaceShipId, int cargoLoadEntityId, int cargoCount) 
        {
            int count = 0;
            List<ICargoLoadEntity> cargos = GS.CurrentInstance.Persistence.GetSpaceShipCargoDAO().GetCargoListByOwnerId(spaceShipId);

            foreach (ICargoLoadEntity cargo in cargos) 
            {
                if (cargo.CargoLoadEntityId == cargoLoadEntityId) 
                {
                    count += cargo.CargoCount;
                }
            }
            if (count < cargoCount) 
                return false;
            return true;
        }

        public bool TraderHasEnoughCargo(int traderId, int cargoId, int cargoCount) 
        {
            int count = 0;
            List<ICargoLoadEntity> cargos = GS.CurrentInstance.Persistence.GetTraderCargoDAO().GetCargoListByOwnerId(traderId);

            foreach (SpaceShipCargo cargo in cargos)
            {
                if (cargo.CargoId == cargoId)
                {
                    count += cargo.CargoCount;
                }
            }
            if (count < cargoCount)
                return false;
            return true;
        }

		/// <summary>
		/// Finds action in SpaceTraffic.Game.Actions namespace and performs it with its arguments.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <param name="actionName">Name of the action (and class in Actions namespace).</param>
		/// <param name="actionArgs">The action arguments.</param>
		/// <returns>ActionCode of created action.</returns>
		/// <exception cref="SpaceTraffic.Services.Contracts.ActionNotFoundException"></exception>
		public int PerformAction(int playerId, string actionName, params object[] actionArgs)
		{
			try { 
				IGameAction action = Activator.CreateInstance(Type.GetType("SpaceTraffic.Game.Actions." + actionName)) as IGameAction;
				if (action == null)
				{//if action does not implemented action
					throw new ActionNotFoundException(String.Format(
						"Action class with name: {0} does not implements IGameAction.",
						actionName
					));
				}
				action.ActionArgs = actionArgs;
				action.PlayerId = playerId;
				GameServer.CurrentInstance.Game.PerformAction(action);
				return action.ActionCode;
			} catch(System.IO.FileNotFoundException e){
				Console.WriteLine("Action file with name " + actionName + " was not found in SpaceTraffic.Game.Actions namespace.");
				throw;
			}
		}

		

		public object GetActionResult(int playerId, int actionCode)
		{
			throw new NotImplementedException();
		}
	}
}
