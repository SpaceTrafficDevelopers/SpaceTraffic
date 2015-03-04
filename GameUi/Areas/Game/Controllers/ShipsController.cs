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

        public PartialViewResult Overview()
        {
            return GetTabView("Overview");
        }

        public PartialViewResult ShipList()
        {
            // získání lodí uživatele
            List<ShipModel> ships = getShips();
            
            return GetTabView("ShipList");
        }
        
        public PartialViewResult FleetList()
        {
            return GetTabView("FleetList");
        }

        public PartialViewResult BuyShip()
        {
            // získání všech dostupných modelů lodí
            List<ShipModel> ships = getShips();
            
            var tabView = GetTabView("BuyShip");
            tabView.ViewBag.Ships = ships;
            return tabView;
        }

        public PartialViewResult NaviComp()
        {
            return GetTabView("NaviComp");
        }

        // dočasná pomocná metoda pro similaci získání lodí daného uživatele
        // nebo všech lodí z katalogu
        private List<ShipModel> getShips()
        {
            List<ShipModel> ships = new List<ShipModel>();
            ShipModel ship;

            for (int i = 1; i < 5; i++)
            {
                ship = new ShipModel();

                ship.CargoholdSpace = 15 + (5 * i);
                ship.CarryingCapacity = 500 + (100 * i);
                ship.Consumption = 0.8 - (0.1 * i);
                ship.FuelCapacity = 25 + (15 * i);
                ship.Manufacturer = "United Space Ships";
                ship.Model = "Shipunto LR-3";
                ship.Power = 200 + (50 * i);
                ship.Price = 1000000 + (800000 * i);

                ships.Add(ship);
            }
            
            return ships;
        }
    }
}
