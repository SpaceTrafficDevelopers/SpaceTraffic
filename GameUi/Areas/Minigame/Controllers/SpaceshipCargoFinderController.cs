using SpaceTraffic.Game.Minigame;
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
using SpaceTraffic.GameUi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Areas.Minigame.Controllers
{
    public class SpaceshipCargoFinderController : AbstractController
    {
        public ActionResult Index(int gameId)
        {
            Result result = GSClient.MinigameService.performAction(gameId, "getGameInfo", null);
            
            if (result.State == ResultState.FAILURE){
                ViewBag.Error = result;
                
                return View();
            }

            SpaceshipCargoFinderGameInfo info = result.ReturnValue as SpaceshipCargoFinderGameInfo;

            ViewBag.GameName = "Spaceship cargo finder";
            ViewBag.GameInfo = result.ReturnValue;
            ViewBag.StartDescription = string.Format("Kapitánovi se rozsypal náklad. " +
                    "Pomož mu nasbírat alespoň {0} jednotek nákladu a dostaneš odměnu {1} kreditů.",
                    info.WinScore, info.RewardCount);

            return View();
        }

        [HttpGet]
        public JsonResult EndGame(int minigameId)
        {
            int gameId = minigameId;

            GSClient.MinigameService.endGame(gameId);
            GSClient.MinigameService.removeGame(gameId);

            return null;
        }
    }
}