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

        /// <summary>
        /// Method returns account information from token
        /// </summary>
        /// <param name="token">Player token</param>
        /// <returns>Acount informations</returns>
        [OperationContract]
        AccountInfo GetAccountInfoByToken(string token);

        /// <summary>
        /// Method returns account information from email
        /// </summary>
        /// <param name="email">Player email</param>
        /// <returns>Acount informations</returns>
        [OperationContract]
        AccountInfo GetAccountInfoByEmail(string email);

        /// <summary>
        /// Method check if username exists
        /// </summary>
        /// <param name="userName">Player username</param>
        /// <returns></returns>
        [OperationContract]
        bool AccountUsernameExists(string userName);

        /// <summary>
        /// Method check if email exists
        /// </summary>
        /// <param name="email">Player email</param>
        /// <returns></returns>
        [OperationContract]
        bool AccountEmailExists(string email);

        /// <summary>
        /// Method check if token exists
        /// </summary>
        /// <param name="email">Player token</param>
        /// <returns></returns>
        [OperationContract]
        bool AccountTokenExists(string token);

        /// <summary>
        /// Method check if user exists
        /// </summary>
        /// <param name="accountId">Player id</param>
        /// <returns></returns>
        [OperationContract]
        bool AccountExists(int accountId);
        
        [OperationContract]
        void RegisterPlayer(Player player);

        /// <summary>
        /// Method update player in DB
        /// </summary>
        /// <param name="player">Player to update</param>
        [OperationContract]
        bool UpdatePlayer(Player player);

        /// <summary>
        /// Method removes player from DB
        /// </summary>
        /// <param name="player">Payer id</param>
        [OperationContract]
        bool RemovePlayer(int playerId);

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
