using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public class PlanItemEntityDAO : AbstractDAO, IPlanItemEntityDAO
    {
        public List<Entities.PlanItemEntity> GetPlanItemsByPathPlanId(int pathPlanId)
        {
            using (var contextDB = CreateContext())
            {
                var items = (from x in contextDB.PlanItem
						where x.PathPlanId.Equals(pathPlanId)
						select x);

                if (items == null)
                    return null;

                return items.OrderBy(x => x.SequenceNumber).ToList<PlanItemEntity>();
            }
        }

        public Entities.PlanItemEntity GetPlanItemById(int planItemID)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.PlanItem.FirstOrDefault(p => p.PlanItemId.Equals(planItemID));
            }
        }

        public int InsertPlanItem(Entities.PlanItemEntity planItem)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add base to context
                    var itemDB = contextDB.PlanItem.Add(planItem);
                    // save context to database
                    contextDB.SaveChanges();
                    return itemDB.PlanItemId;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public bool RemovePlanItem(int planItemID)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var planItem = contextDB.PlanItem.FirstOrDefault(x => x.PlanItemId.Equals(planItemID));
                    // remove base to context
                    contextDB.PlanItem.Remove(planItem);
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

        public bool UpdatePlanItemById(Entities.PlanItemEntity planItem)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var planItemDB = contextDB.PlanItem.FirstOrDefault(x => x.PlanItemId.Equals(planItem.PlanItemId));

                    planItemDB.SolarSystem = planItem.SolarSystem;
                    planItemDB.Index = planItem.Index;
                    planItemDB.IsPlanet = planItem.IsPlanet;
                    planItemDB.SequenceNumber = planItem.SequenceNumber;

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
