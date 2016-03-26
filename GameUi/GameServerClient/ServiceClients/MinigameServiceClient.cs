using SpaceTraffic.Entities.Minigames;
using SpaceTraffic.Game.Minigame;
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
using SpaceTraffic.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{
    public class MinigameServiceClient : ServiceClientBase<IMinigameService>, IMinigameService
    {

        public bool registerMinigame(MinigameDescriptor minigame)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).registerMinigame(minigame);
            }
        }

        public bool deregisterMinigame(int minigameDescriptorId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).deregisterMinigame(minigameDescriptorId);
            }
        }

        public int createGame(int minigameDescriptorId, bool freeGame)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).createGame(minigameDescriptorId, freeGame);
            }
        }

        public Result rewardPlayers(int minigameId, int[] playerIds)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).rewardPlayers(minigameId, playerIds);
            }
        }

        public Result rewardPlayer(int minigameId, int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).rewardPlayer(minigameId, playerId);
            }
        }

        public Result startGame(int minigameId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).startGame(minigameId);
            }
        }

        public Result endGame(int minigameId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).endGame(minigameId);
            }
        }

        public Result addPlayer(int minigameId, int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).addPlayer(minigameId, playerId);
            }
        }

        public Result performActionLock(int minigameId, string actionName, object[] actionArgs, bool lockAction)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).performActionLock(minigameId, actionName, actionArgs, lockAction);
            }
        }

        public Result performAction(int minigameId, string actionName, object[] actionArgs)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).performAction(minigameId, actionName, actionArgs);
            }
        }


        public List<StartAction> getStartActions()
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).getStartActions();
            }
        }

        public bool addRelationshipWithStartActions(string minigameName, string startActionName)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).addRelationshipWithStartActions(minigameName, startActionName);
            }
        }

        public bool removeRelationshipWithStartActions(string minigameName, string startActionName)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).removeRelationshipWithStartActions(minigameName, startActionName);
            }
        }


        public List<int> getMinigameList(string actionName, int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).getMinigameList(actionName, playerId);
            }
        }

        public int getMinigame(string actionName, int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).getMinigame(actionName, playerId);
            }
        }

        public Result removeGame(int minigameId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).removeGame(minigameId);
            }
        }


        public List<MinigameDescriptor> getMinigameDescriptorListByActionName(string actionName, int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).getMinigameDescriptorListByActionName(actionName, playerId);
            }
        }

        public IMinigameDescriptor getMinigameDescriptorByActionName(string actionName, int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).getMinigameDescriptorByActionName(actionName, playerId);
            }
        }


        public bool isPlayerPlaying(int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).isPlayerPlaying(playerId);
            }
        }

        public void updateLastRequestTime(int minigameId)
        {
            using (var channel = this.GetClientChannel())
            {
                (channel as IMinigameService).updateLastRequestTime(minigameId);
            }
        }


        public bool checkMinigameLife(int minigameId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).checkMinigameLife(minigameId);
            }
        }


        public int actualPlayingMinigameId(int playerId)
        {
            using (var channel = this.GetClientChannel())
            {
                return (channel as IMinigameService).actualPlayingMinigameId(playerId);
            }
        }
    }
}