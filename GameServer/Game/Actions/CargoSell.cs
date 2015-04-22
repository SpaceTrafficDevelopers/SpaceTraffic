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

        private int CargoID { get; set; }

        /// <summary>
        /// Amount of goods to buy
        /// </summary>
        private int Count { get; set; }

        private ICargoLoadDao LoadingPlace { get; set; }

        private ICargoLoadDao SellingPlace { get; set; }

        private int BuyerID { get; set; }

        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            ICargoLoadEntity cargo = SellingPlace.GetCargoByID(player.PlayerId, CargoID);

            if (cargo == null)
            {
                result = String.Format("Hráč {0} nemá zboží s ID = {1}.", player.PlayerId, CargoID);
                return;
            }

            if(cargo.CargoCount < Count)
            {
                result = String.Format("Hráč {0} nemá požadovaných {1} jednotek zboží s ID = {1}.", player.PlayerId, Count, CargoID);
                return;
            }


            if (gameServer.Persistence.GetPlayerDAO().IncrasePlayersCredits(player.PlayerId,(int)(cargo.CargoPrice*Count)))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                return;
            }

            cargo.CargoCount -= Count;
            if(cargo.CargoCount == 0)
            {
                SellingPlace.RemoveCargo(cargo);
            }
            else
            {
                SellingPlace.UpdateCargoCountById(cargo);
            }
        }

        private void getArgumentsFromActionArgs()
        {
            CargoID = Convert.ToInt32(ActionArgs[0]);
            Count = Convert.ToInt32(ActionArgs[1]);
            LoadingPlace = (ICargoLoadDao)ActionArgs[2];
            SellingPlace = (ICargoLoadDao)ActionArgs[3];
            BuyerID = Convert.ToInt32(ActionArgs[4]);
        }
    }
}
