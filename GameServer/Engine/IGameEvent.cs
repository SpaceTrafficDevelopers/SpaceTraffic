using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine
{
    public interface IGameEvent : IComparable<IGameEvent>
    {
        GameTime PlannedTime { get; }

        IGameAction BoundAction { get; }
    }
}
