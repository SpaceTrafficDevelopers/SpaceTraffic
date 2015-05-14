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
using SpaceTraffic.Entities;
using SpaceTraffic.Engine;
using SpaceTraffic.Dao;

namespace SpaceTraffic.GameServer
{
	/// <summary>
	/// 
	/// </summary>
	public class StatisticsManager : IStatisticsManager
	{
		private IGameServer gameServer;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameServer"></param>
		public StatisticsManager(IGameServer gameServer)
		{
			this.gameServer = gameServer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="statisticName"></param>
		/// <param name="riseBy"></param>
		public void IncrementStatisticItem(Player player, string statisticName, int riseBy)
		{
			if (riseBy <= 0)
			{
				throw new ArgumentOutOfRangeException("The increment has to be positive.");
			}

			Statistic statToUpdate = player.Statistics.FirstOrDefault(x => x.StatName.Equals(statisticName));

			if (statToUpdate == null)
			{
				throw new ArgumentException("Unknown statistic name.");
			}

			statToUpdate.StatValue += riseBy;

			// save the change to DB
			IStatisticDAO statisticDao = gameServer.Persistence.GetStatisticsDAO();
			statisticDao.UpdateStatisticById(statToUpdate);

			CheckAchievementsUnlocks(player);
		}

		/// <summary>
		/// 
		/// </summary>
		private void CheckAchievementsUnlocks(Player player)
		{
			Entities.Achievements globalAchievements = gameServer.World.Achievements;

			// filter only achievements which haven't been unlocked yet
			IEnumerable<TAchievement> achievementsToCheck =
			from a in globalAchievements.Items
			where player.EarnedAchievements.FirstOrDefault(x => x.AchievementId.Equals(a.AchievementID)) == null
			select a;

			// all relevant achievements
			foreach (TAchievement achievement in achievementsToCheck)
			{
				bool shouldSkip = false;

				// all conditions of each achievement
				foreach (TCondition condition in achievement.Conditions.AchievementConditions)
				{
					Statistic statToCheck = player.Statistics.FirstOrDefault(x => x.StatName.Equals(condition.CondName));

					if (statToCheck == null)
					{
						throw new MissingFieldException("Statistic {0} doesn't exist.", condition.CondName);
					}

					// condition is not met by current statistic value
					if (statToCheck.StatValue < condition.CondValue)
					{
						shouldSkip = true;
						break;
					}
				}

				// achievement should be unlocked
				if (!shouldSkip)
				{
					EarnedAchievement newlyEarnedAchievement = new EarnedAchievement();
					newlyEarnedAchievement.IsJustEarned = true;
					newlyEarnedAchievement.PlayerId = player.PlayerId;
					newlyEarnedAchievement.UnlockedAt = gameServer.Game.currentGameTime.Value;
					newlyEarnedAchievement.AchievementId = achievement.AchievementID;

					// add the earned achievement to DB
					IEarnedAchievementDAO earnedDao = gameServer.Persistence.GetEarnedAchievementDAO();
					earnedDao.InsertEarnedAchievement(newlyEarnedAchievement);

					player.EarnedAchievements.Add(newlyEarnedAchievement);
				}
			}

		}

	}
}

