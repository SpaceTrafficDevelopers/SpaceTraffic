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
using SpaceTraffic.Game.UIMessages;

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
        /// Minimum value for message.
        /// </summary>
        private const int MIN_VALUE_FOR_MESSAGE = 500;

        /// <summary>
        /// Maximum value for message.
        /// </summary>
        private const int MAX_VALUE_FOR_MESSAGE = 3000;

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

        /// <summary>
        /// Game server reference
        /// </summary>
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

        /// <summary>
        /// Method for plannig all economic events.
        /// </summary>
        /// <param name="baseId">base id</param>
        private void planAllEvents(int baseId)
        {
            object[] args = new object[] { baseId };

            IGameAction changeProductionAndPriceAction = new ChangeProductionAndPriceAction();
            changeProductionAndPriceAction.ActionArgs = args;

            this.gameServer.Game.PlanEvent(changeProductionAndPriceAction, DateTime.UtcNow.AddMinutes(NEXT_GENERATING_TIME));

            IGameAction evaluateEconomicLevelAction = new EvaluateEconomicLevelAction();
            evaluateEconomicLevelAction.ActionArgs = args;

            this.gameServer.Game.PlanEvent(evaluateEconomicLevelAction, DateTime.UtcNow.AddMinutes(NEXT_LEVEL_CONTROL_TIME + 0.1));
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

            this.gameServer.World.UIMessages.addPlanetMessage(planet.Base.BaseId,
                UIMessagesFactory.tooFewQuantityMessage(planet.Base.BaseName, cargo.Name));

            return createTraderCargo(trader, economicLevel.LevelItems[0], random.Next(0, 101), cargo);
        }

        /// <summary>
        /// Method for creating trader cargo.
        /// </summary>
        /// <param name="trader">trader</param>
        /// <param name="item">economic level item</param>
        /// <param name="cargoCount">cargo count</param>
        /// <param name="cargo">cargo</param>
        /// <returns>trader cargo</returns>
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

        /// <summary>
        /// Method for produce and consume cargo.
        /// </summary>
        /// <param name="cargo">trader cargo</param>
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

        /// <summary>
        /// Method for calculating price.
        /// </summary>
        /// <param name="trader">trader</param>
        /// <param name="cargo">trader cargo</param>
        /// <param name="defaultPrice">default price</param>
        private void calculatePrice(Trader trader, TraderCargo cargo, int defaultPrice)
        {
            calculateBuyPrice(trader, cargo, defaultPrice);
            calculateSellPrice(trader, cargo, defaultPrice);
        }

        /// <summary>
        /// Method for calculating buy price.
        /// </summary>
        /// <param name="trader">trader</param>
        /// <param name="cargo">trader cargo</param>
        /// <param name="defaultPrice">default price</param>
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

        /// <summary>
        /// Method for calculating sell price.
        /// </summary>
        /// <param name="trader">trader</param>
        /// <param name="cargo">trader cargo</param>
        /// <param name="defaultPrice">default price</param>
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

        /// <summary>
        /// Method for calculating cargo for production.
        /// </summary>
        /// <param name="cargo">trader cargo</param>
        /// <returns>number of produced cargo</returns>
        private int produce(TraderCargo cargo)
        {
            return computeProduceOrConsumeValue(cargo.DailyProduction, cargo.TodayProduced, PRODUCTION_VARIANCE);
        }

        /// <summary>
        /// Method for calculating cargo for consumption.
        /// </summary>
        /// <param name="cargo">trader cargo</param>
        /// <returns>number of consumed cargo</returns>
        private int consume(TraderCargo cargo)
        {
            return computeProduceOrConsumeValue(cargo.DailyConsumption, cargo.TodayConsumed, CONSUMPTION_VARIANCE);
        }

        /// <summary>
        /// Method for calculating consume or produce value.
        /// </summary>
        /// <param name="dailyValue">daily value (production or consumption)</param>
        /// <param name="todayValue">today value (production or consumption)</param>
        /// <param name="variance">variance for production or consumption</param>
        /// <returns>calculated value</returns>
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

        /// <summary>
        /// Method for computing varianced value.
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="percentage">percentage (10% => 10)</param>
        /// <returns>plus minus value</returns>
        private int getPlusMinusValue(int value, int percentage)
        {
            int percentageValue = (int)(value * percentage / 100);
            int deviation = random.Next(0, percentageValue);

            if (random.Next(0, 2) == 0)
                return value + deviation;
            else
                return value - deviation;
        }

        /// <summary>
        /// Method for calculating how many times will be generated cargo.
        /// </summary>
        /// <returns>how many times will be generated cargo</returns>
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
            else
                addQuantityMessage(trader);
        }

        /// <summary>
        /// Method for create quantity messages.
        /// </summary>
        /// <param name="trader">trader with cargo</param>
        private void addQuantityMessage(Trader trader)
        {
            int maxValue = trader.TraderCargos.Max(x => x.CargoCount);
            int minValue = trader.TraderCargos.Min(x => x.CargoCount);
            
            bool min = minValue <= MIN_VALUE_FOR_MESSAGE;
            bool max = maxValue >= MAX_VALUE_FOR_MESSAGE;

            TraderCargo minCargo = trader.TraderCargos.FirstOrDefault(x => x.CargoCount == minValue);
            TraderCargo maxCargo = trader.TraderCargos.FirstOrDefault(x => x.CargoCount == maxValue);

            if(min && max){
                if (random.Next(0, 2) == 0)
                    this.gameServer.World.UIMessages.addPlanetMessage(trader.BaseId,
                        UIMessagesFactory.tooFewQuantityMessage(trader.Base.BaseName, minCargo.Cargo.Name));
                else
                    this.gameServer.World.UIMessages.addPlanetMessage(trader.BaseId,
                        UIMessagesFactory.tooMuchQuantityMessage(trader.Base.BaseName, maxCargo.Cargo.Name));
            }
            else if(min)
                this.gameServer.World.UIMessages.addPlanetMessage(trader.BaseId,
                        UIMessagesFactory.tooFewQuantityMessage(trader.Base.BaseName, minCargo.Cargo.Name));
            else if(max)
                this.gameServer.World.UIMessages.addPlanetMessage(trader.BaseId,
                        UIMessagesFactory.tooMuchQuantityMessage(trader.Base.BaseName, maxCargo.Cargo.Name));
            else
                this.gameServer.World.UIMessages.addPlanetMessage(trader.BaseId,
                        UIMessagesFactory.economicBalanceMessage(trader.Base.BaseName));
        }

        /// <summary>
        /// Method for clear today production and consumption.
        /// </summary>
        /// <param name="cargos">list of trader cargo</param>
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

        /// <summary>
        /// Method for evaluating when planet can be upgraded.
        /// </summary>
        /// <param name="level">economic level</param>
        /// <param name="cargos">list of trader cargos</param>
        /// <returns>true when level can be upgraded, otherwice false</returns>
        private bool canBeUpgraded(EconomicLevel level, ICollection<TraderCargo> cargos)
        {
            foreach (TraderCargo cargo in cargos)
            {
                if (cargo.CargoCount <= level.UpgradeLevelQuantity)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Method for evaluating when planet can be downgraded.
        /// </summary>
        /// <param name="level">economic level</param>
        /// <param name="cargos">list of trader cargos</param>
        /// <returns>true when level can be downgraded, otherwice false</returns>
        private bool canBeDowngraded(EconomicLevel level, ICollection<TraderCargo> cargos)
        {
            foreach (TraderCargo cargo in cargos)
            {
                if (cargo.CargoCount < level.DowngradeLevelQuantity)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Method for upgrade planet to next economic level.
        /// </summary>
        /// <param name="trader">trader with cargo</param>
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
        
        /// <summary>
        /// Method for upgrading consuption and production to next level or add new cargo.
        /// </summary>
        /// <param name="nextLevelItems">next level economic level items</param>
        /// <param name="trader">trader with cargo</param>
        private void upgradeCargo(IList<EconomicLevelItem> nextLevelItems, Trader trader)
        {
            ITraderCargoDAO tcdao = this.gameServer.Persistence.GetTraderCargoDAO();

            foreach (EconomicLevelItem item in nextLevelItems)
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

        /// <summary>
        /// Method for adding new unique cargo.
        /// </summary>
        /// <param name="item">economic level item</param>
        /// <param name="dao">dao</param>
        /// <param name="trader">trader with cargo</param>
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

            this.gameServer.World.UIMessages.addPlanetMessage(trader.BaseId,
                UIMessagesFactory.levelUpgradeMessage(trader.Base.BaseName, uniqueCargo.Name));
        }

        
        /// <summary>
        /// Method for calculating daily value for upgrade.
        /// </summary>
        /// <param name="dailyValue">actual daily value</param>
        /// <param name="percentage">percentage (10% => 10)</param>
        /// <returns>new daily value or actual daily value</returns>
        private int calcDailyValueUpgrade(int dailyValue, double percentage)
        {
            if(percentage != 0 && dailyValue != 0)
            {
                return (int)(dailyValue + (dailyValue * percentage / 100));
            }

            return dailyValue;
        }

        /// <summary>
        /// Method for calculating daily value for downgrade.
        /// </summary>
        /// <param name="dailyValue">actual daily value</param>
        /// <param name="percentage">percentage (10% => 10)</param>
        /// <returns>new daily value or actual daily value</returns>
        private int calcDailyValueDowngrade(int dailyValue, double percentage)
        {
            if (percentage != 0 && dailyValue != 0)
            {
                return (int)(dailyValue - (dailyValue * percentage / (100 + percentage)));
            }

            return dailyValue;
        }

        /// <summary>
        /// Method for downgraded planet to previous economic level.
        /// </summary>
        /// <param name="trader">trader with cargo</param>
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

        /// <summary>
        /// Method for downgrading consuption and production to previous level or remove last added cargo.
        /// </summary>
        /// <param name="actualLevelItems">actual level economic level items</param>
        /// <param name="previousLevelItems">previous level economic level items</param>
        /// <param name="trader">trader with cargo</param>
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
                {
                    this.gameServer.World.UIMessages.addPlanetMessage(trader.BaseId,
                        UIMessagesFactory.levelDowngradeMessage(trader.Base.BaseName, cargo.Cargo.Name));
                    tcdao.RemoveCargoById(cargo.TraderCargoId);
                }
            }
        }
    }
}