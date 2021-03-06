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
using System.Text;
using SpaceTraffic.Services.Contracts;
using NLog;
using SpaceTraffic.Utils.Debugging;
using GS = SpaceTraffic.GameServer.GameServer;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Actions;
using SpaceTraffic.Entities;
using SpaceTraffic.Dao;
using SpaceTraffic.Game.Planner;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Game;

namespace SpaceTraffic.GameServer.ServiceImpl
{
	public class GameService : IGameService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();


		public IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem)
		{
			DebugEx.Entry(starSystem);

			IList<WormholeEndpointDestination> result = GS.CurrentInstance.World.Map.GetStarSystemConnections(starSystem);

			DebugEx.Exit(result);
			return result;
		}
        public IList<StarSystem> GetGalaxyMap(string galaxyMap)
        {
                DebugEx.Entry(galaxyMap);
                try
                {
                    GalaxyMap map = GS.CurrentInstance.World.Map;
                    IList<StarSystem> result = map.GetStarSystems();
                    foreach (StarSystem system in result)
                    {
                        // prekopiruju data z mapy do seznamu, z duvodu serializace
                        system.WormholeEndpointsList = new List<WormholeEndpoint>();
                        foreach (WormholeEndpoint endpoint in system.WormholeEndpoints)
                        {
                            system.WormholeEndpointsList.Add(endpoint);
                        }
                    }
                    DebugEx.Exit(result);
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
        }

		/// <summary>
		/// Returns base by full planet name (name param)
		/// Full planet name (location) = "StarSystem.Name\Planet.Name"
		/// </summary>
		/// <param name="planetName">
		/// Full planet name (location) = "StarSystem.Name\Planet.Name"
		/// Name (not altName) of the planet</param>
		/// <returns></returns>
	public SpaceTraffic.Entities.Base GetBaseByName(string planetName) {
			return GS.CurrentInstance.Persistence.GetBaseDAO().GetBaseByPlanetFullName(planetName);
		}

	/// <summary>
	/// All available bases.
	/// </summary>
	/// <returns></returns>
	public IList<SpaceTraffic.Entities.Base> GetAllBases()
	{
		return GS.CurrentInstance.Persistence.GetBaseDAO().GetBases();
	}



		/// <summary>
		/// Finds action in SpaceTraffic.Game.Actions namespace and performs it with its arguments.
		/// </summary>
		/// <param name="playerId">The player identifier.</param>
		/// <param name="actionName">Name of the action (and class in Actions namespace).</param>
		/// <param name="actionArgs">The action arguments.</param>
		/// <returns>ActionCode of created action.</returns>
		/// <exception cref="SpaceTraffic.Services.Contracts.ActionNotFoundException"></exception>
		public int PerformAction(int playerId, string actionName, params object[] actionArgs)
		{
			try { 
				IGameAction action = Activator.CreateInstance(Type.GetType("SpaceTraffic.Game.Actions." + actionName)) as IGameAction;
				if (action == null)
				{//if action does not implemented action
					throw new ActionNotFoundException(String.Format(
						"Action class with name: {0} does not implements IGameAction.",
						actionName
					));
				}
				action.ActionArgs = actionArgs;
				action.PlayerId = playerId;
				GameServer.CurrentInstance.Game.PerformAction(action);
				return action.ActionCode;
			} catch(System.IO.FileNotFoundException e){
				Console.WriteLine("Action file with name " + actionName + " was not found in SpaceTraffic.Game.Actions namespace.");
				throw;
			}
		}

		public object GetActionResult(int playerId, int actionCode)
		{
			throw new NotImplementedException();
		}
	}
}
