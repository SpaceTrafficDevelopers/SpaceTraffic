using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Events;
using SpaceTraffic.Game.Actions.Ships;
using SpaceTraffic.Game.Actions;

namespace SpaceTraffic.Game.Planner
{
    public class PathPlan : IPathPlan
    {
        List<PlanItem> planItems = new List<PlanItem>();

        public NavPath getNavPath()
        {
            NavPath path = new NavPath();

            foreach (PlanItem item in this.planItems)
            {
                path.Add(item.Place);
            }

            return path;
        }

       public NavPath getPathBetweenTwoItems(PlanItem source, PlanItem dest)
        {
            NavPath path = new NavPath();
           for(int i = this.IndexOf(source) +1; i < this.Count && i <= this.IndexOf(dest); i++)
           {
               path.Add(this.ElementAt(i).Place);
           }

           return path;
        }

        // tady je použito Spaceship jen kvůli tomu že ho chce ten PathPlanner
        // možná by byla lepší ta entita (SpaceShip)???
        public void SolvePath(Spaceship sh, double startTime)
        {
            NavPath path = this.getNavPath();
            PathPlanner.SolvePath(path, sh, startTime);
        }

        public List<IGameEvent> getEventsFromPath()
        {
            List<IGameEvent> eventList = new List<IGameEvent>();

            foreach (PlanItem item in this.planItems)
            {
                addEvents(eventList, item);
            }

            return eventList;
        }

        public void planEventsForNextItem(PlanItem item, IGameServer gameServer, Spaceship ship)
        {
            PlanItem nextItem = this.getNextBusyItem(item);
            if (nextItem != null)
            {
                ShipEvent newEvent;
                GameTime time;
                PlanFlightBetweenPoints(item, nextItem, gameServer, ship);


                foreach (IGameAction action in item.Actions)
                {
                    newEvent = new ShipEvent();
                    time = new GameTime();
                    time.Value = nextItem.Place.TimeOfArrival.AddSeconds(5);
                    newEvent.BoundAction = action;
                    newEvent.PlannedTime = time;
                    gameServer.Game.PlanEvent(newEvent);
                }

                newEvent = new ShipEvent();
                time = new GameTime();
                time.Value = nextItem.Place.TimeOfArrival.AddSeconds(10);
                IGameAction eventsPlan = new PlanEvents();
                eventsPlan.ActionArgs = new object[] { this, nextItem };
                newEvent.BoundAction = eventsPlan;
                newEvent.PlannedTime = time;
                gameServer.Game.PlanEvent(newEvent);
            }

        }

        public void PlanFirstItem(IGameServer gameServer, Spaceship ship)
        {
            PlanItem item = this.ElementAt(0);
            ShipEvent newEvent;
            GameTime time;

            foreach (IGameAction action in item.Actions)
            {
                newEvent = new ShipEvent();
                newEvent.BoundAction = action;
                newEvent.PlannedTime = gameServer.Game.currentGameTime;
                gameServer.Game.PlanEvent(newEvent);
            }

            PlanItem nextItem = this.getNextBusyItem(item);
            if (nextItem != null)
            {
                newEvent = new ShipEvent();
                time = new GameTime();
                time.Value = gameServer.Game.currentGameTime.Value.AddSeconds(5);
                IGameAction eventsPlan = new PlanEvents();
                eventsPlan.ActionArgs = new object[] { this, nextItem };
                newEvent.BoundAction = eventsPlan;
                newEvent.PlannedTime = time;
                gameServer.Game.PlanEvent(newEvent);
            }
        }

        private void PlanFlightBetweenPoints(PlanItem depart, PlanItem dest, IGameServer gameServer, Spaceship ship)
        {
            NavPath path = getPathBetweenTwoItems(depart, dest);
            PathPlanner.SolvePath(path, ship, gameServer.Game.currentGameTime.ValueInSeconds);
            GameTime time = new GameTime();
            ShipEvent shipEvent;
            IGameAction gameAction;

            if (depart.Place.Location is Planet)
            {
                Planet actualPlanet = depart.Place.Location as Planet;
                shipEvent = new ShipEvent();
                gameAction = new ShipTakeOff();

                gameAction.ActionArgs = new object[] { actualPlanet.StarSystem.Name, actualPlanet.Name, ship.Id };

                time.Value = dest.Place.TimeOfArrival;
                shipEvent.BoundAction = gameAction;
                shipEvent.PlannedTime = time;
                gameServer.Game.PlanEvent(shipEvent);
            }

            foreach(NavPoint point in path)
            {
                if(point.Location is WormholeEndpoint)
                {
                    WormholeEndpoint wormHole = point.Location as WormholeEndpoint;

                    shipEvent = new ShipEvent();
                    gameAction = new ShipFlyThroughWormHole();
                    time = new GameTime();
                    gameAction.ActionArgs = new object[] { wormHole.Id, ship.Id };

                    time.Value = point.TimeOfArrival;
                    shipEvent.BoundAction = gameAction;
                    shipEvent.PlannedTime = time;
                    gameServer.Game.PlanEvent(shipEvent);
                }
            }

            if (dest.Place.Location is Planet)
            {
                Planet actualPlanet = dest.Place.Location as Planet;
                shipEvent = new ShipEvent();
                gameAction = new ShipLand();

                gameAction.ActionArgs = new object[] { actualPlanet.StarSystem.Name, actualPlanet.Name, ship.Id };

                time.Value = dest.Place.TimeOfArrival;
                shipEvent.BoundAction = gameAction;
                shipEvent.PlannedTime = time;
                gameServer.Game.PlanEvent(shipEvent);
            }
        }

        private PlanItem getNextPlanet(PlanItem item)
        {
            int actualIndex = this.IndexOf(item);
            for (int i = actualIndex + 1; i < this.Count; i++)
            {
                PlanItem itemOnIndex = this.ElementAt(i);
                if (itemOnIndex.Place.Location is Planet)
                    return itemOnIndex;
            }
            return null;
        }

        private PlanItem getNextBusyItem(PlanItem item)
        {
            int actualIndex = this.IndexOf(item);
            for(int i = actualIndex + 1; i < this.Count; i++)
            {
                PlanItem itemOnIndex = this.ElementAt(i);
                if (itemOnIndex.hasActions())
                    return itemOnIndex;
            }

            if (actualIndex != this.Count - 1)
                return this.ElementAt(this.Count - 1);
            return null;
        }

        private void addEvents(List<IGameEvent> eventList, PlanItem planItem)
        {
            foreach (IGameAction item in planItem.Actions)
            {
                ShipEvent shipEvent = new ShipEvent();

                shipEvent.PlannedTime.Value = planItem.Place.TimeOfArrival;
                shipEvent.BoundAction = item;

                eventList.Add(shipEvent);
            }
        }

        #region IList implementation

        public int IndexOf(PlanItem item)
        {
            return this.planItems.IndexOf(item);
        }

        public void Insert(int index, PlanItem item)
        {
            this.planItems.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.planItems.RemoveAt(index);
        }

        #region IList indexer

        public PlanItem this[int index]
        {
            get
            {
                return this.planItems[index];
            }
            set
            {
                this.planItems[index] = value;
            }
        }

        #endregion

        public void Add(PlanItem item)
        {
            this.planItems.Add(item);
        }

        public void Clear()
        {
            this.planItems.Clear();
        }

        public bool Contains(PlanItem item)
        {
            return this.planItems.Contains(item);
        }

        public void CopyTo(PlanItem[] array, int arrayIndex)
        {
            this.planItems.CopyTo(array, arrayIndex);
        }

        #region IList properties

        public int Count
        {
            get { return this.planItems.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        #endregion

        public bool Remove(PlanItem item)
        {
            return this.planItems.Remove(item);
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<PlanItem> GetEnumerator()
        {
            return this.planItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.planItems.GetEnumerator();
        }

        #endregion
    }
}
