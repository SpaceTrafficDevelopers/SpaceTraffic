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
using SpaceTraffic.Game.Minigame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Engine
{
    /// <summary>
    // Minigame manager interface.
    /// </summary>
    public interface IMinigameManager
    {
        /// <summary>
        /// Method for registering minigame. (Adding into database)
        /// </summary>
        /// <param name="minigame">minigame</param>
        /// <returns>true if registering was successfull</returns>
        bool registerMinigame(MinigameDescriptor minigame);

        /// <summary>
        /// Method for deregistering minigame. (Remove from database)
        /// </summary>
        /// <param name="minigameDescriptorId">minigame id</param>
        /// <returns>true if deregistering was successfull</returns>
        bool deregisterMinigame(int minigameDescriptorId);

        /// <summary>
        /// Method for create minigame instace in minigame manager.
        /// </summary>
        /// <param name="minigameDescriptorId">minigame descriptor id</param>
        /// <param name="freeGame">indication, if game has to create as free game</param>
        /// <returns>return minigame id or -1 if creation failed</returns>
        int createGame(int minigameDescriptorId, bool freeGame);

        /// <summary>
        /// Method for reward players.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="playerIds">rewarding players</param>
        /// <returns>success or failure result</returns>
        Result reward(int minigameId, int[] playerIds);

        /// <summary>
        /// Method for reward player.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="playerId">player id</param>
        /// <returns>success or failure result</returns>
        Result reward(int minigameId, int playerId);

        /// <summary>
        /// Method for start game.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success or failure result</returns>
        Result startGame(int minigameId);

        /// <summary>
        /// Method for end game.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success or failure result</returns>
        Result endGame(int minigameId);

        /// <summary>
        /// Method for getting minigame by start action name for player.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <param name="playerId">player id</param>
        /// <returns>return minigame id or -1</returns>
        int getMinigame(string actionName, int playerId);

        /// <summary>
        /// Method for getting minigames by start action name for player.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <param name="playerId">player id</param>
        /// <returns>return list of minigame descriptors ids or null</returns>
        List<int> getMinigameList(string actionName, int playerId);

        /// <summary>
        /// Method for adding player into game.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="playerId">player id</param>
        /// <returns>success or failure result</returns>
        Result addPlayer(int minigameId, int playerId);

        /// <summary>
        /// Method for perform action with lock in minigame by name.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="actionName">action name</param>
        /// <param name="actionArgs">action arguments</param>
        /// <param name="lockAction">true if minigame instance has to be locked</param>
        /// <returns>success or failure result</returns>
        Result performAction(int minigameId, string actionName, object[] actionArgs, bool lockAction);

        /// <summary>
        /// Method for perform action in minigame by name.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="actionName">action name</param>
        /// <param name="actionArgs">action arguments</param>
        /// <returns>success or failure result</returns>
        Result performAction(int minigameId, string actionName, object[] actionArgs);

        /// <summary>
        /// Method for getting all start actions.
        /// </summary>
        /// <returns>return list of start actions or null</returns>
        List<StartAction> getStartActions();

        /// <summary>
        /// Method for adding relationship between minigame and start action.
        /// </summary>
        /// <param name="minigameName">minigame name</param>
        /// <param name="startActionName">start action name</param>
        /// <returns>true if adding was successfull</returns>
        bool addRelationshipWithStartActions(string minigameName, string startActionName);

        /// <summary>
        /// Method for removing relationship between minigame and start action.
        /// </summary>
        /// <param name="minigameName">minigame name</param>
        /// <param name="startActionName">start action name</param>
        /// <returns>true if removing was successfull</returns>
        bool removeRelationshipWithStartActions(string minigameName, string startActionName);
    }
}
