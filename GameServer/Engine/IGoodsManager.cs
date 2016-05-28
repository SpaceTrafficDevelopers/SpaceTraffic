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
        /// Next time for generating cargo and recalculating price (in minutes).
        /// </summary>
        int NextGeneratingTime { get; }

        /// <summary>
        /// Next time for level control (in minutes).
        /// </summary>
        int NextLevelControlTime { get;}

        /// <summary>
        /// List of all Goods.
        /// </summary>
        IList<IGoods> GoodsList { get; set; }

        /// <summary>
        /// Economic levels.
        /// </summary>
        IList<EconomicLevel> EconomicLevels { get; set; }

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
        /// Method for changing production, consumption and price.
        /// </summary>
        /// <param name="trader">trader with trader cargo and cargo</param>
        void changeProductionAndPrice(Trader trader);

        /// <summary>
        /// Method for plannig all economic event over galaxy.
        /// </summary>
        /// <param name="map">galaxy map</param>
        void planEconomicEvents(GalaxyMap map);

        /// <summary>
        /// Method for upgrading/downgrading economic level and clearing production and consumptions pools.
        /// </summary>
        /// <param name="trader">trader with trader cargo and cargo</param>
        void evaluateEconomicLevel(Trader trader);
    }
}
