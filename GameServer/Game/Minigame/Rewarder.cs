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
using SpaceTraffic.Entities;
using SpaceTraffic.Entities.Minigames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Rewarder servant.
    /// </summary>
    public class Rewarder
    {
        /// <summary>
        /// Game server instance.
        /// </summary>
        private IGameServer gameServer;

        /// <summary>
        /// Reward function delegate.
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="descriptor">minigame descriptor</param>
        private delegate void rewardFunction(Player player, IMinigameDescriptor descriptor);

        /// <summary>
        /// Reward function ditionary. K - reward type, V - reward function
        /// </summary>
        private Dictionary<RewardType, rewardFunction> rewardFunctions;

        /// <summary>
        /// Rewarder constructor.
        /// </summary>
        /// <param name="gameServer">game server instance</param>
        public Rewarder(IGameServer gameServer)
        {
            this.gameServer = gameServer;
            this.rewardFunctions = new Dictionary<RewardType, rewardFunction>()
            {
                { RewardType.EXPERIENCE, this.experienceReward },
                { RewardType.CREDIT, this.creditReward }
            };
        }

        /// <summary>
        /// Method for reward player.
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="descriptor">minigame descriptor</param>
        public void rewardPlayer(Player player, IMinigameDescriptor descriptor)
        {
            this.rewardFunctions[descriptor.RewardType](player, descriptor);
        }

        /// <summary>
        /// Method for experience reward.
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="descriptor">minigame descriptor</param>
        private void experienceReward(Player player, IMinigameDescriptor descriptor)
        {
            this.gameServer.Statistics.IncrementExperiences(player, (int)descriptor.RewardAmount);
        }

        /// <summary>
        /// Method for credit reward.
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="descriptor">minigame descriptor</param>
        private void creditReward(Player player, IMinigameDescriptor descriptor)
        {
            this.gameServer.Persistence.GetPlayerDAO().IncrasePlayersCredits(player.PlayerId, (int) descriptor.RewardAmount);
        }
    }
}
