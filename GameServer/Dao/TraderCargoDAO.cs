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
        public bool InsertCargo(ICargoLoadEntity cargoLoadEntity)
        {
            using (var contextDB = CreateContext())
            {
                TraderCargo tc = cargoLoadEntity as TraderCargo;

                if (tc == null)
                    return false;

                try
                {
                    // add space ship to context
                    contextDB.TraderCargos.Add(tc);
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

        public bool UpdateCargo(ICargoLoadEntity traderCargo) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var traderCargoTab = contextDB.TraderCargos.First(x => x.TraderCargoId.Equals(traderCargo.CargoLoadEntityId));
                    traderCargoTab.CargoCount = traderCargo.CargoCount;
                    traderCargoTab.CargoPrice = traderCargo.CargoPrice;
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveCargoById(int traderCargoId) {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var traderCargoTab = contextDB.TraderCargos.First(x => x.TraderCargoId.Equals(traderCargoId));
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

        public ICargoLoadEntity GetCargoByID(int cargoLoadEntityId)
        {
            using (var contextDB = CreateContext())
            {
                return (ICargoLoadEntity)contextDB.TraderCargos.Where(x => x.TraderCargoId.Equals(cargoLoadEntityId));
            }
        }

        public List<ICargoLoadEntity> GetCargoListByOwnerId(int traderId) {
            using (var contextDB = CreateContext())
            {
                return contextDB.TraderCargos.Where(x => x.TraderId.Equals(traderId)).ToList<ICargoLoadEntity>();
            }
        }

        public bool InsertOrUpdateCargo(ICargoLoadEntity cargo)
        {
            using (var contextDB = CreateContext())
            {
                TraderCargo item = contextDB.TraderCargos.First(x => x.TraderCargoId.Equals(cargo.CargoLoadEntityId));

                if (item == null)
                {
                    return this.InsertCargo(cargo);
                }
                else
                {
                    return this.UpdateCargo(cargo);
                }
            }
        }

        public bool UpdateOrRemoveCargo(ICargoLoadEntity cargo)
        {
            using (var contextDB = CreateContext())
            {
                if (!(cargo is TraderCargo))
                    return false;
                
                if (cargo.CargoCount == 0)
                {
                    return RemoveCargoById(cargo.CargoLoadEntityId);
                }
                else
                {
                    return UpdateCargo(cargo);
                }
            }
        }
    }
}
