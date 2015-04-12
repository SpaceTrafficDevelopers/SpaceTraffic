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
using SpaceTraffic.Engine;
using SpaceTraffic.Entities;


namespace SpaceTraffic.Game.Actions
{
    public class ShipUnloadCargo : IGameAction
    {
        private string result = "Nákup je ve vyřizování.";

        public GameActionState State
        {
            get;
            set;
        }

        public int PlayerId
        {
            get;
            set;
        }

        public int ActionCode
        {
            get;
            set;
        }

        public object Result
        {
            get { return new { result = this.result }; }
        }

        /*
         * 0: starSystemName
         * 1: planetName
         * 2: spaceshipID
         * 3: cargoID
         * 4: count
         */
        public object[] ActionArgs { get; set; }

        private String StarSystemName { get; set; }

        private String PlanetName { get; set; }

        private int SpaceShipID { get; set; }

        private int CargoID { get; set; }

        private int Count { get; set; }

        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();

            Cargo cargo = gameServer.Persistence.GetCargoDAO().GetCargoById(CargoID);
            SpaceShip spaceship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SpaceShipID);

            Entities.Base dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById(spaceship.DockedAtBaseId);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName]; 
            
            if (!dockedBase.Planet.Equals(planet))
            {
                //TODO: lod neni zadokovana na planete kde se provadi akce
                return;
            }

            SpaceShipCargo ssc = getSpaceShipCargoFromShip(gameServer, spaceship, cargo);

            if (ssc == null)
            {
                //TODO: zbozi neni na lodi
                return;
            }


            //TODO: kontola jeslti je misto na planete

            gameServer.Persistence.GetSpaceShipCargoDAO().RemoveSpaceShipCargoById(spaceship.SpaceShipId, cargo.CargoId);

            //TODO: vlozit na planetu
            
        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            SpaceShipID = Convert.ToInt32(ActionArgs[2]);
            CargoID = Convert.ToInt32(ActionArgs[3]);
            Count = Convert.ToInt32(ActionArgs[4]);
        }

        /// <summary>
        /// Return SpaceShipCargo from SpaceShipCargos which contains cargo instance.
        /// Or return null when space ship not contains cargo instance.
        /// </summary>
        /// <param name="gs">game server</param>
        /// <param name="ship">ship</param>
        /// <param name="cargo">cargo</param>
        /// <returns>SpaceShipCargo instace or null</returns>
        private SpaceShipCargo getSpaceShipCargoFromShip(IGameServer gs,SpaceShip ship, Cargo cargo)
        {
            List<SpaceShipCargo> cargoList = gs.Persistence.GetSpaceShipCargoDAO().GetSpaceShipCargoBySpaceShipId(ship.SpaceShipId);

            foreach (SpaceShipCargo ssc in cargoList)
            {
                if (ssc.CargoId == cargo.CargoId)
                    return ssc;
            }

            return null;
        }
        
    }
}
