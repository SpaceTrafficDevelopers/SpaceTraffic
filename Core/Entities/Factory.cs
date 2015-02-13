using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class Factory
    {
        public int FacotryId { get; set; }

        public string Type { get; set; }        

        public virtual Cargo Cargo { get; set; }

        public int CargoId { get; set; }

        public virtual Base Base { get; set; }

        public int BaseId { get; set; }

        public int CargoCount { get; set; }

       
    }
}
