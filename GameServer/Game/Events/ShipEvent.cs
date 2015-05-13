using SpaceTraffic.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Events
{
    public class ShipEvent : IGameEvent
    {
        public GameTime PlannedTime { get; set; }

        public IGameAction BoundAction { get; set; }
    }
}
