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
		public string PlayerName { get; set; }

		[DataMember]
		public virtual ICollection<Statistic> Statistics { get; set; }

		[DataMember]
		public virtual ICollection<EarnedAchievement> EarnedAchievements { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string PsswdHash { get; set; }

		public string PsswdSalt { get; set; }

		public DateTime DateOfBirth { get; set; }

		public string OrionEmail { get; set; }

		public bool IsFavStudent { get; set; }

		public bool IsOrionEmailConfirmed { get; set; }

		public bool IsEmailConfirmed { get; set; }

		public bool IsAccountLocked { get; set; }       

		public DateTime AddedDate { get; set; }

		public DateTime LastVisitedDate { get; set; }

		public string CorporationName { get; set; }

		public int Credit { get; set; }       

		public virtual ICollection<SpaceShip> SpaceShips { get; set; }

		[DataMember]
		public int ExperienceLevel { get; set; }

		[DataMember]
		public int Experiences { get; set; }
	}
}
