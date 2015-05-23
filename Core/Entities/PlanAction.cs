using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    /// <summary>
    /// Class representing planned actions in DB
    /// </summary>
    public class PlanAction
    {
        /// <summary>
        /// Identification number of plan action
        /// </summary>
        public int PlanActionId { get; set; }

        /// <summary>
        /// Identification number of plan item
        /// </summary>
        public int PlanItemId { get; set; }

        /// <summary>
        /// Sequence number for order of elements in path plan
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Instance of path plan item entity
        /// </summary>
        public virtual PlanItemEntity PlanItem { get; set; }

        /// <summary>
        /// Type of action
        /// </summary>
        public String ActionType { get; set; }

        /// <summary>
        /// Serializable action
        /// </summary>
        public byte[] GameAction { get; set; }
    }
}
