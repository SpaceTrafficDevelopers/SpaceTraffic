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
    public class CargoDAO : AbstractDAO, ICargoDAO
    {
        /*
        public List<Cargo> GetCargos()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Cargos.ToList<Cargo>();
            }
        }

        public Cargo GetCargoById(int CargoId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Cargos.FirstOrDefault(x => x.Goods.Equals(CargoId));
            }
        }

        public List<Cargo> GetCargosByType(string type)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Cargos.Where(x => x.Type.Equals(type)).ToList<Cargo>();
            }
        }



       
        public bool InsertCargo(Cargo Cargo)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add Cargo to context
                    contextDB.Cargos.Add(Cargo);
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

        public bool RemoveCargoById(int CargoId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var CargoTab = contextDB.Cargos.FirstOrDefault(x => x.Goods.Equals(CargoId));
                    // remove Cargo to context
                    contextDB.Cargos.Remove(CargoTab);
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

        public bool UpdateCargoById(Cargo Cargo)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var CargoTab = contextDB.Cargos.FirstOrDefault(x => x.Goods.Equals(Cargo.Goods));     
                    CargoTab.Goods = Cargo.Goods;
                    CargoTab.Type = Cargo.Type;
                    CargoTab.PriceCargo = Cargo.PriceCargo;                   
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


      */
    }
}
