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

namespace SpaceTraffic.Services.Contracts
{
    [ServiceContract]
    public interface IMinigameService
    {
        [OperationContract]
        bool registerMinigame(MinigameDescriptor minigame);

        [OperationContract]
        bool deregisterMinigame(int minigameDescriptorId);

        [OperationContract]
        int createGame(int minigameDescriptorId, bool freeGame);

        [OperationContract]
        Result rewardPlayers(int minigameId, int[] playerIds);

        [OperationContract]
        Result rewardPlayer(int minigameId, int playerId);

        [OperationContract]
        Result startGame(int minigameId);

        [OperationContract]
        Result endGame(int minigameId);

        [OperationContract]
        Result addPlayer(int minigameId, int playerId);

        [OperationContract]
        Result performActionLock(int minigameId, string actionName, object[] actionArgs, bool lockAction);

        [OperationContract]
        Result performAction(int minigameId, string actionName, object[] actionArgs);

        [OperationContract]
        List<StartAction> getStartActions();

        [OperationContract]
        bool addRelationshipWithStartActions(string minigameName, string startActionName);

        [OperationContract]
        bool removeRelationshipWithStartActions(string minigameName, string startActionName);
    }
}
