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
using SpaceTraffic.Entities;
using System.Data.Entity;

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

		public IList<SpaceShip> GetPlayersShipsAtBase(int playerId, int baseId) {
			using (var contextDB = CreateContext())
			{
				return (from x in contextDB.SpaceShips
						where x.PlayerId.Equals(playerId)
						where x.DockedAtBaseId != null
						where x.DockedAtBaseId == baseId
						select x).ToList<SpaceShip>();
			}
		}

		public SpaceShip GetSpaceShipById(int spaceShipId)
		{
			using (var contextDB = CreateContext())
			{
				var spaceship = contextDB.SpaceShips;
				return spaceship.FirstOrDefault(x => x.SpaceShipId.Equals(spaceShipId));
			}
		}

		public SpaceShip GetDetailedSpaceShipById(int spaceShipId)
		{
			using (var contextDB = CreateContext())
			{
				
				var spaceship = contextDB.SpaceShips.Include("Base").Include("SpaceShipsCargos").FirstOrDefault(x => x.SpaceShipId.Equals(spaceShipId));
				spaceship.Base.SpaceShips = null;
				foreach (var cargo in spaceship.SpaceShipsCargos)
				{
					cargo.SpaceShip = null;
					cargo.Cargo = contextDB.Cargos.FirstOrDefault(a => a.CargoId.Equals(cargo.CargoId));
					cargo.Cargo.SpaceShipsCargos = null;
				}
				return spaceship;
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
				catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
				{
					Exception raise = dbEx;
					foreach (var validationErrors in dbEx.EntityValidationErrors)
					{
						foreach (var validationError in validationErrors.ValidationErrors)
						{
							string message = string.Format("{0}:{1}",
								validationErrors.Entry.Entity.ToString(),
								validationError.ErrorMessage);
							// raise a new exception nesting
							// the current instance as InnerException
							raise = new InvalidOperationException(message, raise);
						}
					}
					throw raise;
				}
				catch (Exception) {
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
				spaceShipTab.IsAvailable = spaceShip.IsAvailable;
				spaceShipTab.StateText = spaceShip.StateText;
				spaceShipTab.DockedAtBaseId = spaceShip.DockedAtBaseId;
				spaceShipTab.CurrentStarSystem = spaceShip.CurrentStarSystem;
				spaceShipTab.FuelTank = spaceShip.FuelTank;
				spaceShipTab.CurrentFuelTank = spaceShip.CurrentFuelTank;
				spaceShipTab.PlayerId = spaceShip.PlayerId;
                spaceShipTab.CargoSpace = spaceShip.CargoSpace;
                spaceShipTab.Consumption = spaceShip.Consumption;
                spaceShipTab.WearRate = spaceShip.WearRate;
                spaceShipTab.MaxSpeed = spaceShip.MaxSpeed;

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
