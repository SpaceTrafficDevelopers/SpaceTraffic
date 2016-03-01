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
using System.Runtime.Serialization;
using System.Text;

namespace SpaceTraffic.Entities.Minigames
{
    /// <summary>
    /// Codition type enum.
    /// </summary>
    [DataContract]
    public enum ConditionType
    {
        /// <summary>
        /// Nothing condition.
        /// </summary>
        [EnumMember]
        NOTHING,

        /// <summary>
        /// Level condition.
        /// </summary>
        [EnumMember]
        LEVEL,

        /// <summary>
        /// Rank condition.
        /// </summary>
        [EnumMember]
        RANK,

        /// <summary>
        /// Credit condition.
        /// </summary>
        [EnumMember]
        CREDIT,

        /// <summary>
        /// Achievement condition.
        /// </summary>
        [EnumMember]
        ACHIEVEMENT,

        /// <summary>
        /// Planet condition.
        /// </summary>
        [EnumMember]
        PLANET,

        /// <summary>
        /// Star system condition.
        /// </summary>
        [EnumMember]
        STAR_SYTEM,

        /// <summary>
        /// Trader condition.
        /// </summary>
        [EnumMember]
        TRADER,

        /// <summary>
        /// Factory condition.
        /// </summary>
        [EnumMember]
        FACTORY,

        /// <summary>
        /// Cargo condition.
        /// </summary>
        [EnumMember]
        CARGO,

        /// <summary>
        /// Ship condition.
        /// </summary>
        [EnumMember]
        SHIP
    }
}
