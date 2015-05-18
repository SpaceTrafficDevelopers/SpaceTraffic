using SpaceTraffic.Entities.Goods;
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
    /// Class representing cargo in database
    /// </summary>
    public class Cargo
    {
        /// <summary>
        /// Identification number
        /// </summary>
        public int CargoId { get; set; }

        /// <summary>
        /// Default price of goods
        /// </summary>
        public int DefaultPrice { get; set; }

        /// <summary>
        /// Name of goods
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category of goods
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Short description of goods
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Type of goods - mainstream or special
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Player level to buy this goods
        /// </summary>
        public int LevelToBuy { get; set; }

        /// <summary>
        /// Volume of goods
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Collection of factories
        /// </summary>
        public virtual ICollection<Factory> Factories { get; set; }

        /// <summary>
        /// Collection of spaceship cargos
        /// </summary>
        public virtual ICollection<SpaceShipCargo> SpaceShipsCargos { get; set; }

        /// <summary>
        /// Collection of trader cargos
        /// </summary>
        public virtual ICollection<TraderCargo> TraderCargos { get; set; }
    }
}
