using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities.PublicEntities
{
    [DataContract(Name = "PlayerInfo")]
    public class PlayerInfo
    {
        // http://www.diranieh.com/NET_WCF/Serialization.htm

        [DataMember]
        public string PlayerId { get; set; }

        [DataMember]
        public string PlayerName { get; set; }

    }
}
