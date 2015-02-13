using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game;

namespace SpaceTraffic.Engine
{
    public interface IGameServer
    {
        IPersistenceManager Persistence { get; }

        IAssetManager Assets { get; }
        
        IScriptManager Scripts { get; }

        IWorldManager World { get; }

        IGameManager Game { get; }
    }
}
