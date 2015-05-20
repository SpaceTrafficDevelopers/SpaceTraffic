using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;
using System.Collections.ObjectModel;

namespace SpaceTraffic.Game
{
    public class Building : IGameObject
    {
        public int Id { get { return BuildingDAO.BuildingId; } set { BuildingDAO.BuildingId = value; } }
        public Entities.Building BuildingDAO { get; set; }
        public Building(Entities.Building Building)
        {
            BuildingDAO = Building;
        }
        /** Deprecated. 
         *  Returns array of coordinates of coordinates of the building's tiles.
         *  Is now handled in the _Building view.
         */
        public int[,] getTileCoords()
        {
            int[,] coords = new int[BuildingDAO.Size[0] * BuildingDAO.Size[1], 2];
            for (int i = 0; i < BuildingDAO.Size[0]; i++)
            {
                for (int j = 0; j < BuildingDAO.Size[1]; j++)
                {
                    coords[i * BuildingDAO.Size[0] + j, 0] = BuildingDAO.Location[0] + j;
                    coords[i * BuildingDAO.Size[0] + j, 1] = BuildingDAO.Location[1] + i;
                }
            }
            return coords;
        }

        private System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();

        public System.Threading.ReaderWriterLockSlim Lock
        {
            get { return this._lock; }
        }
    }
}