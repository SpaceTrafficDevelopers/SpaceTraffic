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
using System.Runtime.Serialization;

namespace SpaceTraffic.Entities
{
	[DataContract(Name = "PlayerInfo")]
	public class Player
	{

		[DataMember]
		public int PlayerId { get; set; }

        [DataMember]
        public string PlayerToken { get; set; }

        [DataMember]
		public string PlayerName { get; set; }

        [DataMember]
        public string PlayerShowName { get; set; }

        [DataMember]
		public virtual ICollection<Statistic> Statistics { get; set; }

		[DataMember]
		public virtual ICollection<EarnedAchievement> EarnedAchievements { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PsswdHash { get; set; }
        
        [DataMember]
        public string NewPsswdHash { get; set; }

        [DataMember]
        public bool IsEmailConfirmed { get; set; }

        [DataMember]
        public DateTime PassChangeDate { get; set; }

        [DataMember]
        public DateTime AddedDate { get; set; }

        [DataMember]
        public DateTime LastVisitedDate { get; set; }

        [DataMember]
        public int Credit { get; set; }

        [DataMember]
        public virtual ICollection<SpaceShip> SpaceShips { get; set; }

		[DataMember]
		public int ExperienceLevel { get; set; }

		[DataMember]
		public int Experiences { get; set; }

        [DataMember]
        public bool StayLogedIn { get; set; }

        [DataMember]
        public bool SendInGameInfo { get; set; }

        [DataMember]
        public bool SendNewsletter { get; set; }
	}
}
