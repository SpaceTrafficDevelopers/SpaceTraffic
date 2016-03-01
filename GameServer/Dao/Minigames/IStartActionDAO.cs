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
using SpaceTraffic.Entities.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Dao
{
    public interface IStartActionDAO
    {
        /// <summary>
        /// Get start actions from database
        /// </summary>
        /// <returns>Return list of start actions</returns>
        List<StartAction> GetStartActions();

        /// <summary>
        /// Get start actions with Minigames from database.
        /// </summary>
        /// <returns>Return list of start actions with Minigames.</returns>
        List<StartAction> GetStartActionsWithMinigamesList();
        
        /// <summary>
        /// Get start actions with Minigames from database. 
        /// </summary>
        /// <returns>Return dictionary of start action names and start actions with Minigames from database.</returns>
        Dictionary<string, StartAction> GetStartActionsWithMinigamesDictionary();

        /// <summary>
        /// Get start action from database by id
        /// </summary>
        /// <param name="startActionId">Identificator of start action</param>
        /// <returns>Return object of start action by id</returns>
        StartAction GetStartActionById(int startActionId);

        /// <summary>
        /// Get start action from database by name
        /// </summary>
        /// <param name="startActionName">Name of start action</param>
        /// <returns>Return object of start action by name</returns>
        StartAction GetStartActionByName(string startActionName);


        /// <summary>
        /// Insert new minigame to database
        /// </summary>
        /// <param name="startAction">start action</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        bool InsertStartAction(StartAction startAction);

        /// <summary>
        /// Remove exist start action by id
        /// </summary>
        /// <param name="startActionId">Identificator of start action</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        bool RemoveStartActionById(int startActionId);

        /// <summary>
        /// Update exist start action by id
        /// </summary>
        /// <param name="startAction">start action</param>
        /// <returns>Return true if operation of update is successful.</returns>
        bool UpdateStartActionById(StartAction startAction);
    }
}
