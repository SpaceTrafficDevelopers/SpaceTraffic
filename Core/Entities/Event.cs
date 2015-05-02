using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "Event")]
    public class Event
    {
        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public DateTime CreationedTime { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public int SpaceShipId { get; set; }

        public virtual SpaceShip SpaceShip { get; set; }
    }
}
