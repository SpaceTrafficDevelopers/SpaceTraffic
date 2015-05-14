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
