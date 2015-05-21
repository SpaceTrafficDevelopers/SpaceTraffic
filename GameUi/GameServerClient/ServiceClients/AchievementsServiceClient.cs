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
using System.Web;
using SpaceTraffic.Services.Contracts;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Entities;
using System.ServiceModel;

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{
	public class AchievementsServiceClient : ServiceClientBase<IAchievementsService>, IAchievementsService
	{

		public Entities.Achievements GetAchievements()
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IAchievementsService).GetAchievements();
			}
		}

		public List<TAchievement> GetUnviewedAchievements(int playerId)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IAchievementsService).GetUnviewedAchievements(playerId);
			}
		}

		public List<EarnedAchievement> GetEarnedAchievements(int playerId)
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IAchievementsService).GetEarnedAchievements(playerId);
			}
		}

		public Entities.ExperienceLevels GetExperienceLevels()
		{
			using (var channel = this.GetClientChannel())
			{
				return (channel as IAchievementsService).GetExperienceLevels();
			}
		}
	}
}