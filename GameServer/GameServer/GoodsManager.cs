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
using NLog;

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
        private const int NEXT_LEVEL_CONTROL_TIME = 2;

        /// <summary>
        /// Production variance in percentage.
        /// </summary>
        private const int PRODUCTION_VARIANCE = 5;

        /// <summary>
        /// Consumption variance in percentage.
        /// </summary>
        private const int CONSUMPTION_VARIANCE = 5;

        /// <summary>
        /// Logger
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();

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
				if (planet.Details.hasBase && planet.Base.BaseName.CompareTo("Země") == 0)
				{
                    int goodsIndex = random.Next(0, GoodsList.Count);
                    IGoods goods = GoodsList[goodsIndex];
					
                    TraderCargo traderCargo = generateTraderCargo(planet, goods);
                    this.gameServer.Persistence.GetTraderCargoDAO().InsertCargo(traderCargo);
                    break;
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
                    if (planet.Details.hasBase && planet.Base.BaseName.CompareTo("Země") == 0) { 
                        planAllEvents(planet.Base.BaseId);
                        break;
                    }
                }
            }
        }

        private void planAllEvents(int baseId)
        {
            object[] args = new object[] { baseId };

            IGameAction changeProductionAndPriceAction = new ChangeProductionAndPriceAction();
            changeProductionAndPriceAction.ActionArgs = args;

            this.gameServer.Game.PlanEvent(changeProductionAndPriceAction, DateTime.UtcNow.AddMinutes(NEXT_GENERATING_TIME));

            IGameAction evaluateEconomicLevelAction = new EvaluateEconomicLevelAction();
            evaluateEconomicLevelAction.ActionArgs = args;

            this.gameServer.Game.PlanEvent(evaluateEconomicLevelAction, DateTime.UtcNow.AddMinutes(NEXT_LEVEL_CONTROL_TIME));
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

            return createTraderCargo(trader, economicLevel.LevelItems[0], random.Next(0, 101), cargo);
        }

        private TraderCargo createTraderCargo(Trader trader, EconomicLevelItem item, int cargoCount, Cargo cargo)
        {
            TraderCargo traderCargo = new TraderCargo();

            traderCargo.TraderId = trader.TraderId;
            traderCargo.DailyConsumption = (int)item.Consumption;
            traderCargo.DailyProduction = (int)item.Production;
            traderCargo.TodayConsumed = 0;
            traderCargo.TodayProduced = 0;
            traderCargo.SequenceNumber = item.SequenceNumber;
            traderCargo.CargoCount = cargoCount;
            traderCargo.CargoId = cargo.CargoId;

            calculatePrice(trader, traderCargo, cargo.DefaultPrice);

            return traderCargo;
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

            if (cargo.CargoBuyPrice < 1)
                cargo.CargoBuyPrice = 1;
        }

        private void calculateSellPrice(Trader trader, TraderCargo cargo, int defaultPrice)
        {
            //TODO: temp function, this will be changed, probably as method
            int percetage = (int)(-0.1 * cargo.CargoCount + 200);

            if (percetage < 0)
                percetage = 0;

            int price = defaultPrice + (defaultPrice * percetage / 100);
            cargo.CargoSellPrice = price - (price * trader.SalesTax / 100);

            if (cargo.CargoSellPrice < 1)
                cargo.CargoSellPrice = 1;
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
            int deviation = random.Next(0, percentageValue);

            if (random.Next(0, 2) == 0)
                return value + deviation;
            else
                return value - deviation;
        }

        private int getTimesGenerating()
        {
            return NEXT_LEVEL_CONTROL_TIME / NEXT_GENERATING_TIME;
        }

        public void evaluateEconomicLevel(Trader trader)
        {
            clearProductionAndConsumptionPool(trader.TraderCargos);

            EconomicLevel level = this.EconomicLevels[trader.EconomicLevel - 1];

            bool upgrade = canBeUpgraded(level, trader.TraderCargos);
            bool downgrade = canBeDowngraded(level, trader.TraderCargos);

            //condition when downgrade condition value is bigger than upgrade condition value
            if (upgrade && downgrade) {
                string errorMessage = "Logic error. Upgrade and downgrade at the same time is not valid.";
                logger.Error(errorMessage);
                throw new ApplicationException(errorMessage);
            }

            if (upgrade)
                upgradeLevel(trader);
            else if (downgrade)
                downgradeLevel(trader);
        }

        private void clearProductionAndConsumptionPool(ICollection<TraderCargo> cargos)
        {
            ITraderCargoDAO tcdao = this.gameServer.Persistence.GetTraderCargoDAO();

            foreach (TraderCargo cargo in cargos)
            {
                cargo.TodayConsumed = 0;
                cargo.TodayProduced = 0;
                tcdao.UpdateCargo(cargo);
            }
        }

        private bool canBeUpgraded(EconomicLevel level, ICollection<TraderCargo> cargos)
        {
            foreach (TraderCargo cargo in cargos)
            {
                if (cargo.CargoCount <= level.UpgradeLevelQuantity)
                    return false;
            }
            return true;
        }

        private bool canBeDowngraded(EconomicLevel level, ICollection<TraderCargo> cargos)
        {
            foreach (TraderCargo cargo in cargos)
            {
                if (cargo.CargoCount < level.DowngradeLevelQuantity)
                    return true;
            }
            return false;
        }

        private void upgradeLevel(Trader trader)
        {
            if (trader.EconomicLevel == this.EconomicLevels.Count)
                return;
            
            //this gets next level
            EconomicLevel level = this.EconomicLevels[trader.EconomicLevel];
            trader.EconomicLevel = level.Level;
            
            ITraderDAO td = this.gameServer.Persistence.GetTraderDAO();
            td.UpdateTraderById(trader);
            upgradeCargo(level.LevelItems, trader);
        }
        //nextLevelItem
        private void upgradeCargo(IList<EconomicLevelItem> levelItems, Trader trader)
        {
            ITraderCargoDAO tcdao = this.gameServer.Persistence.GetTraderCargoDAO();

            foreach (EconomicLevelItem item in levelItems)
            {
                TraderCargo cargo = trader.TraderCargos.FirstOrDefault(x => x.SequenceNumber.Equals(item.SequenceNumber));

                if (cargo != null)
                {
                    cargo.DailyConsumption = calcDailyValueUpgrade(cargo.DailyConsumption, item.Consumption);
                    cargo.DailyProduction = calcDailyValueUpgrade(cargo.DailyProduction, item.Production);
                    
                    tcdao.UpdateCargo(cargo);
                }
                else
                    addNewTraderCargo(item, tcdao, trader);
            }
            
        }

        private void addNewTraderCargo(EconomicLevelItem item, ITraderCargoDAO dao, Trader trader)
        {
            ICargoDAO cdao = this.gameServer.Persistence.GetCargoDAO();
            List<Cargo> cargos = new List<Cargo>(cdao.GetCargos());

            foreach (TraderCargo tCargo in trader.TraderCargos)
            {
                Cargo cargo = cargos.FirstOrDefault(x => x.CargoId == tCargo.CargoId);

                if (cargo != null)
                    cargos.Remove(cargo);
            }

            //logic error, number of levels has to be less or equel than number of cargos.
            if (cargos.Count == 0) {
                string errorMessage = "Too few cargos. Number of levels has to be less or equel than number of cargos.";
                logger.Error(errorMessage);
                throw new ApplicationException(errorMessage);
            }
            Cargo uniqueCargo = cargos[random.Next(0, cargos.Count)];

            TraderCargo traderCargo = createTraderCargo(trader, item, 0, uniqueCargo);
            dao.InsertCargo(traderCargo);
        }

        

        public int calcDailyValueUpgrade(int dailyValue, double percentage)
        {
            if(percentage != 0 && dailyValue != 0)
            {
                return (int)(dailyValue + (dailyValue * percentage / 100));
            }

            return dailyValue;
        }

        public int calcDailyValueDowngrade(int dailyValue, double percentage)
        {
            if (percentage != 0 && dailyValue != 0)
            {
                return (int)(dailyValue - (dailyValue * percentage / (100 + percentage)));
            }

            return dailyValue;
        }

        private void downgradeLevel(Trader trader)
        {
            if (trader.EconomicLevel == 1)
                return;

            //this gets actual level
            EconomicLevel actualLevel = this.EconomicLevels[trader.EconomicLevel - 1];

            //this gets previous level
            EconomicLevel previousLevel = this.EconomicLevels[trader.EconomicLevel - 2];
            trader.EconomicLevel = previousLevel.Level;

            ITraderDAO td = this.gameServer.Persistence.GetTraderDAO();
            td.UpdateTraderById(trader);

            downgradeCargo(actualLevel.LevelItems, previousLevel.LevelItems, trader);
        }

        private void downgradeCargo(IList<EconomicLevelItem> actualLevelItems, IList<EconomicLevelItem> previousLevelItems, Trader trader)
        {
            ITraderCargoDAO tcdao = this.gameServer.Persistence.GetTraderCargoDAO();

            foreach (TraderCargo cargo in trader.TraderCargos)
            {
                EconomicLevelItem actualItem = actualLevelItems.FirstOrDefault(x => x.SequenceNumber == cargo.SequenceNumber);
                EconomicLevelItem previousItem = previousLevelItems.FirstOrDefault(x => x.SequenceNumber == cargo.SequenceNumber);

                if (previousItem != null)
                {
                    if (previousItem.IsDiscovered)
                    {
                        cargo.DailyConsumption = (int)previousItem.Consumption;
                        cargo.DailyProduction = (int)previousItem.Production;
                    }
                    else
                    {
                        cargo.DailyConsumption = calcDailyValueDowngrade(cargo.DailyConsumption, actualItem.Consumption);
                        cargo.DailyProduction = calcDailyValueDowngrade(cargo.DailyProduction, actualItem.Production);
                    }
                    tcdao.UpdateCargo(cargo);
                }
                else
                    tcdao.RemoveCargoById(cargo.TraderCargoId);
            }
        }
    }
}