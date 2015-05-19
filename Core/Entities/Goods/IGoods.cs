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

namespace SpaceTraffic.Entities.Goods
{
    /// <summary>
    /// Interface for goods categories
    /// </summary>
    public interface IGoods
    {
        /// <summary>
        /// Identification number
        /// </summary>
        int ID { get; set; }
       
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// Short description
        /// </summary>
        string Description { get; set; }
        
        /// <summary>
        /// Indicates how much does this item cost
        /// </summary>
        double Price { get; set; }
        
        /// <summary>
        /// How big is this item
        /// </summary>
        int Volume { get; set; }
        
        /// <summary>
        /// Type of goods
        /// </summary>
        GoodsType Type { get; set; }

        /// <summary>
        /// Lowest player level to buy this item
        /// </summary>
        int LevelToBuy { get; set; }
    }
}

/// <summary>
/// Enum for goods type (mainstream and special goods)
/// </summary>
public enum GoodsType { Mainstream, Special }
