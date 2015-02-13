using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Services.Contracts;
using NLog;
using SpaceTraffic.Utils.Debugging;
using GS = SpaceTraffic.GameServer.GameServer;
using SpaceTraffic.Entities.PublicEntities;

namespace SpaceTraffic.GameServer.ServiceImpl
{
    public class GameService :IGameService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem)
        {
            DebugEx.Entry(starSystem);

            IList<WormholeEndpointDestination> result = GS.CurrentInstance.World.Map.GetStarSystemConnections(starSystem);

            DebugEx.Exit(result);
            return result;
        }




        public int PerformAction(int playerId, string actionName, params object[] actionArgs)
        {
            throw new NotImplementedException();
        }

        public object GetActionResult(int playerId, int actionCode)
        {
            throw new 
                Exception();
        }
    }
}
