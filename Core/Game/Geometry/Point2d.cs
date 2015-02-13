using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Geometry
{
    /// <summary>
    /// Class which defines point in 2D space
    /// Version 1.0
    /// </summary>
    public struct Point2d
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
        public Point2d(double x, double y)
        {
            this._x = x;
            this._y = y;
        }
        #endregion

        /// <summary>
        /// Internal toString method
        /// </summary>
        /// <returns>X:Y</returns>
        internal string toString()
        {
            return ""+this.X + ":" +this.Y;
        }
    }
}
