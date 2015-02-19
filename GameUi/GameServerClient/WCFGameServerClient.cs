/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
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