using SpaceTraffic.Engine;
using SpaceTraffic.Game.Events;
using SpaceTraffic.Game.Planner;
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
using System.Text;

namespace SpaceTraffic.Game.Actions
{
    /// <summary>
    /// Action for planning events from actions
    /// </summary>
    class PlanEvents : IGameAction
    {
        /// <summary>
        /// Time of replan
        /// </summary>
        private static readonly int REPLAN_TIME = 30;

        /// <summary>
        /// Number of tries. If the number of attempts is exceeded, action will not try it again.
        /// </summary>
        private int numberOfTries = 10;

        public object Result { get; set; }

        public GameActionState State { get; set; }

        public int PlayerId { get; set; }

        /// <summary>
        /// Plan for planning
        /// </summary>
        public IPathPlan plan { get; set; }

        /// <summary>
        /// Actual item of plan
        /// </summary>
        public PlanItem actualItem { get; set; }

        /// <summary>
        /// Action code
        /// </summary>
        public int ActionCode { get; set; }

        public object[] ActionArgs { get; set; }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            Result = "Plánují se akce";
            getArgumentsFromActionArgs();

            if (plan == null || plan.Count == 0 || actualItem == null || !plan.Contains(actualItem))
            {
                State = GameActionState.FINISHED;
                return;
            }

            if(!checkActions(actualItem) && numberOfTries > 0)
            {
                replanAction(gameServer);
                State = GameActionState.PREPARED;
                return;
            }

            plan.planEventsForNextItem(actualItem, gameServer);
            
            State = GameActionState.FINISHED;
        }

        /// <summary>
        /// Replan action and decrase number of tries.
        /// </summary>
        /// <param name="gameServer">Instance of game server.</param>
        private void replanAction(IGameServer gameServer)
        {
            numberOfTries--;
            gameServer.Game.PlanEvent(this, gameServer.Game.currentGameTime.Value.AddSeconds(REPLAN_TIME));
        }

        /// <summary>
        /// Check state of action.
        /// </summary>
        /// <param name="item">Instance of plan item</param>
        /// <returns>Value if actions is finished or not.</returns>
        private bool checkActions(PlanItem item)
        {
            foreach(IGameAction action in item.Actions)
            {
                if (action.State == GameActionState.PLANNED || action.State == GameActionState.PREPARED)
                    return false;
            }
            return true;
        }

        private void getArgumentsFromActionArgs()
        {
            plan = (IPathPlan)ActionArgs[0];
            actualItem = (PlanItem)ActionArgs[1];
        }
    }
}
