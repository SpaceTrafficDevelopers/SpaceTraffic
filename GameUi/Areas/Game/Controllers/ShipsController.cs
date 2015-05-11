﻿/**
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

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
	[Authorize]
	public class ShipsController : TabsControllerBase
	{
		protected override void BuildTabs()
		{
			//tabs = new Tabs(5);
			this.Tabs.AddTab("Overview", title:"Overview of all active ships and fleets", partialViewName:"_Overview");
			this.Tabs.AddTab("ShipList", "Ships", "List of all your ships.", partialViewName: "_ShipList");
			//this.Tabs.AddTab("FleetList", "Fleets", "List of all your fleets.", "_FleetList");
			this.Tabs.AddTab("BuyShip", "Buy ship", "Buy new ship", partialViewName: "_BuyShip");
			this.Tabs.AddTab("NaviComp", title: "Navicomp view", partialViewName: "_NaviComp");
			//TODO: Build tabs from method attributes.
		}

		//
		// GET: /Ships/
				
		public ActionResult Index()
		{
			return View(INDEX_VIEW);
		}

		
		//
		// GET: /Ships/BuyModel		
		/// <summary>
		/// Buys the ship of specified model
		/// </summary>
		/// <returns></returns>
		public ActionResult BuyModel(string model)
		{
			ShipModel shipModel = getAllShips().Where(shipType => shipType.Model == model).First();
			if (shipModel != null){			
				if (GSClient.GameService.PlayerHasEnaughCredits(getCurrentPlayer().PlayerId, shipModel.Price)){
					GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "ShipBuy", getCurrentPlayer().PlayerId, "Proxima Centauri", 2, shipModel.FuelCapacity, shipModel.FuelCapacity, shipModel.Model, shipModel.Model, shipModel.Price);
				} else {
					return RedirectToAction("").Warning(String.Format("Nemáš dostatek kreditů na koupi lodě {0}.", shipModel.Model));
				}
			}else{
				return RedirectToAction("").Error("Tento typ lodi neexistuje.");
			}
			return RedirectToAction("").Success(String.Format("Loď typu {0} byla úspěšně zakoupena.", shipModel.Model));
		}

		public PartialViewResult Overview()
		{
			return GetTabView("Overview");
		}

		public PartialViewResult ShipList()
		{
			// získání lodí uživatele
			IList<SpaceShip> ships = GSClient.GameService.GetPlayersShips(getCurrentPlayer().PlayerId);

			var tabView = GetTabView("ShipList");
			tabView.ViewBag.Ships = ships;
			return tabView;
		}
		
		public PartialViewResult FleetList()
		{
			return GetTabView("FleetList");
		}

		public PartialViewResult BuyShip()
		{
			// získání všech dostupných modelů lodí
			List<ShipModel> ships = getAllShips();

			var tabView = GetTabView("BuyShip");
			tabView.ViewBag.Ships = ships;
			return tabView;
		}


		public PartialViewResult NaviComp()
		{
			return GetTabView("NaviComp");
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
				ship.FuelCapacity = (int)shipNode.Descendants("fuelCapacity").FirstOrDefault();
				ship.CarryingCapacity = (int)shipNode.Descendants("carryingCapacity").FirstOrDefault();
				ship.Image = (string)shipNode.Descendants("image").FirstOrDefault();
				ship.Price = (int)shipNode.Descendants("price").FirstOrDefault();
				ship.Description = (string)shipNode.Descendants("description").FirstOrDefault();

				ships.Add(ship);
			}
			return ships;
		}

	}
}
