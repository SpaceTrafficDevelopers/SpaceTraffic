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
using System.Collections.Concurrent;

namespace SpaceTraffic.GameServer
{
    class GameManager : IGameManager
    {
        public const int MAX_EVENTS_PER_UPDATE = 100;
        public const int MAX_ACTIONS_PER_UPDATE = 100;

        private IGameServer gameServer;

        private GameTime currentGameTime;
        private EventQueue gameEventQueue = new EventQueue();
        private ConcurrentQueue<IGameAction> gameActionQueue = new ConcurrentQueue<IGameAction>();        


        private Dictionary<string, IGameSimulation> simulations = new Dictionary<string, IGameSimulation>();
        
        public IDictionary<string, IGameSimulation> Simulations
        {
            get { return this.simulations; }
        }

        public GameManager(IGameServer gameServer)
        {
            this.gameServer = gameServer;
        }

        public void Update(GameTime gameTime)
        {
            this.currentGameTime = gameTime;
            this.DoEvents();
            this.DoActions();
        }

        internal void RestoreGameState()
        {
            //TODO: Obnovení stavu hry
        }

        internal void PersistGameState()
        {
            //TODO: Uložení stavu hry
        }
        
        internal void DoEvents()
        {
            IGameEvent gameEvent;
            for (int i = 0; this.gameEventQueue.HasMore && (i < MAX_EVENTS_PER_UPDATE); i++)
            {
                //events are sorted in queue by time - but if the first one's time haven't come, it returns null 
                gameEvent = this.gameEventQueue.Dequeue(currentGameTime);
                if (gameEvent != null)
                {
                    gameEvent.BoundAction.Perform(this.gameServer);
                } else {//events are sorted, so there is not any older event in queue
                    break;
                }
            }
        }

        internal void DoActions()
        {
            IGameAction gameAction;
            for (int i = 0; i < MAX_ACTIONS_PER_UPDATE; i++)
            {
                if (gameActionQueue.TryDequeue(out gameAction))
                {
                    gameAction.Perform(this.gameServer);
                }
            }
        }

        public object PerformAction(IGameAction action)
        {

            throw new NotImplementedException();
        }

        public void PerformActionAsync(IGameAction action)
        {
            throw new NotImplementedException();
        }


        public void PlanEvent(IGameEvent gameEvent)
        {
            this.gameEventQueue.Enqueue(gameEvent);
        }
    }
}
