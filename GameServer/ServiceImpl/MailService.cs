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
using SpaceTraffic.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;
using MC = SpaceTraffic.Utils.EmailClient;
using NLog;

namespace SpaceTraffic.GameServer.ServiceImpl
{
    public class MailService : IMailService
    {
        private Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Sends activation email to player
        /// </summary>
        /// <param name="playerToActivate">Player to recieve activation email</param>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="activationUrl">Player activation URL</param>
        /// <returns>True if operation succeeded</returns>
        public bool SendActivationMail(Player playerToActivate, string senderAddress, string activationUrl)
        {
            Logger.Info("MailService: Send activation email to {0}", playerToActivate.Email);
            if (playerToActivate != null && senderAddress != null && activationUrl != null)
            {
                return MC.SendActivationMail(playerToActivate, senderAddress, activationUrl);
            }
            return false;
        }

        /// <summary>
        /// Sends custom defined email with base template
        /// </summary>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="recieversAddresses">Coma separated list of recievers addresses</param>
        /// <param name="subject">Email subject</param>
        /// <param name="messageContent">Email content</param>
        /// <returns>True if operation succeeded</returns>
        public bool SendBaseTemplateMail(string senderAddress, string recieversAddresses, string subject, string messageContent)
        {
            Logger.Info("MailService: Send base template email to {0}", recieversAddresses);
            if (senderAddress != null && recieversAddresses != null && subject != null && messageContent != null)
            {
                return MC.SendBaseTemplateMail(senderAddress, recieversAddresses, subject, messageContent);
            }
            return false;
        }

        /// <summary>
        /// Sends custom defined email
        /// </summary>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="recieversAddresses">Coma separated list of recievers addresses</param>
        /// <param name="subject">Email subject</param>
        /// <param name="messageBody">Email body</param>
        /// <param name="isMessageHtml">True if email body is HTML</param>
        /// <returns>True if operation succeeded</returns>
        public bool SendCustomMail(string senderAddress, string recieversAddresses, string subject, string messageBody, bool isMessageHtml)
        {
            Logger.Info("MailService: Send custom email to {0}", recieversAddresses);
            if (senderAddress != null && recieversAddresses != null && subject != null && messageBody != null)
            {
                return MC.SendCustomMail(senderAddress, recieversAddresses, subject, messageBody, isMessageHtml);
            }
            return false;
        }

        /// <summary>
        /// Sends lost password email to player
        /// </summary>
        /// <param name="player">Player to recieve email</param>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="activationUrl">Password activation URL</param>
        /// <param name="newPass">New player password</param>
        /// <returns>True if operation succeeded</returns>
        public bool SendLostPassMail(Player player, string senderAddress, string activationUrl, string newPass)
        {
            Logger.Info("MailService: Send lost password email to {0}", player.Email);
            if (player != null && senderAddress != null && activationUrl != null && newPass != null)
            {
                return MC.SendLostPassMail(player, senderAddress, activationUrl, newPass);
            }
            return false;
        }
    }
}
