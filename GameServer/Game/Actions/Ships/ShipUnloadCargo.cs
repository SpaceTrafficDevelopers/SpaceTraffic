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
    /// <summary>
    /// Action for unload cargo from spaceship
    /// </summary>
    public class ShipUnloadCargo : IPlannableAction
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
         * 3: buyerID
         * 4: cargoLoadEntityID
         * 5: count
         * 6: loadingPlace
         */
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
        /// Spaceship identification number
        /// </summary>
        public int SpaceShipID { get; set; }

        /// <summary>
        /// Buyer identification number
        /// </summary>
        public int BuyerID { get; set; }

        /// <summary>
        /// Cargo load entity identification number
        /// </summary>
        public int CargoLoadEntityID { get; set; }

        /// <summary>
        /// Count of cargo
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Loading place for cargo
        /// </summary>
        public ICargoLoadDao LoadingPlace { get; set; }

        /// <summary>
        /// Duration of action
        /// </summary>
        public double Duration
        {
            get { return 1; }
        }


        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs(gameServer);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SpaceShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            ICargoLoadEntity cargo = gameServer.Persistence.GetSpaceShipCargoDAO().GetCargoByID(CargoLoadEntityID);
            Entities.Base dockedBase = null;

            if (spaceShip.DockedAtBaseId != null)
                dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById((int)spaceShip.DockedAtBaseId);

            // control if spaceship is docked on same place 
            if (dockedBase == null || !dockedBase.Planet.Equals(planet.Location))
            {
                result = String.Format("Loď {0} neni zadokovana na planetě {1}.", spaceShip.SpaceShipName, PlanetName);
                State = GameActionState.FAILED;
                return;
            }

            //control if is anything to unload
            if (cargo == null)
            {
                result = String.Format("Neni co vykladat");
                State = GameActionState.FAILED;
                return;
            }

            //control if spaceship has goods count to unload
            if(cargo.CargoCount < Count)
            {
                result = String.Format("Loď {0} nemá naloženo {1} jednotek zboží id={1}.", spaceShip.SpaceShipName, Count, cargo.CargoId);
                State = GameActionState.FAILED;
                return;
            }

            cargo.CargoCount = Count;

            if (!gameServer.Persistence.GetSpaceShipCargoDAO().UpdateOrRemoveCargo(cargo))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                State = GameActionState.FAILED;
                return;
            }

            cargo.CargoOwnerId = BuyerID;
            LoadingPlace.InsertOrUpdateCargo(cargo);
            
            result = String.Format("Náklad {0} byl vyložen.", cargo.CargoId);
            State = GameActionState.FINISHED;
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
            CargoLoadEntityID = Convert.ToInt32(ActionArgs[3]);
            Count = Convert.ToInt32(ActionArgs[4]);
            LoadingPlace = gameServer.Persistence.GetCargoLoadDao(ActionArgs[5].ToString());
            BuyerID = Convert.ToInt32(ActionArgs[6].ToString());
        }
        
    }
}
