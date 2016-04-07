using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.Game.Utils;
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

namespace SpaceTraffic.Game.Actions
{
    /// <summary>
    /// Action for fly through wormhole with spaceship.
    /// </summary>
    class ShipFlyThroughWormHole : IGameAction
    {
        /// <summary>
        /// Result of action
        /// </summary>
        public object Result { get; set; }
        public GameActionState State { get; set; }

        /// <summary>
        /// Player identification number
        /// </summary>
       public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        public object[] ActionArgs { get; set; }

        /// <summary>
        /// Identification number of wormhole
        /// </summary>
        public int WormHoleId { get; set; }

        /// <summary>
        /// Identification number of spaceship
        /// </summary>
        public int ShipID { get; set; }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            Result = "Loď prolétá červí dírou";
            getArgumentsFromActionArgs();

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerWithIncludes(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);

            if (!ActionControls.checkObjects(this, new Object[] { player, spaceShip }))
                return;

            ActionControls.shipOwnerControl(this, spaceShip, player);
            ActionControls.isShipFlying(this, spaceShip, true);

            if(State == GameActionState.FAILED)
                return;

            WormholeEndpoint wormHole = gameServer.World.Map[spaceShip.CurrentStarSystem].WormholeEndpoints[WormHoleId];

            if (wormHole == null)
            {
                Result = String.Format("V systému {0} není červí díra {1}.", spaceShip.CurrentStarSystem, WormHoleId);
                State = GameActionState.FAILED;
                return;
            }

            spaceShip.CurrentStarSystem = wormHole.Destination.StarSystem.Name;

            if (!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShip(spaceShip))
            {
                Result = String.Format("Změny se nepovedlo zapsat do databáze");
                State = GameActionState.FAILED;
                return;
            }

            State = GameActionState.FINISHED;
        }

        /// <summary>
        /// Get argument from action args by converting to datatypes.
        /// </summary>
        private void getArgumentsFromActionArgs()
        {
            WormHoleId = Convert.ToInt32(ActionArgs[0].ToString());
            ShipID = Convert.ToInt32(ActionArgs[1]);
        }
    }
}
