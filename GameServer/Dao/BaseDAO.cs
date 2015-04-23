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


        public Base GetBaseByPlanetFullName(string planetFullName)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Bases.FirstOrDefault(x => x.Planet.Equals(planetFullName));
            }
        }
    }
}
