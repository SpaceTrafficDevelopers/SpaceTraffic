using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SpaceTraffic.GameServer;

namespace SpaceTraffic.Engine
{
    /// <summary>
    /// Interface for service, which is able to persist and restore the state of the game.
    /// 
    /// Current state of the game is represented by a sequence of <see cref="IGameAction"/>
    /// and <see cref="IGameEvent"/> objects.
    /// </summary>
    public interface IGameStateManager
    {
        /// <summary>
        /// Stores the given game action queue in the database. Previous snapshot of the queue (if any) is discarded.
        /// </summary>
        /// <param name="actionQueue">Current snapshot of the game action queue. Can be the queue iself,
        /// as the <see cref="ConcurrentQueue{T}"/> provides current snapshot enumerator.</param>
        /// <seealso cref="GameManager.gameActionQueue"/>
        void PersistActions(IEnumerable<IGameAction> actionQueue);

        /// <summary>
        /// Stores the given game event queue in the database. Previous snapshot of the queue (if any) is discarded.
        /// </summary>
        /// <param name="gameEventQueue">Current snapshot of the game event queue.</param>
        /// <seealso cref="GameManager.gameActionQueue"/>
        void PersistEvents(IEnumerable<IGameEvent> gameEventQueue);

        /// <summary>
        /// Loads the snapshot of the game action queue from the database.
        /// </summary>
        /// <returns>Collection of the game actions.</returns>
        IEnumerable<IGameAction> RestoreActions();

        /// <summary>
        /// Loads the snapshot of the game event queue from the database.
        /// </summary>
        /// <returns>Collection of the game events.</returns>
        IEnumerable<IGameEvent> RestoreEvents();
    }
}
