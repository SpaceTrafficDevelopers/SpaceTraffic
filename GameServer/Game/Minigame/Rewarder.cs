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
    public class Rewarder
    {
        private IGameServer gameServer;

        private IMinigameDescriptor descriptor;

        private delegate void rewardFunction(Player player, IMinigameDescriptor descriptor);

        private Dictionary<RewardType, rewardFunction> rewardFunctions;

        public Rewarder(IGameServer gameServer)
        {
            this.gameServer = gameServer;
            this.rewardFunctions = new Dictionary<RewardType, rewardFunction>()
            {
                { RewardType.EXPERIENCE, this.experienceReward },
                { RewardType.CREDIT, this.creditReward }
            };
        }

        public void rewardPlayer(Player player, IMinigameDescriptor descriptor)
        {
            this.rewardFunctions[descriptor.RewardType](player, descriptor);
        }

        private void experienceReward(Player player, IMinigameDescriptor descriptor)
        {
            this.gameServer.Statistics.IncrementExperiences(player, (int)this.descriptor.RewardAmount);
        }

        private void creditReward(Player player, IMinigameDescriptor descriptor)
        {
            this.gameServer.Persistence.GetPlayerDAO().IncrasePlayersCredits(player.PlayerId, (int) descriptor.RewardAmount);
        }
    }
}
