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
using System.Web.Security;
using SpaceTraffic.Services.Contracts;
using SpaceTraffic.GameUi.GameServerClient;
using SpaceTraffic.Entities.PublicEntities;

namespace SpaceTraffic.GameUi.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class SpaceTrafficMembershipProvider : MembershipProvider
    {
        #region Fields
        private string _ApplicationName;
        private bool _EnablePasswordReset;
        private bool _EnablePasswordRetrieval = false;
        private int _MaxInvalidPasswordAttempts;
        private int _MinRequiredNonAlphanumericCharacters;
        private int _MinRequiredPasswordLength;
        private int _PasswordAttemptWindow;
        private MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;
        private string _PasswordStrengthRegularExpression;
        private bool _RequiresQuestionAndAnswer = false;
        private bool _RequiresUniqueEmail = true;
        //private string _ServiceEndpoint;
        SpaceTraffic.Utils.Security.PasswordHasher pwdHasher;


        private readonly IGameServerClient GSClient = GameServerClientFactory.GetClientInstance();
        #endregion

        #region Properties

        /// <summary>
        /// The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>The name of the application using the custom membership provider.</returns>
        public override string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                this._ApplicationName = value;
            }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
        public override bool EnablePasswordReset
        {
            get
            {
                return _EnablePasswordReset;
            }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return _EnablePasswordRetrieval;
            }
        }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>The number of invalid password or password-answer attempts allowed before the membership user is locked out.</returns>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return _MaxInvalidPasswordAttempts;
            }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>The minimum number of special characters that must be present in a valid password.</returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return _MinRequiredNonAlphanumericCharacters;
            }
        }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>The minimum length required for a password. </returns>
        public override int MinRequiredPasswordLength
        {
            get
            {
                return _MinRequiredPasswordLength;
            }
        }


        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.</returns>
        public override int PasswordAttemptWindow
        {
            get
            {
                return _PasswordAttemptWindow;
            }
        }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"/> values indicating the format for storing passwords in the data store.</returns>
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return _PasswordFormat;
            }
        }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>A regular expression used to evaluate a password.</returns>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return _PasswordStrengthRegularExpression;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return _RequiresQuestionAndAnswer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <returns>true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.</returns>
        public override bool RequiresUniqueEmail
        {
            get
            {
                return _RequiresUniqueEmail;
            }
        }

        ///// <summary>
        ///// Gets the endpoint of AccountService, to use with this membership provider.
        ///// </summary>
        ///// <return>name of the AccountService endpoint defined in configuration to be used by this instance.</return>
        //public string ServiceEndpoint
        //{
        //    get
        //    {
        //        return _ServiceEndpoint;
        //    }
        //}
        #endregion

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            string usernameLower = username.ToLower();
            if (!GSClient.AccountService.AccountUsernameExists(usernameLower) || !GSClient.AccountService.Authenticate(usernameLower, oldPassword))
            {
                return false;
            }

            int playerId = GSClient.AccountService.GetAccountInfoByUserName(usernameLower).PlayerId;
            Entities.Player player = GSClient.PlayerService.GetPlayer(playerId);
            string newPwdHash = pwdHasher.HashPassword(newPassword);

            player.PsswdHash = newPwdHash;

            return GSClient.AccountService.UpdatePlayer(player);
        }

        /// <summary>
        /// Not supported in this implementation.
        /// </summary>
        /// <returns>
        /// Always false.
        /// </returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            string usernameLower = username.ToLower();
            email = email.ToLower();

            if (!ValidateUserData(username, password, email, out status))
                return null;
            if (AccountDataTaken(usernameLower, email, out status))
                return null;

            string pwdHash = pwdHasher.HashPassword(password);

            Entities.Player newPlayer = new Entities.Player()
            {
                PlayerToken = string.Empty,
                PlayerName = usernameLower,
                PlayerShowName = username,
                Email = email,
                PsswdHash = pwdHash,
                NewPsswdHash = string.Empty,
                IsEmailConfirmed = false,
                PassChangeDate = DateTime.Now.AddDays(-3),
                AddedDate = DateTime.Now,
                LastVisitedDate = DateTime.Now,
                Credit = 18000,
                ExperienceLevel = 0,
                Experiences = 0,
                StayLogedIn = false,
                SendInGameInfo = false,
                SendNewsletter = false
            };

            string token = GeneratePlayerToken(newPlayer);
            newPlayer.PlayerToken = token;

            GSClient.AccountService.RegisterPlayer(newPlayer);

            string appUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            
            if (!GSClient.AccountService.AccountUsernameExists(usernameLower))
            {
                status = MembershipCreateStatus.ProviderError;
                return null;
            }

            GSClient.GameService.PerformAction(GSClient.AccountService.GetAccountInfoByUserName(usernameLower).PlayerId, "InactivePlayerRemove", newPlayer.PlayerShowName);

            GSClient.MailService.SendActivationMail(newPlayer, "info@spacetraffic.zcu.cz", appUrl + "/Account/ActivationToken?Token=" + token);

            status = MembershipCreateStatus.Success;
            return GetUser(username, false);
        }

        private bool ValidateUserData(string username, string password, string email, out MembershipCreateStatus status)
        {
            //TODO: Evaluate if implementation is needed
            status = MembershipCreateStatus.Success;
            return true;
        }

        /// <summary>
        /// Check if account data are taken by another user
        /// </summary>
        /// <param name="username">Player username</param>
        /// <param name="email">Player email</param>
        /// <param name="status">Accoun create status</param>
        /// <returns></returns>
        private bool AccountDataTaken(string username, string email, out MembershipCreateStatus status)
        {
            if (GSClient.AccountService.AccountUsernameExists(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return true;
            }
            else if (GSClient.AccountService.AccountEmailExists(email))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return true;
            }

            status = MembershipCreateStatus.Success;
            return false;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            username = username.ToLower();
            if (GSClient.AccountService.AccountUsernameExists(username))
            {
                int id = GSClient.AccountService.GetAccountInfoByUserName(username).PlayerId;
                return GSClient.AccountService.RemovePlayer(id);
            }
            else
                return false;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            //TODO: FindUsersByEmail, implement later.
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            //TODO: FindUsersByName, implement later if needed.
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            //TODO: GetAllUsers, implement later if needed.
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            //TODO: GetNumberOfUsersOnline, implement later.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not supported in this implementation
        /// </summary>
        /// <exception cref="NotSupportedException">when called.</exception>
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException("GetPassword is not supported by this implementation.");
        }

        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            AccountInfo accountInfo =  GSClient.AccountService.GetAccountInfoByUserName(username);

            MembershipUser user = new MembershipUser(this.Name, accountInfo.PlayerName,accountInfo.PlayerId, "", "", "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            return user;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            AccountInfo accountInfo = GSClient.AccountService.GetAccountInfoByAccountId((int)providerUserKey);

            MembershipUser user = new MembershipUser(this.Name, accountInfo.PlayerName, accountInfo.PlayerId, "", "", "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
            return user;
        }

        public override string GetUserNameByEmail(string email)
        {
            //TODO: GetUserNameByEmail, implement later.
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            string usernameLower = username.ToLower();
            if (!GSClient.AccountService.AccountUsernameExists(usernameLower))
            {
                return null;
            }

            int playerId = GSClient.AccountService.GetAccountInfoByUserName(usernameLower).PlayerId;
            Entities.Player player = GSClient.PlayerService.GetPlayer(playerId);

            string token = GeneratePlayerToken(player);
            string newPass = pwdHasher.GenerateRandomPassword(10);
            string newHash = pwdHasher.HashPassword(newPass);

            player.PlayerToken = token;
            player.NewPsswdHash = newHash;
            player.PassChangeDate = DateTime.Now;

            string appUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

            if (GSClient.AccountService.UpdatePlayer(player))
            {
                if (GSClient.MailService.SendLostPassMail(player, "info@spacetraffic.zcu.cz", appUrl+"/Account/ResetToken?Token="+token, newPass))
                    return "";
                else
                    return null;
            }
            else
                return null;
        }

        public override bool UnlockUser(string userName)
        {
            //TODO: UnlockUser, implement later.
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            //TODO: UpdateUser, implement later.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        public override bool ValidateUser(string username, string password)
        {
            

            //TODO: Exception handling.
            

            return GSClient.AccountService.Authenticate(username.ToLower(), password);
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("Missing config.");

            if (name == null || name.Length == 0)
                name = "SpaceTrafficMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Space Traffic Security Provider");
            }

            base.Initialize(name, config);

            this._ApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            this._MaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            this._PasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            this._MinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "0"));
            this._MinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "8"));
            this._EnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            this._PasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            //this._ServiceEndpoint = GetConfigValue(config["serviceEndpoint"], "AccountService");
            pwdHasher = new SpaceTraffic.Utils.Security.PasswordHasher(SpaceTraffic.Utils.Security.PasswordHasher.DEF_ITERATION_COUNT);
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public bool AddPlayerIntoActivePlayers(int playerId)
        {
            if(GSClient.AccountService.AccountExists(playerId))
            {
                return GSClient.AccountService.AddPlayerIntoActivePlayers(playerId);
            }
            return false;
        }

        public void RemovePlayerFromActivePlayers(int playerId)
        {
            if (GSClient.AccountService.AccountExists(playerId))
            {
                GSClient.AccountService.RemovePlayerFromActivePlayers(playerId);
            }
        }

        /// <summary>
        /// Generates token representing player
        /// </summary>
        /// <param name="player">Player to generate token</param>
        /// <returns>Player token</returns>
        private string GeneratePlayerToken(Entities.Player player)
        {
            string data = player.PlayerName + player.PlayerId + player.Email + player.PsswdHash;

            string token = pwdHasher.HashPassword(data).Replace('+','a').Replace('/','b').Replace('=','c');

            return token;
        }
    }
}