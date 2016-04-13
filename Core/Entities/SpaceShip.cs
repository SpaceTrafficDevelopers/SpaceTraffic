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

		/// <summary>
		/// True if player can interact with ship, false when ship is flying, refueling etc.
		/// </summary>
		private bool _IsAvailable;
		public bool IsAvailable
		{
			get {
				return _IsAvailable && !IsFlying;
			}
			set
			{
			_IsAvailable = value;
		} }
		/// <summary>
		/// What is ship doing right now
		/// </summary>
		public string StateText { get; set; }

		/// <summary>
		/// Appearance class.
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Used image name.
		/// </summary>
		public string Image { get; set; }

		public static string StateTextDefault { get { return "Připravena."; } }

        public int? DockedAtBaseId { get; set; }

		public virtual Base Base { get; set; }

		public int FuelTank { get; set; }

		public int CurrentFuelTank { get; set; }

		/// <summary>
		/// Maximum space for cargo.
		/// </summary>
		public int CargoSpace { get; set; }

		
		public int CurrentCargoSize{
			get{
				int currCargo = 0;
				if (SpaceShipsCargos == null) {
					return -1;
				}
				foreach(var cargo in SpaceShipsCargos){
					currCargo += cargo.CargoCount * cargo.Cargo.Volume;
				}
				return currCargo;
			}
		}

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
