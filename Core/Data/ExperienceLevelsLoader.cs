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
