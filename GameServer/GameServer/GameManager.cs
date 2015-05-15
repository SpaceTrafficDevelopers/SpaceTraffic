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
using SpaceTraffic.Game.Actions;
using SpaceTraffic.Game.Events;

namespace SpaceTraffic.GameServer
{
    class GameManager : IGameManager
    {
        public const int MAX_EVENTS_PER_UPDATE = 100;
        public const int MAX_ACTIONS_PER_UPDATE = 100;

        private IGameServer gameServer;
        /// <summary>
        /// Manager used to persist and restore game state.
        /// </summary>
        private IGameStateManager gameStateManager;
        private Random rand = new System.Random();

        public GameTime currentGameTime { get; private set; }
        private EventQueue gameEventQueue = new EventQueue();
        private ConcurrentQueue<IGameAction> gameActionQueue = new ConcurrentQueue<IGameAction>();        


        private Dictionary<string, IGameSimulation> simulations = new Dictionary<string, IGameSimulation>();

        public IDictionary<string, IGameSimulation> Simulations
        {
            get { return this.simulations; }
        }

        public GameManager(IGameServer gameServer, IGameStateManager gameStateManager)
        {
            this.gameServer = gameServer;
            this.gameStateManager = gameStateManager;
        }

        public void Update(GameTime gameTime)
        {
            this.currentGameTime = gameTime;
            this.DoEvents();
            this.DoActions();
        }

        /// <summary>
        /// Restores previously stored state of the game.
        /// 
        /// Only list of events and actions is restored. Game time is determined by the system time.
        /// </summary>
        internal void RestoreGameState()
        {
            IEnumerable<IGameAction> actions = gameStateManager.RestoreActions();
            foreach (var action in actions)
            {
                gameActionQueue.Enqueue(action);
            }
            IEnumerable<IGameEvent> events = gameStateManager.RestoreEvents();
            foreach (var evnt in events)
            {
                gameEventQueue.Enqueue(evnt);
            }
        }

        /// <summary>
        /// Stores state of the game to the persistence store.
        /// 
        /// Only list of events and actions is stored. Game time is determined by the system time.
        /// </summary>
        internal void PersistGameState()
        {
            gameStateManager.PersistActions(gameActionQueue);
            gameStateManager.PersistEvents(gameEventQueue.GetItems());
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
            action.ActionCode = GetUniqueActionId();
            action.State = GameActionState.PREPARED;
            this.gameActionQueue.Enqueue(action);
            return action.ActionCode;
        }

        public void PerformActionAsync(IGameAction action)
        {
            throw new NotImplementedException();
        }


        public void PlanEvent(IGameEvent gameEvent)
        {
            this.gameEventQueue.Enqueue(gameEvent);
        }

        /// <summary>
        /// Generates unique action identifer in List of actions.
        /// </summary>
        /// <returns>Unique identifer.</returns>
        private int GetUniqueActionId()
        {
            int unique = rand.Next();
            while(hasActionCode(gameActionQueue, unique)){
                unique = rand.Next();
            }

            return unique;
        }

        /// <summary>
        /// Determines whether given queue of action has action with given action code
        /// </summary>
        /// <param name="actionQueue">The action queue.</param>
        /// <param name="unique">The action code.</param>
        /// <returns></returns>
        private bool hasActionCode(IEnumerable<IGameAction> actionQueue, int unique)
        {
            foreach(IGameAction action in actionQueue){
                if(action.ActionCode == unique){
                    return true;
                }
            }
            return false;
        }

        public void ReplanEvent(IGameAction action, double seconds)
        {
            if (action != null && seconds >= 0)
            {
                action.State = GameActionState.PREPARED;
                GeneralEvent newEvent = new GeneralEvent();
                GameTime time = new GameTime();
                time.Value = this.currentGameTime.Value.AddSeconds(seconds);

                newEvent.BoundAction = action;
                newEvent.PlannedTime = time;

                PlanEvent(newEvent);
            }
        }
    }
}
