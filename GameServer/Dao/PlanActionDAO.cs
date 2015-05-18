using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class PlanActionDAO : AbstractDAO, IPlanActionDAO
    {
        public List<Entities.PlanAction> GetPlanActionsByPlanItemId(int planItemId)
        {
            using (var contextDB = CreateContext())
            {
                var actions = (from x in contextDB.PlanAction
                               where x.PlanItemId.Equals(planItemId)
                               select x);

                if (actions == null)
                    return null;

                return actions.OrderBy(x => x.SequenceNumber).ToList<PlanAction>(); ;
            }
        }

        public Entities.PlanAction GetPlanActionById(int planActionID)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.PlanAction.FirstOrDefault(x => x.PlanActionId.Equals(planActionID));
            }
        }

        public bool InsertPlanAction(Entities.PlanAction planAction)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add base to context
                    contextDB.PlanAction.Add(planAction);
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

        public bool RemovePlanAction(int planActionID)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var planAtion = contextDB.PlanAction.FirstOrDefault(x => x.PlanActionId.Equals(planActionID));
                    // remove base to context
                    contextDB.PlanAction.Remove(planAtion);
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

        public bool UpdatePlanActionById(Entities.PlanAction planAction)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var planActionDB = contextDB.PlanAction.FirstOrDefault(x => x.PlanActionId.Equals(planAction.PlanActionId));

                    planActionDB.ActionType = planAction.ActionType;
                    planActionDB.GameAction = planAction.GameAction;
                    planActionDB.SequenceNumber = planAction.SequenceNumber;

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
