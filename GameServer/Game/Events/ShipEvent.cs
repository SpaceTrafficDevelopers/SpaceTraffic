using SpaceTraffic.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Events
{
    /// <summary>
    /// Class for events with spaceship.
    /// </summary>
    public class ShipEvent : IGameEvent
    {
        /// <summary>
        /// Planned time of action
        /// </summary>
        public GameTime PlannedTime { get; set; }

        /// <summary>
        /// Action which is bounded on event
        /// </summary>
        public IGameAction BoundAction { get; set; }
    }
}
