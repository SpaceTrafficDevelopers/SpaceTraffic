using SpaceTraffic.GameUi.Controllers;
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
using SpaceTraffic.Game;
using SpaceTraffic.GameUi.GameServerClient;
using SpaceTraffic.Utils.Debugging;
using System.IO;
using SpaceTraffic.GameUi.Configuration;
using SpaceTraffic.Entities.PublicEntities;

namespace SpaceTraffic.GameUi.Areas.Game.Controllers
{
    [Authorize]
    public class MapController : TabsControllerBase
    {
        //
        // GET: /Map/
        private readonly IGameServerClient GSClient = GameServerClientFactory.GetClientInstance();
        /// <summary>
        /// Získání xml galaxymapy na základě jeho jména. K získání cesty využívá klíč assetPath 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpGet]
        public FilePathResult Get(string id)
        {
            DebugEx.Entry(id);
            // cesta k Assets/Map + jmeno galaxymapy
            string filename = Path.Combine(GameUiConfiguration.MapPath, id + ".xml");
            DebugEx.WriteLineF("GalaxyMap filename: {0}", Path.GetFullPath(filename));

            FilePathResult filePathResult = File(filename, "text/xml", id + ".xml");
            DebugEx.Exit(filePathResult);
            return filePathResult;
        }

        [HttpGet]
        public JsonResult GetGalaxyMap(string galaxyMap)
        {
            DebugEx.Entry("GalaxyMap");
            galaxyMap = "GalaxyMap";
            IList<StarSystem> data = GSClient.GameService.GetGalaxyMap(galaxyMap);
            
            JsonResult result = Json(data, JsonRequestBehavior.AllowGet);

            DebugEx.Exit(result);
            return result;
        }


        public ActionResult Index()
        {
            return View(INDEX_VIEW);
        }

        public PartialViewResult GalaxyMap()
        {
            return this.GetTabView("GalaxyMap");
        }

        protected override void BuildTabs()
        {
            this.Tabs.AddTab("GalaxyMap", title: "GalaxyMap", partialViewName: "GalaxyMap");
        }
    }
}
