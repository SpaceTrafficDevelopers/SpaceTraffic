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
