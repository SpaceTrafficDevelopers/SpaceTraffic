using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class Base
    {
        public int BaseId { get; set; }

        public string Planet { get; set; }

        public virtual ICollection<SpaceShip> SpaceShips { get; set; }

        public virtual ICollection<Factory> Factories { get; set; }  
    }
}
