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
using System.Runtime.Serialization;
using NLog;

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// The class ExperienceLevels contains all levels
    /// </summary>
    [System.Serializable()]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "SpaceTrafficData")]
    [System.Xml.Serialization.XmlRoot(ElementName = "Levels", IsNullable = false, Namespace = "SpaceTrafficData")]
    public class ExperienceLevels
    {
		/// <summary>
		/// Divider used for calculate number of experiences gained for buying ship (divider for a price of the ship)
		/// </summary>
		public static int FRACTION_OF_SHIP_PRICE = 5000;

		/// <summary>
		/// Divider used for calculate number of experiences gained for buying cargo (divider for a price of the cargo)
		/// </summary>
		public static int FRACTION_OF_CARGO_PRICE = 100;

		/// <summary>
		/// How many XP will player get for earning the achievement
		/// </summary>
		public static int XP_FOR_ACHIEVEMENT_GET = 400;

        #region "Fields and Properties"

        /// <summary>
        /// List of levels
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName = "Level")]
        public List<TLevel> Items { get; set; }

        #endregion

        #region "Constructors"

        public ExperienceLevels()
        {
            Items = new List<TLevel>();
        }

        #endregion

        #region "Private Methods"
        #endregion

        #region "Public Methods"

        public TLevel GetLevel(int levelId)
        {
            if (Items.Exists(p => p.LevelID == levelId))
            {
                return Items.First(p => p.LevelID == levelId);
            }

            return null;
        }

        #endregion

		
	}

    /// <summary>
    /// Description of a single level
    /// </summary>
    [System.Serializable()]
    [System.Xml.Serialization.XmlType(Namespace = "SpaceTrafficData")]
    public class TLevel
    {
        #region "Fields and Properties"

        [System.Xml.Serialization.XmlAttribute(AttributeName = "levelID")]
        public int LevelID { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElement(ElementName = "RequiredXP")]
        public int RequiredXP { get; set; }

        #endregion

        #region "Constructors"

        /// <summary>
        /// 
        /// </summary>
        public TLevel()
        {
        }

        #endregion

        #region "Private Methods"
        #endregion

        #region "Public Methods"
        #endregion
    }
}
