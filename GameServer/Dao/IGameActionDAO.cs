using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Operations with persisted game actions.
    /// </summary>
    public interface IGameActionDAO
    {
        /// <summary>
        /// Returns ordered list of all actions in the persistence store.
        /// </summary>
        /// <returns>List of all actions ordered by their sequence number.</returns>
        List<GameAction> GetAllActions();

        /// <summary>
        /// Preforms a bulk-insert of the given actions to the persistence store.
        /// </summary>
        /// <param name="gameActions">List of game actions to persist.</param>
        void InsertActions(IEnumerable<GameAction> gameActions);

        /// <summary>
        /// Removes all persisted actions.
        /// </summary>
        void RemoveAllActions();
    }
}
