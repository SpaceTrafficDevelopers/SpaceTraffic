using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using SpaceTraffic.Entities.PublicEntities;

namespace SpaceTraffic.Services.Contracts
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract]
        IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem);

        [OperationContract]
        int PerformAction(int playerId, string actionName, params object[] actionArgs);

        [OperationContract]
        object GetActionResult(int playerId, int actionCode);
    }

}
