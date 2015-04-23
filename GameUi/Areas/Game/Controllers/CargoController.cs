using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.GameUi.Extensions;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    /// <summary>
    /// Cargo Controller
    /// 
    /// </summary>
    [Authorize]
    public class CargoController : TabsControllerBase
    {
        protected override void BuildTabs()
        {
            //TODO: create views for actions
        }

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public ActionResult BuyCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string buyingPlace, int buyerShipId, int traderId)
        {
            if(!GSClient.GameService.SpaceShipDockedAtBase(buyerShipId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }
            if(!GSClient.GameService.PlayerHasEnaughCreditsForCargo(getCurrentPlayer().PlayerId, cargoLoadEntityId, count, traderId))
            {
                return RedirectToAction("").Warning(String.Format("Nemáš dostatek kreditů na koupi zboží."));
            }
            if(!GSClient.GameService.SpaceShipHasCargoSpace(buyerShipId, buyingPlace, cargoLoadEntityId)) 
            {
                return RedirectToAction("").Warning(String.Format("Nemáš dostatek místa na lodi."));
            }
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, buyerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Loď s id={0} nevlastní hráč s id={1}.", buyerShipId, getCurrentPlayer().PlayerId));
            }
            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "CargoBuy", getCurrentPlayer().PlayerId, starSystemName, planetName, cargoLoadEntityId, count, buyingPlace, buyerShipId);
				
            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

        public ActionResult LoadCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string buyingPlace, int buyerShipId, int traderId)
        {
            if (!GSClient.GameService.SpaceShipDockedAtBase(buyerShipId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }
            if (!GSClient.GameService.SpaceShipHasCargoSpace(buyerShipId, buyingPlace, cargoLoadEntityId))
            {
                return RedirectToAction("").Warning(String.Format("Nemáš dostatek místa na lodi."));
            }
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, buyerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Loď s id={0} nevlastní hráč s id={1}.", buyerShipId, getCurrentPlayer().PlayerId));
            }
            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

        public ActionResult UnloadCargo(int cargoId, int objectId /*interface ILoadable*/)
        {

            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }
        public ActionResult SellCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string loadingPlace, int buyerId, int traderId)
        {
            if (!GSClient.GameService.SpaceShipDockedAtBase(buyerId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }
            if (!GSClient.GameService.TraderHasEnoughCargo(traderId, cargoLoadEntityId, count))
            {
                return RedirectToAction("").Warning(String.Format("Hráč nemá tolik zboží na prodání"));
            }
            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "CargoBuy", getCurrentPlayer().PlayerId, starSystemName, planetName, cargoLoadEntityId, count, loadingPlace, buyerId);
				
            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

    }
}