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

        public void GenerateGoodsOnPlanets(IList<Planet> planets)
        {
            Random r = new Random();
            foreach (Planet planet in planets)
            {
                //List<PlanetGoods> planetGoodsList = new List<PlanetGoods>();
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

        public void GenerateGoodsOverGalaxyMap(GalaxyMap map)
        {
            foreach (StarSystem starSys in map.GetStarSystems())
            {
                IList<Planet> list = starSys.Planets;

                this.GenerateGoodsOnPlanets(list);
            }          
        }

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


        private void InsertTraderCargo(TraderCargo tc)
        {
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
        /// Change price goods on planet.
        /// </summary>
        /// <param name="percent">Percent of change price goods</param>
        /// <exception cref="DivideByZeroException">When percent &lt= 0</exception>
        public void ChangePriceGoods(int percent, Planet planet)
        {
            List<TraderCargo> list = getTraderCargos(planet.Base.Trader);
            
            foreach (TraderCargo tc in list)
            {
                ChangeOneGoodsPrice(percent, tc);
            }
        }

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