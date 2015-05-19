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
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using NLog;

namespace SpaceTraffic.Game.Actions
{
	public class ShipBuy : IPlannableAction
	{
		private Logger logger = LogManager.GetCurrentClassLogger();

		private string result = "Nákup je ve vyřizování.";

		
		public object Result
		{
			get { return new { result = this.result }; }
		}

        public double Duration
        {
            get { return 1; }
        }
        


		void IGameAction.Perform(IGameServer gameServer)
		{
			SpaceShip ship = getSpaceShipFromArgs(gameServer);
			int price = Convert.ToInt32(this.ActionArgs.ElementAt(7));
			if (gameServer.Persistence.GetPlayerDAO().DecrasePlayersCredits(this.PlayerId, price)) {
				gameServer.Persistence.GetSpaceShipDAO().InsertSpaceShip(ship);
				Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerWithIncludes(this.PlayerId);
				// log the ship buy action to statistics
				gameServer.Statistics.SetStatisticItemTo(player, "shipFleet", player.SpaceShips.Count);
				// increase player experiences by a fraction of ship price 
				gameServer.Statistics.IncrementExperiences(player, price / ExperienceLevels.FRACTION_OF_SHIP_PRICE);
				result = String.Format("Loď {0} zakoupena.", ship.SpaceShipName);
			}			
		}

		/// <summary>
		/// Converts arguments given to action to spaceShip object.
		/// </summary>
		/// <param name="gs">GameServer</param>
		/// <returns></returns>
		private SpaceShip getSpaceShipFromArgs(IGameServer gs)
		{
			
			SpaceShip spaceShip = new SpaceShip()
			{
				PlayerId = Convert.ToInt32(this.ActionArgs.ElementAt(0)),
				CurrentStarSystem = this.ActionArgs.ElementAt(1).ToString(),
				IsFlying = false,
				DamagePercent = 0,
				DockedAtBaseId = Convert.ToInt32(this.ActionArgs.ElementAt(2)),
				FuelTank = Convert.ToInt32(this.ActionArgs.ElementAt(3)),
				CurrentFuelTank = Convert.ToInt32(this.ActionArgs.ElementAt(4)),
				SpaceShipModel = this.ActionArgs.ElementAt(5).ToString(),
				SpaceShipName = this.ActionArgs.ElementAt(6).ToString(),
				Consumption = Convert.ToDouble(this.ActionArgs.ElementAt(8)),
				WearRate = Convert.ToDouble(this.ActionArgs.ElementAt(9)),
				MaxSpeed = Convert.ToInt32(this.ActionArgs.ElementAt(10)),
                UserCode = "",
				TimeOfArrival = "",
				CargoSpace = 100    //TODO: je potřeba přidat ještě maximální množství nákladu lodě 
                
			};
			return spaceShip;
		}

		public GameActionState State
		{
			get;
			set;
		}

		public int PlayerId
		{
			get;
			set;
		}

		public int ActionCode
		{
			get;
			set;
		}

		public object[] ActionArgs
		{
			get;
			set;
		}
	}
}
