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
using System.ServiceModel;
using System.Text;
using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.Game.Minigame;
using System.Reflection;

namespace SpaceTraffic.Services.Contracts
{
    /// <summary>
    /// Minigame service contract interface.
    /// </summary>
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(MinigameDescriptorHelper))]
    public interface IMinigameService
    {
        /// <summary>
        /// Method for registering minigame. (Adding into database)
        /// </summary>
        /// <param name="minigame">minigame</param>
        /// <returns>true if registering was successfull</returns>
        [OperationContract]
        bool registerMinigame(MinigameDescriptor minigame);

        /// <summary>
        /// Method for deregistering minigame. (Remove from database)
        /// </summary>
        /// <param name="minigameDescriptorId">minigame id</param>
        /// <returns>true if deregistering was successfull</returns>
        [OperationContract]
        bool deregisterMinigame(int minigameDescriptorId);

        /// <summary>
        /// Method for create minigame instace in minigame manager.
        /// </summary>
        /// <param name="minigameDescriptorId">minigame descriptor id</param>
        /// <param name="freeGame">indication, if game has to create as free game</param>
        /// <returns>return minigame id or -1 if creation failed</returns>
        [OperationContract]
        int createGame(int minigameDescriptorId, bool freeGame);

        /// <summary>
        /// Method for reward players.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="playerIds">rewarding players</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result rewardPlayers(int minigameId, int[] playerIds);

        /// <summary>
        /// Method for reward player.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="playerId">player id</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result rewardPlayer(int minigameId, int playerId);

        /// <summary>
        /// Method for start game.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result startGame(int minigameId);

        /// <summary>
        /// Method for end game.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result endGame(int minigameId);

        /// <summary>
        /// Method for adding player into game.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="playerId">player id</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result addPlayer(int minigameId, int playerId);

        /// <summary>
        /// Method for perform action with lock in minigame by name.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="actionName">action name</param>
        /// <param name="actionArgs">action arguments</param>
        /// <param name="lockAction">true if minigame instance has to be locked</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result performActionLock(int minigameId, string actionName, object[] actionArgs, bool lockAction);

        /// <summary>
        /// Method for perform action in minigame by name.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="actionName">action name</param>
        /// <param name="actionArgs">action arguments</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result performAction(int minigameId, string actionName, object[] actionArgs);

        /// <summary>
        /// Method for getting all start actions.
        /// </summary>
        /// <returns>return list of start actions or null</returns>
        [OperationContract]
        List<StartAction> getStartActions();

        /// <summary>
        /// Method for adding relationship between minigame and start action.
        /// </summary>
        /// <param name="minigameName">minigame name</param>
        /// <param name="startActionName">start action name</param>
        /// <returns>true if adding was successfull</returns>
        [OperationContract]
        bool addRelationshipWithStartActions(string minigameName, string startActionName);

        /// <summary>
        /// Method for removing relationship between minigame and start action.
        /// </summary>
        /// <param name="minigameName">minigame name</param>
        /// <param name="startActionName">start action name</param>
        /// <returns>true if removing was successfull</returns>
        [OperationContract]
        bool removeRelationshipWithStartActions(string minigameName, string startActionName);

        /// <summary>
        /// Method for getting minigames by start action name for player.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <param name="playerId">player id</param>
        /// <returns>return list of minigame descriptors ids or null</returns>
        [OperationContract]
        List<int> getMinigameList(string actionName, int playerId);

        /// <summary>
        /// Method for getting minigames by start action name for player.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <param name="playerId">player id</param>
        /// <returns>return list of minigame descriptors or null</returns>
        [OperationContract]
        List<MinigameDescriptor> getMinigameDescriptorListByActionName(string actionName, int playerId);

        /// <summary>
        /// Method for getting minigame descriptor by start action name for player.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <param name="playerId">player id</param>
        /// <returns>return minigame descriptor or null</returns>
        [OperationContract]
        IMinigameDescriptor getMinigameDescriptorByActionName(string actionName, int playerId);

        /// <summary>
        /// Method for getting minigame by start action name for player.
        /// </summary>
        /// <param name="actionName">start action name</param>
        /// <param name="playerId">player id</param>
        /// <returns>return minigame id or -1</returns>
        [OperationContract]
        int getMinigame(string actionName, int playerId);

        /// <summary>
        /// Method for remove minigame instace from minigame manager.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result removeGame(int minigameId);

        /// <summary>
        /// Method for check if player is playing any minigame.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>return true if player is playing any minigame, otherwise false</returns>
        [OperationContract]
        bool isPlayerPlaying(int playerId);

        /// <summary>
        /// Method for updating minigame last time request
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        [OperationContract]
        void updateLastRequestTime(int minigameId);

        /// <summary>
        /// Method for checking minigame life.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>return true if minigame is alive, otherwise false</returns>
        [OperationContract]
        bool checkMinigameLife(int minigameId);

        /// <summary>
        /// Method for getting minigame id for actual playing game by player.
        /// </summary>
        /// <param name="playerId">player id</param>
        /// <returns>returns actual playing minigame id or -1</returns>
        [OperationContract]
        int actualPlayingMinigameId(int playerId);
    }

    /// <summary>
    /// MinigameDescriptorHelper class. Contains method which returns KnowTypes for IMinigameDescriptor.
    /// </summary>
    static class MinigameDescriptorHelper
    {
        /// <summary>
        /// Method for getting known types of IMinigameDescriptor
        /// </summary>
        /// <param name="provider">provider</param>
        /// <returns>return list of know types</returns>
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            List<Type> knownTypes = new List<Type>();

            knownTypes.Add(typeof(MinigameDescriptor));
            return knownTypes;
        }
    }
}
