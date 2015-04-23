﻿using SpaceTraffic.Entities;
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
    /// Rozhraní GoodsManager pro přidávání a správu zboží na planetách.
    /// </summary>
    public interface IGoodsManager
    {
        /// <summary>
        /// List of all Goods.
        /// </summary>
        IList<IGoods> GoodsList { get; set; }

        /// <summary>
        /// Generuje zboží na planetách. Využívá 50% šanci na přidání příslušného zboží
        /// na planetu. Zároveň generuje množství zboží z intervalu 0-100.
        /// </summary>
        /// <param name="goodsList">seznam zboží</param>
        /// <param name="planets">seznam planet pro generování</param>
        void GenerateGoodsOnPlanets(IList<Planet> planets);

        /// <summary>
        /// Generuje zboží na všech planetách přes celou mapu galaxie. Využívá 
        /// GenerateGoodsOnPlanets.
        /// </summary>
        /// <param name="goodsList">seznam zboží</param>
        /// <param name="map">mapa galaxie</param>
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
