
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
using SpaceTraffic.Entities.Goods;
using SpaceTraffic.Dao;

namespace SpaceTraffic.Game.Actions
{
    public class CargoBuy : IGameAction
    {
        private string result = "Provádí se nákup zboží.";

        public object Result
        {
            get { return new { result = this.result }; }
        }

        public GameActionState State { get; set; }

        /// <summary>
        /// Vrací ID hráče, který tuto akci vyžádal.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Vrací kód akce.
        /// Kód akce je číslo, které jednoznačně identifikuje akci v hráčově seznamu vykonávaných akcí.
        /// </summary>
        public int ActionCode { get; set; }

        /// <summary>
        /// arguments connected with concreate action
        /// </summary>
        public object[] ActionArgs { get; set; }
        
       
        /// <summary>
        /// Identifier of goods to buy
        /// </summary>
        private int CargoID { get; set; }

        /// <summary>
        /// Amount of goods to buy
        /// </summary>
        private int Count { get; set; }

        private ICargoLoadDao LoadingPlace { get; set; }

        private ICargoLoadDao BuyingPlace { get; set; }

        private int ownerID { get; set; }

       /* private string where { get; set; }
        private string from { get; set; }*/


        public void Perform(IGameServer gameServer)
        {
            getArgumentsFromActionArgs();
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            ICargoLoadEntity cargo = BuyingPlace.GetCargoByID(ownerID, CargoID);

            if(player == null || cargo == null)
            {
                result = String.Format("Nastala chyba při vyhledávání položek");
                return;
            }

            if (cargo.CargoCount < Count)
            {
                result = String.Format("U obchodníka id={0} není požadovaných {1} jednotek zboží id={2}.", ownerID, Count, CargoID);
                return;
            }

            if(player.Credit < cargo.CargoPrice * Count)
            {
                result = String.Format("Hráč id={0} nemá peníze na nákup {1} jednotek zboží id={2}.", player.PlayerId, Count, cargo.CargoId);
                return;
            }

            if (gameServer.Persistence.GetPlayerDAO().DecrasePlayersCredits(player.PlayerId, (int)(cargo.CargoPrice * Count)))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                return;
            }

            cargo.CargoCount -= Count;

            if(cargo.CargoCount == 0)
            {
                BuyingPlace.RemoveCargo(cargo);
            }
            else
            {
                BuyingPlace.UpdateCargoCountById(cargo);
            }

            cargo.CargoCount = Count;
            //Zavolat LoadCargo
        }

        private void getArgumentsFromActionArgs()
        {
            CargoID = Convert.ToInt32(ActionArgs[0].ToString());
            Count = Convert.ToInt32(ActionArgs[1]);
            BuyingPlace = (ICargoLoadDao)ActionArgs[2];
            LoadingPlace = (ICargoLoadDao)ActionArgs[3];
            ownerID = Convert.ToInt32(ActionArgs[4]); 
           
        }

    }

   
}
