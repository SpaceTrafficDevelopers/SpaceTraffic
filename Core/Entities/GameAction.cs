using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// Entity which represents a single player action in the game action queue.
    /// 
    /// Used to persist the game state between GameServer runs.
    /// </summary>
    public class GameAction
    {
        /// <summary>
        /// Position in the game action queue.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Assigned ID of the action.
        /// </summary>
        public int ActionCode { get; set; }

        /// <summary>
        /// Type of the action.
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// Action state.
        /// 
        /// This would be better implemented as an enum, but Entity Framework 4 does not
        /// support mapping of enum values.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// ID of player who initiated the action.
        /// 
        /// Could be an reference to actual <see cref="Player"/> entity, but it does not bring
        /// any benefit sice the game actions holds only the ID and the game action queue
        /// is always stored and restored in bulk for all players.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Serialized binary representation of the action arguments.
        /// </summary>
        public byte[] ActionArgs { get; set; }
    }
}
