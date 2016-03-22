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
        private Logger Logger = LogManager.GetCurrentClassLogger();

        public bool Authenticate(string userName, string password)
        {
            Logger.Info("AccountService: Authenticate {0}", userName);

            return (userName==password);
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


        public void RegisterPlayer(Entities.Player player)
        {
            Logger.Info("RegisterPlayer: {0}", player);

            throw new NotImplementedException();
        }

        public bool AddPlayerIntoActivePlayers(int playerId)
        {
            return GameServer.CurrentInstance.World.AddPlayer(playerId);
        }

        public void RemovePlayerFromActivePlayers(int playerId)
        {
            GameServer.CurrentInstance.World.RemovePlayer(playerId);
        }

    }
}
