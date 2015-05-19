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
    class ShipLand : IGameAction
    {
        public object Result { get; set; }
        public GameActionState State { get; set; }

       public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        public object[] ActionArgs { get; set; }

        public String StarSystemName { get; set; }

        public String PlanetName { get; set; }

        public int ShipID { get; set; }

        public double FlightTime { get; set; }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            Result = "Loď přilétá na planetu";
            getArgumentsFromActionArgs();

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];

            if (!ActionControls.checkObjects(this, new Object[] { player, spaceShip, planet }))
                return;

            ActionControls.shipOwnerControl(this, spaceShip, player);
            ActionControls.isShipFlying(this, spaceShip, true);
            int baseID = ActionControls.checkBaseAtPlanet(this, planet);

            if (State == GameActionState.FAILED)
                return;
            
            spaceShip.CurrentFuelTank -= (int)(spaceShip.Consumption * FlightTime);
            spaceShip.DamagePercent += (int)(spaceShip.WearRate * FlightTime);
            spaceShip.DockedAtBaseId = baseID;
            spaceShip.IsFlying = false;
            
            if(spaceShip.CurrentFuelTank < 0 || spaceShip.DamagePercent > 100)
            {
                Result = String.Format("Lodi {1} došlo palivo nebo je zničená a nemůže přistát", spaceShip.SpaceShipName);
                State = GameActionState.FAILED;
                return;
            }

            if(!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShipById(spaceShip))
            {
                Result = String.Format("Změny se nepovedlo zapsat do databáze");
                State = GameActionState.FAILED;
                return;
            }

            State = GameActionState.FINISHED;
        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            ShipID = Convert.ToInt32(ActionArgs[2]);
            FlightTime = Convert.ToDouble(ActionArgs[3]);
        }
    }
}
