using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpaceTraffic.GameUi.GameServerClient.ServiceClients;

namespace SpaceTraffic.GameUi.GameServerClient
{
    /// <summary>
    /// Client for access to GameServer via WCF.
    /// </summary>
    public class WCFGameServerClient : IGameServerClient
    {
        private AccountServiceClient _AccountService = new AccountServiceClient();
        private GameServiceClient _GameService = new GameServiceClient();

        /// <summary>
        /// Gets the account service of GameServer.
        /// </summary>
        public Services.Contracts.IAccountService AccountService
        {
            get { return _AccountService; }
        }

        public Services.Contracts.IGameService GameService
        {
            get { return _GameService; }
        }
    }
}