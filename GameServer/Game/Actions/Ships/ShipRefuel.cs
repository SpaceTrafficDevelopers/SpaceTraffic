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
    /// Action for refuel a fuel to spaceship.
    /// </summary>
    [Serializable]
    class ShipRefuel : IPlannableAction
    {
        /// <summary>
        /// Time of refuel one liter of fuel.
        /// </summary>
        private static readonly double LITER_REFUEL_TIME = 0.2;

        public object Result { get; set; }
        public GameActionState State { get; set; }

        public int PlayerId { get; set; }

        public int ActionCode { get; set; }

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
        /// Identification number of spaceship
        /// </summary>
        public int ShipID { get; set; }

        /// <summary>
        /// Number of liters to refuel
        /// </summary>
        public int Liters { get; set; }

        /// <summary>
        /// Price of one liter of fuel
        /// </summary>
        public int PricePerLiter { get; set; }

        /// <summary>
        /// Value if refueling is finished or not
        /// </summary>
        private bool RefuelingFinished { get; set; }

        /// <summary>
        /// Duration of action.
        /// </summary>
        public double Duration
        {
            get
            {
                getArgumentsFromActionArgs();
                return Liters * LITER_REFUEL_TIME;
            }
        }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            Result = "Loď tankuje";

            getArgumentsFromActionArgs();
            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerWithIncludes(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];

            if (!ActionControls.checkObjects(this, new Object[] { player, spaceShip, planet}))
                return;

            ActionControls.shipDockedAtBase(this, spaceShip, planet);
            ActionControls.shipOwnerControl(this, spaceShip, player);
            ActionControls.checkPlayersCredit(this, player, Liters * PricePerLiter);
			

            if (State == GameActionState.FAILED)
                return;

       


            if (RefuelingFinished)
            {
				spaceShip.IsAvailable = true;
				spaceShip.StateText = SpaceShip.StateTextDefault;
                spaceShip.CurrentFuelTank = Math.Min(spaceShip.FuelTank, spaceShip.CurrentFuelTank + Liters);
				// log the ship buy action to statistics
				gameServer.Statistics.IncrementStatisticItem(player, "fuelTank", Liters);

                if (!gameServer.Persistence.GetPlayerDAO().DecrasePlayersCredits(PlayerId, Liters * PricePerLiter))
                {
                    Result = String.Format("Změny se nepovedlo zapsat do databáze");
                    State = GameActionState.FAILED;
                    return;
                }

                //update
				if (!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShipById(spaceShip))
				{
					Result = String.Format("Změny se nepovedlo zapsat do databáze");
					State = GameActionState.FAILED;
					return;
				}

                State = GameActionState.FINISHED;
            }
            else
            {
				spaceShip.IsAvailable = false;
				spaceShip.StateText = "Tankuje...";
				if (!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShipById(spaceShip))
				{
					Result = String.Format("Změny se nepovedlo zapsat do databáze");
					State = GameActionState.FAILED;
					return;
				}
                RefuelingFinished = true;
                gameServer.Game.PlanEvent(this, gameServer.Game.currentGameTime.Value.AddSeconds(Duration));
            }

        }

        private void getArgumentsFromActionArgs()
        {
            if (ActionArgs != null && ActionArgs.Count() == 5)
            {
                StarSystemName = ActionArgs[0].ToString();
                PlanetName = ActionArgs[1].ToString();
                ShipID = Convert.ToInt32(ActionArgs[2]);
                Liters = Math.Abs(Convert.ToInt32(ActionArgs[3]));
                PricePerLiter = Math.Abs(Convert.ToInt32(ActionArgs[4]));                
            }
        }
    }
}
