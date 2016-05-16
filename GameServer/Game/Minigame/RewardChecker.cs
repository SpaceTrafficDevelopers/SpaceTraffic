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
using System.Text;
using SpaceTraffic.Entities.Minigames;

namespace SpaceTraffic.Game.Minigame
{
    /// <summary>
    /// Reward checker servant.
    /// </summary>
    public class RewardChecker
    {
        /// <summary>
        /// Check function delegate.
        /// </summary>
        /// <param name="specificReward">specific reward</param>
        /// <param name="rewardAmount">reward amount</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        private delegate bool checkFunction(string specificReward, double rewardAmount);

        /// <summary>
        /// Check function dictionary. K - reward type, V - check function
        /// </summary>
        private Dictionary<RewardType, checkFunction> checkFunctions;

        /// <summary>
        /// Reward checker constructor.
        /// </summary>
        public RewardChecker()
        {
            this.checkFunctions = new Dictionary<RewardType, checkFunction>(){
                { RewardType.CREDIT, this.creditCheckFunction },
                { RewardType.EXPERIENCE, this.experienceCheckFunction },
                { RewardType.NOTHING, (x, y) => true }
            };
        }

        /// <summary>
        /// Method for checking reward.
        /// </summary>
        /// <param name="minigame">minigame descriptor instance</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        public bool checkReward(IMinigameDescriptor minigame)
        {
            return this.checkFunctions[minigame.RewardType](minigame.SpecificReward, minigame.RewardAmount);
        }

        /// <summary>
        /// Method for checking credit type reward.
        /// </summary>
        /// <param name="specificReward">specific reward</param>
        /// <param name="rewardAmount">reward amount</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        private bool creditCheckFunction(string specificReward, double rewardAmount)
        {
            return rewardAmount >= 1;
        }

        /// <summary>
        /// Method for checking experience type reward.
        /// </summary>
        /// <param name="specificReward">specific reward</param>
        /// <param name="rewardAmount">reward amount</param>
        /// <returns>true, if control was successfull, otherwise false</returns>
        private bool experienceCheckFunction(string specificReward, double rewardAmount)
        {
            return rewardAmount >= 1;
        }

        /// <summary>
        /// Method for parsing string to int.
        /// </summary>
        /// <param name="args">parsing value</param>
        /// <returns>parsed int or null</returns>
        private int? parseArgumentInt(string args)
        {
            int value = 0;
            try
            {
                value = int.Parse(args);
            }
            catch (Exception)
            {
                return null;
            }

            return value;
        }
    }
}
