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
using SpaceTraffic.Services.Contracts;
using SpaceTraffic.Entities.PublicEntities;
using NLog;
using SpaceTraffic.Entities;

namespace SpaceTraffic.GameServer.ServiceImpl
{
    public class AccountService : IAccountService
    {
        private Utils.Security.PasswordHasher pwdHasher;

        private Logger Logger = LogManager.GetCurrentClassLogger();

        public bool Authenticate(string userName, string password)
        {
            Logger.Info("AccountService: Authenticate {0}", userName);
            Player player = GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerByName(userName);
            
            if(pwdHasher == null)
            {
                pwdHasher = new Utils.Security.PasswordHasher(Utils.Security.PasswordHasher.DEF_ITERATION_COUNT);
            }


            return (player != null && player.PlayerName == userName && pwdHasher.ValidatePassword(password,player.PsswdHash));
        }


        public AccountInfo GetAccountInfoByUserName(string userName)
        {
            Logger.Info("AccountService: GetAccountInfoByUserName {0}", userName);
			Player player = GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerByName(userName);
            return new AccountInfo { PlayerName = player.PlayerName, PlayerId = player.PlayerId };
        }

        public AccountInfo GetAccountInfoByAccountId(int accountId)
        {
            Logger.Info("AccountService: GetAccountInfoByAccountId {0}", accountId);
			Player player = GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(accountId);
			return new AccountInfo { PlayerName = player.PlayerName, PlayerId = player.PlayerId };
        }

        /// <summary>
        /// Method returns account information from token
        /// </summary>
        /// <param name="token">Player token</param>
        /// <returns>Acount informations</returns>
        public AccountInfo GetAccountInfoByToken(string token)
        {
            Logger.Info("AccountService: GetAccountInfoByAccountToken {0}", token);
            Player player = GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerByToken(token);
            return new AccountInfo { PlayerName = player.PlayerName, PlayerId = player.PlayerId };
        }


        public void RegisterPlayer(Player player)
        {
            Logger.Info("RegisterPlayer: {0}", player);
            if(GameServer.CurrentInstance.Persistence.GetPlayerDAO().InsertPlayer(player))
                Logger.Info("Player registration for: {0} succeded", player);
            else
                Logger.Info("Player registration for: {0} failed", player);
        }

        /// <summary>
        /// Method update player in DB
        /// </summary>
        /// <param name="player">Player to update</param>
        public bool UpdatePlayer(Player player)
        {
            Logger.Info("UpdatePlayer: {0}", player);
            bool result = GameServer.CurrentInstance.Persistence.GetPlayerDAO().UpdatePlayerById(player);
            Logger.Info("Player update for: {0} " + (result ? "succeded" : "failed"), player);

            return result;
        }

        /// <summary>
        /// Method removes player from DB
        /// </summary>
        /// <param name="player">Payer id</param>
        public bool RemovePlayer(int playerId)
        {
            Logger.Info("RemovePlayer: {0}", playerId);
            bool result = GameServer.CurrentInstance.Persistence.GetPlayerDAO().RemovePlayerById(playerId);
            
            Logger.Info("Remove of player: {0} " + (result ? "succeded" : "failed"), playerId);

            return result;
        }

        public bool AddPlayerIntoActivePlayers(int playerId)
        {
            return GameServer.CurrentInstance.World.AddPlayer(playerId);
        }

        public void RemovePlayerFromActivePlayers(int playerId)
        {
            GameServer.CurrentInstance.World.RemovePlayer(playerId);
        }

        /// <summary>
        /// Method check if username exists
        /// </summary>
        /// <param name="userName">Player username</param>
        /// <returns></returns>
        public bool AccountUsernameExists(string userName)
        {
            Logger.Info("AccountService: AccountUsernameExists {0}", userName);
            Player player = GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerByName(userName);

            return (player != null);
        }

        /// <summary>
        /// Method check if email exists
        /// </summary>
        /// <param name="email">Player email</param>
        /// <returns></returns>
        public bool AccountEmailExists(string email)
        {
            Logger.Info("AccountService: AccountEmailExists {0}", email);
            Player player = GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerByEmail(email);

            return (player != null);
        }

        /// <summary>
        /// Method check if user exists
        /// </summary>
        /// <param name="accountId">Player id</param>
        /// <returns></returns>
        public bool AccountExists(int accountId)
        {
            Logger.Info("AccountService: AccountExists {0}", accountId);
            Player player = GameServer.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(accountId);

            return (player != null);
        }
    }
}
