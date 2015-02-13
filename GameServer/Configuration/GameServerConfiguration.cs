using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SpaceTraffic.GameServer.Configuration
{
    public class GameServerConfiguration
    {
        private static GameServerConfigurationSection gameServerConfig;

        public static GameServerConfigurationSection GameServerConfig
        {
            get
            {
                if (gameServerConfig == null)
                {
                    gameServerConfig = ConfigurationManager.GetSection("gameServerConfig") as GameServerConfigurationSection;
                }
                return gameServerConfig;
            }
        }
    }
}
