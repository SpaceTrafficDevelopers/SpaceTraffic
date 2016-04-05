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
    /// Ajax handler for performing actions by spaceship cargo finder game.
    /// </summary>
    public class PerformActionSpaceshipCargoFinder : IAjaxHandleable
    {
        /// <summary>
        /// Method for handeling ajax request.
        /// </summary>
        /// <param name="data">data (required minigameId and action)</param>
        /// <param name="controller">controller</param>
        /// <returns>result or null</returns>
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

        /// <summary>
        /// Method for calling action method by action name.
        /// </summary>
        /// <param name="gameId">minigame id</param>
        /// <param name="data">data</param>
        /// <param name="controller">controller</param>
        /// <returns>result or null</returns>
        private object callMethod(int gameId, dynamic data, AbstractController controller)
        {
            string action = data["action"];

            if (action.CompareTo("addScore") == 0)
                return addScore(gameId, data, controller);

            else if (action.CompareTo("checkCollision") == 0)
                return checkCollision(gameId, data, controller);

            else if (action.CompareTo("updateRequest") == 0)
                return updateRequest(gameId, controller);

            else if (action.CompareTo("removeGame") == 0)
                return removeGame(gameId, controller);

            else
                return null;
        }

        /// <summary>
        /// Action method for check snake collision.
        /// </summary>
        /// <param name="gameId">minigame id</param>
        /// <param name="data">data (required body as array snake body)</param>
        /// <param name="controller">controller</param>
        /// <returns>returns result</returns>
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
            handleResult(result, gameId, controller, false);

            return result;
        }

        /// <summary>
        /// Action method for adding score.
        /// </summary>
        /// <param name="gameId">minigame id</param>
        /// <param name="data">data</param>
        /// <param name="controller">controller</param>
        /// <returns>result</returns>
        private object addScore(int gameId, dynamic data, AbstractController controller)
        {
            Result result = controller.GSClient.MinigameService.performActionLock(gameId, "addScore", true, null);
            handleResult(result, gameId, controller, true);

            return result;
        }

        /// <summary>
        /// Action method for update last time request and check if minigame exists.
        /// </summary>
        /// <param name="gameId">minigame id</param>
        /// <param name="controller">controller</param>
        /// <returns>result</returns>
        private object updateRequest(int gameId, AbstractController controller)
        {
            return controller.GSClient.MinigameService.checkMinigameLifeAndUpdateLastRequestTime(gameId);
        }

        /// <summary>
        /// Method for handle result for check collision and add score.
        /// </summary>
        /// <param name="result">result</param>
        /// <param name="gameId">minigame id</param>
        /// <param name="controller">controller</param>
        /// <param name="reward">true if player has to be rewarded</param>
        private void handleResult(Result result, int gameId, AbstractController controller, bool reward)
        {
            if ((bool)result.ReturnValue)
            {
                controller.GSClient.MinigameService.endGame(gameId);
                
                if (reward)
                    controller.GSClient.MinigameService.rewardPlayer(gameId, controller.getCurrentPlayerId());
                else
                    controller.GSClient.MinigameService.removeGame(gameId);
            }
        }

        /// <summary>
        /// Action method for remove game.
        /// </summary>
        /// <param name="gameId">minigame id</param>
        /// <param name="controller">controller</param>
        /// <returns>null</returns>
        private object removeGame(int gameId, AbstractController controller)
        {
            controller.GSClient.MinigameService.removeGame(gameId);

            return null;
        }
    
    }
}