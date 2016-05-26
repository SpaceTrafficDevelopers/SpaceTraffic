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
using System.IO;
using System.Xml.Schema;
using System.Reflection;
using System;
using System.Collections.Generic;
using NLog;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Game;

namespace SpaceTraffic.Data
{
    /// <summary>
    /// This class is used to generate EconomicLevel from XML file. 
    /// </summary>
    public class EconomicLevelLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private XmlSchema _economicLevelSchema = null;

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether loaded xml data failed validation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this loaded xml is invalid according to schema; otherwise, <c>false</c>.
        /// </value>
        protected bool HasValidationFailed { get; set; }

        /// <summary>
        /// Gets the economic level XML schema.
        /// </summary>
        protected XmlSchema EconomicLevelSchema
        {
            get
            {
                if (this._economicLevelSchema == null)
                {
                    this.LoadSchema();
                }
                return this._economicLevelSchema;
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
        /// Loads the economic levels.
        /// </summary>
        /// <param name="economicLevelFileName">Name of the economic level file.</param>
        /// <param name="dataService">The data service, providig access to data stream.</param>
        /// <returns>Economic level list</returns>
        public IList<EconomicLevel> LoadEconomicLevels(string economicLevelFileName, IEconomicLevelDataStreamProvider dataService)
        {
            logger.Info("Loading economic levels: {0}", economicLevelFileName);
            this.HasValidationFailed = false;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            // puvodní komentář ze StarSystemLoader
            // Pro validaci při načítání, možnost zvýšení výkonu.
            //readerSettings.ValidationType = ValidationType.Schema; 
            //readerSettings.ValidationFlags = readerSettings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            //readerSettings.Schemas.Add(this.StarSystemSchema);

            using (XmlReader reader = XmlReader.Create(dataService.GetEconomicLevelStream(economicLevelFileName), readerSettings))
            {
                XmlDocument doc = new XmlDocument();
                doc.Schemas.Add(this.EconomicLevelSchema);
                doc.Load(reader);
                doc.Validate(ValidationEventHandler);

                if (this.HasValidationFailed)
                    throw new XmlException(String.Format("Xml validation failed for '{0}'", economicLevelFileName));

                XmlNode economicLevels = doc.GetElementsByTagName("economic_levels")[0];
                IList<EconomicLevel> economicLevelsList = economicLevels.ParseEconomicLevels();

                return economicLevelsList;
            }
        }

        /// <summary>
        /// Loads the economic level schema. (EconomicLevelSchema.xsd from resources)
        /// </summary>
        private void LoadSchema()
        {
            using (XmlReader schemaReader = new XmlTextReader(new MemoryStream(SpaceTraffic.Properties.Resources.EconomicLevelSchema)))
            {
                this._economicLevelSchema = XmlSchema.Read(schemaReader, ValidationEventHandler);
            }
        }


    }
}
