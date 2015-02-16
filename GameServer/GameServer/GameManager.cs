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
