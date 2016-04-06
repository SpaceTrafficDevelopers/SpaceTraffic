using SpaceTraffic.GameUi.Controllers;
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
using SpaceTraffic.GameUi.Extensions;
using SpaceTraffic.Entities;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
	[Authorize]
	public class BasesController : AbstractController
	{
		//
		// GET: /Bases/
				
		public ActionResult Index()
		{
			var partialView = PartialView();
			partialView.ViewBag.bases = GSClient.GameService.GetAllBases();
			return partialView;
		}

		//
		// GET: /Bases/Info
		public PartialViewResult Info(string planetName)
		{
			var partialView = PartialView("_Info");
			partialView = addHeaderViewBag(partialView, planetName);
			return partialView;
		}

		

		//
		// GET: /Bases/Ships
		public PartialViewResult Ships(string planetName)
		{
			var partialView = PartialView("_Ships");
			partialView = addHeaderViewBag(partialView, planetName);
			partialView.ViewBag.ships = GSClient.PlayerService.GetPlayersShipsAtBase(getCurrentPlayerId(), partialView.ViewBag.currentBase.BaseId);
			return partialView;
		}

		//
		// GET: /Bases/ShipDetail
		public ActionResult ShipDetail(int shipId)
		{
			if (!GSClient.PlayerService.PlayerHasSpaceShip(getCurrentPlayerId(), shipId))
			{
				return new EmptyResult().Error("Tato loď ti nepatří!");
			}
			var partialView = PartialView("_ShipDetail");
			SpaceShip ship = GSClient.ShipsService.GetDetailedSpaceShip(shipId);
			partialView.ViewBag.ship = ship;

			return partialView;
		}

		//
		// GET: /Bases/Goods
		public PartialViewResult Goods(string planetName)
		{
			var partialView = PartialView("_Goods");
			partialView = addHeaderViewBag(partialView, planetName);
			return partialView;
		}


		/// <summary>
		/// Creates parameters for header partial view
		/// Full planet name (location) = "StarSystem.Name\Planet.Name"
		/// </summary>
		/// <param name="partialView">Partial view to modify.</param>
		/// <param name="planetName">Name of the planet.</param>
		/// <returns></returns>
		private PartialViewResult addHeaderViewBag(PartialViewResult partialView, string planetName)
		{
			var currentBase = GSClient.GameService.GetBaseByName(getCurrentStarSystem() + "\\" + planetName);
			partialView.ViewBag.currentBase = currentBase;
			partialView.ViewBag.planetName = planetName;
			return partialView;
		}

	}
}
