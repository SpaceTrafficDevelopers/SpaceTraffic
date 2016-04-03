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
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Actions
{
    /// <summary>
    /// Action for check if all minigames are alive.
    /// </summary>
    public class MinigameAllGamesLifeAction : IGameAction
    {
        /// <summary>
        /// Next control time in minutes.
        /// </summary>
        private const int NEXT_CONTROL_TIME = 30;

        public GameActionState State { get; set; }

        public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        public object Result { get; set; }

        public object[] ActionArgs { get; set; }

        public void Perform(IGameServer gameServer)
        {
            gameServer.Minigame.checkLifeOfAllMinigames();
            gameServer.Game.PlanEvent(this, gameServer.Game.currentGameTime.Value.AddMinutes(NEXT_CONTROL_TIME));

            this.State = GameActionState.FINISHED;
        }
    }
}
