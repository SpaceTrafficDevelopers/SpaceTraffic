using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface ICargoDAO
    {
        /// <summary>
        /// Return list of Cargos
        /// </summary>
        /// <returns>List of Cargo</returns>
        List<Cargo> GetCargos();

        /// <summary>
        /// Return Cargo by Id
        /// </summary>
        /// <param name="CargoId">Cargo id</param>
        /// <returns>Cargo</returns>
        Cargo GetCargoById(int CargoId);

    

        /// <summary>
        /// Return list of Cargos by type
        /// </summary>
        /// <param name="type">Type of Cargo</param>
        /// <returns>List of Cargo</returns>
        List<Cargo> GetCargosByType(string type);

        /// <summary>
        /// Insert Cargo to database
        /// </summary>
        /// <param name="Cargo">Cargo</param>
        /// <returns>result of inserting</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertCargo(Cargo Cargo);

        /// <summary>
        /// Remove Cargo from database
        /// </summary>
        /// <param name="CargoId">Cargo id</param>
        /// <returns>result of removing</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveCargoById(int CargoId);

        /// <summary>
        /// Update Cargo to database 
        /// </summary>
        /// <param name="Cargo">Cargo</param>
        /// <returns>Result of updating</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateCargoById(Cargo Cargo);



    }
}
