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
            this.queue.Sort();
            //TODO: optimalizace přidávání do fronty
        }

        public IGameEvent Dequeue()
        {
            IGameEvent gameEvent = this.queue[0];
            this.queue.RemoveAt(0);
            return gameEvent;
            //TODO: optimalizace přidávání do fronty
        }
    }
}
