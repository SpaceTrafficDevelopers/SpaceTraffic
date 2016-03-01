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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Minigame controls class.
    /// </summary>
    public class MinigameControls
    {   
        /// <summary>
        /// Game server instance.
        /// </summary>
        private IGameServer gameServer;

        /// <summary>
        /// Reward checker instance.
        /// </summary>
        private RewardChecker rewardChecker;

        /// <summary>
        /// Condition checker instance.
        /// </summary>
        private ConditionChecker conditionChecker;

        /// <summary>
        /// Minigame controls constructor.
        /// </summary>
        /// <param name="gameServer"></param>
        public MinigameControls(IGameServer gameServer)
        {
            this.gameServer = gameServer;
            this.rewardChecker = new RewardChecker();
            this.conditionChecker = new ConditionChecker();
        }

        /// <summary>
        /// Method for checking number of players in minigame.
        /// </summary>
        /// <param name="minigame">minigame</param>
        /// <returns>true, if minigame has maximum number of players, otherwise false</returns>
        public bool checkNumberOfPlayers(IMinigame minigame)
        {
            return minigame.Players.Count == minigame.Descriptor.PlayerCount && checkState(minigame, MinigameState.PLAYED);
        }

        /// <summary>
        /// Method for checking if player is in game or not.
        /// </summary>
        /// <param name="minigame">minigame</param>
        /// <param name="playerId">player id</param>
        /// <returns>true, if player is in the game</returns>
        public bool isPlayerInMinigame(IMinigame minigame, int playerId)
        {
            return minigame.Players.ContainsKey(playerId);        
        }

        /// <summary>
        /// Method for checking minigame state.
        /// </summary>
        /// <param name="minigame">minigame</param>
        /// <param name="state">checking state</param>
        /// <returns>true, if minigame state is equal with checking state</returns>
        public bool checkState(IMinigame minigame, MinigameState state)
        {
            return minigame.State == state;
        }

        /// <summary>
        /// Method for checking minigame descriptor.
        /// </summary>
        /// <param name="minigame">minigame descriptor</param>
        /// <returns>true, if minigame descriptor passed through controls</returns>
        public bool checkMinigameDescriptor(IMinigameDescriptor minigame)
        {
            if (minigame.PlayerCount < 1) 
                return false;

            if(!this.rewardChecker.checkReward(minigame)) 
                return false;

            if(!string.IsNullOrEmpty(minigame.MinigameClassFullName) && Type.GetType(minigame.MinigameClassFullName) == null)
                return false;

            if (!conditionChecker.checkCondition(minigame))
                return false;

            if(minigame.ExternalClient){
                if(minigame.ClientURL == null || !Uri.IsWellFormedUriString(minigame.ClientURL, UriKind.RelativeOrAbsolute))
                    return false;
            }

            return true;
        }
    }
}
