using SpaceTraffic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Dao
{
    /// <summary>
    /// Interface DAO for path plan entity.
    /// </summary>
    public interface IPathPlanEntityDAO
    {
        /// <summary>
        /// Get list of all path plans.
        /// </summary>
        /// <returns>List of all path plans</returns>
        List<PathPlanEntity> GetPathPlans();

        /// <summary>
        /// Get one path plan by identification number.
        /// </summary>
        /// <param name="planID">identification number of plan.</param>
        /// <returns>Path plan entity</returns>
        PathPlanEntity GetPathPlanById(int planID);

        /// <summary>
        /// Insert path plan entity to DB.
        /// </summary>
        /// <param name="plan">Path plan entity.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        int InsertPathPlan(PathPlanEntity plan);

        /// <summary>
        /// Remove path plan entity from DB.
        /// </summary>
        /// <param name="planID">Identification numner of plan.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        bool RemovePathPlan(int planID);
        
        /// <summary>
        /// Update path plan entity in DB by entity in parameter.
        /// </summary>
        /// <param name="plan">Path plan entity.</param>
        /// <returns>Return true if operation of delete is successful.</returns>
        bool UpdatePathPlanById(PathPlanEntity plan);

    }
}
