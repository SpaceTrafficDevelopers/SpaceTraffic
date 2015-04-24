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
            //this.Tabs.AddTab("BuyCargo", "Buy cargo", "Buy new cargo", partialViewName: "_BuyCargo");
            //TODO: create views for actions
        }

        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public ActionResult BuyCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string buyingPlace, int buyerShipId, int traderId)
        {
            /*
                částečně funkční, spadne to při nakládání, resp. neprovede se nakládání sou tam nějak blbě argumenty buď parsovaný nebo předávaný
             
             * dokování zatim nefunguje takže bych ho ani netestovatl HOTOVO 
             * kontrolovat jestli má hráč loď je třeba určitě dřív než místo na lodi :) HOTOVO
             * ten link na cargo buy jak tam teď je tak by měl prolézt SUPER
             * když se volá ta akce tak jako první param nedávat id hráče, tak jak je to tady je dobře
             * hadyho daa nefungujou, hlavně tam kde má podmínku where, je třeba tam dát first případně first or default
             * v SpaceShipHasCargoSpace si porovnával CargoSpace což by měla být maximální nosnost lodi (byla tam špatná podmínka) DÍK
             * v akci Cargo buy hady parsoval argumenty a typoval tam string názvu toho daa rovnou na objekt rozhraní toho daa cože je blbě,
               určitě počítal s tim že mu tou službou pošleš objekt toho rozhraní což je blbost (asi by to šlo poslat, nevim jak ty služby fungujou
               ale rozhodně by se tady neměli vytvářet a pak je tam posílat)
             * ve chvíli kdy to neprojde nějakou kontrolou a volá se to RedirectToAction tak to hodí vyjímku že tu nejsou taby
               ty nevim jak se sem dávaj to můžeš zkusit nastudovat
             
             */




            /*if(!GSClient.GameService.SpaceShipDockedAtBase(buyerShipId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, buyerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Tuto loď hráč nevlastní.", buyerShipId, getCurrentPlayer().PlayerId));
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
				
            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

        public ActionResult LoadCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string buyingPlace, int buyerShipId, int traderId)
        {
            if (!GSClient.GameService.PlayerHasSpaceShip(getCurrentPlayer().PlayerId, buyerShipId))
            {
                return RedirectToAction("").Warning(String.Format("Loď s id={0} nevlastní hráč s id={1}.", buyerShipId, getCurrentPlayer().PlayerId));
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

            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

        public ActionResult UnloadCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string loadingPlace, int buyerId, int spaceShipId)
        {
            /* if (!GSClient.GameService.SpaceShipDockedAtBase(buyerId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
            if(!GSClient.GameService.PlayerHasEnoughCargoOnSpaceShip(spaceShipId, cargoLoadEntityId, count)) {
                return RedirectToAction("").Warning(String.Format("Nemáš tolik zboží, abys ho mohl prodat"));
            }
            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "ShipUnloadCargo", getCurrentPlayer().PlayerId, starSystemName, planetName, spaceShipId, cargoLoadEntityId, count, loadingPlace, buyerId);
			

            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }
        public ActionResult SellCargo(string starSystemName, string planetName, int cargoLoadEntityId, int count, string loadingPlace, int buyerId,  int spaceShipId)
        {
            /* if (!GSClient.GameService.SpaceShipDockedAtBase(buyerId, starSystemName, planetName))
            {
                return RedirectToAction("").Warning(String.Format("Loď není zadokována na stejné planetě jako obchodník"));
            }*/
            GSClient.GameService.PerformAction(getCurrentPlayer().PlayerId, "CargoSell", getCurrentPlayer().PlayerId, starSystemName, planetName, cargoLoadEntityId, count, loadingPlace, buyerId);
				
            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

    }
}