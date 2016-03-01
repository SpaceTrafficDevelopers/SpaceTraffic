﻿/**
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
using System.Runtime.Serialization;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    [DataContract]
    public class Minigame : IMinigame
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public IDictionary<int, Player> Players { get; set; }

        [DataMember]
        public IMinigameDescriptor Descriptor { get; set; }

        [DataMember]
        public MinigameState State { get; set; }

        [DataMember]
        public bool FreeGame { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public DateTime LastRequestTime { get; set; }

        private object lockObj = new object();

        /// <summary>
        /// Minigame constructor without parameters for serialization.
        /// </summary>
        public Minigame() { }

        /// <summary>
        /// Minigame constructor.
        /// </summary>
        /// <param name="id">minigame id (has to be unique)</param>
        /// <param name="descriptor">minigame descriptor</param>
        /// <param name="createTime">time when the game was created</param>
        /// <param name="freeGame">indication, if the game has to be played as free game</param>
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