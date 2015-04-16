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

namespace SpaceTraffic.Game.Actions
{
    class CargoSell : IGameAction
    {
        private string result = "Provádí se prodej zboží.";

        public object Result
        {
            get { return new { result = this.result }; }
        }

        public GameActionState State { get; set; }

        /// <summary>
        /// Seller ID
        /// </summary>
        public int PlayerId { get; set; }

        public int ActionCode { get; set; }

        /// <summary>
        /// Arguments connected with concreate action
        /// </summary>
        public object[] ActionArgs { get; set; }

        /// <summary>
        /// Star system where sale was made
        /// </summary>
        private String StarSystemName { get; set; }

        /// <summary>
        /// Planet where sale was made
        /// </summary>
        private String PlanetName { get; set; }

        /// <summary>
        /// Identifier of goods to sell
        /// </summary>
        private int CargoID { get; set; }

        /// <summary>
        /// Amount of goods to sell
        /// </summary>
        private int Count { get; set; }

        /// <summary>
        /// Ship with sold goods
        /// </summary>
        private int ShipID { get; set; }

        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            SpaceShip ship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);
            List<PlanetGoods> planetGoods = planet.PlanetGoodsList;
            SpaceShipCargo cargo = null;

            foreach (SpaceShipCargo car in ship.SpaceShipsCargos)
            {
                if (CargoID == car.CargoId)
                {
                    cargo = car;
                    break;
                }
            }


            if (cargo == null || cargo.CargoCount < Count)
            {
                //TODO: V seznamu na planetě není požadovaný počet zboží
                result = String.Format("Na lodi {0} není požadovaných {1} jednotek nákaldu s ID = {2}.", planet.Name, Count, cargo.CargoId);
                return;
            }

            player.Credit += (int)(cargo.Cargo.Price * Count);

            if (gameServer.Persistence.GetPlayerDAO().UpdatePlayerById(player))
            {
                //TODO: Chyba při přístupu do databáze - neprovedl se update
                return;
            }

            //TODO: Odebrání zboží ze seznamu a převod na Cargo?
        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            CargoID = Convert.ToInt32(ActionArgs[2]);
            Count = Convert.ToInt32(ActionArgs[3]);
            ShipID = Convert.ToInt32(ActionArgs[4]);
        }
    }
}
