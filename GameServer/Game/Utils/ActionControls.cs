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

namespace SpaceTraffic.Game.Utils
{
    class ActionControls
    {
        public static IGameServer gameServer = GameServer.GameServer.CurrentInstance;

        public static bool hasShipEnoughFuel(int ShipID, double flightTime)
        {
            SpaceShip ship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);

            if (ship == null)
                return false;

            ship.CurrentFuelTank -= (int) (flightTime * ship.Consumption);            
            return ship.CurrentFuelTank >= 0;
        }

        public static bool isShipTooMuchDamaged(int ShipID, double flightTime)
        {
            SpaceShip ship = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);

            if (ship == null)
                return false;

            ship.DamagePercent += (int)(flightTime * ship.WearRate);
            return ship.DamagePercent > 100;
        }

        public static bool isShipReadyForTravel(int ShipID, double flightTime)
        {
            return !isShipTooMuchDamaged(ShipID, flightTime) && hasShipEnoughFuel(ShipID, flightTime);
        }

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

        public static void isShipFlying(IGameAction action, SpaceShip ship, bool shouldFly)
        {
            if (action == null || ship == null)
            {
                action.State = GameActionState.FAILED;
                return;
            }

            if (!ship.IsFlying && shouldFly)
            {
                action.Result = String.Format("Loď {0} nemůže provést akci, protože neletí", ship.SpaceShipName);
                action.State = GameActionState.FAILED;
            }
            else if(ship.IsFlying && !shouldFly)
            {
                action.Result = String.Format("Loď {0} nemůže provést akci, protože letí", ship.SpaceShipName);
                action.State = GameActionState.FAILED;
            }
        }

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

        public static void hasShipEnoughCargoSpace(IGameAction action, SpaceShip ship, ICargoLoadEntity cargo)
        {
            if (!checkSpaceShipCargos(ship, cargo))
            {

                action.Result = String.Format("Loď {0} nemá dostatek místa na naložení nákladu.", ship.SpaceShipName);
                action.State = GameActionState.FAILED;
            }
        }

        public static void checkCargoCount(IGameAction action, ICargoLoadEntity cargo, int demandedNumber)
        {
            if (cargo.CargoCount < demandedNumber)
            {
                action.Result = String.Format("Požadovaných {0} jednotek zboží id={1} není k dispozici.", demandedNumber, cargo.CargoId);
                action.State = GameActionState.FAILED;
            }
        }

        /// <summary>
        /// Check space for cargo in space ship.
        /// </summary>
        /// <param name="gameServer">game server</param>
        /// <param name="spaceShip">space ship</param>
        /// <returns>true when ship has space for cargo, otherwise fale</returns>
        public static bool checkSpaceShipCargos(SpaceShip spaceShip, ICargoLoadEntity cargo)
        {
            IGameServer gameServer = GameServer.GameServer.CurrentInstance;
            List<ICargoLoadEntity> cargoList = gameServer.Persistence.GetSpaceShipCargoDAO().GetCargoListByOwnerId(spaceShip.SpaceShipId);

            int freeSpace = spaceShip.CargoSpace;

            foreach (SpaceShipCargo ssc in cargoList)
            {
                freeSpace -= ssc.CargoCount * gameServer.Persistence.GetCargoDAO().GetCargoById(cargo.CargoId).Volume;
            }

            int demandedSpace = gameServer.Persistence.GetCargoDAO().GetCargoById(cargo.CargoId).Volume * cargo.CargoCount;

            return demandedSpace < freeSpace;
        }

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
