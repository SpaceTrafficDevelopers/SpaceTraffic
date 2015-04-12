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
    class ShipLoadCargo : IGameAction
    {
        private string result = "Zboží se nakládá.";

		
		public object Result
		{
			get { return new { result = this.result }; }
		}

        void IGameAction.Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs(gameServer);
            Cargo cargo = gameServer.Persistence.GetCargoDAO().GetCargoById(CargoID);
            SpaceShip spaceship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SpaceShipID);
            List<SpaceShip> spaceships = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipsByPlayer(PlayerId);
            //Base basePlanet = gameServer.Persistence.GetBaseDAO().GetBaseById(spaceship.DockedAtBaseId);
            String location = gameServer.World.Map[StarSystemName].Planets[PlanetName].Location;

            if (String.Compare(location, StarSystemName + "\\" + PlanetName) != 0) { 
                //TODO: zboží není na stejný planetě jako 
            }
            if (!spaceships.Contains(spaceship))
            {
                //TODO: loď nepatří hráči, nemůže na ni nakládat
                // result = String.Format("Loď s ID {0} Vám nepatří", SpaceShipID);
            }
            else { 
                SpaceShipCargo spaceshipcargo = new SpaceShipCargo() {
                    Cargo = cargo,
                    CargoId = cargo.CargoId,
                    CargoCount = Count,
                    SpaceShip = spaceship,
                    SpaceShipId = spaceship.SpaceShipId
                };
                gameServer.Persistence.GetSpaceShipCargoDAO().InsertSpaceShipCargo(spaceshipcargo);
            }
        }

        /// <summary>
        /// Get all arguments to properties from action args.
        /// </summary>
        /// <param name="gameServer">Instance of game server</param>
        void getArgumentsFromActionArgs(IGameServer gameServer) {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            SpaceShipID = Convert.ToInt32(ActionArgs[2]);
            CargoID = Convert.ToInt32(ActionArgs[3]);
            Count = Convert.ToInt32(ActionArgs[4]);
        }

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
    }
}
