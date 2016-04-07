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
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Extensions;
using SpaceTraffic.GameUi.Controllers;
using SpaceTraffic.Entities;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    /// <summary>
    /// Controller for planner.
    /// </summary>
    [Authorize]
    public class PlannerController : AbstractController
    {
        public ActionResult Index()
        {
			return PartialView();
        }

        /// <summary>
        /// Create test path for planner. Test is called by link in user interface.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult TestPlanner()
        {

            int pathPlanID = GSClient.PlanningService.CreatePathPlan(1, 1, true);

            if(pathPlanID == -1)
                return RedirectToAction("").Error("Vytváření plánu se nepovedlo. Máš loď?");

            int firstItemID = GSClient.PlanningService.AddPlanItem(pathPlanID,"Proxima Centauri",true,"Proxima Centauri 1",1);
            
            object[] args = new object[]{
                    "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    10,
                    "TraderCargoDAO",
                    1
            };

            bool cargoBuyResult = GSClient.PlanningService.AddPlanAction(firstItemID, 1, 1, "CargoBuy", args);

            args = new object[]{
                   "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    30,
                    1
            };

            //bool shipRepairResult = GSClient.GameService.AddPlanAction(firstItemID,2,1,"ShipRepair",args);

            int secondItemID = GSClient.PlanningService.AddPlanItem(pathPlanID, "Proxima Centauri", false, "0", 2);

			int thirdItemID = GSClient.PlanningService.AddPlanItem(pathPlanID, "Solar system", true, "Sol 1", 3);

			int fourthItemID = GSClient.PlanningService.AddPlanItem(pathPlanID, "Proxima Centauri", false, "0", 4);

			int fifthItemID = GSClient.PlanningService.AddPlanItem(pathPlanID, "Proxima Centauri", true, "Proxima Centauri 1", 5);

            args = new object[]{
                   "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    30,
                    1
            };

			//bool shipRepairResult1 = GSClient.PlanningService.AddPlanAction(fifthItemID, 1, 1, "ShipRepair", args);
            
            args = new object[]{
                    "Solar system",
                    "Sol 1",
                    1,
                    200,
                    1
            };

			bool shipRefuelResult = GSClient.PlanningService.AddPlanAction(thirdItemID, 1, 1, "ShipRefuel", args);

            args = new object[]{
                    "Solar system",
                    "Sol 1",
                    1,
                    10,
                    "TraderCargoDAO",
                    1,
                    1
            };

			bool cargoSellResult = GSClient.PlanningService.AddPlanAction(thirdItemID, 2, 1, "CargoSell", args);


			string startPlanResult = GSClient.PlanningService.StartPathPlan(pathPlanID);

			if (String.IsNullOrEmpty(startPlanResult))
				return RedirectToAction("").Success("Naplánováno jak nikdy :D");

			return RedirectToAction("").Error(startPlanResult);
				
        }

		
		//GET /Planner/FlyTo
		public ActionResult FlyTo(int shipId)
		{

			SpaceShip ship = GSClient.ShipsService.GetDetailedSpaceShip(shipId);
			var partial = PartialView("_FlyTo");
			partial.ViewBag.ship = ship;
			return partial;
		}
       
    }
}