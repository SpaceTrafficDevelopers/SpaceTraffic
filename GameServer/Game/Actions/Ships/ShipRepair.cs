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
    /// Action for repairing spaceship if spaceship is damaged.
    /// </summary>
    [Serializable]
    class ShipRepair: IPlannableAction
    {
        /// <summary>
        /// Percent repair time
        /// </summary>
        private static readonly int PERCENT_REPAIR_TIME = 3;

        public object Result{ get; set; }
      
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
        /// Percentage of repair.
        /// </summary>
        public int RepairPercentage { get; set; }

        /// <summary>
        /// Tax of percent of repair.
        /// </summary>
        public int PercentRepairTax {get; set; }

        /// <summary>
        /// Value if repair is finished or not.
        /// </summary>
        private bool RepairFinished { get; set; }

        /// <summary>
        /// Deuration of action.
        /// </summary>
        public double Duration
        {
            get
            {
                getArgumentsFromActionArgs();
                return RepairPercentage * PERCENT_REPAIR_TIME;
            }
        }

        public void Perform(IGameServer gameServer)
        {
            State = GameActionState.PLANNED;
            Result = "Loď je v opravě";
            getArgumentsFromActionArgs();

            Player player = gameServer.Persistence.GetPlayerDAO().GetPlayerWithIncludes(PlayerId);
            SpaceShip spaceShip = gameServer.Persistence.GetSpaceShipDAO().GetSpaceShipById(ShipID);
            Planet planet = gameServer.World.Map[StarSystemName].Planets[PlanetName];

            if(!ActionControls.checkObjects(this, new object[] { player, spaceShip, planet }))
                return;

            ActionControls.shipDockedAtBase(this, spaceShip, planet);
            ActionControls.checkPlayersCredit(this, player, RepairPercentage * PercentRepairTax);
            ActionControls.shipOwnerControl(this, spaceShip, player);

            if (State == GameActionState.FAILED)
                return;
           

            if (RepairFinished)
            {
				spaceShip.DamagePercent = Math.Max(0, spaceShip.DamagePercent - RepairPercentage);
                // log the ship buy action to statistics
                
                gameServer.Statistics.IncrementStatisticItem(player, "percentageRepaired", RepairPercentage);

                if (!gameServer.Persistence.GetPlayerDAO().DecrasePlayersCredits(PlayerId, RepairPercentage * PercentRepairTax))
                {
                    Result = String.Format("Změny se nepovedlo zapsat do databáze");
                    State = GameActionState.FAILED;
                    return;
                }
				spaceShip.IsAvailable = true;
				spaceShip.StateText = SpaceShip.StateTextDefault;
                
                if (!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShip(spaceShip))
                {
                    Result = String.Format("Změny se nepovedlo zapsat do databáze");
                    State = GameActionState.FAILED;
                    return;
                }

                State = GameActionState.FINISHED;
            }
            else
            {
				if (!spaceShip.IsAvailable)
				{
					Result = "Loď nebyla dostupná.";
					return;
				}
				spaceShip.IsAvailable = false;
				spaceShip.StateText = "Loď se opravuje...";
                RepairFinished = true;

				if (!gameServer.Persistence.GetSpaceShipDAO().UpdateSpaceShip(spaceShip))
				{
					Result = String.Format("Změny se nepovedlo zapsat do databáze");
					State = GameActionState.FAILED;
					return;
				}

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
                RepairPercentage = Math.Abs(Convert.ToInt32(ActionArgs[3]));
                PercentRepairTax = Math.Abs(Convert.ToInt32(ActionArgs[4]));
            }
        }
    }
}
