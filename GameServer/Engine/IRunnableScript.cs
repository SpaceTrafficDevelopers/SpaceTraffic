using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine
{
    public interface IRunnableScript
    {
        object Run(IGameServer gameServer, params object[] args);
    }
}
