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
using SpaceTraffic.Engine;

namespace SpaceTraffic.GameServer
{
    internal class EventQueue
    {
        //TODO: implementace pomocí HEAP datové struktury

        private List<IGameEvent> queue = new List<IGameEvent>();

        public bool IsEmpty
        {
            get { return !(queue.Count > 0); }
        }

        public bool HasMore
        {
            get { return queue.Count > 0; }
        }

        public void Enqueue(IGameEvent gameEvent)
        {
            this.queue.Add(gameEvent);

            if(this.queue.Count > 1)
                this.queue.Sort((a, b) => a.PlannedTime.CompareTo(b.PlannedTime));

            //TODO: optimalizace přidávání do fronty
        }

        public IGameEvent Dequeue(GameTime time)
        {
            IGameEvent gameEvent = this.queue[0];
            if (gameEvent.PlannedTime.Value.CompareTo(time.Value) <= 0)
            {
                this.queue.RemoveAt(0);
                return gameEvent;
            }else{
                return null;
            }
            //TODO: optimalizace přidávání do fronty
        }

        /// <summary>
        /// Gets ordered list of items in the queue.
        /// </summary>
        /// <returns>Collection of items from the queue.</returns>
        public IEnumerable<IGameEvent> GetItems()
        {
            // It would be better to clone the queue for safety reasons,
            // but since the implementation is quite internal, it is better to
            // use the more performant approach.
            return queue;
        }
    }
}
