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

namespace SpaceTraffic.Game.UIMessages
{
    /// <summary>
    /// Factory for UIMessages.
    /// </summary>
    public static class UIMessagesFactory
    {
        /// <summary>
        /// Level upgrade message pattern.
        /// </summary>
        private const string LEVEL_UPGRADE = "Planeta {0} dosáhla technologického pokroku! {1} nově ve výrobě.";

        /// <summary>
        /// Level downgrade message pattern.
        /// </summary>
        private const string LEVEL_DOWNGRADE = "Kvůli lenochům byla planeta {0} nucena ukončit výrobu. {1} už se zde nevyrábí.";

        /// <summary>
        /// Too much quantity message pattern.
        /// </summary>
        private const string TOO_MUCH_QUANTITY = "Sklady na planetě {0} přetékají! {1} za hubičku!";

        /// <summary>
        /// Too few quantity message pattern.
        /// </summary>
        private const string TOO_FEW_QUANTITY = "Na planetě {0} chybí {1} a vláda hlásí: Království a půl princezny i za jednotku!";

        /// <summary>
        /// Economic balance message pattern.
        /// </summary>
        private const string ECONOMIC_BALANCE = "Úhel odrazu se rovná... se rovná... až se narovná! Stejně jako ekonomika na planetě {0}.";

        /// <summary>
        /// Method for getting level upgrade message.
        /// </summary>
        /// <param name="baseName">base name</param>
        /// <param name="cargoName">cargo name</param>
        /// <returns>level upgrade message</returns>
        public static string levelUpgradeMessage(string baseName, string cargoName)
        {
            return string.Format(LEVEL_UPGRADE, baseName, cargoName);
        }

        /// <summary>
        /// Method for getting level downgrade message.
        /// </summary>
        /// <param name="baseName">base name</param>
        /// <param name="cargoName">cargo name</param>
        /// <returns>level downgrade message</returns>
        public static string levelDowngradeMessage(string baseName, string cargoName)
        {
            return string.Format(LEVEL_DOWNGRADE, baseName, cargoName);
        }

        /// <summary>
        /// Method for getting too much quantity message.
        /// </summary>
        /// <param name="baseName">base name</param>
        /// <param name="cargoName">cargo name</param>
        /// <returns>too much quantity message</returns>
        public static string tooMuchQuantityMessage(string baseName, string cargoName)
        {
            return string.Format(TOO_MUCH_QUANTITY, baseName, cargoName);
        }

        /// <summary>
        /// Method for getting too few quantity message.
        /// </summary>
        /// <param name="baseName">base name</param>
        /// <param name="cargoName">cargo name</param>
        /// <returns>too few quantity message</returns>
        public static string tooFewQuantityMessage(string baseName, string cargoName)
        {
            return string.Format(TOO_FEW_QUANTITY, baseName, cargoName);
        }

        /// <summary>
        /// Method for gettin economic balance message.
        /// </summary>
        /// <param name="baseName">base name</param>
        /// <returns>economic balance message</returns>
        public static string economicBalanceMessage(string baseName)
        {
            return string.Format(ECONOMIC_BALANCE, baseName);
        }
    }
}
