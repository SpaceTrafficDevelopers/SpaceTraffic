using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Extensions;
using SpaceTraffic.GameUi.Controllers;
using SpaceTraffic.Entities;
using SpaceTraffic.GameUi.Areas.Game.Models;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    /// <summary>
    /// Cargo Controller
    /// 
    /// </summary>
    [Authorize]
    public class CargoController : AbstractController
    {
       /* protected override void BuildTabs()
        {
            //this.Tabs.AddTab("BuyCargo", "Buy cargo", "Buy new cargo", partialViewName: "_BuyCargo");
            //TODO: create views for actions
        }*/


        public ActionResult Index()
        {
			return PartialView();
        }
        public bool BuyCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string buyingPlace, int buyerShipId, int traderId)
        {
            /*if(!GSClient.GameService.SpaceShipDockedAtBase(buyerShipId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
			if (!GSClient.PlayerService.PlayerHasSpaceShip(getCurrentPlayerId(), buyerShipId))
            {
                this.ErrorMessage = "Hráč nemá zakoupenou tuto loď";
				return false;
            }
			if (!GSClient.CargoService.PlayerHasEnaughCreditsForCargo(getCurrentPlayerId(), cargoLoadEntityId, count))
            {
				this.ErrorMessage = "Nemáš dostatek kreditů na koupi zboží.";
				return false;
            }
            if(!GSClient.CargoService.SpaceShipHasCargoSpace(buyerShipId, cargoLoadEntityId, count)) 
            {
				this.ErrorMessage = "Nemáš dostatek místa na lodi.";
				return false;
            }
			if (!GSClient.ShipsService.SpaceShipDockedAtBase(buyerShipId, starSystemName, planetName))
			{
				this.ErrorMessage = "Loď není zadokována na stejné planetě jako obchodník";
				return false;
			}
            if (!GSClient.CargoService.TraderHasEnoughCargo(traderId, cargoLoadEntityId, count))
            {
				this.ErrorMessage = "Obchodník nemá tolik zboží na nákup.";
				return false;
            }
			GSClient.GameService.PerformAction(getCurrentPlayerId(), "CargoBuy", starSystemName, planetName, cargoLoadEntityId, count, buyingPlace, buyerShipId);
			//GSClient.GameService.PerformAction(getCurrentPlayerId(), "ShipLoadCargo", starSystemName, planetName, buyerShipId, cargoLoadEntityId, count, buyingPlace);
            return true;
            //Response.Redirect(Request.Headers.Get("Referer"));
        }


        public ActionResult UnloadCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string loadingPlace, int buyerId, int spaceShipId)
        {
            /* if (!GSClient.GameService.SpaceShipDockedAtBase(buyerId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
			if (!GSClient.PlayerService.PlayerHasSpaceShip(getCurrentPlayerId(), spaceShipId))
            {
                return RedirectToAction("").Warning(String.Format("Hráč nemá zakoupenou tuto loď"));
            }
            if(!GSClient.CargoService.PlayerHasEnoughCargoOnSpaceShip(spaceShipId, cargoLoadEntityId, count)) {
                return RedirectToAction("").Warning(String.Format("Nemáš tolik zboží, abys ho mohl prodat"));
            }
			GSClient.GameService.PerformAction(getCurrentPlayerId(), "ShipUnloadCargo", starSystemName, planetName, spaceShipId, cargoLoadEntityId, count, loadingPlace, buyerId);
            return RedirectToAction("").Success(String.Format("Výklad proběhl v pořádku"));
            //Response.Redirect(Request.Headers.Get("Referer"));
        }
        public ActionResult SellCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string loadingPlace, int buyerId, int sellerShipId)
        {
            /* if (!GSClient.GameService.SpaceShipDockedAtBase(buyerId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
			if (!GSClient.PlayerService.PlayerHasSpaceShip(getCurrentPlayerId(), sellerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Hráč nemá zakoupenou tuto loď"));
            }
            if (!GSClient.CargoService.PlayerHasEnoughCargoOnSpaceShip(sellerShipId, cargoLoadEntityId, count))
            {
                return RedirectToAction("").Warning(String.Format("Nemáš tolik zboží, abys ho mohl prodat"));
            }

			GSClient.GameService.PerformAction(getCurrentPlayerId(), "CargoSell", starSystemName, planetName, cargoLoadEntityId, count, loadingPlace, buyerId, sellerShipId);
            //Response.Redirect(Request.Headers.Get("Referer"));
            return RedirectToAction("").Success(String.Format("Prodej proběhl v pořádku"));
            
            //return null;
        }

		//
		// GET: /Cargo/Buy
		public ActionResult Buy(int shipId, int baseId)
		{
			int curPlayerId = getCurrentPlayerId();

			var partialView = PartialView("_CargoBuy");
			int credits = GSClient.PlayerService.GetPlayersCredits(curPlayerId);

			SpaceShip ship = GSClient.ShipsService.GetSpaceShip(shipId);
			partialView.ViewBag.ship = ship;
			if (!controlShipAccess(ship))
			{
				return new EmptyResult().Error(this.ErrorMessage);
			}
			
			Trader trader = GSClient.CargoService.GetTraderAtBaseWithCargo(baseId);
			partialView.ViewBag.trader = trader;
			partialView.ViewBag.credits = credits;
			return partialView;
		}

		[HttpPost]
		public ActionResult Buy(CargoBuyModel values, int traderId, int shipId, int baseId, int cargoId)
		{
			Trader trader = GSClient.CargoService.GetTraderAtBase(baseId);
			if (values.Amount > 0)
			{
				string[] planetString = trader.Base.Planet.Split('\\');
				if (this.BuyCargo(planetString[0], planetString[1], cargoId, values.Amount, "TraderCargoDAO", shipId, traderId))
				{
					return new EmptyResult().Success("Nakoupeno " + values.Amount + " jednotek zboží.");
				}
				else
				{
					return new EmptyResult().Error(this.ErrorMessage);
				}
			}else{
				return new EmptyResult().Warning("Množství musí být větší než 0.");
			}
			
		}


        /*public ActionResult Planner()
        {
            GSClient.GameService.TestPlanner();
            return RedirectToAction("").Success("Možná se to naplánovalo :D");
        }*/
    }
}