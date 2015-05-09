using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Events;

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
