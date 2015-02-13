using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Services.Contracts
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        bool Authenticate(string userName, string password);

        [OperationContract]
        AccountInfo GetAccountInfoByUserName(string userName);

        [OperationContract]
        AccountInfo GetAccountInfoByAccountId(int accountId);

        [OperationContract]
        void RegisterPlayer(Player player);
    }
}
