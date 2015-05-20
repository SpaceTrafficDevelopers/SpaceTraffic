using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "Building")]
    public class Building
    {
        [DataMember]
        public int BuildingId { get; set; }

        [DataMember]
        public int LandId { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int[] Location { get; set; }

        [DataMember]
        public int[] Size { get; set; }

    }
}
