/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Dao
{
    public interface IFactoryDAO
    {
        /// <summary>
        /// Return list of factories
        /// </summary>
        /// <returns>List of factory</returns>
        List<Factory> GetFactories();

        /// <summary>
        /// Return Factory by Id
        /// </summary>
        /// <param name="factoryId">Factory id</param>
        /// <returns>Factory</returns>
        Factory GetFactoryById(int factoryId);
        
        /// <summary>
        /// Return list of factories by planet
        /// </summary>
        /// <param name="planet">name</param>
        /// <returns>List of factory/returns>
        List<Factory> GetFactoriesByPlanet(string planet);

        /// <summary>
        /// Return list of factories by type
        /// </summary>
        /// <param name="type">Type of factory</param>
        /// <returns>List of factory</returns>
        List<Factory> GetFactoriesByType(string type);

        /// <summary>
        /// Insert factory to database
        /// </summary>
        /// <param name="factory">Factory</param>
        /// <returns>result of inserting</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool InsertFactory(Factory factory);

        /// <summary>
        /// Remove factory from database
        /// </summary>
        /// <param name="factoryId">Factory id</param>
        /// <returns>result of removing</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool RemoveFactoryById(int factoryId);

        /// <summary>
        /// Update factory to database 
        /// </summary>
        /// <param name="factory">Factory</param>
        /// <returns>Result of updating</returns>
        /// <exception cref="OptimisticConcurrencyException">The exception that is thrown when an optimistic concurrency violation occurs.</exception>
        /// <exception cref="UpdateException" >The exception that is thrown when modifications to object instances cannot be persisted to the data store.</exception>
        bool UpdateFactoryById(Factory factory);



    }
}
