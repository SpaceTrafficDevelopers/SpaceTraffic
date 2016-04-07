using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Events;
using SpaceTraffic.Game.Actions;
using SpaceTraffic.Entities;
using SpaceTraffic.Game.Utils;

namespace SpaceTraffic.Game.Planner
{
    public class PathPlan : IPathPlan
    {
        private static readonly int TIME_BETWEEN_EVENTS = 5;

        List<PlanItem> planItems = new List<PlanItem>();

        public int PlayerID { get; set; }

        public Spaceship ship { get; set; }

        public bool IsCycled { get; set; }

        public PathPlan(int _PlayerID, Spaceship ship, bool isCycled)
        {
            PlayerID = _PlayerID;
            this.ship = ship;
            this.IsCycled = isCycled;
        }

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
           for(int i = this.IndexOf(source); i < this.Count && i <= this.IndexOf(dest); i++)
           {
               NavPoint point = this.ElementAt(i).Place;
               path.Add(point);
           }

           return path;
        }

        public void SolvePath(double startTime)
        {
            NavPath path = this.getNavPath();
            PathPlanner.SolvePath(path, ship, startTime);
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

        public void planEventsForNextItem(PlanItem item, IGameServer gameServer)
        {
            if (!isShipOnItem(item, gameServer))
                return;

            PlanItem nextItem = this.getNextBusyItem(item);
            if (nextItem != null)
            {
                if (!PlanFlightBetweenPoints(item, nextItem, gameServer, ship))
                    return;

                double actionStartDelay = TIME_BETWEEN_EVENTS;
                foreach (IPlannableAction action in nextItem.Actions)
                {
                    action.PlayerId = PlayerID;
                    gameServer.Game.PlanEvent(action, nextItem.Place.TimeOfArrival.AddSeconds(actionStartDelay));
                    actionStartDelay += action.Duration + TIME_BETWEEN_EVENTS;
                }

                IGameAction eventsPlan = new PlanEvents();
                eventsPlan.ActionArgs = new object[] { this, nextItem, ship };
                eventsPlan.PlayerId = PlayerID;

                gameServer.Game.PlanEvent(eventsPlan, nextItem.Place.TimeOfArrival.AddSeconds(actionStartDelay));
            }
            else if (IsCycled && this.ElementAt(0).Place.Location.Equals(this.ElementAt(this.Count-1).Place.Location))
                PlanFirstItem(gameServer);

        }

        public string PlanFirstItem(IGameServer gameServer)
        {
            PlanItem item = this.ElementAt(0);

            if (!isShipOnItem(item, gameServer))
                return "Loď se ztratila.";


            double actionStartDelay = TIME_BETWEEN_EVENTS;
            foreach (IPlannableAction action in item.Actions)
            {
                action.PlayerId = PlayerID;              
                gameServer.Game.PlanEvent(action, gameServer.Game.currentGameTime.Value.AddSeconds(actionStartDelay));
                actionStartDelay += action.Duration + TIME_BETWEEN_EVENTS;
            }

            PlanItem nextItem = this.getNextBusyItem(item);
            if (nextItem != null)
            {
                IGameAction eventsPlan = new PlanEvents();
                eventsPlan.ActionArgs = new object[] { this, item };
                eventsPlan.PlayerId = PlayerID;

				NavPath path = getPathBetweenTwoItems(item, nextItem);
				PathPlanner.SolvePath(path, ship, gameServer.Game.currentGameTime.ValueInSeconds);
				Double flightTime = (nextItem.Place.TimeOfArrival.Subtract(item.Place.TimeOfArrival)).TotalSeconds;

				if (ActionControls.isShipTooMuchDamaged(ship.Id, flightTime))
				{
					return "Loď je příliš poškozená a cestu by nezvládla.";
				}
				if (!ActionControls.hasShipEnoughFuel(ship.Id, flightTime))
				{
					return "Loď nemá na cestu dostatek paliva.";
				}
				
                gameServer.Game.PlanEvent(eventsPlan, gameServer.Game.currentGameTime.Value.AddSeconds(actionStartDelay));
            }
			return null;/* ok */
        }

        public bool PlanFlightBetweenPoints(PlanItem depart, PlanItem dest, IGameServer gameServer, Spaceship ship)
        {
            NavPath path = getPathBetweenTwoItems(depart, dest);
            PathPlanner.SolvePath(path, ship, gameServer.Game.currentGameTime.ValueInSeconds);
            GameTime time = new GameTime();
            IGameAction gameAction;
            Double flightTime = (dest.Place.TimeOfArrival.Subtract(depart.Place.TimeOfArrival)).TotalSeconds;

            if (!ActionControls.isShipReadyForTravel(ship.Id, flightTime) || !isShipOnItem(depart, gameServer))
                return false;

            if (depart.Place.Location is Planet)
            {
                Planet actualPlanet = depart.Place.Location as Planet;

                gameAction = new ShipTakeOff();
                gameAction.ActionArgs = new object[] { actualPlanet.StarSystem.Name, actualPlanet.Name, ship.Id };
                gameAction.PlayerId = PlayerID;

                gameServer.Game.PlanEvent(gameAction, gameServer.Game.currentGameTime.Value);
            }

            foreach(NavPoint point in path)
            {
                if(point.Location is WormholeEndpoint)
                {
                    WormholeEndpoint wormHole = point.Location as WormholeEndpoint;

                    gameAction = new ShipFlyThroughWormHole();
                    gameAction.ActionArgs = new object[] { wormHole.Id, ship.Id };
                    gameAction.PlayerId = PlayerID;
                    gameServer.Game.PlanEvent(gameAction, point.TimeOfArrival);
                }
            }

            if (dest.Place.Location is Planet)
            {
                Planet actualPlanet = dest.Place.Location as Planet;
                gameAction = new ShipLand();
				
                gameAction.ActionArgs = new object[] { actualPlanet.StarSystem.Name, actualPlanet.Name, ship.Id,  flightTime};
                gameAction.PlayerId = PlayerID;

                gameServer.Game.PlanEvent(gameAction, dest.Place.TimeOfArrival);
            }

            return true;
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

        private bool isShipOnItem(PlanItem item, IGameServer gameServer)
        {
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ship.Id);
            Planet itemPlanet = item.Place.Location as Planet;

            return itemPlanet != null && spaceShip.DockedAtBaseId != null && spaceShip.DockedAtBaseId == itemPlanet.Base.BaseId;
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
