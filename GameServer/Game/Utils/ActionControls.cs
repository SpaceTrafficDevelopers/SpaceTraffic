using SpaceTraffic.Engine;
using SpaceTraffic.Entities;
using SpaceTraffic.Services.Contracts;
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

namespace SpaceTraffic.Game.Utils
{
    /// <summary>
    /// Controls for ship plannable actions 
    /// </summary>
    class ActionControls
    {
        /// <summary>
        /// Instance of game server.
        /// </summary>
        public static IGameServer gameServer = GameServer.GameServer.CurrentInstance;

        /// <summary>
        /// Control if spaceship has fuel for next flight.
        /// </summary>
        /// <param name="ShipID">Indentification number of spaceship.</param>
        /// <param name="flightTime">Time of flight.</param>
        /// <returns>Value if spaceship has fuel for flight or not.</returns>
        public static bool hasShipEnoughFuel(int ShipID, double flightTime)
        {
            SpaceShip ship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);

            if (ship == null)
                return false;

            ship.CurrentFuelTank -= (int) (flightTime * ship.Consumption);            
            return ship.CurrentFuelTank >= 0;
        }

        /// <summary>
        /// Control if spaceship is ready for next flight or too damaged.
        /// </summary>
        /// <param name="ShipID">Identification number of spaceship.</param>
        /// <param name="flightTime">Time of flight.</param>
        /// <returns></returns>
        public static bool isShipTooMuchDamaged(int ShipID, double flightTime)
        {
            SpaceShip ship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);

            if (ship == null)
                return false;

            ship.DamagePercent += (int)(flightTime * ship.WearRate);
            return ship.DamagePercent > 100;
        }

        /// <summary>
        /// Control if spaceship is ready for travel. Control damaged of spaceship and fuel for next flight.
        /// </summary>
        /// <param name="ShipID">Identification number of spaceship.</param>
        /// <param name="flightTime">Time of flight.</param>
        /// <returns></returns>
        public static bool isShipReadyForTravel(int ShipID, double flightTime)
        {
            return !isShipTooMuchDamaged(ShipID, flightTime) && hasShipEnoughFuel(ShipID, flightTime);
        }

        /// <summary>
        /// Control if spaceship belong to player. If not action will have state FAILED.
        /// </summary>
        /// <param name="action">Action which player want to do.</param>
        /// <param name="ship">Instance od spaceship.</param>
        /// <param name="player">Instance od player.</param>
        public static void shipOwnerControl(IGameAction action, SpaceShip ship, Player player)
        {
            if (action == null || ship == null || player == null)
            {
                action.State = GameActionState.FAILED;
                return;
            }

            if (ship.PlayerId != player.PlayerId)
            {
                action.Result = String.Format("Loď {0} nepatří hráči {1}", ship.SpaceShipName, player.PlayerName);
                action.State = GameActionState.FAILED;
            }
        }

        /// <summary>
        /// Control if spaceship is flying or docked at base. If spaceship should not fly, action will have state FAILED.
        /// </summary>
        /// <param name="action">Action which player want to do.</param>
        /// <param name="ship">Instance of spaceship.</param>
        /// <param name="shouldFly">Value if spaceship shoul fly or not.</param>
        public static void isShipFlying(IGameAction action, SpaceShip ship, bool shouldFly)
        {
            if (action == null || ship == null)
            {
                action.State = GameActionState.FAILED;
                return;
            }

            if ((!ship.IsFlying && ship.IsAvailable) && shouldFly)
            {
                action.Result = String.Format("Loď {0} nemůže provést akci, protože neletí", ship.SpaceShipName);
                action.State = GameActionState.FAILED;
            }
            else if((ship.IsFlying || !ship.IsAvailable) && !shouldFly)
            {
                action.Result = String.Format("Loď {0} nemůže provést akci, protože letí", ship.SpaceShipName);
                action.State = GameActionState.FAILED;
            }
        }

        /// <summary>
        /// Control if planet has base or not. If planet has no base, spaceship should not land here and action will have state FAILED.
        /// </summary>
        /// <param name="action">Action which player wnat to do.</param>
        /// <param name="planet">Instance of planet.</param>
        /// <returns>Return base identification number, if planet has no base, return -1.</returns>
        public static int checkBaseAtPlanet(IGameAction action, Planet planet)
        {
            if (action == null || planet == null)
            {
                action.State = GameActionState.FAILED;
                return -1;
            }

            Entities.Base dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseByPlanetFullName(planet.Location);

            if (dockedBase == null)
            {
                action.Result = String.Format("Na planetě {0} není žádná základna.", planet.Name);
                action.State = GameActionState.FAILED;
                return -1;
            }

            return dockedBase.BaseId;
        }

        /// <summary>
        /// Control if array of objects is not null.
        /// </summary>
        /// <param name="action">Action which player want to do.</param>
        /// <param name="objects">Array of objects.</param>
        /// <returns>Value if array of objects is not null.</returns>
        public static bool checkObjects(IGameAction action, Object [] objects)
        {
            foreach(Object obj in objects)
            {
                if (obj == null)
                {
                    action.Result = String.Format("Nastala chyba při vyhledávání položek");
                    action.State = GameActionState.FAILED;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Control if spaceship is docked at base on planet or not. Set action state to FAILED, if is not docket at base on planet.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="ship">Instance of spaceship.</param>
        /// <param name="planet">Instance of planet.</param>
        public static void shipDockedAtBase(IGameAction action, SpaceShip ship, Planet planet)
        {
            Entities.Base dockedBase = null;

            if (ship.DockedAtBaseId != null)
                dockedBase = gameServer.Persistence.GetBaseDAO().GetBaseById((int)ship.DockedAtBaseId);

            if (dockedBase == null || !dockedBase.Planet.Equals(planet.Location))
            {
                action.Result = String.Format("Loď {0} není zadokována na planetě {1}.", ship.SpaceShipName, planet.Name);
                action.State = GameActionState.FAILED;
            }
        }

        /// <summary>
        /// Control if spaceship has cargo space for cargo. If not set state of action to FAILED.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="ship">Instance of spaceship.</param>
        /// <param name="cargo">Cargo load entity.</param>
        public static void hasShipEnoughCargoSpace(IGameAction action, SpaceShip ship, ICargoLoadEntity cargo, int count)
        {
			ICargoService cargoService = gameServer.Game.GetCargoService();
            if (!cargoService.SpaceShipHasCargoSpace(ship.SpaceShipId, cargo.CargoLoadEntityId, count))
            {

                action.Result = String.Format("Loď {0} nemá dostatek místa na naložení nákladu.", ship.SpaceShipName);
                action.State = GameActionState.FAILED;
            }
        }

        /// <summary>
        /// Check count of cargo.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="cargo">Cargo load entity.</param>
        /// <param name="demandedNumber">Demanded number of cargo.</param>
        public static void checkCargoCount(IGameAction action, ICargoLoadEntity cargo, int demandedNumber)
        {
            if (cargo.CargoCount < demandedNumber)
            {
                action.Result = String.Format("Požadovaných {0} jednotek zboží id={1} není k dispozici.", demandedNumber, cargo.CargoId);
                action.State = GameActionState.FAILED;
            }
        }

       

        /// <summary>
        /// Check player credit for purchase.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="player">Instance of player.</param>
        /// <param name="price">Price of purchase.</param>
        public static void checkPlayersCredit(IGameAction action, Player player, int price)
        {
            if(action == null || player == null)
            {
                action.State = GameActionState.FAILED;
                return;
            }

            if (player.Credit < price)
            {
                action.Result = String.Format("Hráč {0} nemá dostatek peněz na zaplacení {1}", player.PlayerName, price);
                action.State = GameActionState.FAILED;
            }
        }
    }
}
