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
using SpaceTraffic.Entities.Minigames;

namespace SpaceTraffic.Dao
{
    public interface IMinigameDescriptorDAO
    {

        /// <summary>
        /// Get minigames from database
        /// </summary>
        /// <returns>Return list of minigames</returns>
        List<MinigameDescriptor> GetMinigames();

        /// <summary>
        /// Get minigames with StartActions from database.
        /// </summary>
        /// <returns>Return list of minigames with StartActions.</returns>
        List<MinigameDescriptor> GetMinigamesWithStartActions();

        /// <summary>
        /// Get minigame from database by id
        /// </summary>
        /// <param name="minigameId">Identificator of minigame</param>
        /// <returns>Return object of minigame by id</returns>
        MinigameDescriptor GetMinigameById(int minigameId);

        /// <summary>
        /// Get minigame with StartActions from database by id
        /// </summary>
        /// <param name="minigameId">Identificator of minigame</param>
        /// <returns>Return object of minigame with StartActions by id</returns>
        MinigameDescriptor GetMinigameWithStartActionsById(int minigameId);

        /// <summary>
        /// Get minigame from database by name
        /// </summary>
        /// <param name="minigameName">Name of minigame</param>
        /// <returns>Return object of minigame by name</returns>
        MinigameDescriptor GetMinigameByName(string minigameName);


        /// <summary>
        /// Insert new minigame to database
        /// </summary>
        /// <param name="minigame">Minigame</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        bool InsertMinigame(MinigameDescriptor minigame);

        /// <summary>
        /// Remove exist minigame by id
        /// </summary>
        /// <param name="minigameId">Identificator of minigame</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        bool RemoveMinigameById(int minigameId);

        /// <summary>
        /// Update exist minigame by id
        /// </summary>
        /// <param name="minigame">Minigame</param>
        /// <returns>Return true if operation of update is successful.</returns>
        bool UpdateMinigameById(MinigameDescriptor minigame);

        /// <summary>
        /// Insert relationship with stars action
        /// </summary>
        /// <param name="minigameId">Identificator of minigame</param>
        /// <param name="startActionId">Identificator of start action</param>
        /// <returns>Return true if operation of insert is successful.</returns>
        bool InsertRelationshipWithStartActions(int minigameId, int startActionId);

        /// <summary>
        /// Remove relationship with stars action
        /// </summary>
        /// <param name="minigameId">Identificator of minigame</param>
        /// <param name="startActionId">Identificator of start action</param>
        /// <returns>Return true if operation of remove is successful.</returns>
        bool RemoveRelationshipWithStartActions(int minigameId, int startActionId);
    }
}
