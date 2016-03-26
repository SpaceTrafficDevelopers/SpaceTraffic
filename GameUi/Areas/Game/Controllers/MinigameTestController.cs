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
using SpaceTraffic.GameUi.Controllers;
using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.Game.Minigame;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    /// <summary>
    /// Minigame test controller
    /// 
    /// </summary>
    [Authorize]
    public class MinigameTestController : AbstractController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GameRequest(bool list)
        {
            object minigames = null;
            if (list)
                 minigames = GSClient.MinigameService.getMinigameDescriptorListByActionName("TestAction", getCurrentPlayerId());
            else
                minigames = GSClient.MinigameService.getMinigameDescriptorByActionName("TestAction", getCurrentPlayerId());

            if (minigames != null)
                Session["minigame"] = minigames;

            return RedirectToAction("");
        }

        public ActionResult AddPlayer(int minigameId)
        {
            this.GSClient.MinigameService.addPlayer(minigameId, this.getCurrentPlayerId());
            
            return RedirectToAction("");
        }
    }
}