using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class BasesController : Controller
    {
        //
        // GET: /Bases/
                
        public ActionResult Index()
        {
            return View();
        }

    }
}
