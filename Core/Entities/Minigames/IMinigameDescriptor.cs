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
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SpaceTraffic.Entities.Minigames
{
    /// <summary>
    /// Interface for Minigame descriptor
    /// </summary>
    public interface IMinigameDescriptor
    {
        /// <summary>
        /// Minigame id.
        /// </summary>
        int MinigameId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Number of players.
        /// </summary>
        int PlayerCount { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Start actions for minigame.
        /// </summary>
        ICollection<StartAction> StartActions { get; set; }

        /// <summary>
        /// Reward type as integer. This is enum mapping because 
        /// entity framework 4 does not support enum maping.
        /// </summary>
        int RewardTypeInt { get; set; }

        /// <summary>
        /// Reward type.
        /// </summary>
        RewardType RewardType { get; set; }
        
        /// <summary>
        /// Specific reward.
        /// </summary>
        string SpecificReward { get; set; }

        /// <summary>
        /// Reward amount.
        /// </summary>
        double RewardAmount { get; set; }
        
        /// <summary>
        /// Start condition type as integer. This is enum mapping because 
        /// entity framework 4 does not support enum maping.
        /// </summary>
        int ConditionTypeInt { get; set; }

        /// <summary>
        /// Start condition type.
        /// </summary>
        ConditionType ConditionType { get; set; }

        /// <summary>
        /// Start codition arguments.
        /// </summary>
        string ConditionArgs { get; set; }

        /// <summary>
        /// External client indication.
        /// </summary>
        bool ExternalClient { get; set; }

        /// <summary>
        /// Client URL.
        /// </summary>
        string ClientURL { get; set; }

        /// <summary>
        /// Assembly qualified name for create minigame instance.
        /// </summary>
        string MinigameClassFullName { get; set; }
    }
}
