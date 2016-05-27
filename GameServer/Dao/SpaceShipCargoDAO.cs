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
    public class SpaceShipCargoDAO : AbstractDAO, ISpaceShipCargoDAO
    {
        public bool InsertCargo(ICargoLoadEntity cargoLoadEntity)
        {
            using (var contextDB = CreateContext())
            {
                SpaceShipCargo ssc = cargoLoadEntity as SpaceShipCargo;

                if (ssc == null)
                {
                    ssc = new SpaceShipCargo();
                    ssc.SpaceShipId = cargoLoadEntity.CargoOwnerId;
                    ssc.CargoId = cargoLoadEntity.CargoId;
                    ssc.CargoCount = cargoLoadEntity.CargoCount;
                }

                try
                {
                    // add space ship to context
                    contextDB.SpaceShipsCargos.Add(ssc);
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
                    var dbCargoLoadEntity = contextDB.SpaceShipsCargos.FirstOrDefault(x => x.SpaceShipCargoId.Equals(cargoLoadEntity.CargoLoadEntityId));

                    if (dbCargoLoadEntity == null)
                        return false;

                    dbCargoLoadEntity.CargoId = cargoLoadEntity.CargoId;
                    dbCargoLoadEntity.CargoCount = cargoLoadEntity.CargoCount;
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
                    var dbCargoLoadEntity = contextDB.SpaceShipsCargos.FirstOrDefault(x => x.CargoId.Equals(cargoLoadEntity.CargoId)
                                && x.SpaceShipId.Equals(cargoLoadEntity.CargoOwnerId));

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

        public bool RemoveCargoById(int spaceShipCargoId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var spaceShipCargoTab = contextDB.SpaceShipsCargos.FirstOrDefault(x => x.SpaceShipCargoId.Equals(spaceShipCargoId));

                    if (spaceShipCargoTab == null)
                    {
                        return true;
                    }

                    contextDB.SpaceShipsCargos.Remove(spaceShipCargoTab);
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

        public bool UpdateOrRemoveCargo(ICargoLoadEntity cargo)
        {
            using (var contextDB = CreateContext())
            {
                if (cargo == null)
                    return false;

                var dbCargo = contextDB.SpaceShipsCargos.FirstOrDefault(x => x.CargoId.Equals(cargo.CargoId)
                                && x.SpaceShipId.Equals(cargo.CargoOwnerId));

                try { 
                    dbCargo.CargoCount -= cargo.CargoCount;
                    contextDB.SaveChanges();
                }
                catch (Exception) { 
                    return false;
                }

                if (dbCargo.CargoCount == 0)
                {
                    return RemoveCargoById(dbCargo.CargoLoadEntityId);
                }

                return true;
            }
        }


        public List<ICargoLoadEntity> GetCargoListByOwnerId(int spaceShipId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.SpaceShipsCargos.Where(x => x.SpaceShipId.Equals(spaceShipId)).ToList<ICargoLoadEntity>();
            }
        }


        public ICargoLoadEntity GetCargoByID(int cargoLoadEntityId)
        {
            using (var contextDB = CreateContext())
            {
                return (ICargoLoadEntity)contextDB.SpaceShipsCargos.FirstOrDefault(x => x.SpaceShipCargoId.Equals(cargoLoadEntityId));
            }
        }

        public bool InsertOrUpdateCargo(ICargoLoadEntity cargo)
        {
            using (var contextDB = CreateContext())
            {
                var item = contextDB.SpaceShipsCargos.FirstOrDefault(x => x.CargoId.Equals(cargo.CargoId) 
                                && x.SpaceShipId.Equals(cargo.CargoOwnerId));

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
    }
}
