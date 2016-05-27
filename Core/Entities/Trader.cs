﻿/**
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
    public class Trader : ICargoLoad
    {
        public int TraderId { get; set; }

        public int BaseId { get; set; }

        public virtual Base Base { get; set; }

		public int FuelPrice { get; set; }

		public int RepairPrice { get; set; }

        /// <summary>
        /// Economic level
        /// </summary>
        public int EconomicLevel { get; set; }

        /// <summary>
        /// Purchase tax as percentage (10% => 10)
        /// </summary>
        public int PurchaseTax { get; set; }

        /// <summary>
        /// Sales tax as percentage (10% => 10);
        /// </summary>
        public int SalesTax { get; set; } 

        public virtual ICollection<TraderCargo> TraderCargos { get; set; }

        public string CargoLoadDaoName
        {
            get { return "TraderCargoDao"; }
        }
    }
}
