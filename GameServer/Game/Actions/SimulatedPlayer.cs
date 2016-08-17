
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
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Dao;
using System.Runtime.Serialization;
using SpaceTraffic.Game.Utils;
using System.IO;

namespace SpaceTraffic.Game.Actions
{

	public class MyCargo {
		public int purchasePrice{get;set;}
		public int amount { get; set; }
		public int cargoID { get; set; }
	}
	/// <summary>
	/// Action for buying cargo.
	/// </summary>
	[Serializable]
	public class SimulatedPlayer : IGameAction
	{
		/// <summary>
		/// Result of action.
		/// </summary>
		public object Result { get; set; }


		public GameActionState State { get; set; }

		/// <summary>
		/// Player identification number who wats this action.
		/// </summary>
		public int PlayerId { get; set; }

		/// <summary>
		/// Return action code.
		/// Action code is number which positively identificates action in player list of actions.
		/// </summary>
		public int ActionCode { get; set; }

		/// <summary>
		/// arguments connected with concreate action
		/// </summary>
		public object[] ActionArgs { get; set; }


		List<MyCargo> myCargoList = new List<MyCargo>();/* idCargo, MyCargo*/

		public StreamWriter outputFile;

		public SimulatedPlayer(int index) {
			outputFile = new StreamWriter(@"npcHráč-" + index + ".csv", false, Encoding.UTF8);
			outputFile.WriteLine("Čas;Činnost;Planeta;Množství;Zboží;Cena;");
		}


		public void Perform(IGameServer gameServer)
		{
			State = GameActionState.PLANNED;
			getArgumentsFromActionArgs(gameServer);

			List<Trader> traders = gameServer.Persistence.GetTraderDAO().GetTradersWithCargo();


			/* buying random cargo */
			Random rand = new Random();
			int randCount = rand.Next(6);

			for (int i = 0; i < randCount; i++)
			{

				int randInt = rand.Next(traders.Count);

				Trader pickedTrader = traders[randInt];
				int randInt2 = rand.Next(pickedTrader.TraderCargos.Count);
				TraderCargo pickedCargo = pickedTrader.TraderCargos.ElementAt(randInt2);
				int randAmount = rand.Next(pickedCargo.CargoCount / 2);/* max half of availible amount */
				myCargoList.Add(new MyCargo() { purchasePrice = pickedCargo.CargoBuyPrice, amount = randAmount, cargoID = pickedCargo.CargoId });

				pickedCargo.CargoCount -= randAmount;
				gameServer.Persistence.GetTraderCargoDAO().UpdateCargo(pickedCargo);
				outputFile.WriteLine(gameServer.Game.currentGameTime.Value + ";" + "nákup;" + pickedTrader.Base.BaseName + ";" + randAmount + ";" + pickedCargo.Cargo.Name + ";" + pickedCargo.CargoBuyPrice + ";");
			}


			/* selling random cargo */
			if (myCargoList.Count > 0)
			{
				int randSellCount = rand.Next(myCargoList.Count);
				int randStart = rand.Next(myCargoList.Count);

				for (int i = 0; i < randSellCount; i++)
				{
					int index = (i + randStart) % myCargoList.Count;

					MyCargo selectedCargo = myCargoList.ElementAt(index);

					foreach (Trader trader in traders)
					{
						TraderCargo toSell = trader.TraderCargos.FirstOrDefault(x => x.CargoId == selectedCargo.cargoID);
						if(toSell != null && toSell.CargoSellPrice > selectedCargo.purchasePrice){
							toSell.CargoCount += selectedCargo.amount;
							gameServer.Persistence.GetTraderCargoDAO().UpdateCargo(toSell);
							myCargoList.RemoveAt(index);
							outputFile.WriteLine(gameServer.Game.currentGameTime.Value + ";" + "prodej;" + trader.Base.BaseName + ";" + selectedCargo.amount + ";" + toSell.Cargo.Name + ";" + toSell.CargoSellPrice + ";");
							break;
						}
					}
				}
			}
			outputFile.Flush();

			gameServer.Game.PlanEvent(this, gameServer.Game.currentGameTime.Value.AddMinutes(rand.Next(120) + 1));

		}

		/// <summary>
		/// Get all arguments to properties from action args.
		/// </summary>
		/// <param name="gameServer">Instance of game server</param>
		private void getArgumentsFromActionArgs(IGameServer gameServer)
		{
			
		}

	}


}
