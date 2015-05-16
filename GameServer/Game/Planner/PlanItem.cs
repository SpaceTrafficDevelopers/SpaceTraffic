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

        public List<IPlannableAction> Actions { get; set; }

        public bool hasActions()
        {
            if (this.Actions == null || this.Actions.Count == 0)
                return false;
            return true;
        }

        public PlanItem()
        {
            this.Actions = new List<IPlannableAction>();
        }

    }
}
