using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class SpaceShipCargo
    {
        public int SpaceShipId { get; set; }

        public virtual SpaceShip SpaceShip { get; set; } 

        public int CargoId { get; set; }

        public virtual Cargo Cargo { get; set; }

        public int CargoCount { get; set; }

       

          
    }
}
