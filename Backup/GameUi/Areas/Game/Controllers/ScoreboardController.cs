using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class ScoreboardController : TabsControllerBase
    {
        protected override void BuildTabs()
        {
            this.Tabs.AddTab("TopTen", text: "Top 10", partialViewName: "_TopTen");
            this.Tabs.AddTab("DailyScore", text: "Daily score", partialViewName: "_DailyScore");
            this.Tabs.AddTab("TopScore", text: "Top score", partialViewName: "_TopScore");
        }

        //
        // GET: /Scoreboard/

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public PartialViewResult TopTen()
        {
            return GetTabView("TopTen");
        }

        public PartialViewResult DailyScore()
        {
            return GetTabView("DailyScore");
        }

        public PartialViewResult TopScore()
        {
            return GetTabView("TopScore");
        }

    }
}
