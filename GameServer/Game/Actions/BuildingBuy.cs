﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;
using SpaceTraffic.Engine;

namespace SpaceTraffic.Game.Actions.Buildings
{
    class BuildingBuy : IGameAction
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
            int landId = Convert.ToInt32(ActionArgs.ElementAt(0));
            string type = ActionArgs.ElementAt(1).ToString();
            //Tries if the integer arguments are really integers and in range (for browsers that don't support range input correctly)
            int temp;
            for (int i = 2; i < 6; i++)
            {
                if (int.TryParse(ActionArgs.ElementAt(i).ToString(), out temp))
                {
                    if ((i < 4 && (temp < 1 || temp > 3)) || (i > 3 && (temp < 0 || temp > 7))) { return; }
                }
                else { return; }
            }
            int width = Convert.ToInt32(ActionArgs.ElementAt(2));
            int height = Convert.ToInt32(ActionArgs.ElementAt(3));
            int x = Convert.ToInt32(ActionArgs.ElementAt(4));
            int y = Convert.ToInt32(ActionArgs.ElementAt(5));
            Player player = gameServer.World.GetPlayer(PlayerId).PlayerDao;
            /**Creates a persistent land if there is none. 
             * TOCHANGE when persistent informationn about player's current base is solved.
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
                foreach (SpaceTraffic.Game.Land temp2 in player.Lands)
                {
                    if (temp2.LandDAO.BaseId == player.currentBase.BaseId) { land = temp2; }
                }
            }
            string bought = land.BuyBuilding(x, y, width, height, type, player.Credit);
            int cost;
            if (int.TryParse(bought, out cost))
            {
                player.Credit -= cost;
            }
        }
    }
}