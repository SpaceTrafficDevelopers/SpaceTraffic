using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Dao
{
    public interface IPathPlanEntityDAO
    {

        List<PathPlanEntity> GetPathPlans();

        PathPlanEntity GetPathPlanById(int planID);

        int InsertPathPlan(PathPlanEntity plan);

        bool RemovePathPlan(int planID);
        
        bool UpdatePathPlanById(PathPlanEntity plan);

    }
}
