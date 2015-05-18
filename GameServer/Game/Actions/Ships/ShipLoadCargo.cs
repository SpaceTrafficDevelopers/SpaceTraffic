using SpaceTraffic.Dao;
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
    /// <summary>
    /// Action for load cargo to space ship. 
    /// </summary>
    public class ShipLoadCargo : IPlannableAction
    {
        private string result = "Náklad se nakládá.";

        /// <summary>
        /// Result of action
        /// </summary>
        public object Result
        {
            get { return new { result = this.result }; }
        }

        /// <summary>
        /// Duration of action
        /// </summary>
        public double Duration
        {
            get { return 1; }
        }

        void IGameAction.Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs(gameServer);

            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SpaceShipID);
            ICargoLoadEntity cargo = BuyingPlace.GetCargoByID(CargoLoadEntityId);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            Entities.Base dockedBase = null;

            if (spaceShip.DockedAtBaseId != null)
                dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById((int)spaceShip.DockedAtBaseId);

            // control if spaceship is docked on same planet where want to load cargo
            if (dockedBase == null || !dockedBase.Planet.Equals(planet.Location))
            {
                result = String.Format("Loď {0} není zadokována na planetě {1}.", spaceShip.SpaceShipName, PlanetName);
                State = GameActionState.FAILED;
                return;
            }

            // control if spaceship belong to player
            if (spaceShip.PlayerId != PlayerId)
            {
                result = String.Format("Loď {0} Vám nepatří, nemůžete na ní naložit náklad.", spaceShip.SpaceShipName);
                State = GameActionState.FAILED;
                return;
            }

            // control if spaceship has space for load cargo
            if(!checkSpaceShipCargos(gameServer,spaceShip, cargo)){
                
                result = String.Format("Loď {0} nemá dostatek místa na naložení nákladu.", spaceShip.SpaceShipName);
                State = GameActionState.FAILED;
                return;
            }

            // control if trader has required count of goods
            if (cargo.CargoCount < Count)
            {
                result = String.Format("U obchodníka id={0} není požadovaných {1} jednotek zboží id={2}.", cargo.CargoOwnerId, Count, cargo.CargoId);
                State = GameActionState.FAILED;
                return;
            }

            cargo.CargoCount = Count;

            if (!BuyingPlace.UpdateOrRemoveCargo(cargo))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                State = GameActionState.FAILED;
                return;
            }

            cargo.CargoOwnerId = PlayerId;

            gameServer.Persistence.GetSpaceShipCargoDAO().InsertOrUpdateCargo(cargo);
            result = String.Format("Náklad byl úspěšně naložen na loď s {0}.", spaceShip.SpaceShipName);
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
                CargoLoadEntityId = Convert.ToInt32(ActionArgs[3]);
                Count = Convert.ToInt32(ActionArgs[4]);
                BuyingPlace = gameServer.Persistence.GetCargoLoadDao(ActionArgs[5].ToString());
        }

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

        /* Arguments in action args
         * 0: starSystemName
         * 1: planetName
         * 2: spaceshipID
         * 3: cargoLoadEntityId
         * 4: count
         * 5: buying place
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
        /// Cargo load entity identification number
        /// </summary>
        public int CargoLoadEntityId { get; set; }

        /// <summary>
        /// Count of cargo
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Buying place
        /// </summary>
        public ICargoLoadDao BuyingPlace { get; set; }
    }
}
