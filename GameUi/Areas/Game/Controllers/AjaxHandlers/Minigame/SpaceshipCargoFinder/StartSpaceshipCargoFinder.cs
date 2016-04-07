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
    /// <summary>
    /// Ajax handler for spaceship cargo finder start dialog.
    /// </summary>
    public class StartSpaceshipCargoFinder : IAjaxHandleable
    {
        /// <summary>
        /// Method for handeling ajax request. This adding player into game and start game.
        /// </summary>
        /// <param name="data">data (required minigame id)</param>
        /// <param name="controller">controller</param>
        /// <returns>result or null</returns>
        public object handleRequest(dynamic data, AbstractController controller)
        {
            if (data.ContainsKey("minigameId"))
            {
                int gameId = int.Parse(data["minigameId"].ToString());
                Result addPlayerResult = controller.GSClient.MinigameService.addPlayer(gameId, controller.getCurrentPlayerId());

                if(addPlayerResult.State == ResultState.FAILURE)
                    return addPlayerResult;

                return controller.GSClient.MinigameService.startGame(gameId);
            }

            return null;
        }
    }
}