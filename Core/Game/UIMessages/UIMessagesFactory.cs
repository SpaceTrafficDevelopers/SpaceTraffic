using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.UIMessages
{
    public static class UIMessagesFactory
    {

        private const string LEVEL_UPGRADE = "Planeta {0} dosáhla technologického pokroku! {1} nově ve výrobě.";

        private const string LEVEL_DOWNGRADE = "Kvůli lenochům byla planeta {0} nucena ukončit výrobu. {1} už se zde nevyrábí.";

        private const string TOO_MUCH_QUANTITY = "Sklady na planetě {0} přetékají! {1} za hubičku!";

        private const string TOO_FEW_QUANTITY = "Na planetě {0} chybí {1} a vláda hlásí: Království a půl princezny i za jednotku!";

        private const string ECONOMIC_BALANCE = "Úhel odrazu se rovná... se rovná... až se narovná! Stejně jako ekonomika na planetě {0}.";

        public static string levelUpgradeMessage(string baseName, string cargoName)
        {
            return string.Format(LEVEL_UPGRADE, baseName, cargoName);
        }

        public static string levelDowngradeMessage(string baseName, string cargoName)
        {
            return string.Format(LEVEL_DOWNGRADE, baseName, cargoName);
        }

        public static string tooMuchQuantityMessage(string baseName, string cargoName)
        {
            return string.Format(TOO_MUCH_QUANTITY, baseName, cargoName);
        }

        public static string tooFewQuantityMessage(string baseName, string cargoName)
        {
            return string.Format(TOO_FEW_QUANTITY, baseName, cargoName);
        }

        public static string economicBalanceMessage(string baseName)
        {
            return string.Format(ECONOMIC_BALANCE, baseName);
        }
    }
}
