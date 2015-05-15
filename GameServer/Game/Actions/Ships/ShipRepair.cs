﻿using SpaceTraffic.Engine;
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

namespace SpaceTraffic.Game.Actions
{
    class ShipRepair: IGameAction
    {
        private static readonly int PERCENT_REPAIR_TIME = 3;
        private string result = "Loď je v opravě";

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

        public int RepairPercentage { get; set; }

        public int PercentRepairTax {get; set; }

        public bool RepairFinished { get; set; }

        public double Duration
        {
            get
            {
                return RepairPercentage * PERCENT_REPAIR_TIME;
            }
            set
            {
                RepairPercentage = (int)(value / PERCENT_REPAIR_TIME);
            }
        }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;

            getArgumentsFromActionArgs();
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            Entities.Base dockedBase = null;

            if (spaceShip.DockedAtBaseId != null)
                dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById((int)spaceShip.DockedAtBaseId);

            if (player == null || spaceShip == null)
            {
                result = String.Format("Nastala chyba při vyhledávání položek");
                State = GameActionState.FAILED;
                return;
            }

            if(player.Credit < RepairPercentage * PercentRepairTax)
            {
                result = String.Format("Hráč {0} nemá dostatek peněz na opravu", player.PlayerName);
                State = GameActionState.FAILED;
                return;
            }

            if(spaceShip.PlayerId != PlayerId)
            {
                result = String.Format("Loď {0} nepatří hráči {1}", spaceShip.SpaceShipName, player.PlayerName);
                State = GameActionState.FAILED;
                return;
            }

            if (dockedBase == null || !dockedBase.Planet.Equals(planet.Location))
            {
                result = String.Format("Loď {0} neni zadokovana na planetě {1}.", spaceShip.SpaceShipName, planet.Name);
                State = GameActionState.FAILED;
                return;
            }


            if (RepairFinished)
            {
                spaceShip.DamagePercent = Math.Max(0, spaceShip.DamagePercent - RepairPercentage);

                if (gameServer.Persistence.GetPlayerDAO().DecrasePlayersCredits(PlayerId, RepairPercentage * PercentRepairTax))
                {
                    result = String.Format("Změny se nepovedlo zapsat do databáze");
                    State = GameActionState.FAILED;
                    return;
                }
                
                if (!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShipById(spaceShip))
                {
                    result = String.Format("Změny se nepovedlo zapsat do databáze");
                    State = GameActionState.FAILED;
                    return;
                }
            }
            else
            {
                RepairFinished = true;
                gameServer.Game.ReplanEvent(this, Duration);
            }

            State = GameActionState.FINISHED;
        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            ShipID = Convert.ToInt32(ActionArgs[2]);
            RepairPercentage = Convert.ToInt32(ActionArgs[3]);
            PercentRepairTax = Convert.ToInt32(ActionArgs[4]);
        }
    }
}
