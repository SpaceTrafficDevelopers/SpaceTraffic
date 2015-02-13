using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using SpaceTraffic.Services.Contracts;
using SpaceTraffic.Entities.PublicEntities;
using NLog;

namespace SpaceTraffic.GameServer.ServiceImpl
{
    public class AccountService : IAccountService
    {
        private Logger Logger = LogManager.GetCurrentClassLogger();

        public bool Authenticate(string userName, string password)
        {
            Logger.Info("AccountService: Authenticate {0}", userName);

            return (userName==password);// ? new AccountInfo { PlayerName = "Tester", PlayerId = "0" }: null;
        }


        public AccountInfo GetAccountInfoByUserName(string userName)
        {
            Logger.Info("AccountService: GetAccountInfoByUserName {0}", userName);

            return new AccountInfo { PlayerName = "Tester", PlayerId = "0" };
        }

        public AccountInfo GetAccountInfoByAccountId(int accountId)
        {
            Logger.Info("AccountService: GetAccountInfoByAccountId {0}", accountId);
            return new AccountInfo { PlayerName = "Tester", PlayerId = "0" };
        }


        public void RegisterPlayer(Entities.Player player)
        {
            Logger.Info("RegisterPlayer: {0}", player);

            throw new NotImplementedException();
        }
    }
}
