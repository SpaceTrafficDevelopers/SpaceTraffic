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
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities.PublicEntities
{

    /// <summary>
    /// Data contract for sending connection informations of star system.
    /// </summary>
    [DataContract(Name = "WormholeEndpointDestination")]
    public class WormholeEndpointDestination
    {
        /// <summary>
        /// Gets or sets the star system's wormhole endpoint id.
        /// </summary>
        /// <value>
        /// The endpoint id.
        /// </value>
        [DataMember]
        public int EndpointId { get; set; }

        /// <summary>
        /// Gets or sets the name of the destination star system.
        /// </summary>
        /// <value>
        /// The name of the destination star system.
        /// </value>
        [DataMember]
        public string DestinationStarSystemName { get; set; }
    }
}
