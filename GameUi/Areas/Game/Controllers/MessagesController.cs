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
			return PartialView(INDEX_VIEW);
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
