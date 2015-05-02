using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Engine;
using System.Diagnostics;

namespace SpaceTraffic.Game.Events
{
    class DefaultEvent : IGameEvent
    {
        public GameTime PlannedTime
        {
            get;
            set;
        }

        public IGameAction BoundAction
        {
            get;
            set;
        }

        public int CompareTo(IGameEvent other)
        {
            return PlannedTime.Value.CompareTo(other.PlannedTime.Value); 
        }
    }
}
