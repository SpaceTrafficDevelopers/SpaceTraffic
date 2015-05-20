using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Models.Ui;
using SpaceTraffic.GameUi.GameServerClient;
using SpaceTraffic.Entities.PublicEntities;
using NLog;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class LandController : TabsControllerBase
    {
        private readonly IGameServerClient GSClient = GameServerClientFactory.GetClientInstance();

        protected override void BuildTabs()
        {
            int baseId = Convert.ToInt32(Request.QueryString["baseId"]);/* getting parameter from url */
            string starSystem = Request.QueryString["starSystemName"];/* getting parameter from url */

            this.Tabs.AddTab("Overview", title: "Overview of the land/buildings available to you on this base.", partialViewName: "_Overview", partialViewModel: new { Area = "Game" });
            this.Tabs.AddTab("Land", title: "Buy/Sell Land on this base.", partialViewName: "_Land", partialViewModel: new { Area = "Game" });
            this.Tabs.AddTab("Buildings", title: "Buy Buildings on this base.", partialViewName: "_Buildings", partialViewModel: new { Area = "Game" });
            this.Tabs.AddTab("Demolish", title: "Demolish Buildings on this base.", partialViewName: "_Demolish", partialViewModel: new { Area = "Game" });
        }

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public PartialViewResult Overview()
        {
            return GetTabView("Overview");
        }

        public PartialViewResult Land()
        {
            return GetTabView("Land");
        }

        public PartialViewResult Buildings()
        {
            return GetTabView("Buildings");
        }

        public PartialViewResult Demolish()
        {
            return GetTabView("Demolish");
        }


        [HttpPost]
        public ActionResult Land(int playerId, int landId, int width, int height)
        {
            object[] args = new object[3];
            args[0] = landId;
            args[1] = width;
            args[2] = height;
            GSClient.GameService.PerformAction(playerId, "BuySellLand", args);

            return RedirectToAction("Index", "Land");
        }

        [HttpPost]
        public ActionResult Buildings(int playerId, int LandId, string type, int width, int height, int x, int y)
        {
            object[] args = new object[6];
            args[0] = LandId;
            args[1] = type;
            args[2] = width;
            args[3] = height;
            args[4] = x;
            args[5] = y;
            GSClient.GameService.PerformAction(playerId, "BuyBuilding", args);

            return RedirectToAction("Index", "Land");
        }

        [HttpPost]
        public ActionResult Demolish(int playerId, int LandId, string BuildingId)
        {
            object[] args = new object[2];
            args[0] = LandId;
            args[1] = BuildingId;
            GSClient.GameService.PerformAction(playerId, "DemolishBuilding", args);

            return RedirectToAction("Index", "Land");
        }
    }
}