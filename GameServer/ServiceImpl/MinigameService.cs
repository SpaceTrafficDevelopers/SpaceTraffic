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
using SpaceTraffic.Engine;
using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.Game.Minigame;
using SpaceTraffic.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SpaceTraffic.GameServer.ServiceImpl
{
    /// <summary>
    /// Minigame service.
    /// </summary>
    [ServiceBehavior(Namespace = "http://spacetraffic.kiv.zcu.cz/MinigameService")]
    public class MinigameService : IMinigameService
    {
        /// <summary>
        /// Minigame manager instance.
        /// </summary>
        private IMinigameManager manager = GameServer.CurrentInstance.Minigame;

        public int createGame(int minigameDescriptorId, bool freeGame)
        {
            return manager.createGame(minigameDescriptorId, freeGame);
        }

        public Result rewardPlayers(int minigameId, int[] playerIds)
        {
            return manager.reward(minigameId, playerIds);
        }

        public Result rewardPlayer(int minigameId, int playerId)
        {
            return manager.reward(minigameId, playerId);
        }

        public Result startGame(int minigameId)
        {
            return manager.startGame(minigameId);
        }

        public Result endGame(int minigameId)
        {
            return manager.endGame(minigameId);
        }

        public Result addPlayer(int minigameId, int playerId)
        {
            return manager.addPlayer(minigameId, playerId);
        }

        public Result performActionLock(int minigameId, string actionName, bool lockAction, params object[] actionArgs)
        {
            return manager.performAction(minigameId, actionName, lockAction, actionArgs);
        }

        public Result performAction(int minigameId, string actionName, params object[] actionArgs)
        {
            return manager.performAction(minigameId, actionName, actionArgs);
        }

        public List<StartAction> getStartActions()
        {
            return manager.getStartActions();
        }

        public List<int> getMinigameList(string actionName, int playerId)
        {
            return manager.getMinigameList(actionName, playerId);
        }

        public int getMinigame(string actionName, int playerId)
        {
            return manager.getMinigame(actionName, playerId);
        }

        public Result removeGame(int minigameId)
        {
            return manager.removeGame(minigameId);
        }

        public List<MinigameDescriptor> getMinigameDescriptorListByActionName(string actionName, int playerId)
        {
            return manager.getMinigameDescriptorListByActionName(actionName, playerId);
        }

        public IMinigameDescriptor getMinigameDescriptorByActionName(string actionName, int playerId)
        {
            return manager.getMinigameDescriptorByActionName(actionName, playerId);
        }


        public bool isPlayerPlaying(int playerId)
        {
            return manager.isPlayerPlaying(playerId);
        }

        public void updateLastRequestTime(int minigameId)
        {
            manager.updateLastRequestTime(minigameId);
        }

        public bool checkMinigameLife(int minigameId)
        {
            return manager.checkMinigameLife(minigameId);
        }

        public int actualPlayingMinigameId(int playerId)
        {
            return manager.actualPlayingMinigameId(playerId);
        }

        public Result checkMinigameLifeAndUpdateLastRequestTime(int minigameId)
        {
            return manager.checkMinigameLifeAndUpdateLastRequestTime(minigameId);
        }

        public int authenticatePlayerForMinigame(string userName, string passwd)
        {
            return manager.authenticatePlayerForMinigame(userName, passwd);
        }

        public Result checkAnswersSupportMethod(int minigameId, string answersXml)
        {
            return manager.performAction(minigameId, "checkAnswers", answersXml);
        }
    }
}
