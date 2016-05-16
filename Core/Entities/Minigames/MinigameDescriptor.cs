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
using System.Text;

namespace SpaceTraffic.Entities.Minigames
{
    [DataContract]
    //[KnownType(typeof(IMinigameDescriptor))]
    public class MinigameDescriptor : IMinigameDescriptor
    {
        [DataMember]
        public int MinigameId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int PlayerCount { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Controls { get; set; }

        [DataMember]
        public virtual ICollection<StartAction> StartActions { get; set; }

        //Mapping on RewardType enum.
        public int RewardTypeInt
        {
            get { return (int)RewardType; }
            set
            {
                if (Enum.IsDefined(typeof(RewardType), value))
                    RewardType = (RewardType)value;
                else
                    throw new ArgumentException(string.Format("Value {0} is not RewardType enumeration value.", value.ToString()));
            }
        }

        [DataMember]
        public RewardType RewardType { get; set; }

        [DataMember]
        public string SpecificReward { get; set; }

        [DataMember]
        public double RewardAmount { get; set; }

        //Mapping on ConditionType enum.
        public int ConditionTypeInt
        {
            get { return (int)ConditionType; }
            set
            {
                if (Enum.IsDefined(typeof(ConditionType), value))
                    ConditionType = (ConditionType)value;
                else
                    throw new ArgumentException(string.Format("Value {0} is not ConditionType enumeration value.", value.ToString()));
            }
        }

        [DataMember]
        public ConditionType ConditionType { get; set; }

        [DataMember]
        public string ConditionArgs { get; set; }

        [DataMember]
        public bool ExternalClient { get; set; }

        [DataMember]
        public string ClientURL { get; set; }

        [DataMember]
        public string MinigameClassFullName { get; set; }
    }

}
