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
            // get the default web app dir
            string defaultWebConfigDir = Path.GetDirectoryName(System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/").FilePath);
            // combine to get assets
            assetPath = Path.Combine(defaultWebConfigDir, assetPath);

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