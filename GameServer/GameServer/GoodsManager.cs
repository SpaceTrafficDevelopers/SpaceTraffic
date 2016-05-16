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
using SpaceTraffic.Game;
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Entities;
using SpaceTraffic.Engine;

namespace SpaceTraffic.GameServer
{
    public class GoodsManager : IGoodsManager
    {
        public IList<IGoods> GoodsList { get; set; }

        private IGameServer gameServer;

        public GoodsManager(IGameServer gameServer) 
        {
            this.gameServer = gameServer;
        }

        /// <summary>
        /// Generate goods on all planets from list and insert as TraderCargo into db.
        /// </summary>
        /// <param name="planets">planet list</param>
        public void GenerateGoodsOnPlanets(IList<Planet> planets)
        {
            Random r = new Random();
            foreach (Planet planet in planets)
            {
				if (planet.Details.hasBase)
				{
					List<TraderCargo> list = new List<TraderCargo>();
					foreach (IGoods goods in GoodsList)
					{
						if (r.Next(0, 2) == 1) // 50% sance, ze se zbozi prida na planetu 
						{
							TraderCargo traderCargo = generateTraderCargo(planet, goods);
							InsertTraderCargo(traderCargo);
						}
					}
				}
            }
        }

        /// <summary>
        /// Generate goods over all galaxy. (Using GenerateGoodsOnPlanets).
        /// </summary>
        /// <param name="map">galaxy map</param>
        public void GenerateGoodsOverGalaxyMap(GalaxyMap map)
        {
            foreach (StarSystem starSys in map.GetStarSystems())
            {
                IList<Planet> list = starSys.Planets;

                this.GenerateGoodsOnPlanets(list);
            }          
        }

        /// <summary>
        /// Insert goods from GoodsList into db.
        /// </summary>
        public void InsertCargoIntoDb()
        {
            foreach (IGoods goods in GoodsList)
            {
                Cargo cargo = new Cargo();
                cargo.DefaultPrice = (int)goods.Price;
                cargo.Description = goods.Description;
                cargo.Name = goods.Name;
                cargo.Type = goods.Type.ToString();
                cargo.LevelToBuy = goods.LevelToBuy;
                cargo.Volume = goods.Volume;
                cargo.Category = goods.GetType().Name;

                this.gameServer.Persistence.GetCargoDAO().InsertCargo(cargo);
            }
        }

        /// <summary>
        /// Generate trader cargo.
        /// </summary>
        /// <param name="planet">planet</param>
        /// <param name="goods">goods</param>
        /// <returns>generated trader cargo</returns>
        private TraderCargo generateTraderCargo(Planet planet, IGoods goods)
        {
            Random r = new Random();

            TraderCargo traderCargo = new TraderCargo();

            Trader trader = gameServer.Persistence.GetTraderDAO().GetTraderByBaseId(planet.Base.BaseId);
            Cargo cargo = gameServer.Persistence.GetCargoDAO().GetCargoByName(goods.Name);

            traderCargo.TraderId = trader.TraderId;
            traderCargo.CargoPrice = (int)goods.Price;
            traderCargo.CargoCount = r.Next(1, 101); // generuje pocet zbozi na planete
            traderCargo.CargoId = cargo.CargoId;

            return traderCargo;
        }

        /// <summary>
        /// Insert trader cargo into db.
        /// </summary>
        /// <param name="tc">trader cargo</param>
        private void InsertTraderCargo(TraderCargo tc)
        {
			Random rand = new Random();
			int originalPrice = tc.CargoPrice;
			int cargoPriceFraction = (int)(originalPrice * 0.4); /*40% of price*/
			tc.CargoPrice = (tc.CargoPrice - cargoPriceFraction) + rand.Next(cargoPriceFraction * 2); /* price is modified - +-40% of price*/
            this.gameServer.Persistence.GetTraderCargoDAO().InsertCargo(tc);
        }

        /// <summary>
        /// Change price for only one goods
        /// </summary>
        /// <param name="percent">Percent od change price goods</param>
        /// <param name="goods">Entity of goods</param>
        /// <exception cref="DivideByZeroException">When percent &lt= 0</exception>
        public void ChangeOneGoodsPrice(int percent, TraderCargo traderCargo)
        {
            traderCargo.CargoPrice = traderCargo.Cargo.DefaultPrice * percent / 100;

            this.gameServer.Persistence.GetTraderCargoDAO().UpdateCargo(traderCargo);
        }

        /// <summary>
        /// Change price goods in traderCargo and update in database.
        /// </summary>
        /// <param name="percent">percet</param>
        /// <param name="traderCargo">trader cargo</param>
        /// <exception cref="DivideByZeroException">When percent &lt= 0</exception>
        public void ChangePriceGoods(int percent, Planet planet)
        {
            List<TraderCargo> list = getTraderCargos(planet.Base.Trader);
            
            foreach (TraderCargo tc in list)
            {
                ChangeOneGoodsPrice(percent, tc);
            }
        }

        /// <summary>
        /// Gets list of trader cargos by trader from db.
        /// </summary>
        /// <param name="trader">trader</param>
        /// <returns>list of trader cargos by trader</returns>
        private List<TraderCargo> getTraderCargos(Trader trader)
        {
            List<TraderCargo> list =
                this.gameServer.Persistence.GetTraderCargoDAO().GetCargoListByOwnerId(trader.TraderId).Cast<TraderCargo>().ToList();

            foreach (TraderCargo tc in list)
            {
                tc.Cargo = this.gameServer.Persistence.GetCargoDAO().GetCargoById(tc.CargoId);
            }

            return list;
        }
    }
}