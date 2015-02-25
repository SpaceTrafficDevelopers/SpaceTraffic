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
using System.Text;
using SpaceTraffic.Services.Contracts;
using NLog;
using SpaceTraffic.Utils.Debugging;
using GS = SpaceTraffic.GameServer.GameServer;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Actions;

namespace SpaceTraffic.GameServer.ServiceImpl
{
    public class GameService :IGameService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        

        public IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem)
        {
            DebugEx.Entry(starSystem);

            IList<WormholeEndpointDestination> result = GS.CurrentInstance.World.Map.GetStarSystemConnections(starSystem);

            DebugEx.Exit(result);
            return result;
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
                        "Action class with name: {0} does not does not implements IGameAction.",
                        actionName
                    ));
                }
                action.ActionArgs = actionArgs;
                action.PlayerId = playerId;
                action.State = GameActionState.PLANNED;
                GameServer.CurrentInstance.Game.PerformAction(action);
            } catch(System.IO.FileNotFoundException e){
                Console.WriteLine("Action file with name " + actionName + " was not found in SpaceTraffic.Game.Actions namespace.");
                throw;
            }           
            return 0;
        }

        

        public object GetActionResult(int playerId, int actionCode)
        {
            throw new 
                Exception();
        }
    }
}
