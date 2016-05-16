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
    [ServiceContract(Namespace = "http://spacetraffic.kiv.zcu.cz/MinigameService")]
    [ServiceKnownType("GetKnownTypes", typeof(MinigameHelper))]
    public interface IMinigameService
    {
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
        Result performActionLock(int minigameId, string actionName, bool lockAction, params object[] actionArgs);

        /// <summary>
        /// Method for perform action in minigame by name.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="actionName">action name</param>
        /// <param name="actionArgs">action arguments</param>
        /// <returns>success or failure result</returns>
        [OperationContract]
        Result performAction(int minigameId, string actionName, params object[] actionArgs);

        /// <summary>
        /// Method for getting all start actions.
        /// </summary>
        /// <returns>return list of start actions or null</returns>
        [OperationContract]
        List<StartAction> getStartActions();

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
        
        /// <summary>
        /// Method for check minigame life and if minigame is alive it is updated last request time.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <returns>success result with true return value if minigame is alive,
        /// otherwiser returns failure result with false return value</returns>
        [OperationContract]
        Result checkMinigameLifeAndUpdateLastRequestTime(int minigameId);

        /// <summary>
        /// Method for player authentication.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="passwd">encrypt password</param>
        /// <returns>playerId or -1</returns>
        [OperationContract]
        int authenticatePlayerForMinigame(string userName, string passwd);
    
        /// <summary>
        /// Support method for call checkAnswers method in LogoQuiz. This is because perform action
        /// called from android, cannot passed list of object.
        /// </summary>
        /// <param name="minigameId">minigame id</param>
        /// <param name="answersXml">answers in xml as string</param>
        /// <returns>success or failure result with return value</returns>
        [OperationContract]
        Result checkAnswersSupportMethod(int minigameId, string answersXml);

    }

    /// <summary>
    /// MinigameDescriptorHelper class. Contains method which returns KnowTypes for IMinigameDescriptor.
    /// </summary>
    static class MinigameHelper
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
            knownTypes.Add(typeof(Position));
            knownTypes.Add(typeof(List<Position>));
            knownTypes.Add(typeof(SpaceshipCargoFinderGameInfo));
            knownTypes.Add(typeof(Logo));
            knownTypes.Add(typeof(Question));
            knownTypes.Add(typeof(List<Question>));

            return knownTypes;
        }
    }
}
