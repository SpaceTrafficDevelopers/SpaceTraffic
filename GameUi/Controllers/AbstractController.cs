﻿/**
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
using SpaceTraffic.GameUi.Models.Ui;
using SpaceTraffic.Utils.Debugging;
using System.IO;
using SpaceTraffic.GameUi.Configuration;
using System.Xml.Linq;
using SpaceTraffic.GameUi.GameServerClient;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.GameUi.Extensions;
using System.Web.Security;
using SpaceTraffic.Entities;

namespace SpaceTraffic.GameUi.Controllers
{
	/// <summary>
	/// Base controller for all controllers. All controllers should be capable to call supportive methods from this one.
	/// </summary>
	public abstract class AbstractController : Controller
	{
		public readonly IGameServerClient GSClient = GameServerClientFactory.GetClientInstance();
		protected string ErrorMessage = "";

		/// <summary>
		/// Loads the XML assembly from assembly folder
		/// </summary>
		/// <param name="folder">Folder in Assets folder - can be path.</param>
		/// <param name="xmlName">Name of the XML file.</param>
		/// <returns></returns>
		public XDocument loadXmlAssembly(string folder, string xmlName)
		{
			DebugEx.Entry(xmlName);

			string folderName = Path.Combine(GameUiConfiguration.AssetPath, folder);
			string filePath = Path.Combine(folderName, xmlName + ".xml");
			DebugEx.WriteLineF("Assembly filename: {0}", Path.GetFullPath(filePath));

			XDocument assembly = XDocument.Load(filePath);
			DebugEx.Exit(filePath);
			return assembly;
		}

		/// <summary>
		/// Returns id of player who is currently signed in.
		/// </summary>
		/// <returns></returns>
		public int getCurrentPlayerId()
		{
			return (int)Membership.GetUser().ProviderUserKey;
		}

		/// <summary>
		/// Returns username of player who is currently signed in.
		/// </summary>
		/// <returns></returns>
		public string getCurrentPlayerName()
		{
			return HttpContext.User.Identity.Name;
		}

		/// <summary>
		/// Returns name of the starsystem player is currently in
		/// </summary>
		/// <returns></returns>
		public string getCurrentStarSystem()
		{
			string cookieValue = Request.Cookies.Get("currentStarSystem").Value;
			return cookieValue.Replace("%20", " ");
		}

		/// <summary>
		/// Controls the ship access. Sets ErrorMessage when there is a problem.
		/// </summary>
		/// <returns>True when is everything ok</returns>

		protected bool controlShipAccess(SpaceShip ship)
		{
			int curPlayerId = getCurrentPlayerId();
			if (ship == null)
			{
				this.ErrorMessage = "Tato loď neexistuje!";
				return false;
			}
			if (ship.PlayerId != curPlayerId)
			{
				this.ErrorMessage = "Tato loď ti nepatří!";
				return false;
			}
			if (!ship.IsAvailable)
			{
				this.ErrorMessage = "Tato loď je zaneprázdněna činností: " + ship.StateText;
				return false;
			}
			return true;
		}

	}
}
