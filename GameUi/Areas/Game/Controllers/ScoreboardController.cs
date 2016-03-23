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
			return PartialView(INDEX_VIEW);
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
