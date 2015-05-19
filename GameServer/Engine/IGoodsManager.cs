using SpaceTraffic.Entities;
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
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine
{
    /// <summary>
    /// Interface GoodsManager for inserting and management goods on planets.
    /// </summary>
    public interface IGoodsManager
    {
        /// <summary>
        /// List of all Goods.
        /// </summary>
        IList<IGoods> GoodsList { get; set; }

        /// <summary>
        /// Generates goods on planets. 50% chance on adding of one goods on planet. 
        /// Generates goods count in interval from 0 to 100.
        /// </summary>
        /// <param name="goodsList">Goods list</param>
        /// <param name="planets">Planets list for generating.</param>
        void GenerateGoodsOnPlanets(IList<Planet> planets);

        /// <summary>
        /// Generates goods on all planets over the galaxy map. Uses GenerateGoodsOnPlanets.
        /// </summary>
        /// <param name="goodsList">Goods list</param>
        /// <param name="map">Galaxy map</param>
        void GenerateGoodsOverGalaxyMap(GalaxyMap map);

        /// <summary>
        /// Insert all goods as Cargo into db.
        /// </summary>
        void InsertCargoIntoDb();

        /// <summary>
        /// Change price goods in traderCargo and update in database.
        /// </summary>
        /// <param name="percent">percet</param>
        /// <param name="traderCargo">trader cargo</param>
        void ChangeOneGoodsPrice(int percent, TraderCargo traderCargo);

        /// <summary>
        /// Change price of all goods on planet and update in database.
        /// </summary>
        /// <param name="percent">percent</param>
        /// <param name="planet">planet</param>
        void ChangePriceGoods(int percent, Planet planet);
    }
}
