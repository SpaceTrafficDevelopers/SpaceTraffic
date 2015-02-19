using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Models.Ui;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class ProfileController : TabsControllerBase
    {
        protected override void BuildTabs()
        {
            this.Tabs.AddTab("Overview", "Profile", "Profile overview", "_Overview");
            this.Tabs.AddTab("Personal", title: "Personal informations.", partialViewName: "_Personal");
            this.Tabs.AddTab("Settings", title: "Profile settings.", partialViewName: "_Settings");
            this.Tabs.AddTab("Achievements", title: "Achievements", partialViewName: "_Achievements");
        }

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public PartialViewResult Overview()
        {
            return GetTabView("Overview");
        }

        public PartialViewResult Personal()
        {
            return GetTabView("Personal");
        }

        public PartialViewResult Settings()
        {
            return GetTabView("Settings");
        }

        public PartialViewResult Achievements()
        {
            return GetTabView("Achievements");
        }
    }
}
