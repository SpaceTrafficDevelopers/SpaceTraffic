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
    public class PerformActionSpaceshipCargoFinder : IAjaxHandleable
    {
        public object handleRequest(dynamic data, AbstractController controller)
        {
            if (data.ContainsKey("minigameId"))
            {
                int gameId = int.Parse(data["minigameId"].ToString());

                if (data.ContainsKey("action"))
                    return callMethod(gameId, data, controller);
            }

            return null;
        }

        private object callMethod(int gameId, dynamic data, AbstractController controller)
        {
            string action = data["action"];

            if (action.CompareTo("addScore") == 0)
                return addScore(gameId, data, controller);
            else if (action.CompareTo("checkCollision") == 0)
                return checkCollision(gameId, data, controller);
            else if (action.CompareTo("updateRequest") == 0)
                return updateRequest(gameId, controller);
            else
                return null;
        }

        private object checkCollision(int gameId, dynamic data, AbstractController controller)
        {
            var bodyData = data["body"];

            List<Position> body = new List<Position>();

            for (int i = 0; i < bodyData.Length; i++)
            {
                Position p = new Position 
                { 
                    X = int.Parse(bodyData[i]["x"].ToString()), 
                    Y = int.Parse(bodyData[i]["y"].ToString())
                };

                body.Add(p);
            }

            Result result = controller.GSClient.MinigameService.performAction(gameId, "checkCollision", body);

            return handleResult(result, gameId, controller, false);
        }

        private object addScore(int gameId, dynamic data, AbstractController controller)
        {
            Result result = controller.GSClient.MinigameService.performActionLock(gameId, "addScore", true, null);

            return handleResult(result, gameId, controller, true);
        }

        private object updateRequest(int gameId, AbstractController controller)
        {
            return controller.GSClient.MinigameService.checkMinigameLifeAndUpdateLastRequestTime(gameId);
        }

        private object handleResult(Result result, int gameId, AbstractController controller, bool reward)
        {
            if (result.State == ResultState.FAILURE)
                return result;

            if ((bool)result.ReturnValue)
            {
                controller.GSClient.MinigameService.endGame(gameId);
                if(reward)
                    controller.GSClient.MinigameService.rewardPlayer(gameId, controller.getCurrentPlayerId());

                controller.GSClient.MinigameService.removeGame(gameId);

                return true;
            }

            return false;
        }
    
    }
}