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
            getArgumentsFromActionArgs();
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(SellerShipId);

            Entities.Base dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById(spaceShip.DockedAtBaseId);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            ICargoLoadEntity cargo = gameServer.Persistence.GetSpaceShipCargoDAO().GetCargoByID(CargoLoadEntityID);

            if (cargo == null)
            {
                result = String.Format("Hráč {0} nemá zboží s ID = {1}.", player.PlayerId, CargoLoadEntityID);
                return;
            }

            if (!dockedBase.Planet.Equals(planet))
            {
                result = String.Format("Loď {0} neni zadokovana na planetě {1}.", spaceShip.SpaceShipName, PlanetName);
                return;
            }

            if(cargo.CargoCount < Count)
            {
                result = String.Format("Hráč {0} nemá požadovaných {1} jednotek zboží s ID = {1}.", player.PlayerId, Count, CargoLoadEntityID);
                return;
            }


            if (gameServer.Persistence.GetPlayerDAO().IncrasePlayersCredits(player.PlayerId,(int)(cargo.CargoPrice*Count)))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                return;
            }

            ShipUnloadCargo unloadingAction = new ShipUnloadCargo();
            unloadingAction.PlayerId = PlayerId;
            unloadingAction.SpaceShipID = SellerShipId;
            unloadingAction.StarSystemName = StarSystemName;
            unloadingAction.PlanetName = PlanetName;
            unloadingAction.CargoLoadEntityID = cargo.CargoLoadEntityId;
            unloadingAction.LoadingPlace = LoadingPlace;
            unloadingAction.BuyerID = BuyerID;


            gameServer.Game.PerformAction(unloadingAction);
        }

        private void getArgumentsFromActionArgs()
        {
            StarSystemName = ActionArgs[0].ToString();
            PlanetName = ActionArgs[1].ToString();
            CargoLoadEntityID = Convert.ToInt32(ActionArgs[2]);
            Count = Convert.ToInt32(ActionArgs[3]);
            LoadingPlace = (ICargoLoadDao)ActionArgs[4];
            BuyerID = Convert.ToInt32(ActionArgs[5]);
        }
    }
}
