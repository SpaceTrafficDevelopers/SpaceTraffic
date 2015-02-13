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
