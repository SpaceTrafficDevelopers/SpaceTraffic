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
using System.Net.Mail;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Utils
{
    //TODO: poslat reset hesla, poslat def template mail
    public class EmailClient
    {
        private static Dictionary<string, string> emailFormats;

        public static Dictionary<string, string> EmailFormats
        {
            get
            {
                if (emailFormats == null)
                    emailFormats = new Dictionary<string, string>();

                return emailFormats;
            }
            set
            {
                emailFormats = value;
            }
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
        public static bool SendCustomMail(string senderAddress, string recieversAddresses, string subject, string messageBody, bool isMessageHtml)
        {
            var message = new MailMessage();
            message.From = new MailAddress(senderAddress);
            message.To.Add(recieversAddresses);
            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = isMessageHtml;
            bool result;
            using (var smtp = new SmtpClient())
            {
                try
                {
                    smtp.Send(message);
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Sends activation email to player
        /// </summary>
        /// <param name="playerToActivate">Player to recieve activation email</param>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="activationUrl">Player activation URL</param>
        /// <returns>True if operation succeeded</returns>
        public static bool SendActivationMail(Player playerToActivate, string senderAddress, string activationUrl)
        {
            string text = FormatTemplates("activation_text_template.html", playerToActivate.PlayerShowName);
            string messageContent = FormatTemplates("button_email_template.html", text, activationUrl, "Aktivovat účet", "");
            string messageBody = FormatTemplates("base_email_template.html", messageContent, DateTime.Now.Year.ToString());

            return SendCustomMail(senderAddress, playerToActivate.Email, "Aktivujte váš nový Space Trafic účet", messageBody, true);
        }

        /// <summary>
        /// Sends lost password email to player
        /// </summary>
        /// <param name="player">Player to recieve email</param>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="activationUrl">Password activation URL</param>
        /// <param name="newPass">New player password</param>
        /// <returns>True if operation succeeded</returns>
        public static bool SendLostPassMail(Player player, string senderAddress, string activationUrl, string newPass)
        {
            string text = FormatTemplates("lostpassword_text_template.html", player.PlayerShowName, newPass);
            string messageContent = FormatTemplates("button_email_template.html", text, activationUrl, "Aktivovat nové heslo", "");
            string messageBody = FormatTemplates("base_email_template.html", messageContent, DateTime.Now.Year.ToString());

            return SendCustomMail(senderAddress, player.Email, "Reset hesla Space Traffic", messageBody, true);
        }

        /// <summary>
        /// Sends custom defined email with base template
        /// </summary>
        /// <param name="senderAddress">Email sender address</param>
        /// <param name="recieversAddresses">Coma separated list of recievers addresses</param>
        /// <param name="subject">Email subject</param>
        /// <param name="messageContent">Email content</param>
        /// <returns>True if operation succeeded</returns>
        public static bool SendBaseTemplateMail(string senderAddress, string recieversAddresses, string subject, string messageContent)
        {
            string messageBody = FormatTemplates("base_email_template.html", messageContent, DateTime.Now.Year.ToString());

            return SendCustomMail(senderAddress, recieversAddresses, subject, messageBody, true);
        }

        /// <summary>
        /// Format template with given parameters
        /// </summary>
        /// <param name="templateName">Loaded template file name with extension</param>
        /// <param name="templateParams">Parameters to place to template</param>
        /// <returns>Formated template or new line separated parameters if template does not exists</returns>
        private static string FormatTemplates(string templateName, params string[] templateParams)
        {
            if (emailFormats.ContainsKey(templateName))
            {
                try
                {
                    return string.Format(emailFormats[templateName], templateParams);
                }
                catch (Exception)
                {
                    return ParamsString(templateParams);
                }
            }
            else
            {
                return ParamsString(templateParams);
            }
        }

        /// <summary>
        /// Process array to one string separated with new line
        /// </summary>
        /// <param name="templateParams">Array to process</param>
        /// <returns>New line separated array</returns>
        private static string ParamsString(string[] templateParams)
        {
            string result = "{0}";
            bool first = true;
            foreach (string param in templateParams)
            {
                result = string.Format(result, ((first ? "" : "\n") + param + "{0}"));
                first = false;
            }
            return string.Format(result, "");
        }
    }
}
