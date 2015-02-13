using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface IBaseDAO
    {
        /// <summary>
        /// Gets all bases from DB.
        /// </summary>
        /// <returns>List of bases.</returns>
        List<Base> GetBases();
        /// <summary>
        /// Gets the base by id from DB.
        /// </summary>
        /// <param name="baseId">The base id.</param>
        /// <returns>Base.</returns>
        Base GetBaseById(int baseId);
        /// <summary>
        /// Inserts the base to DB.
        /// </summary>
        /// <param name="?">The base.</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertBase(Base bbase);
        /// <summary>
        /// Removes the base by id from DB.
        /// </summary>
        /// <param name="baseId">The base id.</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveBaseById(int baseId);
        /// <summary>
        /// Updates the base by id.
        /// </summary>
        /// <param name="bbase">The base.</param>
        /// <returns>Return true if operation of update is successful.</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateBaseById(Base bbase);
    }
}
