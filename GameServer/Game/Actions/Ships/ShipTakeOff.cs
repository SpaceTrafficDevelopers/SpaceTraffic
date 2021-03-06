﻿using SpaceTraffic.Engine;
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
    /// Action for taking off from planet with spaceship
    /// </summary>
    class ShipTakeOff : IGameAction
    {
        public object Result { get; set; }
        public GameActionState State { get; set; }

       public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        public object[] ActionArgs { get; set; }

        /// <summary>
        /// Star system name
        /// </summary>
        public String StarSystemName { get; set; }

        /// <summary>
        /// Planet name
        /// </summary>
        public String PlanetName { get; set; }

        /// <summary>
        /// Identification number of spaceship
        /// </summary>
        public int ShipID { get; set; }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            Result = "Loď odlétá z planety";
            getArgumentsFromActionArgs();

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerWithIncludes(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];

            if (!ActionControls.checkObjects(this, new object[] { player, spaceShip, planet }))
                return;

            ActionControls.shipDockedAtBase(this, spaceShip, planet);
            ActionControls.shipOwnerControl(this, spaceShip, player);
            ActionControls.isShipFlying(this, spaceShip, false);

            if (State == GameActionState.FAILED)
                return;

            spaceShip.DockedAtBaseId = null;
            spaceShip.IsFlying = true;
			spaceShip.StateText = "Je na cestě...";

            if (!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShip(spaceShip))
            {
                Result = String.Format("Změny se nepovedlo zapsat do databáze");
                State = GameActionState.FAILED;
                return;
			}
			gameServer.Statistics.IncrementStatisticItem(player, "takeOff", 1);

            State = GameActionState.FINISHED;
        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            ShipID = Convert.ToInt32(ActionArgs[2]);
        }
    }
}
