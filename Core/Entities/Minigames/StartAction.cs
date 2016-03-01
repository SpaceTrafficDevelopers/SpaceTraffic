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
    /// Minigame start action. Actions which starts minigame.
    /// </summary>
    [DataContract]
    public class StartAction
    {
        /// <summary>
        /// Start action ID.
        /// </summary>
        [DataMember]
        public int StartActionID { get; set; }

        /// <summary>
        /// Action name.
        /// </summary>
        [DataMember]
        public string ActionName { get; set; }

        /// <summary>
        /// Minigames for start action.
        /// </summary>
        public virtual ICollection<MinigameDescriptor> Minigames { get; set; }
    }
}
