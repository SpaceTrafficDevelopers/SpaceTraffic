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
using SpaceTraffic.GameUi.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mime;

namespace SpaceTraffic.GameUi.Areas.Minigame.Controllers
{
    /// <summary>
    /// Default controller for Minigame.
    /// </summary>
    public class DefaultController : AbstractController
    {
        //
        // GET: /Minigame/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Method for download external game as apk.
        /// </summary>
        /// <param name="file">file name</param>
        /// <returns>file or 404</returns>
        public ActionResult DownloadGame(string file){

            string filePath = Server.MapPath("~/Content/minigames/" + file);

            if (!System.IO.File.Exists(filePath))
                return HttpNotFound();

            byte[] filedata = System.IO.File.ReadAllBytes(filePath);
            string contentType = "application/vnd.android.package-archive";

            ContentDisposition cd = new ContentDisposition
            {
                FileName = file,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
    }
}