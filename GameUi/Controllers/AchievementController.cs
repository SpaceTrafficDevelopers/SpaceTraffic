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
using SpaceTraffic.GameUi.GameServerClient;

namespace SpaceTraffic.GameUi.Controllers
{
    public class AchievementController : AbstractController
    {
        private readonly IGameServerClient GSClient = GameServerClientFactory.GetClientInstance();

        //
        // GET: /GameServer/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEarnedAchievements()
        {
            string playerName = HttpContext.User.Identity.Name;
            JsonResult result = Json(GSClient.GameService.GetEarnedAchievements(playerName), JsonRequestBehavior.AllowGet);
            return result;
        }

    }
}