using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "Tile")]
    public class Tile
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public bool purchased { get; set; }

        [DataMember]
        public bool occupied { get; set; }

    }
}
