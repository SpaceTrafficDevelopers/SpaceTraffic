using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace SpaceTraffic.Data
{

    /// <summary>
    /// This static class is used to load all experience levels from XML file
    /// </summary>
    public class ExperienceLevelsLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Load levels from XML file.
        /// </summary>
        /// <param name="fileName">XML file name.</param>
        /// <returns>class ExperienceLevels with loaded data</returns>
        public static SpaceTraffic.Entities.ExperienceLevels LoadExperienceLevels(string fileName)
        {
            SpaceTraffic.Entities.ExperienceLevels experienceLevels = null;

            try
            {
                System.IO.FileStream fsOpen = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(SpaceTraffic.Entities.ExperienceLevels));

                experienceLevels = (SpaceTraffic.Entities.ExperienceLevels)xmlSerializer.Deserialize(fsOpen);

                fsOpen.Close();
            }
            catch (Exception exception)
            {
                logger.Error("Experience levels loading failed:{0}", exception.Message, exception);
                return null;
            }

            return experienceLevels;
        }
    }
}
