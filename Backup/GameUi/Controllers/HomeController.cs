using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.Services.Contracts;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Diagnostics;

namespace SpaceTraffic.GameUi.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome";

            return View();
        }

        public ActionResult RunTests()
        {
            ViewBag.Message = "Welcome";

            return View();
        }

    }
}
