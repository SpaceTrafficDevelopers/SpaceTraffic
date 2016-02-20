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
using SpaceTraffic.Entities;
using SpaceTraffic.Entities.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    public class Minigame : IMinigame
    {
        public int ID { get; set; }

        public IDictionary<int, Player> Players { get; set; }

        public IMinigameDescriptor Descriptor { get; set; }

        public MinigameState State { get; set; }

        public bool FreeGame { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastRequestTime { get; set; }

        private object lockObj = new object();

        public Minigame() { }

        public Minigame(int id, IMinigameDescriptor descriptor, DateTime createTime, bool freeGame)
        {
            this.ID = id;
            this.Players = new Dictionary<int, Player>();
            this.State = MinigameState.CREATED;
            this.Descriptor = descriptor;
            this.FreeGame = freeGame;
            this.CreateTime = createTime;
            this.LastRequestTime = createTime;
        }

        public object performAction(string actionName, object[] actionArgs)
        {
            try
            {
                MethodInfo method = this.GetType().GetMethod(actionName);
                object returnValue = method.Invoke(this, actionArgs);

                return returnValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public object performActionWithLock(string actionName, object[] actionArgs)
        {
            lock (lockObj)
            {
                return performAction(actionName, actionArgs);
            }
        }
    }
}
