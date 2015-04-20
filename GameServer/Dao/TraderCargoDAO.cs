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
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class TraderCargoDAO : AbstractDAO, ITraderCargoDAO
    {
        public bool InsertTraderCargo(TraderCargo traderCargo) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add space ship to context
                    contextDB.TraderCargos.Add(traderCargo);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool UpdateCargoCountById(TraderCargo traderCargo) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var traderCargoTab = contextDB.TraderCargos.FirstOrDefault(x => x.TraderId.Equals(traderCargo.TraderId) && x.CargoId.Equals(traderCargo.CargoId));
                    traderCargoTab.CargoCount += traderCargo.CargoCount;
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool UpdateCargoPriceById(TraderCargo traderCargo) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var traderCargoTab = contextDB.TraderCargos.FirstOrDefault(x => x.TraderId.Equals(traderCargo.TraderId) && x.CargoId.Equals(traderCargo.CargoId));
                    traderCargoTab.CargoPrice = traderCargo.CargoPrice;
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveTraderCargoById(int traderId, int cargoId) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var traderCargoTab = contextDB.TraderCargos.FirstOrDefault(x => x.TraderId.Equals(traderId) && x.CargoId.Equals(cargoId));
                    contextDB.TraderCargos.Remove(traderCargoTab);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public ICargoLoadEntity GetCargoByID(int traderId, int cargoId)
        {
            using (var contextDB = CreateContext())
            {

                return (ICargoLoadEntity)contextDB.TraderCargos.Where(x => x.TraderId.Equals(traderId) && x.CargoId.Equals(cargoId));
            }
        }

        public List<TraderCargo> GetTraderCargoByTraderId(int traderId) {
            using (var contextDB = CreateContext())
            {
                return contextDB.TraderCargos.Where(x => x.TraderId.Equals(traderId)).ToList();
            }
        }

        public bool InsertCargo(ICargoLoadEntity cargoLoadEntity)
        {
            TraderCargo tc = cargoLoadEntity as TraderCargo;

            if (tc == null)
                return false;

            return this.InsertTraderCargo(tc);
        }

        public bool UpdateCargoPriceById(ICargoLoadEntity cargoLoadEntity)
        {
            TraderCargo tc = cargoLoadEntity as TraderCargo;

            if (tc == null)
                return false;

            return this.UpdateCargoPriceById(tc);
        }

        public bool UpdateCargoCountById(ICargoLoadEntity cargoLoadEntity)
        {
            TraderCargo tc = cargoLoadEntity as TraderCargo;

            if (tc == null)
                return false;

            return this.UpdateCargoCountById(tc);
        }

        public bool RemoveCargo(ICargoLoadEntity cargoLoadEntity)
        {
            TraderCargo tc = cargoLoadEntity as TraderCargo;

            if (tc == null)
                return false;

            return this.RemoveTraderCargoById(tc.TraderId, tc.CargoId);
        }

      /*  public ICargo UpdateOrRemoveCargoByCountAndID(int traderID, int cargoID, int Count)
        {
            TraderCargo tc = cargoLoadEntity as TraderCargo;

            if (tc == null)
                return false;

            return this.UpdateCargoCountById(tc);
        }*/
    }
}
