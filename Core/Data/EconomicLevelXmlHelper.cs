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
using System.Collections.Generic;
using System;
using System.Xml;
using SpaceTraffic.Xml;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Game;

namespace SpaceTraffic.Data
{
    /// <summary>
    /// Class for parsing XML from economic level XML file
    /// </summary>
    public static class EconomicLevelXmlHelper
    {
        /// <summary>
        /// Extension method for parsing EconomicLevels instance from XmlNode.
        /// </summary>
        /// <param name="economicLevelsNode">Xml node economic level</param>
        /// <returns>List of economic level instances filled with data from xml.</returns>
        public static IList<EconomicLevel> ParseEconomicLevels(this XmlNode economicLevelsNode)
        {
            List<EconomicLevel> economicLevelList = new List<EconomicLevel>();

            foreach (XmlNode childNode in economicLevelsNode.ChildNodes)
            {
                EconomicLevel economicLevel = childNode.ParseLevel();
                economicLevelList.Add(economicLevel);
            }

            economicLevelList.Sort((x, y) => x.Level.CompareTo(y.Level));

            return economicLevelList;
        }

        /// <summary>
        /// Extension method for parsing level of economic level.
        /// </summary>
        /// <param name="levelNode">Node with a level</param>
        /// <returns>Economic level from xml.</returns>
        public static EconomicLevel ParseLevel(this XmlNode levelNode)
        {
            EconomicLevel economicLevel = new EconomicLevel();

            foreach (XmlNode levelElement in levelNode.ChildNodes)
            {
                switch (levelElement.Name)
                {
                    case "level":
                        economicLevel.Level = levelElement.IntValue();
                        break;
                    case "upgrade_percentage":
                        economicLevel.UpgradeLevelPercentage = levelElement.IntValue();
                        break;

                    case "downgrade_percentage":
                        economicLevel.DowngradeLevelPercentage = levelElement.IntValue();
                        break;

                    case "economic_items":
                        levelElement.ParseEconomicItems(economicLevel);
                        break;

                    default:
                        throw new XmlException("Unexpected element.");
                }
            }

            if (economicLevel.DowngradeLevelPercentage >= economicLevel.UpgradeLevelPercentage)
                throw new ApplicationException("Logic error. Downgrade level cannot be bigger or equal than Upgrade level.");

            return economicLevel;
        }

        /// <summary>
        /// Extension method for parsing economic level items.
        /// </summary>
        /// <param name="economicItemNode">node with a economic items</param>
        /// <param name="economicLevel">economic level</param>
        public static void ParseEconomicItems(this XmlNode economicItemNode, EconomicLevel economicLevel)
        {
            economicLevel.LevelItems = new List<EconomicLevelItem>();

            foreach (XmlNode item in economicItemNode.ChildNodes)
            {
                economicLevel.LevelItems.Add(item.ParseEconomicItemsValues());
            }

            ((List<EconomicLevelItem>)economicLevel.LevelItems).Sort((x, y) => x.SequenceNumber.CompareTo(y.SequenceNumber));
        }

        /// <summary>
        /// Extension method for parsing economic level item values.
        /// </summary>
        /// <param name="item">item node</param>
        /// <returns>economic level item</returns>
        public static EconomicLevelItem ParseEconomicItemsValues(this XmlNode item)
        {
            EconomicLevelItem eli = new EconomicLevelItem();

            eli.SequenceNumber = item.Attributes["sequence_number"].IntValue();
            eli.IsDiscovered = item.Attributes["is_discovered"].BoolValue();

            foreach (XmlNode it in item.ChildNodes)
            {
                switch (it.Name)
                {
                    case "production":
                        eli.Production = it.DoubleValue();
                        break;

                    case "consumption":
                        eli.Consumption = it.DoubleValue();
                        break;

                    default:
                        throw new XmlException("Unexpected element.");
                }
            }

            return eli;
        }
    }
}
