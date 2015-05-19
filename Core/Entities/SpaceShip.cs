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
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
	public class SpaceShip : ICargoLoad
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

        public int? DockedAtBaseId { get; set; }

		public virtual Base Base { get; set; }

		public int FuelTank { get; set; }

		public int CurrentFuelTank { get; set; }

		/// <summary>
		/// Maximum space for cargo.
		/// </summary>
		public int CargoSpace { get; set; }

		public virtual ICollection<SpaceShipCargo> SpaceShipsCargos { get; set; }

        public int MaxSpeed { get; set; }

		/// <value>
		/// Amount of gas needed for length unit (or time) of travel
		/// </value>
		public double Consumption { get; set; }

		/// <value>
		/// Amount of damage incrase for length unit (or time) of travel
		/// </value>
		public double WearRate { get; set; }



		public string CargoLoadDaoName
		{
			get { return "SpaceShipCargoDao"; }
		}
	}
}
