﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Extensions;
using SpaceTraffic.GameUi.Controllers;

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
            return View();
        }
        public ActionResult BuyCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string buyingPlace, int buyerShipId, int traderId)
        {
            /*if(!GSClient.GameService.SpaceShipDockedAtBase(buyerShipId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, buyerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Hráč nemá zakoupenou tuto loď"));
            }
            if(!GSClient.GameService.PlayerHasEnaughCreditsForCargo(getCurrentPlayer().PlayerId, cargoLoadEntityId, count))
            {
                return RedirectToAction("").Warning(String.Format("Nemáš dostatek kreditů na koupi zboží."));
            }
            if(!GSClient.GameService.SpaceShipHasCargoSpace(buyerShipId, cargoLoadEntityId, count)) 
            {
                return RedirectToAction("").Warning(String.Format("Nemáš dostatek místa na lodi."));
            }
            if (!GSClient.GameService.TraderHasEnoughCargo(traderId, cargoLoadEntityId, count))
            {
                return RedirectToAction("").Warning(String.Format("Obchodník nemá tolik zboží na nákup."));
            }
            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "CargoBuy", starSystemName, planetName, cargoLoadEntityId, count, buyingPlace, buyerShipId);
            return RedirectToAction("").Success(String.Format("Nákup proběhl v pořádku"));
            //Response.Redirect(Request.Headers.Get("Referer"));
        }

        public ActionResult LoadCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string buyingPlace, int buyerShipId, int traderId)
        {
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, buyerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Hráč nemá zakoupenou tuto loď"));
            }
            if (!GSClient.GameService.SpaceShipDockedAtBase(buyerShipId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }
            if (!GSClient.GameService.SpaceShipHasCargoSpace(buyerShipId, cargoLoadEntityId, count))
            {
                return RedirectToAction("").Warning(String.Format("Nemáš dostatek místa na lodi."));
            }

            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "ShipLoadCargo", starSystemName, planetName, buyerShipId, cargoLoadEntityId);
            return RedirectToAction("").Success(String.Format("Náklad proběhl v pořádku"));
            //Response.Redirect(Request.Headers.Get("Referer"));
        }

        public ActionResult UnloadCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string loadingPlace, int buyerId, int spaceShipId)
        {
            /* if (!GSClient.GameService.SpaceShipDockedAtBase(buyerId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, spaceShipId))
            {
                return RedirectToAction("").Warning(String.Format("Hráč nemá zakoupenou tuto loď"));
            }
            if(!GSClient.GameService.PlayerHasEnoughCargoOnSpaceShip(spaceShipId, cargoLoadEntityId, count)) {
                return RedirectToAction("").Warning(String.Format("Nemáš tolik zboží, abys ho mohl prodat"));
            }
            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "ShipUnloadCargo", starSystemName, planetName, spaceShipId, cargoLoadEntityId, count, loadingPlace, buyerId);
            return RedirectToAction("").Success(String.Format("Výklad proběhl v pořádku"));
            //Response.Redirect(Request.Headers.Get("Referer"));
        }
        public ActionResult SellCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string loadingPlace, int buyerId, int sellerShipId)
        {
            /* if (!GSClient.GameService.SpaceShipDockedAtBase(buyerId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, sellerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Hráč nemá zakoupenou tuto loď"));
            }
            if (!GSClient.GameService.PlayerHasEnoughCargoOnSpaceShip(sellerShipId, cargoLoadEntityId, count))
            {
                return RedirectToAction("").Warning(String.Format("Nemáš tolik zboží, abys ho mohl prodat"));
            }
            
            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "CargoSell", starSystemName, planetName, cargoLoadEntityId, count, loadingPlace, buyerId, sellerShipId);
            //Response.Redirect(Request.Headers.Get("Referer"));
            return RedirectToAction("").Success(String.Format("Prodej proběhl v pořádku"));
            
            //return null;
        }


        /*public ActionResult Planner()
        {
            GSClient.GameService.TestPlanner();
            return RedirectToAction("").Success("Možná se to naplánovalo :D");
        }*/
    }
}