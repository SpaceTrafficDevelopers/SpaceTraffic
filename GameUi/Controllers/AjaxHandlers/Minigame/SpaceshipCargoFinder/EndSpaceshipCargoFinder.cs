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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaceTraffic.GameUi.Controllers.AjaxHandlers
{
    public class EndSpaceshipCargoFinder : IAjaxHandleable
    {
        public object handleRequest(dynamic data, AbstractController controller)
        {
            if (data.ContainsKey("minigameId"))
            {
                int gameId = int.Parse(data["minigameId"].ToString());
                controller.GSClient.MinigameService.endGame(gameId);

                if (!data.ContainsKey("force")) 
                    controller.GSClient.MinigameService.rewardPlayer(gameId, controller.getCurrentPlayerId());

                return controller.GSClient.MinigameService.removeGame(gameId);
            }

            return null;
        }
    }
}