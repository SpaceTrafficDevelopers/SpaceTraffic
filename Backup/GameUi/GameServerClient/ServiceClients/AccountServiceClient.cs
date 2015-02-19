using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            throw new NotImplementedException();
        }
    }
}