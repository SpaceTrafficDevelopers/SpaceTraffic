using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        //
        // GET: /Game/

        
        public ActionResult Index()
        {
            return View();
        }
    }
}
