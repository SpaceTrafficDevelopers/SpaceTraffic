﻿/**
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

namespace SpaceTraffic.Entities.Minigames
{
    public interface IMinigameDescriptor
    {
        int MinigameId { get; set; }

        string Name { get; set; }

        int PlayerCount { get; set; }

        string Description { get; set; }

        ICollection<StartAction> StartActions { get; set; }

        int RewardTypeInt { get; set; }

        RewardType RewardType { get; set; }

        string SpecificReward { get; set; }

        double RewardAmount { get; set; }

        int ConditionTypeInt { get; set; }

        ConditionType ConditionType { get; set; }

        string ConditionArgs { get; set; }

        bool ExternalClient { get; set; }

        string ClientURL { get; set; }

        string MinigameClassFullName { get; set; }
    }

}
