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
		public Player() {
			this.Statistics = new HashSet<Entities.Statistic>();
			this.EarnedAchievements = new HashSet<Entities.EarnedAchievement>();

			this.ExperienceLevel = 1;

			// fill statistics
			initStatistics();
		}

		#region StatisticsFunctions
		/// <summary>
		/// 
		/// </summary>
		private void initStatistics()
		{
			Statistics.Add(new Statistic("shipFleet"));

			Statistics.Add(new Statistic("soilFactory3x3"));
			Statistics.Add(new Statistic("bioGasFactory3x3"));
			Statistics.Add(new Statistic("spicesFactory3x3"));
			Statistics.Add(new Statistic("jewleryFactory3x3"));
			Statistics.Add(new Statistic("crystalFactory3x3"));
			Statistics.Add(new Statistic("pcbFactory3x3"));
			Statistics.Add(new Statistic("weaponsFactory3x3"));
			Statistics.Add(new Statistic("satelliteFactory3x3"));
			Statistics.Add(new Statistic("droneFactory3x3"));

			Statistics.Add(new Statistic("terraWheatFarm"));
			Statistics.Add(new Statistic("terraPlanktonFarm"));
			Statistics.Add(new Statistic("terraFlowerFarm"));
			Statistics.Add(new Statistic("terraFruitsFarm"));
			Statistics.Add(new Statistic("terraMeatFarm"));

			Statistics.Add(new Statistic("terraOreMine"));
			Statistics.Add(new Statistic("terraSiliconMine"));
			Statistics.Add(new Statistic("terraIceMine"));
			Statistics.Add(new Statistic("terraGoldMine"));

			Statistics.Add(new Statistic("factoryCount"));
		}
		#endregion

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
