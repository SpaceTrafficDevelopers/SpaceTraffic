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
using SpaceTraffic.Entities.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Dao
{
    public class StartActionDAO : AbstractDAO, IStartActionDAO
    {

        public List<StartAction> GetStartActions()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.StartActions.ToList<StartAction>();
            }
        }

        public List<StartAction> GetStartActionsWithMinigamesList()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.StartActions.Include("Minigames").ToList<StartAction>();
            }
        }

        public Dictionary<string, StartAction> GetStartActionsWithMinigamesDictionary()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.StartActions.Include("Minigames").ToDictionary(x => x.ActionName, x => x);
            }
        }

        public StartAction GetStartActionById(int startActionId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.StartActions.FirstOrDefault(x => x.StartActionID.Equals(startActionId));
            }
        }

        public StartAction GetStartActionByName(string startActionName)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.StartActions.FirstOrDefault(x => x.ActionName.Equals(startActionName));
            }
        }

        public bool InsertStartAction(StartAction startAction)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add start action to context
                    contextDB.StartActions.Add(startAction);
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

        public bool RemoveStartActionById(int startActionId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var startAction = contextDB.StartActions.Include("Minigames").FirstOrDefault(x => x.StartActionID.Equals(startActionId));
                    
                    // remove start action from context
                    contextDB.StartActions.Remove(startAction);
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

        public bool UpdateStartActionById(StartAction startAction)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var st = contextDB.StartActions.FirstOrDefault(x => x.StartActionID.Equals(startAction.StartActionID));

                    st.ActionName = startAction.ActionName;
                    st.Minigames = startAction.Minigames;

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
