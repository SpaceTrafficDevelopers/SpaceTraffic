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
    public interface IMinigameManager
    {
        bool registerMinigame(MinigameDescriptor minigame);

        bool deregisterMinigame(int minigameDescriptorId);

        int createGame(int minigameDescriptorId, bool freeGame);

        Result reward(int minigameId, int[] playerIds);

        Result reward(int minigameId, int playerId);

        Result startGame(int minigameId);

        Result endGame(int minigameId);

        int getMinigame(string actionName, int playerId);

        Result addPlayer(int minigameId, int playerId);

        int getNewMinigameId();

        Result performAction(int minigameId, string actionName, object[] actionArgs, bool lockAction);

        Result performAction(int minigameId, string actionName, object[] actionArgs)
    }
}
