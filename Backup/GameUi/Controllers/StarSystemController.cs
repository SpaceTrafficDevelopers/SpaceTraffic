using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpaceTraffic.Utils.Debugging;
using SpaceTraffic.GameUi.GameServerClient;
using SpaceTraffic.Entities.PublicEntities;
using System.Web.Configuration;
using System.IO;
using SpaceTraffic.GameUi.Configuration;
using NLog;

namespace SpaceTraffic.GameUi.Controllers
{

    /// <summary>
    /// Kontroler postkytující data o star systemu.
    /// Obsahuje akce pro získání xml star systemu, jeho napojení a o dynamických objektech, které se v něm nachází.
    /// </summary>
    public class StarSystemController : Controller
    {
        private readonly IGameServerClient GSClient = GameServerClientFactory.GetClientInstance();
        //
        // GET: /StarSystem/
        /// <summary>
        /// Získání xml star systemu na základě jeho jména. K získání cesty využívá klíč assetPath 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpGet]
        public FilePathResult Get(string id)
        {
            DebugEx.Entry(id);
            
            string filename = Path.Combine(GameUiConfiguration.MapPath, id + ".xml");
            DebugEx.WriteLineF("Starsystem filename: {0}", Path.GetFullPath(filename));

            FilePathResult filePathResult = File(filename, "text/xml", id + ".xml");
            DebugEx.Exit(filePathResult);
            return filePathResult;
        }

        [HttpGet]
        public JsonResult GetConnections(string id)
        {
            DebugEx.Entry(id);

            if (id == null)
            {
                throw new HttpException(404, "NotFound");
            }

            IList<WormholeEndpointDestination> data = GSClient.GameService.GetStarSystemConnections(id);
            JsonResult result = Json(data, JsonRequestBehavior.AllowGet);

            DebugEx.Exit(result);
            return result;

        }

    }
}
