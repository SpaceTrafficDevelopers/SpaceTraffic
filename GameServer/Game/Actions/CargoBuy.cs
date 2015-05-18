﻿
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
using System.Runtime.Serialization;

namespace SpaceTraffic.Game.Actions
{
    /// <summary>
    /// Action for buying cargo.
    /// </summary>
    [Serializable]
    public class CargoBuy : IPlannableAction
    {
        private string result = "Provádí se nákup zboží.";

        /// <summary>
        /// Result of action.
        /// </summary>
        public object Result
        {
            get { return new { result = this.result }; }
        }

        /// <summary>
        /// Duration of action.
        /// </summary>
        public double Duration
        {
            get { return 1; }
        }

        public GameActionState State { get; set; }

        /// <summary>
        /// Player identification number who wats this action.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Return action code.
        /// Action code is number which positively identificates action in player list of actions.
        /// </summary>
        public int ActionCode { get; set; }

        /// <summary>
        /// arguments connected with concreate action
        /// </summary>
        public object[] ActionArgs { get; set; }
        
       
        /// <summary>
        /// Identifier of goods to buy
        /// </summary>
        public int CargoLoadEntityID { get; set; }

        /// <summary>
        /// Amount of goods to buy
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Buyer ship identification number
        /// </summary>
        public int BuyerShipID { get; set; }

        /// <summary>
        /// Buying place
        /// </summary>
        public ICargoLoadDao BuyingPlace { get; set; }

        /// <summary>
        /// Star system name
        /// </summary>
        public String StarSystemName { get; set; }

        /// <summary>
        /// Planet name
        /// </summary>
        public String PlanetName { get; set; }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            getArgumentsFromActionArgs(gameServer);
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerById(PlayerId);
            ICargoLoadEntity cargo = BuyingPlace.GetCargoByID(CargoLoadEntityID);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(BuyerShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];
            Entities.Base dockedBase = null;

            if (spaceShip.DockedAtBaseId != null)
                dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById((int)spaceShip.DockedAtBaseId);

            if (dockedBase == null || !dockedBase.Planet.Equals(planet.Location))
            {
                result = String.Format("Loď {0} neni zadokovana na planetě {1}.", spaceShip.SpaceShipName, PlanetName);
                State = GameActionState.FAILED;
                return;
            }

            if(player == null || cargo == null)
            {
                result = String.Format("Nastala chyba při vyhledávání položek");
                State = GameActionState.FAILED;
                return;
            }

            if (cargo.CargoCount < Count)
            {
                result = String.Format("U obchodníka id={0} není požadovaných {1} jednotek zboží id={2}.", cargo.CargoOwnerId, Count, cargo.CargoId);
                State = GameActionState.FAILED;
                return;
            }

            if(player.Credit < cargo.CargoPrice * Count)
            {
                result = String.Format("Hráč id={0} nemá peníze na nákup {1} jednotek zboží id={2}.", player.PlayerId, Count, cargo.CargoId);
                State = GameActionState.FAILED;
                return;
            }

            if (!gameServer.Persistence.GetPlayerDAO().DecrasePlayersCredits(player.PlayerId, (int)(cargo.CargoPrice * Count)))
            {
                result = String.Format("Změny se nepovedlo zapsat do databáze");
                State = GameActionState.FAILED;
                return;
            }

            cargo.CargoCount = Count;
            cargo.CargoOwnerId = player.PlayerId;

            ShipLoadCargo loadingAction = new ShipLoadCargo();


            Object[] args = { StarSystemName, PlanetName, BuyerShipID, CargoLoadEntityID, Count, ActionArgs[4].ToString() };
            loadingAction.ActionArgs = args;
            loadingAction.PlayerId = PlayerId;
            gameServer.Game.PerformAction(loadingAction);
            
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
            Count = Convert.ToInt32(ActionArgs[3]);
            BuyingPlace = gameServer.Persistence.GetCargoLoadDao(ActionArgs[4].ToString());
            BuyerShipID = Convert.ToInt32(ActionArgs[5]); 
           
        }

    }

   
}
