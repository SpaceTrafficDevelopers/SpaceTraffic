using SpaceTraffic.Engine;
using SpaceTraffic.Game.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Planner
{
    interface IPathPlan : IList<PlanItem>
    {
        NavPath getNavPath();

        void SolvePath(Spaceship sh, double startTime);

        List<IGameEvent> getEventsFromPath();
    }
}
