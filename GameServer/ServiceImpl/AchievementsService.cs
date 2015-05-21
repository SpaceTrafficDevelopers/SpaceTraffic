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
using SpaceTraffic.Services.Contracts;
using NLog;
using SpaceTraffic.Utils.Debugging;
using GS = SpaceTraffic.GameServer.GameServer;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Engine;
using SpaceTraffic.Game.Actions;
using SpaceTraffic.Entities;
using SpaceTraffic.Dao;
using SpaceTraffic.Game.Planner;
using SpaceTraffic.Game.Navigation;
using SpaceTraffic.Game;

namespace SpaceTraffic.GameServer.ServiceImpl
{
	public class AchievementsService : IAchievementsService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();


		public Entities.Achievements GetAchievements()
		{
			return GS.CurrentInstance.World.Achievements;
		}

		public Entities.TAchievement GetAchievementById(int id)
		{
			return GameServer.CurrentInstance.World.GetAchievementById(id);
		}

		public Entities.ExperienceLevels GetExperienceLevels()
		{
			return GameServer.CurrentInstance.World.ExperienceLevels;
		}

		public List<EarnedAchievement> GetEarnedAchievements(int playerId)
		{

			EarnedAchievementDAO earnedDao = (EarnedAchievementDAO)GameServer.CurrentInstance.Persistence.GetEarnedAchievementDAO();
			return earnedDao.GetEarnedAchievementsByPlayerId(playerId);
		}


		public List<TAchievement> GetUnviewedAchievements(int playerId)
		{

			List<TAchievement> result = new List<TAchievement>();
			Player player = GS.CurrentInstance.Persistence.GetPlayerDAO().GetPlayerById(playerId);

			if (player != null)
			{
				EarnedAchievementDAO earnedDao = (EarnedAchievementDAO)GameServer.CurrentInstance.Persistence.GetEarnedAchievementDAO();
				var unviewedAchievements = earnedDao.GetUnviewedAchievementsByPlayerId(playerId);

				foreach (EarnedAchievement earnedAchv in unviewedAchievements)
				{
					result.Add(GetAchievementById(earnedAchv.AchievementId));
					earnedAchv.IsJustEarned = false;

					// update earned status in DB
					earnedDao.UpdateEarnedAchievementById(earnedAchv);
				}
			}

			return result;
		}
		
	}
}
