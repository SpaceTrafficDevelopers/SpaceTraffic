using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SpaceTraffic.GameUi.Areas.Game.Models
{
    public class ShipModel
    {
        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [Display(Name = "Consumption")]
        public double Consumption { get; set; }

        [Required]
        [Display(Name = "Power")]
        public int Power { get; set; }

        [Required]
        [Display(Name = "Carrying capacity")]
        public int CarryingCapacity { get; set; }

        [Required]
        [Display(Name = "Cargohold space")]
        public int CargoholdSpace { get; set; }

        [Required]
        [Display(Name = "Fuel capacity")]
        public int FuelCapacity { get; set; }

        [Required]
        [Display(Name = "Price")]
        public long Price { get; set; }  
    }
}