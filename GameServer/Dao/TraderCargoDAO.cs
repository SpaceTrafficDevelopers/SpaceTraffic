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
                {
                    tc = new TraderCargo();
                    tc.CargoPrice = cargoLoadEntity.CargoPrice;
                    tc.TraderId = cargoLoadEntity.CargoOwnerId;
                    tc.CargoId = cargoLoadEntity.CargoId;
                    tc.CargoCount = cargoLoadEntity.CargoCount;
                }

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

        public bool UpdateCargo(ICargoLoadEntity cargoLoadEntity)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var dbCargoLoadEntity = contextDB.TraderCargos.FirstOrDefault(x => x.TraderCargoId.Equals(cargoLoadEntity.CargoLoadEntityId));

                    if (dbCargoLoadEntity == null)
                        return false;

                    dbCargoLoadEntity.CargoId = cargoLoadEntity.CargoId;
                    dbCargoLoadEntity.CargoCount = cargoLoadEntity.CargoCount;
                    dbCargoLoadEntity.CargoPrice = cargoLoadEntity.CargoPrice;

                    contextDB.SaveChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private bool UpdateCargoCount(ICargoLoadEntity cargoLoadEntity)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var dbCargoLoadEntity = contextDB.TraderCargos.FirstOrDefault(x => x.CargoId.Equals(cargoLoadEntity.CargoId)
                                && x.CargoPrice.Equals(cargoLoadEntity.CargoPrice) && x.TraderId.Equals(cargoLoadEntity.CargoOwnerId));

                    if (dbCargoLoadEntity == null)
                        return false;

                    dbCargoLoadEntity.CargoCount += cargoLoadEntity.CargoCount;
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveCargoById(int traderCargoId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var traderCargoTab = contextDB.TraderCargos.First(x => x.TraderCargoId.Equals(traderCargoId));
                    if (traderCargoTab == null)
                    {
                        return true;
                    }

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
                return (ICargoLoadEntity)contextDB.TraderCargos.FirstOrDefault(x => x.TraderCargoId.Equals(cargoLoadEntityId));
            }
        }

        public List<ICargoLoadEntity> GetCargoListByOwnerId(int traderId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.TraderCargos.Where(x => x.TraderId.Equals(traderId)).ToList<ICargoLoadEntity>();
            }
        }

        public bool InsertOrUpdateCargo(ICargoLoadEntity cargo)
        {
            using (var contextDB = CreateContext())
            {
                var item = contextDB.TraderCargos.FirstOrDefault(x => x.CargoId.Equals(cargo.CargoId)
                                && x.CargoPrice.Equals(cargo.CargoPrice) && x.TraderId.Equals(cargo.CargoOwnerId));

                if (item == null)
                {
                    return this.InsertCargo(cargo);
                }
                else
                {
                    return this.UpdateCargoCount(cargo);
                }
            }
        }

        public bool UpdateOrRemoveCargo(ICargoLoadEntity cargo)
        {
            using (var contextDB = CreateContext())
            {
                if (cargo == null)
                    return false;

                var dbCargo = contextDB.TraderCargos.FirstOrDefault(x => x.CargoId.Equals(cargo.CargoId)
                                && x.CargoPrice.Equals(cargo.CargoPrice) && x.TraderId.Equals(cargo.CargoOwnerId));

                try
                {
                    dbCargo.CargoCount -= cargo.CargoCount;
                    contextDB.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }

				/* uncomment if you want cargo to disappear instead of set 0 count. It is this way because ship can sell to trader just things he has */
                /*if (dbCargo.CargoCount == 0)
                {
                    return RemoveCargoById(dbCargo.CargoLoadEntityId);
                }*/

                return true;
            }
        }
    }
}
