﻿/**
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
using SpaceTraffic.Entities;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Services.Contracts;

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{
    /// <summary>
    /// Account service client.
    /// </summary>
    public class AccountServiceClient : ServiceClientBase<IAccountService>, IAccountService
    {
        /// <summary>
        /// Authenticates the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool Authenticate(string userName, string password)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IAccountService).Authenticate(userName, password);
            }
        }


        /// <summary>
        /// Gets the name of the account info by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public Entities.PublicEntities.AccountInfo GetAccountInfoByUserName(string userName)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IAccountService).GetAccountInfoByUserName(userName);
            }
        }

        /// <summary>
        /// Method returns account information from token
        /// </summary>
        /// <param name="token">Player token</param>
        /// <returns>Acount informations</returns>
        public Entities.PublicEntities.AccountInfo GetAccountInfoByToken(string token)
        {
            Entities.PublicEntities.AccountInfo result;
            using (var channel = this.GetClientChannel())
            {
                result = (channel as IAccountService).GetAccountInfoByToken(token);
            }

            return result;
        }

        /// <summary>
        /// Gets the account info by account id.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <returns></returns>
        public Entities.PublicEntities.AccountInfo GetAccountInfoByAccountId(int accountId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IAccountService).GetAccountInfoByAccountId(accountId);
            }
        }


        public void RegisterPlayer(Entities.Player player)
        {
            using (var channel = this.GetClientChannel())
            {
                (channel as IAccountService).RegisterPlayer(player);
            }
        }

        /// <summary>
        /// Method update player in DB
        /// </summary>
        /// <param name="player">Player to update</param>
        public bool UpdatePlayer(Player player)
        {
            bool result;
            using (var channel = this.GetClientChannel())
            {
                result = (channel as IAccountService).UpdatePlayer(player);
            }
            return result;
        }

        /// <summary>
        /// Method removes player from DB
        /// </summary>
        /// <param name="player">Payer id</param>
        public bool RemovePlayer(int playerId)
        {
            bool result;
            using (var channel = this.GetClientChannel())
            {
                result = (channel as IAccountService).RemovePlayer(playerId);
            }
            return result;
        }


        public bool AddPlayerIntoActivePlayers(int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IAccountService).AddPlayerIntoActivePlayers(playerId);
            }
        }


        public void RemovePlayerFromActivePlayers(int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                (channel as IAccountService).RemovePlayerFromActivePlayers(playerId);
            }
        }

        /// <summary>
        /// Method check if username exists
        /// </summary>
        /// <param name="userName">Player username</param>
        /// <returns></returns>
        public bool AccountUsernameExists(string userName)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IAccountService).AccountUsernameExists(userName);
            }
        }

        /// <summary>
        /// Method check if email exists
        /// </summary>
        /// <param name="email">Player email</param>
        /// <returns></returns>
        public bool AccountEmailExists(string email)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IAccountService).AccountEmailExists(email);
            }
        }

        /// <summary>
        /// Method check if user exists
        /// </summary>
        /// <param name="accountId">Player id</param>
        /// <returns></returns>
        public bool AccountExists(int accountId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IAccountService).AccountExists(accountId);
            }
        }

        /// <summary>
        /// Method check if token exists
        /// </summary>
        /// <param name="email">Player token</param>
        /// <returns></returns>
        public bool AccountTokenExists(string token)
        {
            bool result;
            using (var channel = this.GetClientChannel())
            {
                result = (channel as IAccountService).AccountTokenExists(token);
            }
            return result;
        }

        /// <summary>
        /// Method returns account information from email
        /// </summary>
        /// <param name="email">Player email</param>
        /// <returns>Acount informations</returns>
        public AccountInfo GetAccountInfoByEmail(string email)
        {
            Entities.PublicEntities.AccountInfo result;
            using (var channel = this.GetClientChannel())
            {
                result = (channel as IAccountService).GetAccountInfoByEmail(email);
            }

            return result;
        }
    }
}