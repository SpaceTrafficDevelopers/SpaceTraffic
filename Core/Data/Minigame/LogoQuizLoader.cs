using NLog;
using SpaceTraffic.Game.Minigame;
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
using System.Xml;

namespace SpaceTraffic.Data.Minigame
{
    public class LogoQuizLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Load logos from XML file.
        /// </summary>
        /// <param name="fileName">XML file name.</param>
        /// <returns>list of logos or null</returns>
        public static List<Logo> loadLogos(string fileName)
        {
            List<Logo> logos = new List<Logo>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNode root = doc.DocumentElement;
                parseDocument(root, logos);
                
            }
            catch (Exception e)
            {
                logger.Error("Logos loading failed:{0}", e.Message, e);
                return null;
            }

            return logos;
        }

        /// <summary>
        /// Method for parsing logos xml document.
        /// </summary>
        /// <param name="root">root element</param>
        /// <param name="logos">list for logos</param>
        private static void parseDocument(XmlNode root, List<Logo> logos)
        {
            foreach (XmlNode logoNode in root.ChildNodes)
            {
                Logo logo = new Logo();

                foreach (XmlNode attr in logoNode.ChildNodes) 
                { 
                    switch (attr.Name)
                    {
                        case "name":
                            logo.Name = attr.InnerText;
                            break;
                        case "file_name":
                            logo.ImageName = attr.InnerText;
                            break;
                    }
                }

                logos.Add(logo);
            }
        }
    }
}
