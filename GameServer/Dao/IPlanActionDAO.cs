using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface IPlanActionDAO
    {
        List<PlanAction> GetPlanActionsByPlanItemId(int planItemId);

        PlanAction GetPlanActionById(int planActionID);

        bool InsertPlanAction(PlanAction planAction);

        bool RemovePlanAction(int planActionID);

        bool UpdatePlanActionById(PlanAction planAction);
    }
}
