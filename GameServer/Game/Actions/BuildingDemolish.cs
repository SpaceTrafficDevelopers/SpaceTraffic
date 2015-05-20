using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;
using SpaceTraffic.Engine;

namespace SpaceTraffic.Game.Actions
{
    class BuildingDemolish : IGameAction
    {
        public object[] ActionArgs { get; set; }

        public GameActionState State
        {
            get { throw new NotImplementedException(); }
            set { }
        }

        public int PlayerId { get; set; }

        public int ActionCode
        {
            get { throw new NotImplementedException(); }
            set { }
        }

        public object Result
        {
            get { throw new NotImplementedException(); }
            set { }
        }

        public void Perform(IGameServer gameServer)
        {
            Player player = gameServer.World.GetPlayer(PlayerId).PlayerDao;
            int landId = Convert.ToInt32(ActionArgs.ElementAt(0));
            int buildingId;
            //If no building is selected, terminate
            if (ActionArgs.ElementAt(1).ToString().Equals("Default"))
            {
                return;
            }
            buildingId = Convert.ToInt32(ActionArgs.ElementAt(1));
            /**Creates a persistent land if there is none. 
             * TOCHANGE when the saving of persistent information about player's currently visited is solved.
             * picks the current land when there are lands created.
            */
            Land land = null;
            if (!(player.Lands != null))
            {
                player.currentBase = new SpaceTraffic.Entities.Base();
                player.currentBase.BaseId = 999;
                player.Lands = new Collection<SpaceTraffic.Game.Land>();
                land = new SpaceTraffic.Game.Land();
                land.LandDAO.BaseId = player.currentBase.BaseId;
                land.LandDAO.LandId = player.Lands.Count;
                player.Lands.Add(land);
            }
            else
            {
                foreach (SpaceTraffic.Game.Land temp in player.Lands)
                {
                    if (temp.LandDAO.BaseId == player.currentBase.BaseId) { land = temp; }
                }
            }
            land.demolishBuilding(buildingId);
        }
    }
}