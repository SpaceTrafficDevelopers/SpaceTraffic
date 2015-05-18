using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Entities
{
    public class PlanAction
    {
        public int PlanActionId { get; set; }

        public int PlanItemId { get; set; }

        public int SequenceNumber { get; set; }

        public virtual PlanItemEntity PlanItem { get; set; }

        public String ActionType { get; set; }

        public byte[] GameAction { get; set; }
    }
}
