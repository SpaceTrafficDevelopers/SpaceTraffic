using SpaceTraffic.Engine;
using SpaceTraffic.Game.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Planner
{
    /// <summary>
    /// Class representing one item of path plan.
    /// </summary>
    public class PlanItem
    {
        /// <summary>
        /// Place where item is practised.
        /// </summary>
        public NavPoint Place { get; set; }

        /// <summary>
        /// List of plannable actions.
        /// </summary>
        public List<IPlannableAction> Actions { get; set; }

        /// <summary>
        /// Control if list of actions is empty or not.
        /// </summary>
        /// <returns>Value if list of actions is empty or not.</returns>
        public bool hasActions()
        {
            if (this.Actions == null || this.Actions.Count == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Constructor for plan item. Initialize actions.
        /// </summary>
        public PlanItem()
        {
            this.Actions = new List<IPlannableAction>();
        }

    }
}
