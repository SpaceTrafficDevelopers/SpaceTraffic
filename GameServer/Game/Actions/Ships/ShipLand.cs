using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
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

namespace SpaceTraffic.Game.Actions.Ships
{
    class ShipLand : IGameAction
    {
        private string result = "Loď přilétá na planetu";

        public object Result
        {
            get { return new { result = this.result }; }
        }
        public GameActionState State { get; set; }

       public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        public object[] ActionArgs { get; set; }

        public String StarSystemName { get; set; }

        public String PlanetName { get; set; }

        public int ShipID { get; set; }

        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];

            if (player == null || spaceShip == null)
            {
                result = String.Format("Nastala chyba při vyhledávání položek");
                return;
            }

            if (spaceShip.PlayerId != PlayerId)
            {
                result = String.Format("Loď {0} nepatří hráči {1}", spaceShip.SpaceShipName, player.PlayerName);
                return;
            }

            if(!spaceShip.IsFlying)
            {
                result = String.Format("Loď {0} neletí a nemůže tedy ani přistát", spaceShip.SpaceShipName);
                return;
            }

            Entities.Base dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseByPlanetFullName(planet.Location);

            if (dockedBase == null)
            {
                result = String.Format("Na planetě {0} není žádná základna.", planet.Name);
                return;
            }

            spaceShip.DockedAtBaseId = dockedBase.BaseId;
            spaceShip.IsFlying = false;
            
            if(!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShipById(spaceShip))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                return;
            }

        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            ShipID = Convert.ToInt32(ActionArgs[2]);
        }
    }
}
