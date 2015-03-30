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

namespace SpaceTraffic.Data
{
    /// <summary>
    /// This class is used to generate Goods entities from XML file. 
    /// </summary>
    public class GoodsLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private XmlSchema _goodsSchema = null;

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether loaded xml data failed validation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this loaded xml is invalid according to schema; otherwise, <c>false</c>.
        /// </value>
        protected bool HasValidationFailed { get; set; }

        /// <summary>
        /// Gets the goods XML schema.
        /// </summary>
        protected XmlSchema GoodsSchema
        {
            get
            {
                if (this._goodsSchema == null)
                {
                    this.LoadSchema();
                }
                return this._goodsSchema;
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
        /// Loads the goods.
        /// </summary>
        /// <param name="goodsFileName">Name of the goods file.</param>
        /// <param name="dataService">The data service, providig access to data stream.</param>
        /// <returns></returns>
        public IList<IGoods> LoadGoods(string goodsFileName, IGoodsDataStreamProvider dataService)
        {
            logger.Info("Loading goods: {0}", goodsFileName);
            this.HasValidationFailed = false;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            // puvodní komentář ze StarSystemLoader
            // Pro validaci při načítání, možnost zvýšení výkonu.
            //readerSettings.ValidationType = ValidationType.Schema; 
            //readerSettings.ValidationFlags = readerSettings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            //readerSettings.Schemas.Add(this.StarSystemSchema);

            using (XmlReader reader = XmlReader.Create(dataService.GetGoodsStream(goodsFileName), readerSettings))
            {
                XmlDocument doc = new XmlDocument();
                doc.Schemas.Add(this.GoodsSchema);
                doc.Load(reader);
                doc.Validate(ValidationEventHandler);

                if (this.HasValidationFailed)
                    throw new XmlException(String.Format("Xml validation failed for '{0}'", goodsFileName));

                XmlNode goodsNode = doc.GetElementsByTagName("goods")[0];
                IList<IGoods> goodsList = goodsNode.ParseGoods();

                return goodsList;
            }
        }

        /// <summary>
        /// Loads the goods schema. (GoodsSchema.xsd from resources)
        /// </summary>
        private void LoadSchema()
        {
            using (XmlReader schemaReader = new XmlTextReader(new MemoryStream(SpaceTraffic.Properties.Resources.GoodsSchema)))//;//Assembly.GetExecutingAssembly().GetManifestResourceStream("SpaceTraffic.Resources.SpaceTrafficStarSystemSchema.xsd")))
            {
                this._goodsSchema = XmlSchema.Read(schemaReader, ValidationEventHandler);
            }
        }


    }
}
