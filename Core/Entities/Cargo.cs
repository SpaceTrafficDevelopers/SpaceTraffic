using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class Cargo
    {
        public int CargoId { get; set; }

        public int Price { get; set; }

        public string Type { get; set; }

        public virtual ICollection<Factory> Factories { get; set; }

        public virtual ICollection<SpaceShipCargo> SpaceShipsCargos { get; set; }  
    }
}
