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
using System.Xml;
using SpaceTraffic.Game;
using System.IO;
using System.Xml.Schema;
using System.Reflection;
using System;
using System.Collections.Generic;
using NLog;

namespace SpaceTraffic.Data
{
    /// <summary>
    /// This static class is used to generate Star system and it's subordinate object like planets and stars from XML file. 
    /// It use extension methods from PT2XmlParser.
    /// </summary>
    public class StarSystemLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private XmlSchema _starSystemSchema = null;

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether loaded xml data failed validation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this loaded xml is invalid according to schema; otherwise, <c>false</c>.
        /// </value>
        protected bool HasValidationFailed { get; set; }

        /// <summary>
        /// Gets the star system XML schema.
        /// </summary>
        protected XmlSchema StarSystemSchema
        {
            get
            {
                if (this._starSystemSchema == null)
                {
                    this.LoadSchema();
                }
                return this._starSystemSchema;
            }
        }
        #endregion

        /// <summary>
        /// Validations the event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance containing the event data.</param>
        private void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
            {
                logger.Fatal("Xml validation error: {0}", args.Message);
                this.HasValidationFailed = true;
            }
            else
            {
                logger.Error("Xml validation warning: {0}", args.Message);
            }
        }

        /// <summary>
        /// Loads the star system.
        /// </summary>
        /// <param name="starSystemName">Name of the star system.</param>
        /// <param name="dataService">The data service, providig access to data stream.</param>
        /// <returns></returns>
        public StarSystem LoadStarSystem(string starSystemName, IGalaxyMapDataStreamProvider dataService)
        {
            logger.Info("Loading star system: {0}", starSystemName);
            this.HasValidationFailed = false;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            // Pro validaci při načítání, možnost zvýšení výkonu.
            //readerSettings.ValidationType = ValidationType.Schema; 
            //readerSettings.ValidationFlags = readerSettings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            //readerSettings.Schemas.Add(this.StarSystemSchema);
    
            XmlReader reader = XmlReader.Create(dataService.GetStarSystemStream(starSystemName), readerSettings);
            
                XmlDocument doc = new XmlDocument();
                doc.Schemas.Add(this.StarSystemSchema);
                doc.Load(reader);
                reader.Close();
                doc.Validate(ValidationEventHandler);
                
                if (this.HasValidationFailed)
                    throw new XmlException(String.Format("Xml validation failed for '{0}'", starSystemName));

                XmlNode starSystemNode = doc.GetElementsByTagName("starsystem")[0];

                StarSystem starSystem = starSystemNode.ParseStarSystem();
                
                return starSystem;
            
        }

        /// <summary>
        /// Loads the Sto je jen tak tarSystemSchema.xsd from resources.
        /// </summary>
        private void LoadSchema()
        {
            using (XmlReader schemaReader = new XmlTextReader(new MemoryStream(SpaceTraffic.Properties.Resources.StarSystemSchema)))//;//Assembly.GetExecutingAssembly().GetManifestResourceStream("SpaceTraffic.Resources.SpaceTrafficStarSystemSchema.xsd")))
            {
                this._starSystemSchema = XmlSchema.Read(schemaReader, ValidationEventHandler);
            }
        }
    }
}
