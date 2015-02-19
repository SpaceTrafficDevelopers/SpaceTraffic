using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpaceTraffic.Services.Contracts;
using SpaceTraffic.Entities.PublicEntities;

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{
    public class GameServiceClient : ServiceClientBase<IGameService>, IGameService
    {
        public IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).GetStarSystemConnections(starSystem);
            }
        }

        public int PerformAction(int playerId, string actionName, params object[] actionArgs)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).PerformAction(playerId, actionName, actionArgs);
            }
        }

        public object GetActionResult(int playerId, int actionCode)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IGameService).GetActionResult(playerId, actionCode);
            }
        }
    }
}