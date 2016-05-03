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
using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SpaceTraffic.Services.Contracts
{
    [ServiceContract]
    public interface IMailService
    {
        /// <summary>
        /// Sends custom defined email
        /// </summary>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="recieversAddresses">Coma separated list of recievers addresses</param>
        /// <param name="subject">Email subject</param>
        /// <param name="messageBody">Email body</param>
        /// <param name="isMessageHtml">True if email body is HTML</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool SendCustomMail(string senderAddress, string recieversAddresses, string subject, string messageBody, bool isMessageHtml);

        /// <summary>
        /// Sends activation email to player
        /// </summary>
        /// <param name="playerToActivate">Player to recieve activation email</param>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="activationUrl">Player activation URL</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool SendActivationMail(Player playerToActivate, string senderAddress, string activationUrl);

        /// <summary>
        /// Sends lost password email to player
        /// </summary>
        /// <param name="player">Player to recieve email</param>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="activationUrl">Password activation URL</param>
        /// <param name="newPass">New player password</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool SendLostPassMail(Player player, string senderAddress, string activationUrl, string newPass);

        /// <summary>
        /// Sends custom defined email with base template
        /// </summary>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="recieversAddresses">Coma separated list of recievers addresses</param>
        /// <param name="subject">Email subject</param>
        /// <param name="messageContent">Email content</param>
        /// <returns>True if operation succeeded</returns>
        [OperationContract]
        bool SendBaseTemplateMail(string senderAddress, string recieversAddresses, string subject, string messageContent);
    }
}