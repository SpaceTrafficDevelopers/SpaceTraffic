using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult BuyCargo(int cargoId)
        {
            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

        public ActionResult LoadCargo(int cargoId, int objectId /*interface ILoadable*/)
        {

            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

        public ActionResult UnloadCargo(int cargoId, int objectId /*interface ILoadable*/)
        {

            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }
        public ActionResult SellCargo(int cargoId)
        {
            Response.Redirect(Request.Headers.Get("Referer"));
            return null;
        }

    }
}