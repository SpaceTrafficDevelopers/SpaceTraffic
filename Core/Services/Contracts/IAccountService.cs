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

        /// <summary>
        /// Method for adding player into active players.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>return false when player not exists, otherwise true</returns>
        [OperationContract]
        bool AddPlayerIntoActivePlayers(int playerId);

        /// <summary>
        /// Method for removing player from active players.
        /// </summary>
        /// <param name="playerId">player id</param>
        [OperationContract]
        void RemovePlayerFromActivePlayers(int playerId);
    }
}
