using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class SpaceShipDAO : AbstractDAO, ISpaceShipDAO
    {
        public List<SpaceShip> GetSpaceShips()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.SpaceShips.ToList<SpaceShip>();
            }
        }

        public List<SpaceShip> GetSpaceShipsByPlayer(int playerId)
        {
            using (var contextDB = CreateContext())
            {
                return (from x in contextDB.SpaceShips
                        where x.PlayerId.Equals(playerId)
                        select x).ToList<SpaceShip>();
            }
        }

        public SpaceShip GetSpaceShipById(int spaceShipId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.SpaceShips.FirstOrDefault(x => x.SpaceShipId.Equals(spaceShipId));
            }
        }

        public bool InsertSpaceShip(SpaceShip spaceShip)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add space ship to context
                    contextDB.SpaceShips.Add(spaceShip);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveSpaceShipById(int spaceShipId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var spaceShipTab = contextDB.SpaceShips.FirstOrDefault(x => x.SpaceShipId.Equals(spaceShipId));
                    // remove space ship to context
                    contextDB.SpaceShips.Remove(spaceShipTab);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool UpdateSpaceShipById(SpaceShip spaceShip)
        {
            using (var contextDB = CreateContext())
            try
            {
                var spaceShipTab = contextDB.SpaceShips.FirstOrDefault(x => x.SpaceShipId.Equals(spaceShip.SpaceShipId));
                spaceShipTab.DamagePercent = spaceShip.DamagePercent;
                spaceShipTab.UserCode = spaceShip.UserCode;
                spaceShipTab.TimeOfArrival = spaceShip.TimeOfArrival;
                spaceShipTab.IsFlying = spaceShip.IsFlying;
                spaceShipTab.SpaceShipName = spaceShip.SpaceShipName;
                spaceShipTab.SpaceShipModel = spaceShip.SpaceShipModel;
                spaceShipTab.DockedAtBaseId = spaceShip.DockedAtBaseId;
                spaceShipTab.CurrentStarSystem = spaceShip.CurrentStarSystem;
                spaceShipTab.FuelTank = spaceShip.FuelTank;
                spaceShipTab.CurrentFuelTank = spaceShip.CurrentFuelTank;
                spaceShipTab.PlayerId = spaceShip.PlayerId;
                // save context to database
                contextDB.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
