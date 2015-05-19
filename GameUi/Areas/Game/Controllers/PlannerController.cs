using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Extensions;
using SpaceTraffic.GameUi.Controllers;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{

    [Authorize]
    public class PlannerController : AbstractController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestPlanner()
        {

            int pathPlanID = GSClient.GameService.CreatePathPlan(1, 1, true);

            if(pathPlanID == -1)
                return RedirectToAction("").Error("Vytváření plánu se nepovedlo. Máš loď?");

            int firstItemID = GSClient.GameService.AddPlanItem(pathPlanID,"Proxima Centauri",true,"Proxima Centauri 1",1);
            
            object[] args = new object[]{
                    "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    10,
                    "TraderCargoDAO",
                    1
            };

            bool cargoBuyResult = GSClient.GameService.AddPlanAction(firstItemID, 1, 1, "CargoBuy", args);

            args = new object[]{
                   "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    30,
                    1
            };

            //bool shipRepairResult = GSClient.GameService.AddPlanAction(firstItemID,2,1,"ShipRepair",args);

            int secondItemID = GSClient.GameService.AddPlanItem(pathPlanID, "Proxima Centauri", false, "0", 2);

            int thirdItemID = GSClient.GameService.AddPlanItem(pathPlanID, "Solar system", true, "Sol 1", 3);

            int fourthItemID = GSClient.GameService.AddPlanItem(pathPlanID, "Proxima Centauri", false, "0", 4);

            int fifthItemID = GSClient.GameService.AddPlanItem(pathPlanID, "Proxima Centauri", true, "Proxima Centauri 1", 5);

            args = new object[]{
                   "Proxima Centauri",
                    "Proxima Centauri 1",
                    1,
                    30,
                    1
            };

            //bool shipRepairResult1 = GSClient.GameService.AddPlanAction(fifthItemID, 1, 1, "ShipRepair", args);
            
            args = new object[]{
                    "Solar system",
                    "Sol 1",
                    1,
                    200,
                    1
            };

            bool shipRefuelResult = GSClient.GameService.AddPlanAction(thirdItemID, 1, 1, "ShipRefuel", args);

            args = new object[]{
                    "Solar system",
                    "Sol 1",
                    1,
                    10,
                    "TraderCargoDAO",
                    1,
                    1
            };

            bool cargoSellResult = GSClient.GameService.AddPlanAction(thirdItemID, 2, 1, "CargoSell", args);


            bool startPlanResult = GSClient.GameService.StartPathPlan(pathPlanID);

            if(!startPlanResult)
                return RedirectToAction("").Error("Při plánování nastala chyba.");

            return RedirectToAction("").Success("Naplánováno jak nikdy :D");
        }
       
    }
}