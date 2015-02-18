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

namespace SpaceTraffic.Game.Geometry
{
    /// <summary>
    /// Class which defines stacionary trajectory
    /// Version 1.0
    /// </summary>
    public struct Stacionary : Trajectory
    {
        private double _x;
        private double _y;

        #region Properties

        /// <summary>
        /// Gets or sets the coordinate axes X.
        /// </summary>
        /// <value>
        /// The coordinate axes X.
        /// </value>
        public double X
        {
            get { return _x; }
            set { this._x = value; }
        }

        /// <summary>
        /// Gets or sets the coordinate axes Y.
        /// </summary>
        /// <value>
        /// The coordinate axes Y.
        /// </value>
        public double Y
        {
            get { return _y; }
            set { this._y = value; }
        }

        /// <summary>
        /// Contructor for point
        /// </summary>
        /// <param name="posX">Position on x axis</param>
        /// <param name="posY">Position on y axis</param>
        public Stacionary(double x, double y)
        {
            this._x = x;
            this._y = y;
        }
        #endregion

        /// <summary>
        /// Method for calculating position in given time
        /// </summary>
        /// <param name="timeInSec">time for calculation</param>
        /// <returns>Calculated position</returns>
        public Point2d CalculatePosition(double timeInSec)
        {
            return new Point2d(this.X, this.Y);
        }
        /// <summary>
        /// Internal toString method..
        /// </summary>
        /// <returns>X:Y</returns>
        internal string toString()
        {
            return "" + this.X + ":" + this.Y;
        }
    }
}
