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
using System.Data.Entity;

namespace SpaceTraffic.Entities
{
    public class TraderCargo : ICargoLoadEntity
    {
        public int TraderCargoId { get; set; }

        public int TraderId { get; set; }

        public virtual Trader Trader { get; set; }

        public int CargoId { get; set; }

        public virtual Cargo Cargo { get; set; }

        public int CargoCount { get; set; }

        /// <summary>
        /// Cargo buy price with purchase tax
        /// </summary>
        public int CargoBuyPrice { get; set; }

        /// <summary>
        /// Cargo sell price with sales tax
        /// </summary>
        public int CargoSellPrice { get; set; }

        /// <summary>
        /// Maximum production for day
        /// </summary>
        public int DailyProduction { get; set; }

        /// <summary>
        /// Maximum consumption for day
        /// </summary>
        public int DailyConsumption { get; set; }

        /// <summary>
        /// Today produced
        /// </summary>
        public int TodayProduced { get; set; }

        /// <summary>
        /// Today consumed
        /// </summary>
        public int TodayConsumed { get; set; }

        /// <summary>
        /// Sequence number for economic level
        /// </summary>
        public int SequenceNumber { get; set; }

        public int CargoOwnerId
        {
            get
            {
                return TraderId;
            }
            set
            {
                TraderId = value;
            }
        }

        public int CargoLoadEntityId
        {
            get
            {
                return TraderCargoId;
            }
            set
            {
                TraderCargoId = value;
            }
        }
    }
}
