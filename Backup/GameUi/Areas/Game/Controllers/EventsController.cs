using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class EventsController : TabsControllerBase
    {
        protected override void BuildTabs()
        {
            this.Tabs.AddTab("NewList", text: "New", title: "New events", partialViewName: "_New");
            this.Tabs.AddTab("AllList", text: "All", title: "All events.", partialViewName: "_All");
        }

        //
        // GET: /Events/

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }
        
        public ActionResult NewList()
        {
            return this.GetTabView("NewList");
        }

        public ActionResult AllList()
        {
            return this.GetTabView("AllList");
        }

    }
}
