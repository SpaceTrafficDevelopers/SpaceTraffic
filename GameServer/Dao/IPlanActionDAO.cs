using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Interface DAO for plan action entity in DB.
    /// </summary>
    public interface IPlanActionDAO
    {
        /// <summary>
        /// Get plan actions by identification number of plan item.
        /// </summary>
        /// <param name="planItemId">Identification number of plan item.</param>
        /// <returns>List of plan actions.</returns>
        List<PlanAction> GetPlanActionsByPlanItemId(int planItemId);

        /// <summary>
        /// Get plan action by identification number of plan action.
        /// </summary>
        /// <param name="planActionID">Identification number of plan action</param>
        /// <returns>Instance of plan action.</returns>
        PlanAction GetPlanActionById(int planActionID);

        /// <summary>
        /// Insert one plan action to DB.
        /// </summary>
        /// <param name="planAction">Instance of plan action.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        bool InsertPlanAction(PlanAction planAction);

        /// <summary>
        /// Remove one plan action from DB by identification number.
        /// </summary>
        /// <param name="planActionID">Identification number of plan action.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        bool RemovePlanAction(int planActionID);

        /// <summary>
        /// Update plan action by instance in parameter.
        /// </summary>
        /// <param name="planAction">Instance of plan action.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        bool UpdatePlanActionById(PlanAction planAction);
    }
}
