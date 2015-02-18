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
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Game.Navigation
{
    /// <summary>
    /// NavPoint represents navigation point in ships path.
    /// It consists of specific location on the path, represented by SpaceObject instance
    /// and time of arrival to that particular location, if this information is available for given path.
    /// </summary>
    public class NavPoint
    {

        /// <summary>
        /// Unknown time of arrival for this NavPoint.
        /// </summary>
        public static readonly DateTime UNKNOWN_TIME_OF_ARRIVAL = DateTime.MaxValue;
        public static readonly TimeSpan UNKNOWN_REMAINING_TIME_TO_ARRIVAL = TimeSpan.MaxValue;

        private DateTime _timeOfArrival;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public VisibleObject Location { get; set; }

        /// <summary>
        /// Gets or sets the time of arrival.
        /// </summary>
        /// <value>
        /// The time of arrival.
        /// </value>
        public DateTime TimeOfArrival
        {
            get { return this._timeOfArrival; }
            set
            {
                this._timeOfArrival = value;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance has time of arrival.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has time of arrival; otherwise, <c>false</c>.
        /// </value>
        public bool HasTimeOfArrival
        {
            get { return this._timeOfArrival != NavPoint.UNKNOWN_TIME_OF_ARRIVAL;  }
        }

        /// <summary>
        /// Gets or sets to this NavPoint.
        /// </summary>
        public Trajectory TrajectoryToDest { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavPoint"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="timeOfArrival">The time of arrival.</param>
        public NavPoint(VisibleObject location = null)
        {
            this.TimeOfArrival = NavPoint.UNKNOWN_TIME_OF_ARRIVAL;
            this.Location = location;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NavPoint"/> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="timeOfArrival">The time of arrival.</param>
        public NavPoint(VisibleObject location, DateTime timeOfArrival)
        {
            this.TimeOfArrival = timeOfArrival;
            this.Location = location;
        }

        /// <summary>
        /// Calculates the remaining time to arrival.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>estimated time of arrival or <c>UNKNOWN_REMAINING_TIME_TO_ARRIVAL</c> 
        /// if this <c>NavPoint</c> does not have proper TimeOfArrival value.</returns>
        public TimeSpan CalculateRta(DateTime currentTime)
        {
            if (this.HasTimeOfArrival)
            {
                return this.TimeOfArrival.Subtract(currentTime);
            }
            else
            {
                return NavPoint.UNKNOWN_REMAINING_TIME_TO_ARRIVAL;
            }
        }

        //TODO: Implementation equals.
    }
}
