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
    /// Economic level class.
    /// </summary>
    public class EconomicLevel
    {
        /// <summary>
        /// Level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Upgrade level percentage (80 => 80%).
        /// </summary>
        public int UpgradeLevelPercentage { get; set; }

        /// <summary>
        /// Downgrade level percentage (80 => 80%).
        /// </summary>
        public int DowngradeLevelPercentage { get; set; }

        /// <summary>
        /// List of level items.
        /// </summary>
        public IList<EconomicLevelItem> LevelItems { get; set; }
        
    }

    /// <summary>
    /// Economic level item class.
    /// </summary>
    public class EconomicLevelItem
    {
        /// <summary>
        /// Sequence number.
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Production (static value or percetage 1.5 => 150%).
        /// </summary>
        public double Production { get; set; }

        /// <summary>
        /// Consumption (static value or percetage 1.5 => 150%).
        /// </summary>
        public double Consumption { get; set; }

        /// <summary>
        /// Indication if item was discovered at this level.
        /// If the attribute is false, production and consuption are as percent values
        /// otherwise as static values.
        /// </summary>
        public bool IsDiscovered { get; set; }
    }
}
