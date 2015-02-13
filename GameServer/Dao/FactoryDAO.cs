using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class FactoryDAO : AbstractDAO, IFactoryDAO
    {
        public List<Factory> GetFactories()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Factories.ToList<Factory>();
            }
        }

        public Factory GetFactoryById(int factoryId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Factories.FirstOrDefault(x => x.FacotryId.Equals(factoryId));
            }
        }

        public List<Factory> GetFactoriesByPlanet(string planet)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Factories.Where(x => x.Base.Planet.Equals(planet)).ToList<Factory>();
            }
        }

        public List<Factory> GetFactoriesByType(string type)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Factories.Where(x => x.Type.Equals(type)).ToList<Factory>();
            }
        }

        public bool InsertFactory(Factory factory)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add factory to context
                    contextDB.Factories.Add(factory);
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

        public bool RemoveFactoryById(int factoryId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var factoryTab = contextDB.Factories.FirstOrDefault(x => x.FacotryId.Equals(factoryId));
                    // remove factory to context
                    contextDB.Factories.Remove(factoryTab);
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

        public bool UpdateFactoryById(Factory factory)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var factoryTab = contextDB.Factories.FirstOrDefault(x => x.FacotryId.Equals(factory.FacotryId));
                    factoryTab.BaseId = factory.BaseId;
                    factoryTab.CargoId = factory.CargoId;
                    factoryTab.Type = factory.Type;
                    factoryTab.CargoCount = factory.CargoCount;                    
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
