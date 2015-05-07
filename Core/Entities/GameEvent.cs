using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class GameEvent
    {
        /// <summary>
        /// ID of the event entity, since the event does not contain its own unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Time to which is the event planned.
        /// </summary>
        public DateTime PlannedTime { get; set; }

        /// <summary>
        /// Type of the event.
        /// </summary>
        public String EventType { get; set; }

        /// <summary>
        /// Assigned ID of the action.
        /// </summary>
        public int ActionCode { get; set; }

        /// <summary>
        /// Type of the action.
        /// </summary>
        public String ActionType { get; set; }

        /// <summary>
        /// State of the bound action.
        /// 
        /// This would be better implemented as an enum, but Entity Framework 4 does not
        /// support mapping of enum values.
        /// </summary>
        public int ActionState { get; set; }

        /// <summary>
        /// ID of player who is responsible for the bound action.
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
