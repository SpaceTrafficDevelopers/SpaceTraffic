using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Interface DAO for plan item entity.
    /// </summary>
    public interface IPlanItemEntityDAO
    {
        /// <summary>
        /// Get all items of path plan by path plan identification number.
        /// </summary>
        /// <param name="pathPlanId">Identification number of path plan.</param>
        /// <returns>List of plan items.</returns>
        List<PlanItemEntity> GetPlanItemsByPathPlanId(int pathPlanId);

        /// <summary>
        /// Get one plan item by identification number.
        /// </summary>
        /// <param name="planItemID">Identification number of plan item.</param>
        /// <returns>Plan item entity.</returns>
        PlanItemEntity GetPlanItemById(int planItemID);

        /// <summary>
        /// Insert one instance of plan item to DB.
        /// </summary>
        /// <param name="planItem">Instance of plan item.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        int InsertPlanItem(PlanItemEntity planItem);

        /// <summary>
        /// Remove one instance of plan item from DB by identification number of plan item.
        /// </summary>
        /// <param name="planItemID">Identification number of plan item.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        bool RemovePlanItem(int planItemID);

        /// <summary>
        /// Update plan item by instance in parameter.
        /// </summary>
        /// <param name="planItem">Instance of plan item.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        bool UpdatePlanItemById(PlanItemEntity planItem);

    }
}
