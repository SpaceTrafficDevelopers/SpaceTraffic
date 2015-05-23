using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class PathPlanEntityDAO : AbstractDAO, IPathPlanEntityDAO
    {

        public List<Entities.PathPlanEntity> GetPathPlans()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.PathPlan.ToList<PathPlanEntity>();
            }
        }

        public Entities.PathPlanEntity GetPathPlanById(int planID)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.PathPlan.FirstOrDefault(x => x.PathPlanId.Equals(planID));
            }
        }

        public int InsertPathPlan(Entities.PathPlanEntity plan)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add base to context
                    var planDB = contextDB.PathPlan.Add(plan);
                    // save context to database
                    contextDB.SaveChanges();

                    return planDB.PathPlanId;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public bool RemovePathPlan(int planID)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var plan = contextDB.PathPlan.FirstOrDefault(x => x.PathPlanId.Equals(planID));
                    // remove base to context
                    contextDB.PathPlan.Remove(plan);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool UpdatePathPlanById(Entities.PathPlanEntity plan)
        {
            using (var contextDB = CreateContext()) 
            { 
                try
                {
                    var planDB = contextDB.PathPlan.FirstOrDefault(x => x.PathPlanId.Equals(plan.PathPlanId));
                    
                    planDB.IsPlanned = plan.IsPlanned;
                    planDB.IsCycled = plan.IsCycled;
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
           }
        }
    }
}
