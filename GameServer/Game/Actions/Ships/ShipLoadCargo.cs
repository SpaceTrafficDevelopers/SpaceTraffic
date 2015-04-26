﻿using SpaceTraffic.Dao;
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
    public class ShipLoadCargo : IGameAction
    {
        private string result = "Náklad se nakládá.";


        public object Result
        {
            get { return new { result = this.result }; }
        }

        void IGameAction.Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs(gameServer);

            SpaceShip spaceship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SpaceShipID);
            ICargoLoadEntity cargo = BuyingPlace.GetCargoByID(CargoLoadEntityId);

            Entities.Base dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById(spaceship.DockedAtBaseId);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];

            /*if (!dockedBase.Planet.Equals(planet))
            {
                result = String.Format("Loď {0} není zadokována na planetě {1}.", spaceship.SpaceShipName, PlanetName);
                return;
            }*/
            if (spaceship.PlayerId != PlayerId)
            {
                result = String.Format("Loď {0} Vám nepatří, nemůžete na ní naložit náklad.", spaceship.SpaceShipName);
                return;
            }

            if(!checkSpaceShipCargos(gameServer,spaceship, cargo)){
                
                result = String.Format("Loď {0} nemá dostatek místa na naložení nákladu.", spaceship.SpaceShipName);
                return;
            }

            if (cargo.CargoCount < Count)
            {
                result = String.Format("U obchodníka id={0} není požadovaných {1} jednotek zboží id={2}.", cargo.CargoOwnerId, Count, cargo.CargoId);
                return;
            }

            cargo.CargoCount -= Count;

            if (!BuyingPlace.UpdateOrRemoveCargo(cargo))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                return;
            }

            cargo.CargoCount = Count;
            cargo.CargoOwnerId = PlayerId;

            gameServer.Persistence.GetSpaceShipCargoDAO().InsertOrUpdateCargo(cargo);
            result = String.Format("Náklad byl úspěšně naložen na loď s {0}.", spaceship.SpaceShipName);
        }

        /// <summary>
        /// Get all arguments to properties from action args.
        /// </summary>
        /// <param name="gameServer">Instance of game server</param>
        private void getArgumentsFromActionArgs(IGameServer gameServer)
        {
                StarSystemName = ActionArgs[0].ToString();
                PlanetName = ActionArgs[1].ToString();
                SpaceShipID = Convert.ToInt32(ActionArgs[2]);
                CargoLoadEntityId = Convert.ToInt32(ActionArgs[3]);
                Count = Convert.ToInt32(ActionArgs[4]);
                BuyingPlace = (ICargoLoadDao)ActionArgs[5];
        }

       /* /// <summary>
        /// Get spaceship cargo from arguments
        /// </summary>
        /// <param name="gameServer">Instance of game server</param>
        /// <param name="cargo">Instance of cargo for load</param>
        /// <param name="spaceship">Instance of spaceship</param>
        /// <returns>Spaceship cargo</returns>
        private SpaceShipCargo getSpaceshipCargoFromArguments(IGameServer gameServer, Cargo cargo, SpaceShip spaceship)
        {
            SpaceShipCargo spaceshipcargo = new SpaceShipCargo()
            {
                Cargo = cargo,
                CargoId = cargo.CargoId,
                CargoCount = CargoCount,
                SpaceShip = spaceship,
                SpaceShipId = spaceship.SpaceShipId
            };
            return spaceshipcargo;
        }*/

        /// <summary>
        /// Check space for cargo in space ship.
        /// </summary>
        /// <param name="gameServer">game server</param>
        /// <param name="spaceShip">space ship</param>
        /// <returns>true when ship has space for cargo, otherwise fale</returns>
        private bool checkSpaceShipCargos(IGameServer gameServer, SpaceShip spaceShip, ICargoLoadEntity cargo)
        {
            List<ICargoLoadEntity> cargoList = gameServer.Persistence.GetSpaceShipCargoDAO().GetCargoListByOwnerId(spaceShip.SpaceShipId);

            int freeSpace = spaceShip.CargoSpace;

            foreach (SpaceShipCargo ssc in cargoList)
            {
                freeSpace -= ssc.CargoCount * gameServer.Persistence.GetCargoDAO().GetCargoById(cargo.CargoId).Volume;
            }

            int demandedSpace = gameServer.Persistence.GetCargoDAO().GetCargoById(cargo.CargoId).Volume * cargo.CargoCount;
            
            return demandedSpace < freeSpace;
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
         * 3: cargoLoadEntityId
         */
        public object[] ActionArgs { get; set; }

        public String StarSystemName { get; set; }

        public String PlanetName { get; set; }

        public int SpaceShipID { get; set; }

        public int CargoLoadEntityId { get; set; }
        public int Count { get; set; }

        public ICargoLoadDao BuyingPlace { get; set; }
    }
}
