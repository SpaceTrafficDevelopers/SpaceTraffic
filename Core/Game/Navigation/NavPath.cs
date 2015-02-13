using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Navigation
{
    public class NavPath : IList<NavPoint>
    {
        private List<NavPoint> navPoints = new List<NavPoint>();
        
        //TODO: Trajectory handling for path.
        
        #region Properties
        /// <summary>
        /// Gets the destination of this path.
        /// </summary>
        public NavPoint Destination
        {
            get
            {
                //TODO: Test performance of this method; if implementation is correct for array list.
                return this.navPoints.Last();
            }
        }

        /// <summary>
        /// Gets the start of this path.
        /// </summary>
        public NavPoint Start
        {
            get
            {
                return this.navPoints.First();
            }
        }

        #region IList properties
        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get { return this.navPoints.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return true; }
        }
        #endregion
        #endregion

        #region IList indexer
        /// <summary>
        /// Gets or sets the <see cref="SpaceTraffic.Data.NavPoint"/> at the specified index.
        /// </summary>
        public NavPoint this[int index]
        {
            get
            {
                return this.navPoints[index];
            }
            set
            {
                this.navPoints[index] = value;
            }
        }
        #endregion

        #region IList implementation
        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="navPoint">The nav point.</param>
        /// <returns></returns>
        public int IndexOf(NavPoint navPoint)
        {
            return this.navPoints.IndexOf(navPoint);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="navPoint">The nav point.</param>
        public void Insert(int index, NavPoint navPoint)
        {
            this.navPoints.Insert(index, navPoint);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            this.navPoints.RemoveAt(index);
        }



        /// <summary>
        /// Adds the specified nav point.
        /// </summary>
        /// <param name="navPoint">The nav point.</param>
        public void Add(NavPoint navPoint)
        {
            this.navPoints.Add(navPoint);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.navPoints.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified nav point].
        /// </summary>
        /// <param name="navPoint">The nav point.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified nav point]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(NavPoint navPoint)
        {
            return this.navPoints.Contains(navPoint);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(NavPoint[] array, int arrayIndex)
        {
            this.navPoints.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified nav point.
        /// </summary>
        /// <param name="navPoint">The nav point.</param>
        /// <returns></returns>
        public bool Remove(NavPoint navPoint)
        {
            return this.navPoints.Remove(navPoint);
        }
        #endregion

        #region IEnumerable implementation
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<NavPoint> GetEnumerator()
        {
            return this.navPoints.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.navPoints.GetEnumerator();
        }
        #endregion


        /// <summary>
        /// Calculates the remaining time to arrival to the destination <c>NavPoint</c> from start of this path.
        /// </summary>
        /// <returns>remaining time to arrival to the destination from start navpoint of this path or <c>NavPoint.UNKNOWN_TIME_OF_ARRIVAL</c>.</returns>
        public TimeSpan CalculateRtaToDestination()
        {
            if (this.Count > 0)
            {
                if (this.Start.HasTimeOfArrival)
                    return this.Destination.CalculateRta(this.Start.TimeOfArrival);
                else
                    return NavPoint.UNKNOWN_REMAINING_TIME_TO_ARRIVAL;
            }
            else
            {
                throw new InvalidOperationException("NavPath is empty.");
            }
        }

        /// <summary>
        /// Calculates the remaining time to arrival to the destination <c>NavPoint</c> from given current time.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>remaining time to arrival to the destination from start navpoint of this path or <c>NavPoint.UNKNOWN_TIME_OF_ARRIVAL</c>.</returns>
        public TimeSpan CalculateRtaToDestination(DateTime currentTime)
        {
            if (this.Count > 0)
            {
                return this.Destination.CalculateRta(currentTime);
            }
            else
            {
                throw new InvalidOperationException("NavPath is empty.");
            }
        }

        /// <summary>
        /// Gets the estimated time of arrival to destination.
        /// </summary>
        /// <returns>estimated time of arrival to destination</returns>
        public DateTime GetEtaToDestination()
        {
            if (this.Count > 0)
            {
                return this.Destination.TimeOfArrival;
            }
            else
            {
                throw new InvalidOperationException("NavPath is empty.");
            }
        }

        //TODO: GetNextNavPoint(currentTime)
        //TODO: GetEtaT
    }
}
