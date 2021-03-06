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
using System.Runtime.Serialization;
using NLog;

namespace SpaceTraffic.Entities
{
    [DataContract(Name = "EarnedAchievement")]
    public class EarnedAchievement
    {

        [DataMember]
        public int EarnedAchievementsId { get; set; }

        [DataMember]
        public int AchievementId { get; set; }

        [DataMember]
        public DateTime UnlockedAt { get; set; }

        // TODO db or not? remove from the eaDAO and contextManager table.
        //[IgnoreDataMemberAttribute]
        [DataMember]
        public bool IsJustEarned { get; set; }

        // Player ID
        [DataMember]
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public EarnedAchievement()
        {
            this.IsJustEarned = false;
        }
    }
}
