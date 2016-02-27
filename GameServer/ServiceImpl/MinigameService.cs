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
using System.Text;

namespace SpaceTraffic.GameServer.ServiceImpl
{
    public class MinigameService : IMinigameService
    {

        private IMinigameManager manager = GameServer.CurrentInstance.Minigame;

        public bool registerMinigame(MinigameDescriptor minigame)
        {
            return manager.registerMinigame(minigame);
        }

        public bool deregisterMinigame(int minigameDescriptorId)
        {
            return manager.deregisterMinigame(minigameDescriptorId);
        }

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

        public Result performActionLock(int minigameId, string actionName, object[] actionArgs, bool lockAction)
        {
            return manager.performAction(minigameId, actionName, actionArgs, lockAction);
        }

        public Result performAction(int minigameId, string actionName, object[] actionArgs)
        {
            return manager.performAction(minigameId, actionName, actionArgs);
        }

        public List<StartAction> getStartActions()
        {
            return manager.getStartActions();
        }

        public bool addRelationshipWithStartActions(string minigameName, string startActionName)
        {
            return manager.addRelationshipWithStartActions(minigameName, startActionName);
        }

        public bool removeRelationshipWithStartActions(string minigameName, string startActionName)
        {
            return manager.removeRelationshipWithStartActions(minigameName, startActionName);
        }
    }
}
