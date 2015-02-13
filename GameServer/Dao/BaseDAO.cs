using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class BaseDAO : AbstractDAO, IBaseDAO
    {
        public List<Base> GetBases()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Bases.ToList<Base>();
            }
        }

        public Base GetBaseById(int baseId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Bases.FirstOrDefault(x => x.BaseId.Equals(baseId));
            }
        }

        public bool InsertBase(Base bbase)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add base to context
                    contextDB.Bases.Add(bbase);
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

        public bool RemoveBaseById(int baseId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var baseTab = contextDB.Bases.FirstOrDefault(x => x.BaseId.Equals(baseId));
                    // remove base to context
                    contextDB.Bases.Remove(baseTab);
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

        public bool UpdateBaseById(Base bbase)
        {
            using (var contextDB = CreateContext())
                try
                {
                    var baseTab = contextDB.Bases.FirstOrDefault(x => x.BaseId.Equals(bbase.BaseId));
                    baseTab.Planet = bbase.Planet;
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
