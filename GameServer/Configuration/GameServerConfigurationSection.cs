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

        [ConfigurationProperty("goods", IsRequired = true)]
        public GoodsElement Goods
        {
            get { return (GoodsElement)this["goods"]; }
        }

        [ConfigurationProperty("economic_levels", IsRequired = true)]
        public EconomicLevelsElement EconomicLevels
        {
            get { return (EconomicLevelsElement)this["economic_levels"]; }
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

    public class GoodsElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "Goods.xml", IsRequired = true)]
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

    public class EconomicLevelsElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "EconomicLevels.xml", IsRequired = true)]
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
