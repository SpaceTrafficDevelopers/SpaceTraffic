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
using SpaceTraffic.Dao;
using SpaceTraffic.Game.Actions;

namespace SpaceTraffic.GameServer
{
    public class GoodsManager : IGoodsManager
    {
        /// <summary>
        /// Next time for generating cargo and recalculating price (in minutes).
        /// </summary>
        private const int NEXT_GENERATING_TIME = 1;

        /// <summary>
        /// Next time for level control (in minutes).
        /// </summary>
        private const int NEXT_LEVEL_CONTROL_TIME = 5;

        /// <summary>
        /// Production variance in percentage.
        /// </summary>
        private const int PRODUCTION_VARIANCE = 5;

        /// <summary>
        /// Consumption variance in percentage.
        /// </summary>
        private const int CONSUMPTION_VARIANCE = 5;

        /// <summary>
        /// Random.
        /// </summary>
        private Random random = new Random();

        public IList<IGoods> GoodsList { get; set; }

        public IList<EconomicLevel> EconomicLevels { get; set; }

        public int NextGeneratingTime 
        {
            get { return NEXT_GENERATING_TIME; } 
        }

        public int NextLevelControlTime 
        {
            get { return NEXT_LEVEL_CONTROL_TIME; }
        }

        private IGameServer gameServer;

        public GoodsManager(IGameServer gameServer) 
        {
            this.gameServer = gameServer;
        }

        /// <summary>
        /// Generate goods on all planets from list and insert as TraderCargo into db.
        /// </summary>
        /// <param name="planets">planet list</param>
        private void GenerateGoodsOnPlanets(IList<Planet> planets)
        {
            foreach (Planet planet in planets)
            {
				if (planet.Details.hasBase)
				{
                    int goodsIndex = random.Next(0, GoodsList.Count);
                    IGoods goods = GoodsList[goodsIndex];
					
                    TraderCargo traderCargo = generateTraderCargo(planet, goods);
                    this.gameServer.Persistence.GetTraderCargoDAO().InsertCargo(traderCargo);
				}
            }
        }

        public void planEconomicEvents(GalaxyMap map)
        {
            foreach (StarSystem starSys in map.GetStarSystems())
            {
                IList<Planet> list = starSys.Planets;

                foreach (Planet planet in list)
                {
                    if (planet.Details.hasBase)
                        planAllEvents(planet.Base.BaseId);
                }
            }
        }

        private void planAllEvents(int baseId)
        {
            IGameAction changeProductionAndPriceAction = new ChangeProductionAndPriceAction();
            changeProductionAndPriceAction.ActionArgs = new object[] { baseId };

            this.gameServer.Game.PlanEvent(changeProductionAndPriceAction, DateTime.UtcNow.AddMinutes(NEXT_GENERATING_TIME));
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
            TraderCargo traderCargo = new TraderCargo();

            Trader trader = gameServer.Persistence.GetTraderDAO().GetTraderByBaseId(planet.Base.BaseId);
            Cargo cargo = gameServer.Persistence.GetCargoDAO().GetCargoByName(goods.Name);

            EconomicLevel economicLevel = this.EconomicLevels[trader.EconomicLevel - 1];

            traderCargo.TraderId = trader.TraderId;
            traderCargo.DailyConsumption = (int)economicLevel.LevelItems[0].Consumption;
            traderCargo.DailyProduction = (int)economicLevel.LevelItems[0].Production;
            traderCargo.TodayConsumed = 0;
            traderCargo.TodayProduced = 0;
            traderCargo.SequenceNumber = 1;
            traderCargo.CargoCount = random.Next(1, 101); 
            traderCargo.CargoId = cargo.CargoId;

            calculatePrice(trader, traderCargo, cargo.DefaultPrice);

            return traderCargo;
        }

        ///// <summary>
        ///// Change price for only one goods
        ///// </summary>
        ///// <param name="percent">Percent od change price goods</param>
        ///// <param name="goods">Entity of goods</param>
        ///// <exception cref="DivideByZeroException">When percent &lt= 0</exception>
        //public void ChangeOneGoodsPrice(int percent, TraderCargo traderCargo)
        //{
        //    //TODO
        //    //traderCargo.CargoPrice = traderCargo.Cargo.DefaultPrice * percent / 100;

        //    this.gameServer.Persistence.GetTraderCargoDAO().UpdateCargo(traderCargo);
        //}

        ///// <summary>
        ///// Change price goods in traderCargo and update in database.
        ///// </summary>
        ///// <param name="percent">percet</param>
        ///// <param name="traderCargo">trader cargo</param>
        ///// <exception cref="DivideByZeroException">When percent &lt= 0</exception>
        //public void ChangePriceGoods(int percent, Planet planet)
        //{
        //    List<TraderCargo> list = getTraderCargos(planet.Base.Trader);
            
        //    foreach (TraderCargo tc in list)
        //    {
        //        ChangeOneGoodsPrice(percent, tc);
        //    }
        //}

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

        public void changeProductionAndPrice(Trader trader)
        {
            ITraderCargoDAO tcdao = this.gameServer.Persistence.GetTraderCargoDAO();
            foreach (TraderCargo cargo in trader.TraderCargos)
            {
                produceAndConsume(cargo);
                calculatePrice(trader, cargo, cargo.Cargo.DefaultPrice);
                tcdao.UpdateCargo(cargo);
            }
        }

        private void produceAndConsume(TraderCargo cargo)
        {
            int produceVal = produce(cargo);
            int consumeVal = consume(cargo);

            if (produceVal != 0 || consumeVal != 0) 
            { 
                int diff = produceVal - consumeVal;

                cargo.TodayConsumed += consumeVal;
                cargo.TodayProduced += produceVal;

                cargo.CargoCount += diff;

                if (cargo.CargoCount < 0)
                    cargo.CargoCount = 0;
            }
        }

        private void calculatePrice(Trader trader, TraderCargo cargo, int defaultPrice)
        {
            calculateBuyPrice(trader, cargo, defaultPrice);
            calculateSellPrice(trader, cargo, defaultPrice);
        }

        private void calculateBuyPrice(Trader trader, TraderCargo cargo, int defaultPrice)
        {
            //TODO: temp function, this will be changed, probably as method
            int percetage = (int)(-0.1 * cargo.CargoCount + 200);

            if(percetage < 0)
                percetage = 0;

            int price = defaultPrice + (defaultPrice * percetage / 100);
            cargo.CargoBuyPrice = price + (price * trader.PurchaseTax / 100);
        }

        private void calculateSellPrice(Trader trader, TraderCargo cargo, int defaultPrice)
        {
            //TODO: temp function, this will be changed, probably as method
            int percetage = (int)(-0.1 * cargo.CargoCount + 200);

            if (percetage < 0)
                percetage = 0;

            int price = defaultPrice + (defaultPrice * percetage / 100);
            cargo.CargoSellPrice = price - (price * trader.SalesTax / 100);
        }

        private int produce(TraderCargo cargo)
        {
            return computeProduceOrConsumeValue(cargo.DailyProduction, cargo.TodayProduced, PRODUCTION_VARIANCE);
        }

        private int consume(TraderCargo cargo)
        {
            return computeProduceOrConsumeValue(cargo.DailyConsumption, cargo.TodayConsumed, CONSUMPTION_VARIANCE);
        }

        private int computeProduceOrConsumeValue(int dailyValue, int todayValue, int variance)
        {
            //if planet generate maximum for day, return 0
            if (dailyValue <= todayValue)
                return 0;

            int timesGenerating = getTimesGenerating();
            int valuePerCycle = dailyValue / timesGenerating;

            valuePerCycle = getPlusMinusValue(valuePerCycle, variance);

            int remainValue = dailyValue - todayValue;

            if (remainValue < valuePerCycle)
                return remainValue;
            else
                return valuePerCycle;
        }

        private int getPlusMinusValue(int value, int percentage)
        {
            int percentageValue = (int)(value * percentage / 100);

            if (random.Next(0, 2) == 0)
                return value + percentageValue;
            else
                return value - percentageValue;
        }

        private int getTimesGenerating()
        {
            return NEXT_LEVEL_CONTROL_TIME / NEXT_GENERATING_TIME;
        }
    }
}