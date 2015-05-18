using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Dao
{
    public interface IPlanItemEntityDAO
    {

        List<PlanItemEntity> GetPlanItemsByPathPlanId(int pathPlanId);

        PlanItemEntity GetPlanItemById(int planItemID);

        int InsertPlanItem(PlanItemEntity planItem);

        bool RemovePlanItem(int planItemID);

        bool UpdatePlanItemById(PlanItemEntity planItem);

    }
}
