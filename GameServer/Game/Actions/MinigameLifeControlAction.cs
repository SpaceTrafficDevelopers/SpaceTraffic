﻿/**
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
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Events;
using SpaceTraffic.Game.Minigame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Actions
{
    /// <summary>
    /// Minigame check life action. When minigame is not alive, than it is end and remove.
    /// </summary>
    public class MinigameLifeControlAction : IGameAction
    {
        /// <summary>
        /// Next control time in minutes.
        /// </summary>
        private const int NEXT_CONTROL_TIME = 1;

        public GameActionState State { get; set; }

        public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        public object Result { get; set; }

        //0 -> minigame id
        public object[] ActionArgs { get; set; }

        public void Perform(IGameServer gameServer)
        {
            int minigameId = int.Parse(this.ActionArgs[0].ToString());
            bool isAlive = gameServer.Minigame.checkMinigameLife(minigameId);

            if (isAlive){
                gameServer.Game.PlanEvent(this, gameServer.Game.currentGameTime.Value.AddMinutes(NEXT_CONTROL_TIME));
                this.Result = string.Format("MinigameLifeControlAction: Minigame with id {0} is still alive. Action has been re-planned.", minigameId);
            }
            else
            {
                gameServer.Minigame.endGame(minigameId);
                gameServer.Minigame.removeGame(minigameId);
                this.Result = string.Format("MinigameLifeControlAction: Minigame with id {0} is not alive and it has been removed.", minigameId);
            }

            this.State = GameActionState.FINISHED;
        }
    }
}
