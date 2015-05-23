using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Provides operations with persisted game actions.
    /// </summary>
    class GameActionDAO : AbstractDAO, IGameActionDAO
    {
        /// <summary>
        /// Returns ordered list of all actions in the persistence store.
        /// </summary>
        /// <returns>List of all actions ordered by their sequence number.</returns>
        public List<GameAction> GetAllActions()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.GameActions.OrderBy(action => action.Sequence).ToList();
            }
        }

        /// <summary>
        /// Preforms a bulk-insert of the given actions to the persistence store.
        /// </summary>
        /// <param name="gameActions">List of game actions to persist.</param>
        public void InsertActions(IEnumerable<GameAction> gameActions)
        {
            using (var contextDB = CreateContext())
            {
                foreach (var gameAction in gameActions)
                {
                    contextDB.GameActions.Add(gameAction);
                }
                contextDB.SaveChanges();
            }
        }

        /// <summary>
        /// Removes all persisted action events.
        /// </summary>
        public void RemoveAllActions()
        {
            using (var contextDB = CreateContext())
            {
                // EF does not support any batch operations:
                // http://stackoverflow.com/a/10450893
                // Therefore, to avoid loading all entities (which is NOT a good idea), we have to use an SQL command:
                contextDB.Database.ExecuteSqlCommand("DELETE FROM [GameActions]");
            }
        }
    }
}
