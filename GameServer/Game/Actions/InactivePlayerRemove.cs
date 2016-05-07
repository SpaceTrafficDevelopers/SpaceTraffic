/**
Copyright 2016 FAV ZCU

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
using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Actions
{
    [Serializable]
    class InactivePlayerRemove : IPlannableAction
    {
        /// <summary>
        /// Wait time in seconds
        /// </summary>
        private const int WAIT_TIME = 172800;

        /// <summary>
        /// Arguments connected with concreate action
        /// </summary>
        public object[] ActionArgs { get; set; }

        /// <summary>
        /// Unique action identifier
        /// </summary>
        public int ActionCode { get; set; }

        /// <summary>
        /// Duration of action.
        /// </summary>
        public double Duration
        {
            get { return WAIT_TIME; }
        }

        /// <summary>
        /// ID of player bound to this action
        /// </summary>
        public int PlayerId { get; set; }

        public object Result { get; set; }

        public GameActionState State { get; set; }

        /// <summary>
        /// Determines if player removal is planed
        /// </summary>
        private bool RemoveActive { get; set; }

        private string PlayerShowName { get; set; }

        /// <summary>
        /// Performs action
        /// </summary>
        /// <param name="gameServer">game server instance</param>
        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();
            State = GameActionState.PLANNED;
            Result = "Vyčkávání před odstraněním hráče: " + PlayerShowName;

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);

            if(player == null)
            {
                Result = string.Format("Hráč {0} již neexistuje.", PlayerShowName);
                State = GameActionState.FAILED;
                return;
            }

            if (player.IsEmailConfirmed)
            {
                Result = string.Format("Hráč {0} neodstraněn. Aktivace proběhla úspěšně", PlayerShowName);
                State = GameActionState.FINISHED;
                return;
            }

            if (RemoveActive)
            {
                

                if (!gameServer.Persistence.GetPlayerDAO().RemovePlayerById(PlayerId))
                {
                    Result = "Změny se nepovedlo zapsat do databáze.";
                    State = GameActionState.FAILED;
                }
                else
                {
                    Result = string.Format("Hráč {0} byl odstraněn.", PlayerShowName);
                    State = GameActionState.FINISHED;
                }
            }
            else
            {
                RemoveActive = true;
                gameServer.Game.PlanEvent(this, gameServer.Game.currentGameTime.Value.AddSeconds(Duration));
            }
        }

        /// <summary>
        /// Gets data from arguments
        /// </summary>
        private void getArgumentsFromActionArgs()
        {
            if (ActionArgs != null && ActionArgs.Count() == 1)
            {
                PlayerShowName = ActionArgs[0].ToString();
            }
        }
    }
}
