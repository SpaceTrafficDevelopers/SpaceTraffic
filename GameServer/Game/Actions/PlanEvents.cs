﻿using SpaceTraffic.Engine;
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
    class PlanEvents : IGameAction
    {
        private string result = "Plánují se akce";

        public object Result
        {
            get { return new { result = this.result }; }
        }
        public GameActionState State { get; set; }

        public int PlayerId { get; set; }

        public PathPlan plan { get; set; }

        public PlanItem actualItem { get; set; }

        public Spaceship ship { get; set; }

        public int ActionCode { get; set; }

        public object[] ActionArgs { get; set; }

        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();

            if (plan == null || plan.Count == 0 || actualItem == null || !plan.Contains(actualItem) || ship == null)
                return;

            if(!checkActions(actualItem))
            {
                replanAction(gameServer);
                return;
            }

            plan.planEventsForNextItem(actualItem, gameServer,ship);

        }

        private void replanAction(IGameServer gameServer)
        {
            ShipEvent newPlanEvent = new ShipEvent();
                newPlanEvent.BoundAction = this;
                GameTime eventTime = new GameTime();
                eventTime.Value =  gameServer.Game.currentGameTime.Value;
                eventTime.Value.AddSeconds(10);
                newPlanEvent.PlannedTime = eventTime;

                gameServer.Game.PlanEvent(newPlanEvent);
        }

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
            plan = (PathPlan)ActionArgs[0];
            actualItem = (PlanItem)ActionArgs[1];
            ship = (Spaceship)ActionArgs[2];
        }
    }
}
