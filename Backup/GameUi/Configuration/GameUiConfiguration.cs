using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.IO;
using SpaceTraffic.Utils.Debugging;

namespace SpaceTraffic.GameUi.Configuration
{
    public class GameUiConfiguration
    {
        public const string ASSET_PATH_KEY = "assetPath";

        private static string assetPath;
        private static string mapPath;

        static GameUiConfiguration()
        {
            DebugEx.Entry();
            assetPath = WebConfigurationManager.AppSettings[ASSET_PATH_KEY];
            DebugEx.WriteLineF("assetPath={0}", assetPath);
            mapPath = Path.Combine(assetPath, "Map");

            DebugEx.Exit();
        }

        public static string AssetPath
        {
            get
            {
                return assetPath;
            }
        }

        public static string MapPath
        {
            get
            {
                return mapPath;
            }
        }
    }
}