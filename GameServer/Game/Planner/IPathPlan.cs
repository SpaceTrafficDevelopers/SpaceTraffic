﻿using SpaceTraffic.Engine;
using SpaceTraffic.Game.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Planner
{
    public interface IPathPlan : IList<PlanItem>
    {
        int PlayerID { get; set; }
        Spaceship ship { get; set; }
        bool IsCycled { get; set; }
        NavPath getNavPath();
        NavPath getPathBetweenTwoItems(PlanItem source, PlanItem dest);
        void SolvePath(double startTime);
        List<IGameEvent> getEventsFromPath();
        void planEventsForNextItem(PlanItem item, IGameServer gameServer);
        void PlanFirstItem(IGameServer gameServer);
        void PlanFlightBetweenPoints(PlanItem depart, PlanItem dest, IGameServer gameServer, Spaceship ship);


    }
}
