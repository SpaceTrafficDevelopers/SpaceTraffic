using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Provides operations with persisted game events.
    /// </summary>
    class GameEventDAO : AbstractDAO, IGameEventDAO
    {
        /// <summary>
        /// Returns ordered list of all events in the persistence store.
        /// </summary>
        /// <returns>List of all events ordered by their sequence number.</returns>
        public List<GameEvent> GetAllEvents()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.GameEvents.OrderBy(evnt => evnt.PlannedTime).ToList();
            }
        }

        /// <summary>
        /// Preforms a bulk-insert of the given events to the persistence store.
        /// </summary>
        /// <param name="gameEvents">List of game events to persist.</param>
        public void InsertEvents(IEnumerable<GameEvent> gameEvents)
        {
            using (var contextDB = CreateContext())
            {
                foreach (var gameEvent in gameEvents)
                {
                    contextDB.GameEvents.Add(gameEvent);
                }
                contextDB.SaveChanges();
            }
        }

        /// <summary>
        /// Removes all persisted events.
        /// </summary>
        public void RemoveAllEvents()
        {
            using (var contextDB = CreateContext())
            {
                // EF does not support any batch operations:
                // http://stackoverflow.com/a/10450893
                // Therefore, to avoid loading all entities (which is NOT a good idea), we have to use an SQL command:
                contextDB.Database.ExecuteSqlCommand("DELETE FROM [GameEvents]");
            }
        }
    }
}
