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
using SpaceTraffic.Game.Actions.Ships;

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

        public bool PlayerHasSpaceShip(int playerId, int spaceShipId) 
        {
            SpaceShip spaceShip = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
            if (spaceShip == null)
                return false;
            return spaceShip.PlayerId == playerId;
        }

        public bool PlayerHasEnoughCargo(string loadingPlace, int cargoLoadEntityId, int cargoCount) 
        {
            ICargoLoadDao loading = GS.CurrentInstance.Persistence.GetCargoLoadDao(loadingPlace);
            ICargoLoadEntity cargo = loading.GetCargoByID(cargoLoadEntityId);
            if (cargo == null)
                return false;
            int count = cargo.CargoCount;
            return count >= cargoCount;
        }

        public bool PlayerHasEnoughCargoOnSpaceShip(int spaceShipId, int cargoLoadEntityId, int cargoCount)
        {
            SpaceShip spaceShip = GS.CurrentInstance.Persistence.GetSpaceShipDAO().GetSpaceShipById(spaceShipId);
            ICargoLoadEntity cargo = GS.CurrentInstance.Persistence.GetSpaceShipCargoDAO().GetCargoByID(cargoLoadEntityId);
            
            if (cargo == null || spaceShipId != cargo.CargoOwnerId)
                return false;
            
            int count = cargo.CargoCount;
            return count >= cargoCount;
        }

        public bool TraderHasEnoughCargo(int traderId, int cargoLoadEntityId, int cargoCount) 
        {
            TraderCargo tc = (TraderCargo) GS.CurrentInstance.Persistence.GetTraderCargoDAO().GetCargoByID(cargoLoadEntityId);
            if (traderId != tc.CargoOwnerId || tc == null)
                return false;
            int count = tc.CargoCount;
            return count >= cargoCount;
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


        public bool TestPlanner()
        {
            PathPlan plan = new PathPlan(1);
            PlanItem item1 = new PlanItem();
            
            NavPoint firstPoint = new NavPoint();
            firstPoint.Location = GameServer.CurrentInstance.World.Map["Proxima Centauri"].Planets["Proxima Centauri 1"];

            item1.Place = firstPoint;
            CargoBuy cba = new CargoBuy();

            cba.PlayerId = 1;
            cba.ActionArgs = new object[]{
                    "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    10,
                    "TraderCargoDAO",
                    1
            };

            item1.Actions.Add(cba);

            ShipRepair srpa = new ShipRepair();

            srpa.PlayerId = 1;
            srpa.ActionArgs = new object[]{
                   "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    30,
                    1
            };
            item1.Actions.Add(srpa);


            NavPoint hole = new NavPoint();
            hole.Location = GameServer.CurrentInstance.World.Map["Proxima Centauri"].WormholeEndpoints[0];

            PlanItem holik = new PlanItem();
            holik.Place = hole;

           

            NavPoint secondPoint = new NavPoint();
            secondPoint.Location = GameServer.CurrentInstance.World.Map["Solar system"].Planets["Sol 1"];

            PlanItem item2 = new PlanItem();

            item2.Place = secondPoint;
            
            ShipRefuel sra = new ShipRefuel();

            sra.PlayerId = 1;
            sra.ActionArgs = new object[]{
                    "Solar system",
                    "Sol 1",
                    1,
                    200,
                    1
            };

            item2.Actions.Add(sra);

            CargoSell csa = new CargoSell();

            csa.PlayerId = 1;
            csa.ActionArgs = new object[]{
                    "Solar system",
                    "Sol 1",
                    1,
                    10,
                    "TraderCargoDAO",
                    1,
                    1
            };

            item2.Actions.Add(csa);
            //item2.Actions.Add(sra);

            plan.Add(item1);
            plan.Add(holik);
            plan.Add(item2);

            Spaceship sh = new Spaceship(1, "Ship de la Test");

            sh.MaxSpeed = 20;

            plan.PlanFirstItem(GameServer.CurrentInstance, sh);

            return true;
        }
    }
}
