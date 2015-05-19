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

        
    }
}
