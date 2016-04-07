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

namespace SpaceTraffic.GameUi.Controllers.AjaxHandlers
{
    public class MinigameStarter : IAjaxHandleable
    {
        /// <summary>
        /// Method for handlig MinigameStarter request. When data contains close attribute (true)
        /// then minigame session is removed. When data contains selectedId attribute then minigame
        /// is created and id is returned. When data does not contains any data and when Session contains
        /// minigame (list or id) than it is returned.
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="controller">controller</param>
        /// <returns></returns>
        public object handleRequest(dynamic data, AbstractController controller)
        {
            if (data.Count != 0) {
                if (data.ContainsKey("close") && data["close"]){
                    controller.Session.Remove("minigame");
                    
                    return null;
                }

                if(data.ContainsKey("selectedGameId")){
                    int gameId = int.Parse(data["selectedGameId"].ToString());
                    return controller.GSClient.MinigameService.createGame(gameId, false);
                }
            }

            if (controller.Session["minigame"] != null)
            {
                var minigame = controller.Session["minigame"];

                if (minigame != null) { 
                    return minigame;
                }
            }

            return null;
        }
    }
}