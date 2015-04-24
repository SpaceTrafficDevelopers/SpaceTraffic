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
using SpaceTraffic.Dao;


namespace SpaceTraffic.Game.Actions
{
    public class ShipUnloadCargo : IGameAction
    {
        private string result = "Náklad se vykládá.";

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

        public String StarSystemName { get; set; }

        public String PlanetName { get; set; }

        public int SpaceShipID { get; set; }
        public int BuyerID { get; set; }

        public int CargoLoadEntityID { get; set; }
        public int Count { get; set; }
        public ICargoLoadDao LoadingPlace { get; set; }


        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs(gameServer);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SpaceShipID);

            Entities.Base dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById(spaceShip.DockedAtBaseId);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            ICargoLoadEntity cargo = gameServer.Persistence.GetSpaceShipCargoDAO().GetCargoByID(CargoLoadEntityID);
            
            if (!dockedBase.Planet.Equals(planet))
            {
                result = String.Format("Loď {0} neni zadokovana na planetě {1}.", spaceShip.SpaceShipName, PlanetName);
                return;
            }

            if (cargo == null)
            {
                result = String.Format("Neni co vykladat");
                return;
            }

            if(cargo.CargoCount < Count)
            {
                result = String.Format("Loď {0} nemá naloženo {1} jednotek zboží id={1}.", spaceShip.SpaceShipName, Count, cargo.CargoId);
                return;
            }

            cargo.CargoCount -= Count;

            if (!gameServer.Persistence.GetSpaceShipCargoDAO().UpdateOrRemoveCargo(cargo))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                return;
            }

            cargo.CargoCount = Count;
            cargo.CargoOwnerId = BuyerID;
            LoadingPlace.InsertOrUpdateCargo(cargo);
            
            result = String.Format("Náklad {0} byl vyložen.", cargo.CargoId);
        }

        private void getArgumentsFromActionArgs(IGameServer gameServer)
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            SpaceShipID = Convert.ToInt32(ActionArgs[2]);
            CargoLoadEntityID = Convert.ToInt32(ActionArgs[3]);
            Count = Convert.ToInt32(ActionArgs[4]);
            LoadingPlace = gameServer.Persistence.GetCargoLoadDao(ActionArgs[5].ToString());
            BuyerID = Convert.ToInt32(ActionArgs[6]);
        }

      /*  /// <summary>
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
        }*/
        
    }
}
