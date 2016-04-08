/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
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
		[Display(Name = "Výrobce")]
		public string Manufacturer { get; set; }

		[Required]
		[Display(Name = "Spotřeba")]
		public double Consumption { get; set; }

		[Required]
		[Display(Name = "Rychlost opotřebování")]
		public double WearRate { get; set; }

		[Required]
		[Display(Name = "Výkon")]
		public int Power { get; set; }

		[Required]
		[Display(Name = "Kapacita nákladového prostoru")]
		public int CarryingCapacity { get; set; }


		[Required]
		[Display(Name = "Kapacita nádrže")]
		public int FuelCapacity { get; set; }

		[Required]
		[Display(Name = "Cena")]
		public long Price { get; set; }

		//image with its type
		[Display(Name = "")]
		public string Image { get; set; }

		[Display(Name = "Popis")]
		public string Description { get; set; }

        [Required]
        [Display(Name = "Maximální rychlost")]
        public int MaxSpeed { get; set; }
		
		[Display(Name = "")]
		public string CssClass { get; set; }
	}
}