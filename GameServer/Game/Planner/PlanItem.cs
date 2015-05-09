using SpaceTraffic.Engine;
using SpaceTraffic.Game.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Planner
{
    public class PlanItem
    {
        public NavPoint Place { get; set; }

        public List<IGameAction> Actions { get; set; }


        public PlanItem()
        {
            this.Actions = new List<IGameAction>();
        }

    }
}
