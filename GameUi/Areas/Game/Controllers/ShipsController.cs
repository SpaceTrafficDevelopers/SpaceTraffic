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
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Models.Ui;
using SpaceTraffic.GameUi.Areas.Game.Models;
using System.Xml.Linq;
using SpaceTraffic.Entities;
using SpaceTraffic.GameUi.Extensions;
using SpaceTraffic.GameUi.Controllers;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
	[Authorize]
	public class ShipsController : AbstractController
	{

		public PartialViewResult Index() {
			return ShipList();
		}
		
		//
		// GET: /Ships/BuyModel		
		/// <summary>
		/// Buys the ship of specified model
		/// </summary>
		/// <returns></returns>
		public ActionResult BuyModel(int baseId, string starSystemName, string model)
		{
			ActionResult result = new EmptyResult();
			ShipModel shipModel = getAllShips().Where(shipType => shipType.Model == model).First();
			if (shipModel != null){
				if (GSClient.PlayerService.PlayerHasEnaughCredits(getCurrentPlayerId(), shipModel.Price))
				{
					GSClient.GameService.PerformAction(getCurrentPlayerId(), "ShipBuy", getCurrentPlayerId(), starSystemName, baseId, shipModel.FuelCapacity, shipModel.FuelCapacity, shipModel.Model, shipModel.Model, shipModel.Price, shipModel.Consumption, shipModel.WearRate, shipModel.MaxSpeed);
				} else {
					return result.Warning(String.Format("Nemáš dostatek kreditů na koupi lodě {0}.", shipModel.Model));
				}
			}else{
				return result.Error("Tento typ lodi neexistuje.");
			}
			return result.Success(String.Format("Loď typu {0} byla úspěšně zakoupena.", shipModel.Model));
		}


		public PartialViewResult ShipList()
		{
			// získání lodí uživatele
			IList<SpaceShip> ships = GSClient.PlayerService.GetPlayersShips(getCurrentPlayerId());

			var viewResult = PartialView("_ShipList");
			viewResult.ViewBag.Ships = ships;
			return viewResult;
		}
		
		

		public PartialViewResult BuyShip(int baseId)
		{
			// získání všech dostupných modelů lodí
			List<ShipModel> ships = getAllShips();

			var viewResult = PartialView("_BuyShip");
			viewResult.ViewBag.Ships = ships;
			viewResult.ViewBag.baseId = baseId;
			viewResult.ViewBag.starSystemName = getCurrentStarSystem();
			return viewResult;
		}



		/// <summary>
		/// Returns the list of all ships defined in SpaceShips.xml
		/// </summary>
		/// <returns>List of ship models with parameters from xml.</returns>
		private List<ShipModel> getAllShips()
		{
			List<ShipModel> ships = new List<ShipModel>();

			XDocument shipsXML = this.loadXmlAssembly("Ship", "SpaceShips");//getting XML

			var shipsQuery = from shipsNode in shipsXML.Descendants("ships").Descendants("ship")
							 select shipsNode;// getting ship node in ships XML

			foreach (XElement shipNode in shipsQuery) {
				ShipModel ship = new ShipModel();

				ship.Model = (string)shipNode.Descendants("model").FirstOrDefault();
				ship.Manufacturer = (string)shipNode.Descendants("manufacturer").FirstOrDefault();
				ship.Power = (int)shipNode.Descendants("power").FirstOrDefault();
				ship.Consumption = (double)shipNode.Descendants("consumption").FirstOrDefault();
				ship.WearRate = (double)shipNode.Descendants("wearRate").FirstOrDefault();
				ship.FuelCapacity = (int)shipNode.Descendants("fuelCapacity").FirstOrDefault();
				ship.CarryingCapacity = (int)shipNode.Descendants("carryingCapacity").FirstOrDefault();
				ship.Image = (string)shipNode.Descendants("image").FirstOrDefault();
				ship.Price = (int)shipNode.Descendants("price").FirstOrDefault();
				ship.Description = (string)shipNode.Descendants("description").FirstOrDefault();
                ship.MaxSpeed = (int)shipNode.Descendants("maxSpeed").FirstOrDefault();

				ships.Add(ship);
			}
			return ships;
		}

	}
}
