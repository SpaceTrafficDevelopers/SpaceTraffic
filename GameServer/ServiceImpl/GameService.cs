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
	public class GameService : IGameService
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

		public Entities.Achievements GetAchievements()
		{
			return GS.CurrentInstance.World.Achievements;
		}

		public Entities.TAchievement GetAchievementById(int id)
		{
			return GameServer.CurrentInstance.World.GetAchievementById(id);
		}

		public Entities.ExperienceLevels GetExperienceLevels()
		{
			return GameServer.CurrentInstance.World.ExperienceLevels;
		}

		/// <summary>
		/// Returns player from database.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <returns></returns>
		public Player GetPlayer(int playerId) {
			return GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId);
		}

		public List<TAchievement> GetUnviewedAchievements(int playerId)
		{

			List<TAchievement> result = new List<TAchievement>();
			Player player = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId);

			if (player != null)
			{
				EarnedAchievementDAO earnedDao = (EarnedAchievementDAO)GameServer.CurrentInstance.Persistence.GetEarnedAchievementDAO();
				var unviewedAchievements = earnedDao.GetUnviewedAchievementsByPlayerId(playerId);

				foreach (EarnedAchievement earnedAchv in unviewedAchievements)
				{
					result.Add(GetAchievementById(earnedAchv.AchievementId));
					earnedAchv.IsJustEarned = false;

					// update earned status in DB
					earnedDao.UpdateEarnedAchievementById(earnedAchv);
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the earned achievements of specified player.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <returns></returns>
		public List<EarnedAchievement> GetEarnedAchievements(int playerId)
		{

			EarnedAchievementDAO earnedDao = (EarnedAchievementDAO)GameServer.CurrentInstance.Persistence.GetEarnedAchievementDAO();
			return earnedDao.GetEarnedAchievementsByPlayerId(playerId);
		}

		public List<int> GetAllEarnedAchievementsIndexes(string playerName)
		{

			List<int> result = new List<int>();
			Player player = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerByName(playerName);
			var earnedAchievements = from achv in player.EarnedAchievements
									 select achv;

			foreach (EarnedAchievement earnedAchv in earnedAchievements)
			{
				result.Add(earnedAchv.AchievementId);

			}

			//TODO DIWA Static data for purpose of achievement view test. Remove before use. Achivement id = 5 appears as unlocked (earned) at app view.
			//begin
			result.Clear();
			result.Add(5);
			//end

			return result;
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


        public int CreatePathPlan(int playerId, int spaceShipId)
        {
            IPathPlanEntityDAO pped = GameServer.CurrentInstance.Persistence.GetPathPlanEntityDAO();

            PathPlanEntity plan = new PathPlanEntity();
            plan.PlayerId = playerId;
            plan.SpaceShipId = spaceShipId;

            return pped.InsertPathPlan(plan);
        }

        public int AddPlanItem(int pathPlanId, string solarSystem, bool isPlanet, string index, int sequenceNumber)
        {
            IPlanItemEntityDAO pied = GameServer.CurrentInstance.Persistence.GetPlanItemEntityDAO();

            PlanItemEntity item = new PlanItemEntity();
            item.PathPlanId = pathPlanId;
            item.SolarSystem = solarSystem;
            item.IsPlanet = isPlanet;
            item.Index = index;
            item.SequenceNumber = sequenceNumber;

            return pied.InsertPlanItem(item);
        }

        public bool AddPlanAction(int planItemId, int sequenceNumber, int playerId, string actionName, params object[] actionArgs)
        {
            IPlanActionDAO pad = GameServer.CurrentInstance.Persistence.GetPlanActionDAO();
            PlanAction pa = new PlanAction() { PlanItemId = planItemId, SequenceNumber = sequenceNumber };

            try
            {
                IPlannableAction action = Activator.CreateInstance(Type.GetType("SpaceTraffic.Game.Actions." + actionName)) as IPlannableAction;
                
                if (action == null)
                    throw new ActionNotFoundException(
                        String.Format("Action class with name: {0} does not implements IGameAction.",
                        actionName));

                action.ActionArgs = actionArgs;
                action.PlayerId = playerId;
                
                pa.ActionType = action.GetType().ToString();
                pa.GameAction = PlannerConverter.convertPlannableAction(action);

                if(pad.InsertPlanAction(pa))
                    return true;

                return false;
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine("Action file with name " + actionName + " was not found in SpaceTraffic.Game.Actions namespace.");
                throw;
            } 
        }

        public bool StartPathPlan(int pathPlanId)
        {
            IPathPlanEntityDAO pped = GameServer.CurrentInstance.Persistence.GetPathPlanEntityDAO();
            PathPlanEntity plan = pped.GetPathPlanById(pathPlanId);

            if (plan != null) {
                PlannerConverter converter = new PlannerConverter();
                IPathPlan pathPlan = converter.createPathPlan(plan);

                pathPlan.PlanFirstItem(GameServer.CurrentInstance);

                plan.IsPlanned = true;
                pped.UpdatePathPlanById(plan);

                return true;
            }

            return false;
        }
    }
}
