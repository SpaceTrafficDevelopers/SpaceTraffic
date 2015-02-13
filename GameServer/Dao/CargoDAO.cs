using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class CargoDAO : AbstractDAO, ICargoDAO
    {
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
                return contextDB.Cargos.FirstOrDefault(x => x.CargoId.Equals(CargoId));
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
                    var CargoTab = contextDB.Cargos.FirstOrDefault(x => x.CargoId.Equals(CargoId));
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
                    var CargoTab = contextDB.Cargos.FirstOrDefault(x => x.CargoId.Equals(Cargo.CargoId));     
                    CargoTab.CargoId = Cargo.CargoId;
                    CargoTab.Type = Cargo.Type;
                    CargoTab.Price = Cargo.Price;                   
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


      
    }
}
