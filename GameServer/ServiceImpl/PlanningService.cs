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
	public class PlanningService : IPlanningService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();



		/// <summary>
		/// Create path plan and insert to database.
		/// </summary>
		/// <param name="playerId">Identification number of player.</param>
		/// <param name="spaceShipId">Identification number of spaceship.</param>
		/// <param name="isCycled">Cycled of plan.</param>
		/// <returns>Value of inserting to database.</returns>
		public int CreatePathPlan(int playerId, int spaceShipId, bool isCycled)
		{
			IPathPlanEntityDAO pped = GameServer.CurrentInstance.Persistence.GetPathPlanEntityDAO();

			PathPlanEntity plan = new PathPlanEntity();
			plan.PlayerId = playerId;
			plan.SpaceShipId = spaceShipId;
			plan.IsCycled = isCycled;

			return pped.InsertPathPlan(plan);
		}

		/// <summary>
		/// Add plan item entity to database.
		/// </summary>
		/// <param name="pathPlanId"></Identification number of path plan.</param>
		/// <param name="solarSystem">Solar system.</param>
		/// <param name="isPlanet">Value if item is planet or not.</param>
		/// <param name="index">Index of item.</param>
		/// <param name="sequenceNumber">Sequence number of item.</param>
		/// <returns>Result of inserting to database.</returns>
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

		/// <summary>
		/// Add plan action to database.
		/// </summary>
		/// <param name="planItemId">Identification number of path plan.</param>
		/// <param name="sequenceNumber">Sequence number of action.</param>
		/// <param name="playerId">Identification number of player.</param>
		/// <param name="actionName">Name of action.</param>
		/// <param name="actionArgs">Arguments of action.</param>
		/// <returns>Result of inserting to database.</returns>
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

				if (pad.InsertPlanAction(pa))
					return true;

				return false;
			}
			catch (System.IO.FileNotFoundException e)
			{
				Console.WriteLine("Action file with name " + actionName + " was not found in SpaceTraffic.Game.Actions namespace.");
				throw;
			}
		}

		/// <summary>
		/// Start path plan. Update path plan to planned to database.
		/// </summary>
		/// <param name="pathPlanId">Identification number of path plan.</param>
		/// <returns>Result of planning. Is null or empty if everything is ok.</returns>
		public string StartPathPlan(int pathPlanId)
		{
			IPathPlanEntityDAO pped = GameServer.CurrentInstance.Persistence.GetPathPlanEntityDAO();
			PathPlanEntity plan = pped.GetPathPlanById(pathPlanId);

			if (plan != null)
			{
				PlannerConverter converter = new PlannerConverter();
				IPathPlan pathPlan = converter.createPathPlan(plan);

				string planResult = pathPlan.PlanFirstItem(GameServer.CurrentInstance);
				if(!String.IsNullOrEmpty(planResult)){
					return planResult;
				}

				plan.IsPlanned = true;
				pped.UpdatePathPlanById(plan);

				return null;
			}

			return "Navigační počítač je rozbitý a plán neexistuje. (kontaktujte vývojáře)";
		}
	}
}
