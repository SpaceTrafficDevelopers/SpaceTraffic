using SpaceTraffic.Dao;
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
    /// <summary>
    /// Action for selling cargo.
    /// </summary>
    [Serializable]
    class CargoSell : IPlannableAction
    {
        /// <summary>
        /// Result of action.
        /// </summary>
        public object Result { get; set;}

        /// <summary>
        /// Duration of action.
        /// </summary>
        public double Duration
        {
            get { return 1; }
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

        public int CargoLoadEntityID { get; set; }

        /// <summary>
        /// Amount of goods to buy
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Star system name
        /// </summary>
        public String StarSystemName { get; set; }

        /// <summary>
        /// Planet name
        /// </summary>
        public String PlanetName { get; set; }

        /// <summary>
        /// Loading place
        /// </summary>
        public ICargoLoadDao LoadingPlace { get; set; }

        /// <summary>
        /// Seller ship identification number
        /// </summary>
        public int SellerShipId { get; set; }

        /// <summary>
        /// Buyer ship identification number
        /// </summary>
        public int BuyerID { get; set; }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            Result = "Provádí se prodej zboží.";
            getArgumentsFromActionArgs(gameServer);

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SellerShipId);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            ICargoLoadEntity cargo = gameServer.Persistence.GetSpaceShipCargoDAO().GetCargoByID(CargoLoadEntityID);
            
            if (!ActionControls.checkObjects(this, new object[] { player, spaceShip, planet, cargo }))
                return;

            ActionControls.shipDockedAtBase(this, spaceShip, planet);
            ActionControls.checkCargoCount(this, cargo, Count);

            if (State == GameActionState.FAILED)
                return;
            
            if (!gameServer.Persistence.GetPlayerDAO().IncrasePlayersCredits(player.PlayerId,(int)(cargo.CargoPrice*Count)))
            {
                Result = String.Format("Změny se nepovedlo zapsat do databáze");
                State = GameActionState.FAILED;
                return;
            }

            ShipUnloadCargo unloadingAction = new ShipUnloadCargo();
            Object[] args = { StarSystemName, PlanetName, SellerShipId, CargoLoadEntityID, Count, ActionArgs[4].ToString(),BuyerID, SellerShipId };
            unloadingAction.ActionArgs = args;
            unloadingAction.PlayerId = PlayerId;

            gameServer.Game.PerformAction(unloadingAction);
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
            CargoLoadEntityID = Convert.ToInt32(ActionArgs[2].ToString());
            Count = Convert.ToInt32(ActionArgs[3].ToString());
            LoadingPlace = gameServer.Persistence.GetCargoLoadDao(ActionArgs[4].ToString()) ;
            BuyerID = Convert.ToInt32(ActionArgs[5].ToString());
            SellerShipId = Convert.ToInt32(ActionArgs[6].ToString());
        }
    }
}
