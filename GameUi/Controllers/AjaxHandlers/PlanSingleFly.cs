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

using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Extensions;

namespace SpaceTraffic.GameUi.Controllers.AjaxHandlers
{
	public class PlanSingleFly : IAjaxHandleable
	{

		/// <summary>
		/// Handles request about ship planning.
		/// </summary>
		/// <param name="data">The data (empty)</param>
		/// <param name="controller">The controller.</param>
		/// <returns></returns>
		public object handleRequest(dynamic data, AbstractController controller)
		{
			int playerId = controller.getCurrentPlayerId();
			int shipId = data["shipId"];
			var fromStarSystem = data["fromStarSystem"];
			var fromPlanet = data["fromPlanet"];
			var toStarSystem = data["toStarSystem"];
			var toPlanet = data["toPlanet"];
			var wormholes = data["wormholes"];
			int pathPlanID = controller.GSClient.PlanningService.CreatePathPlan(controller.getCurrentPlayerId(), shipId, false);
			if (pathPlanID == -1)
				return new EmptyResult().Error("Vytváření plánu se nepovedlo. Máš loď?");

			int position = 1;

			controller.GSClient.PlanningService.AddPlanItem(pathPlanID, fromStarSystem, true, fromPlanet, position);
			foreach (var wormhole in wormholes) {
				var starsystem = wormhole["starsystem"];
				var index = wormhole["index"];
				position++;
				controller.GSClient.PlanningService.AddPlanItem(pathPlanID, starsystem, false, index + "", position);
			}


			controller.GSClient.PlanningService.AddPlanItem(pathPlanID, toStarSystem, true, toPlanet, 2);

			object[] args = new object[]{
                    toStarSystem,
					toPlanet,
					shipId
            };
			SpaceShip ship = controller.GSClient.ShipsService.GetSpaceShip(shipId);
			if (!controller.controlShipAccess(ship))
			{
				return new EmptyResult().Error(controller.ErrorMessage);
			}
			controller.GSClient.ShipsService.ChangeShipState(shipId, true, "Připravuje se k odletu.");

			string startPlanResult = controller.GSClient.PlanningService.StartPathPlan(pathPlanID);

			
			
			if (String.IsNullOrEmpty(startPlanResult))
			{
				return new EmptyResult().Success("Loď se vydává na svou cestu...");
			}
			else
			{
				controller.GSClient.ShipsService.ChangeShipState(shipId, true, "Loď měla problémy se startem.");
				return new EmptyResult().Error(startPlanResult);
			}
				

		}

	}
}