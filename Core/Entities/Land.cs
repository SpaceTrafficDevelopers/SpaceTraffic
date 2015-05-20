using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "Land")]
    public class Land
    {
        [DataMember]
        public int BaseId { get; set; }

        [DataMember]
        public int LandId { get; set; }

        [DataMember]
        public virtual ICollection<Tile> Tiles { get; set; }

        [DataMember]
        public virtual ICollection<SpaceTraffic.Game.Building> Buildings { get; set; }
    }
}
