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
        public bool InsertSpaceShipCargo(SpaceShipCargo spaceShipCargo)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add space ship to context
                    contextDB.SpaceShipsCargos.Add(spaceShipCargo);
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

        public bool UpdateCargoCountById(SpaceShipCargo spaceShipCargo)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var spaceShipCargoTab = contextDB.SpaceShipsCargos.FirstOrDefault(x => x.SpaceShipId.Equals(spaceShipCargo.SpaceShipId) && x.CargoId.Equals(spaceShipCargo.CargoId));
                    spaceShipCargoTab.CargoCount += spaceShipCargo.CargoCount;            
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

        public bool RemoveSpaceShipCargoById(int spaceShipId, int cargoId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var spaceShipCargoTab = contextDB.SpaceShipsCargos.FirstOrDefault(x => x.SpaceShipId.Equals(spaceShipId) && x.CargoId.Equals(cargoId));
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


        public List<SpaceShipCargo> GetSpaceShipCargoBySpaceShipId(int spaceShipId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.SpaceShipsCargos.Where(x => x.SpaceShipId.Equals(spaceShipId)).ToList();
            }
        }


    }
}
