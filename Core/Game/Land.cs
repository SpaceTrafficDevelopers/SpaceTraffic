using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;
using System.Collections.ObjectModel;

namespace SpaceTraffic.Game
{
  public class Land : IGameObject
    {
        //Number of tiles in each row/column
        public const int num = 8;
        //Cost of an empty tile
        public const int costPerLandTile = 50;
        //Cost of a building per tile
        public const int costPerBuildingTile = 100;
        public int Id { get { return LandDAO.LandId; } set { LandDAO.LandId = value; } }
        public Entities.Land LandDAO { get; set; }
        public Land()
        {
            
            LandDAO = new Entities.Land();
            LandDAO.Tiles = new Collection<Entities.Tile>();
            //initialize the tiles of the land
            for (int i = 0; i < num * num; i++)
            {
                Tile tmpTile = new Tile();
                tmpTile.occupied = false;
                tmpTile.purchased = false;
                LandDAO.Tiles.Add(tmpTile);
            }
            LandDAO.Buildings = new Collection<Building>();
            //temporary to check that the relevant functions work
            this.BuySellTiles(5, 5, 5555);
            this.BuyBuilding(0, 0, 2, 2, "Warehouse", 5555);
            this.BuyBuilding(3, 3, 1, 1, "Factory", 5555);
            this.BuyBuilding(3, 4, 1, 1, "Factory", 5555);
            this.demolishBuilding(this.LandDAO.Buildings.Last().BuildingDAO.BuildingId);
        }
        /** Changes the size of the purchased land.
        * Returns the cost of the land or the error that occured.
        */
        public String BuySellTiles(int sizeX, int sizeY, int playerCredit)
        {
            Tile cur = null;
            int cost = 0;
            //Checks if tiles aren't occupied (if selling) and calculates the potential cost of the change
            for (int i = 0; i < 64; i++){
                cur=LandDAO.Tiles.ElementAt(i);
                if (cur.occupied && (i % num >= sizeX || i / num >= sizeY)) { return "occupied"; }
                if (cur.purchased && (i % num >= sizeX || i / num >= sizeY)) { cost -= costPerLandTile; }
                if (!cur.purchased && (i % num < sizeX && i / num < sizeY)) { cost += costPerLandTile; }
            }
            //return error if player doesn't have enough credits
            if (cost > playerCredit)
            {
                return "noMoney";
            }
            //Applies the change
            for (int i = 0; i < num * num; i++)
            {
                cur=LandDAO.Tiles.ElementAt(i);
                if (cur.purchased && (i % num >= sizeX || i / num >= sizeY)) { cur.purchased = false; }
                if (!cur.purchased && (i % num < sizeX && i / num < sizeY)) { cur.purchased = true; }
            }
            //return the cost of the change if everything is alright
            return ""+cost;
        }
        /** Creates a building in this lannd with parameters specified in arguments.
         * Returns the cost of  the building if the land is created without problems.
         */
        public string BuyBuilding(int x, int y, int sizeX, int sizeY, string type, int playerCredit)
        {
            Tile cur = null;
            //Calculate the cost of the building, return an error in case of insufficient credits
            int cost = costPerBuildingTile * sizeX * sizeY;
            if (cost>playerCredit){
                return "noMoney";
            }
            //Calculate if size of the building doesn't exceed the maximum size of the land or if the land isn't already occupied
            if (x + sizeX > num || y + sizeY > num) { return "outOfBounds"; }
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    cur = LandDAO.Tiles.ElementAt(i + x + (j + y) * num);
                    if (!cur.purchased || cur.occupied) { return "invalidLocation"; }
                }
            }
            //Change the tiles to occupied
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    cur = LandDAO.Tiles.ElementAt(i + x + (j + y) * num);
                    cur.occupied = true;
                }
            }
            //Create the actual building
            Entities.Building tmpBuilding = new Entities.Building();
            int[] tmp={sizeX,sizeY};
            tmpBuilding.Size=tmp;
            int[] tmp2 = { x, y };
            tmpBuilding.Location = tmp2;
            tmpBuilding.Type=type;
            //If there are already buildings, assigns a higher id, if not, assigns 0
            if (this.LandDAO.Buildings.Count > 0)
            {
                tmpBuilding.BuildingId = this.LandDAO.Buildings.Last().BuildingDAO.BuildingId;
            }
            else { tmpBuilding.BuildingId = 0; }
            Building build = new Building(tmpBuilding);
            LandDAO.Buildings.Add(build);
            //Return the cost of the building
            return ""+cost;
        }
        //Very straightforward remmoval of a building
        public void demolishBuilding(int BuildingId)
        {
            Building build=null;
            foreach (Building b in LandDAO.Buildings)
            {
                if (b.BuildingDAO.BuildingId == BuildingId) { build = b; }
            }
            //breaks for invalid BuildingID
            if (!(build != null)) { return; }
            for (int i = 0; i < build.BuildingDAO.Size[0]; i++)
            {
                for (int j = 0; j < build.BuildingDAO.Size[1]; j++)
                {
                    this.LandDAO.Tiles.ElementAt(i * num + j).occupied = false;
                }
            }
            LandDAO.Buildings.Remove(build);
        }

        private System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();

        public System.Threading.ReaderWriterLockSlim Lock
        {
            get { return this._lock; }
        }
    }
}
