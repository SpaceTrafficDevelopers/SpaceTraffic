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
        private bool InsertCargo(ICargoLoadEntity cargoLoadEntity)
        {
            using (var contextDB = CreateContext())
            {
                SpaceShipCargo ssc = cargoLoadEntity as SpaceShipCargo;

                if (ssc == null)
                    return false;

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
                    var spaceShipCargoTab = contextDB.SpaceShipsCargos.First(x => x.SpaceShipCargoId.Equals(cargoLoadEntity.CargoLoadEntityId));
                    spaceShipCargoTab.CargoCount = cargoLoadEntity.CargoCount;
                    spaceShipCargoTab.CargoPrice = cargoLoadEntity.CargoPrice;
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
                    var spaceShipCargoTab = contextDB.SpaceShipsCargos.First(x => x.SpaceShipCargoId.Equals(spaceShipCargoId));
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
                SpaceShipCargo tc = cargo as SpaceShipCargo;

                if (!(cargo is SpaceShipCargo))
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
                return (ICargoLoadEntity)contextDB.SpaceShipsCargos.First(x => x.SpaceShipCargoId.Equals(cargoLoadEntityId));
            }
        }

        public bool InsertOrUpdateCargo(ICargoLoadEntity cargo)
        {
            using (var contextDB = CreateContext())
            {
                SpaceShipCargo item = contextDB.SpaceShipsCargos.First(x => x.SpaceShipCargoId.Equals(cargo.CargoLoadEntityId));

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
    }
}
