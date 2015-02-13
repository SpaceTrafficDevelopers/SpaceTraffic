using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class SpaceShip
    {
        public int SpaceShipId { get; set; }

        public string SpaceShipName { get; set; }

        public string SpaceShipModel { get; set; }

        public float DamagePercent { get; set; }

        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public string UserCode { get; set; }

        public string TimeOfArrival { get; set; }

        public string CurrentStarSystem { get; set; }

        public bool IsFlying { get; set; }

        public int DockedAtBaseId { get; set; }

        public virtual  Base Base { get; set; }        

        public int FuelTank { get; set; }

        public int CurrentFuelTank { get; set; }

        public virtual ICollection<SpaceShipCargo> SpaceShipsCargos { get; set; }  
  



    }
}
