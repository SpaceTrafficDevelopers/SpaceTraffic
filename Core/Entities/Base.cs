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

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// Class representing entity of base on planet
    /// </summary>
    public class Base
    {
        /// <summary>
        /// Identification number
        /// </summary>
        public int BaseId { get; set; }

		/// <summary>
		/// Name of the base
		/// </summary>
		public string BaseName { get; set; }

		/// <summary>
		/// Planet on which base is
		/// </summary>
		public string Planet { get; set; }


        /// <summary>
        /// Trader which trades on the planet
        /// </summary>
        public Trader Trader { get; set; }

        /// <summary>
        /// Collection of spaceships
        /// </summary>
        public virtual ICollection<SpaceShip> SpaceShips { get; set; }

        /// <summary>
        /// Colection of factories
        /// </summary>
        public virtual ICollection<Factory> Factories { get; set; }  
    }
}
