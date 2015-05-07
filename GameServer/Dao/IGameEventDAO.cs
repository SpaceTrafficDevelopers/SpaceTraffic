using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Operations with persisted game events.
    /// </summary>
    public interface IGameEventDAO
    {
        /// <summary>
        /// Returns ordered list of all events in the persistence store.
        /// </summary>
        /// <returns>List of all actions ordered by their planned date.</returns>
        List<GameEvent> GetAllEvents();

        /// <summary>
        /// Preforms a bulk-insert of the given events to the persistence store.
        /// </summary>
        /// <param name="gameActions">List of game events to persist.</param>
        void InsertEvents(IEnumerable<GameEvent> gameActions);

        /// <summary>
        /// Removes all persisted events.
        /// </summary>
        void RemoveAllEvents();
    }
}
