using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Models.Ui;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class MessagesController : TabsControllerBase
    {
        protected override void BuildTabs()
        {
            this.Tabs.AddTab("Received", title: "Received messages", partialViewName: "_Received");
            this.Tabs.AddTab("NewMessage", "New", "New message.", "_NewMessage");
            this.Tabs.AddTab("Sent", title: "Sent messages.", partialViewName: "_Sent");
        }

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public PartialViewResult Received()
        {
            return this.GetTabView("Received");
        }

        public PartialViewResult NewMessage()
        {
            return this.GetTabView("NewMessage");
        }

        public PartialViewResult Sent()
        {
            return this.GetTabView("Sent");
        }
    }
}
