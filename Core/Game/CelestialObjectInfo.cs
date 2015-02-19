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

namespace SpaceTraffic.Game
{
    /// <summary>
    /// Class which defines details of space object.
    /// Version 1.0
    /// </summary>
    public struct CelestialObjectInfo
    {
        private double _gravity;
        private double _mass;
        private string _description;

        #region Properties

        /// <summary>
        /// Gets or sets the gravity of space object.
        /// </summary>
        /// <value>
        /// The gravity of space object.
        /// </value>
        public double Gravity
        {
            get { return _gravity; }
            set { this._gravity = value; }
        }

        /// <summary>
        /// Gets or sets the mass of space object.
        /// </summary>
        /// <value>
        /// The mass of space object.
        /// </value>
        public double Mass
        {
            get { return _mass; }
            set { this._mass = value; }
        }

        /// <summary>
        /// Gets or sets the description of space object.
        /// </summary>
        /// <value>
        /// The description of space object.
        /// </value>
        public string Description
        {
            get { return _description; }
            set { this._description = value; }
        }

        /// <summary>
        /// Contructor for point
        /// </summary>
        /// <param name="posX">Position on x axis</param>
        /// <param name="posY">Position on y axis</param>
        public CelestialObjectInfo(double gravity, double mass, string description)
        {
            this._gravity = gravity;
            this._mass = mass;
            this._description = description;
        }
        #endregion
        
        /// <summary>
        /// Internal toString method
        /// </summary>
        /// <returns>gravity, mass, description</returns>
        internal string toString()
        {
            return "gravity: " + this.Gravity + ", mass:" + this.Mass + ", description:" + this.Description;
        }
    }
}
