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

        public int CargoLoadEntityID { get; set; }

        /// <summary>
        /// Amount of goods to buy
        /// </summary>
        public int Count { get; set; }

        public String StarSystemName { get; set; }

        public String PlanetName { get; set; }

        public ICargoLoadDao LoadingPlace { get; set; }

        public int SellerShipId { get; set; }

        public int BuyerID { get; set; }

        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs(gameServer);
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SellerShipId);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            ICargoLoadEntity cargo = gameServer.Persistence.GetSpaceShipCargoDAO().GetCargoByID(CargoLoadEntityID);
            Entities.Base dockedBase = null;
            
            if(spaceShip.DockedAtBaseId != null)
                dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById((int)spaceShip.DockedAtBaseId);         

            if (cargo == null)
            {
                result = String.Format("Hráč {0} nemá zboží s ID = {1}.", player.PlayerId, CargoLoadEntityID);
                return;
            }

            if (dockedBase == null || !dockedBase.Planet.Equals(planet.Location))
            {
                result = String.Format("Loď {0} neni zadokovana na planetě {1}.", spaceShip.SpaceShipName, PlanetName);
                return;
            }

            if(cargo.CargoCount < Count)
            {
                result = String.Format("Hráč {0} nemá požadovaných {1} jednotek zboží s ID = {1}.", player.PlayerId, Count, CargoLoadEntityID);
                return;
            }


            if (!gameServer.Persistence.GetPlayerDAO().IncrasePlayersCredits(player.PlayerId,(int)(cargo.CargoPrice*Count)))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                return;
            }

			// increase player experiences by a fraction of cargo price; 1 is minimum gain 
			gameServer.Statistics.IncrementExperiences(player, Math.Max(1, (int)(cargo.CargoPrice * Count) / ExperienceLevels.FRACTION_OF_CARGO_PRICE));

            ShipUnloadCargo unloadingAction = new ShipUnloadCargo();
            Object[] args = { StarSystemName, PlanetName, SellerShipId, CargoLoadEntityID, Count, ActionArgs[4].ToString(),BuyerID, SellerShipId };
            unloadingAction.ActionArgs = args;
            unloadingAction.PlayerId = PlayerId;
            /*unloadingAction.PlayerId = PlayerId;
            unloadingAction.SpaceShipID = SellerShipId;
            unloadingAction.StarSystemName = StarSystemName;
            unloadingAction.PlanetName = PlanetName;
            unloadingAction.CargoLoadEntityID = cargo.CargoLoadEntityId;
            unloadingAction.LoadingPlace = LoadingPlace;
            unloadingAction.BuyerID = BuyerID;*/


            gameServer.Game.PerformAction(unloadingAction);
            State = GameActionState.FINISHED;
        }

        private void getArgumentsFromActionArgs(IGameServer gameServer)
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            CargoLoadEntityID = Convert.ToInt32(ActionArgs[2].ToString());
            Count = Convert.ToInt32(ActionArgs[3].ToString());
            LoadingPlace = gameServer.Persistence.GetCargoLoadDao(ActionArgs[4].ToString()) ;
            //LoadingPlace = (ICargoLoadDao)(ActionArgs[5]);
            BuyerID = Convert.ToInt32(ActionArgs[5].ToString());
            SellerShipId = Convert.ToInt32(ActionArgs[6].ToString());
        }
    }
}
