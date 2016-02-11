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
using SpaceTraffic.Entities.Minigames;

namespace SpaceTraffic.Dao
{
    public class MinigameDescriptorDAO : AbstractDAO, IMinigameDescriptorDAO
    {

        public List<MinigameDescriptor> GetMinigames()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Minigames.ToList<MinigameDescriptor>();
            }
        }

        public List<MinigameDescriptor> GetMinigamesWithStartActions()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Minigames.Include("StartActions").ToList<MinigameDescriptor>();
            }
        }

        public MinigameDescriptor GetMinigameById(int minigameId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Minigames.FirstOrDefault(x => x.MinigameId.Equals(minigameId));
            }
        }

        public MinigameDescriptor GetMinigameWithStartActionsById(int minigameId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Minigames.Include("StartActions").FirstOrDefault(x => x.MinigameId.Equals(minigameId));
            }
        }

        public MinigameDescriptor GetMinigameByName(string minigameName)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Minigames.FirstOrDefault(x => x.Name.Equals(minigameName));
            }
        }

        public bool InsertMinigame(MinigameDescriptor minigame)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    //if minigame contains some start actions, get actions from db by name and replace them
                    //this is necessary otherwise the entity framework added new actions to database
                    if (minigame.StartActions != null && minigame.StartActions.Count != 0)
                    {
                        List<StartAction> startActionList = new List<StartAction>();
                        foreach (StartAction sa in minigame.StartActions)
                        {
                            var startActionDB = contextDB.StartActions.FirstOrDefault(x => x.ActionName.CompareTo(sa.ActionName) == 0);

                            if (startActionDB == null)
                                return false;

                            startActionList.Add(startActionDB);
                        }

                        minigame.StartActions = startActionList;
                    }

                    // add minigame to context
                    contextDB.Minigames.Add(minigame);
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

        public bool InsertRelationshipWithStartActions(int minigameId, int startActionId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var minigame = contextDB.Minigames.Include("StartActions").FirstOrDefault(x => x.MinigameId.Equals(minigameId));
                    var startAction = contextDB.StartActions.FirstOrDefault(x => x.StartActionID.Equals(startActionId));

                    // add relationship with start action
                    minigame.StartActions.Add(startAction);

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

        public bool RemoveRelationshipWithStartActions(int minigameId, int startActionId) 
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var minigame = contextDB.Minigames.Include("StartActions").FirstOrDefault(x => x.MinigameId.Equals(minigameId));
                    var startAction = contextDB.StartActions.FirstOrDefault(x => x.StartActionID.Equals(startActionId));
                    
                    // remove relationship with start action
                    minigame.StartActions.Remove(startAction);

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

        public bool RemoveMinigameById(int minigameId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var minigame = contextDB.Minigames.Include("StartActions").FirstOrDefault(x => x.MinigameId.Equals(minigameId));
                    
                    // remove all relationship with start actions
                    minigame.StartActions.Clear();
                    // remove minigame from context
                    contextDB.Minigames.Remove(minigame);
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

        public bool UpdateMinigameById(MinigameDescriptor minigame)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var mg = contextDB.Minigames.FirstOrDefault(x => x.MinigameId.Equals(minigame.MinigameId));

                    mg.Name = minigame.Name;
                    mg.PlayerCount = minigame.PlayerCount;
                    mg.Description = minigame.Description;
                    mg.RewardType = minigame.RewardType;
                    mg.SpecificReward = minigame.SpecificReward;
                    mg.RewardAmount = minigame.RewardAmount;
                    mg.ConditionType = minigame.ConditionType;
                    mg.ConditionArgs = minigame.ConditionArgs;
                    mg.ExternalClient = minigame.ExternalClient;
                    mg.ClientURL = minigame.ClientURL;

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
