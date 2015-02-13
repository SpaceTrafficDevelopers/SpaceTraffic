using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SpaceTraffic.Persistence;
using System.ComponentModel;

namespace SpaceTraffic.GameServer.Configuration
{
    public class GameServerConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("host", IsRequired = true)]
        public HostElement Host
        {
            get { return (HostElement)this["host"]; }
        }

        [ConfigurationProperty("assets", IsRequired = true)]
        public AssetsDirElement Assets
        {
            get { return (AssetsDirElement)this["assets"]; }
        }
        
        [ConfigurationProperty("map", IsRequired = true)]
        public MapElement Map
        {
            get { return (MapElement)this["map"]; }
        }

        [ConfigurationProperty("initializer", IsRequired = true)]
        public InitializerElement Initializer
        {
            get { return (InitializerElement)this["initializer"]; }
        }
        
    }

    public class HostElement : ConfigurationElement
    {
        [ConfigurationProperty("url", DefaultValue = "http://127.0.0.1:8088/gameServer", IsRequired = true)]
        //
        // TODO: Url validation
        public string Url
        {
            get
            {
                return (string)this["url"];
            }

            set
            {
                this["url"] = value;
            }
        }
    }

    public class AssetsDirElement : ConfigurationElement
    {
        [ConfigurationProperty("path", DefaultValue = ".\\Assets\\", IsRequired = true)]
        //TODO: relative path validation
        public string Path
        {
            get
            {
                return (string)this["path"];
            }

            set
            {
                this["path"] = value;
            }
        }
    }

    public class MapElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "GalaxyMap.xml", IsRequired = true)]
        //TODO: filename validation
        public string Name
        {
            get
            {
                return (string)this["name"];
            }

            set
            {
                this["name"] = value;
            }
        }
    }

    public class InitializerElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]      
        //TODO: filename validation
        public SpaceTrafficCustomInitializer.InitializeType Type
        {
            get
            {
                return (SpaceTrafficCustomInitializer.InitializeType)this["type"];
            }

            set
            {
                this["type"] = value;
            }

        }

            [ConfigurationProperty("inputScript", IsRequired = false)]      
        public String InputScript
        {
            get
            {
                return (String)this["inputScript"];
            }

            set
            {
                this["inputScript"] = value;
            }

        }
    }
}
