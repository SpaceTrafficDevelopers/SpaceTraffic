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

namespace SpaceTraffic.Data
{
    /// <summary>
    /// Class for parsing XML from XML file
    /// </summary>
    public static class GoodsXmlHelper
    {
        /// <summary>
        /// Extension method for parsing Goods instance from XmlNode.
        /// </summary>
        /// <param name="goodsNode">Xml node goods</param>
        /// <returns>List of goods instances filled with data from xml.</returns>
        public static IList<IGoods> ParseGoods(this XmlNode goodsNode)
        {
            IList<IGoods> goodsList = new List<IGoods>();

            foreach (XmlNode childNode in goodsNode.ChildNodes)
            {
                IGoods product = childNode.ParseProduct();
                goodsList.Add(product);
            }

            return goodsList;
        }

        /// <summary>
        /// Extension method for parsing goods product. Creates an instance of goods type by the category value.
        /// </summary>
        /// <param name="productNode">Node with a product</param>
        /// <returns>Product from xml.</returns>
        public static IGoods ParseProduct(this XmlNode productNode)
        {
            XmlNode categoryNode = productNode.SelectSingleNode("category");
            Type classGoodsType = Type.GetType("SpaceTraffic.Entities.Goods." + categoryNode.InnerText);

            IGoods product = Activator.CreateInstance(classGoodsType) as IGoods;
            
            //proda.GetType().GetProperty("property_name").SetValue(proda, "value", null);          
            //proda.ID = int.Parse(a.InnerText);

            foreach (XmlNode productParameters in productNode.ChildNodes)
            {
                productParameters.ParseProductParameters(product);
            }

            return product;
        }

        /// <summary>
        /// Extension method for parsing product parameters and sets product attributes.
        /// Throws XmlException when parameter name is not valid or unknown.
        /// </summary>
        /// <param name="productNode">Node with product parameter.</param>
        /// <param name="product">Instance of goods product.</param>
        public static void ParseProductParameters(this XmlNode productNode, IGoods product)
        {
            switch (productNode.Name)
            {
                case "id":
                    product.ID = productNode.IntValue();
                    break;

                case "name":
                    product.Name = productNode.InnerText;
                    break;

                case "description":
                    product.Description = productNode.InnerText;
                    break;

                case "size":
                    product.Volume = productNode.IntValue();
                    break;

                case "price":
                    product.Price = productNode.DoubleValue();
                    break;

                case "type":
                    product.Type = (GoodsType)Enum.Parse(typeof(GoodsType), productNode.InnerText, true);
                    break;

                case "levelToBuy":
                    product.LevelToBuy = productNode.IntValue();
                    break;

                case "category": /* Use for identify goods class. Use on the top. */ 
                    break;

                default:
                    throw new XmlException("Unexpected element.");
            }
        }
    }
}
