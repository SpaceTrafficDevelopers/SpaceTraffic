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
	/// Allows to manipulate with players' statistics data.
	/// </summary>
	public class StatisticsManager : IStatisticsManager
	{
		private IGameServer gameServer;

		/// <summary>
		/// Init the manager.
		/// </summary>
		/// <param name="gameServer">Concrete game server</param>
		public StatisticsManager(IGameServer gameServer)
		{
			this.gameServer = gameServer;
		}

		/// <summary>
		/// Decrement a single statistic.
		/// </summary>
		/// <param name="player">Owner of statistics.</param>
		/// <param name="statisticName">Statistic name.</param>
		/// <param name="declineBy">Decremenatation amount.</param>
		public void DecrementStatisticItem(Player player, string statisticName, int declineBy)
		{
			if (declineBy <= 0)
			{
				throw new ArgumentOutOfRangeException("The decrement has to be positive.");
			}

			Statistic statToUpdate = getStatToUpdate(player, statisticName);

			statToUpdate.StatValue -= declineBy;

			// save the change to DB
			IStatisticDAO statisticDao = gameServer.Persistence.GetStatisticsDAO();
			statisticDao.UpdateStatisticById(statToUpdate);

			CheckAchievementsUnlocks(player);
		}

		/// <summary>
		/// Allways gives a row with statistics. When it have not existed before, creates one.
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="statisticName">Name of the statistic.</param>
		/// <returns></returns>
		private Statistic getStatToUpdate(Player player, string statisticName)
		{
			Statistic statToUpdate = player.Statistics.FirstOrDefault(x => x.StatName.Equals(statisticName));
			if(statToUpdate == null){//if does not exist, is added
				IStatisticDAO statisticDao = gameServer.Persistence.GetStatisticsDAO();
				statisticDao.InsertStatistic(new Statistic() { StatName = statisticName, PlayerId = player.PlayerId, StatValue = 0 });
				statToUpdate = statisticDao.GetStatisticsByPlayerId(player.PlayerId).FirstOrDefault(x => x.StatName.Equals(statisticName));
				player.Statistics.Add(statToUpdate);
			}
			return statToUpdate;
		}


		/// <summary>
		/// Increment a single statistic.
		/// </summary>
		/// <param name="player">Owner of statistics.</param>
		/// <param name="statisticName">Statistic name.</param>
		/// <param name="riseBy">Incrementation amount.</param>
		public void IncrementStatisticItem(Player player, string statisticName, int riseBy)
		{
			if (riseBy <= 0)
			{
				throw new ArgumentOutOfRangeException("The increment has to be positive.");
			}

			Statistic statToUpdate = getStatToUpdate(player, statisticName);

			statToUpdate.StatValue += riseBy;

			// save the change to DB
			IStatisticDAO statisticDao = gameServer.Persistence.GetStatisticsDAO();
			statisticDao.UpdateStatisticById(statToUpdate);

			CheckAchievementsUnlocks(player);
		}

		/// <summary>
		/// Set a single statistic value.
		/// </summary>
		/// <param name="player">Owner of statistics.</param>
		/// <param name="statisticName">Statistic name.</param>
		/// <param name="value">New value.</param>
		public void SetStatisticItemTo(Player player, string statisticName, int value)
		{
			Statistic statToUpdate = getStatToUpdate(player, statisticName);

			statToUpdate.StatValue = value;

			// save the change to DB
			IStatisticDAO statisticDao = gameServer.Persistence.GetStatisticsDAO();
			statisticDao.UpdateStatisticById(statToUpdate);

			CheckAchievementsUnlocks(player);
		}

		/// <summary>
		/// Increment player experiences.
		/// </summary>
		/// <param name="player">Player who to increase.</param>
		/// <param name="riseBy">Incrementation amount.</param>
		public void IncrementExperiences(Player player, int riseBy)
		{
			if (riseBy <= 0)
			{
				throw new ArgumentOutOfRangeException("The increment has to be positive.");
			}

			player.Experiences += riseBy;

			CheckPlayersLevel(player);

			// save experience related changes to DB
			IPlayerDAO playerDao = gameServer.Persistence.GetPlayerDAO();
			playerDao.UpdatePlayerById(player);
		}

		/// <summary>
		/// Check state of player's experience level.
		/// </summary>
		/// <param name="player">Player to check.</param>
		private void CheckPlayersLevel(Player player)
		{
			Entities.ExperienceLevels globalExperienceLevels = gameServer.World.ExperienceLevels;

			// assume the levels are sorted lo->hi
			for (int i = globalExperienceLevels.Items.Count - 1; i >= 0; i--)
			{
				Entities.TLevel level = globalExperienceLevels.Items[i];

				// player has more than required experiences although still has lower level
				if (level.RequiredXP <= player.Experiences && player.ExperienceLevel < level.LevelID)
				{
					player.ExperienceLevel = level.LevelID;
					break;
				}
			}
		}

		/// <summary>
		/// Check state of player's achievements.
		/// </summary>
		/// <param name="player">Player to check.</param>
		private void CheckAchievementsUnlocks(Player player)
		{
			Entities.Achievements globalAchievements = gameServer.World.Achievements;

			// filter only achievements which haven't been unlocked yet
			IEnumerable<TAchievement> achievementsToCheck = getAchievementsToCheck(player, globalAchievements);

			// all relevant achievements
			foreach (TAchievement achievement in achievementsToCheck)
			{
				// achievement should be unlocked
				if (isAchievementUnlocked(achievement, player))
				{
					unlockAchievement(achievement, player);
				}
			}
		}

		/// <summary>
		/// Filters only achievements which haven't been unlocked yet
		/// </summary>
		/// <param name="player">The player.</param>
		/// <param name="globalAchievements">The global achievements.</param>
		/// <returns></returns>
		private IEnumerable<TAchievement> getAchievementsToCheck(Player player, Achievements globalAchievements)
		{
			return from a in globalAchievements.Items
				where player.EarnedAchievements.FirstOrDefault(x => x.AchievementId.Equals(a.AchievementID)) == null
				select a;
		}

		/// <summary>
		/// Checks if achievements conditions are met.
		/// </summary>
		/// <param name="achievement">The achievement.</param>
		/// <param name="player">The player.</param>
		/// <returns></returns>
		private bool isAchievementUnlocked(TAchievement achievement, Player player) {
			foreach (TCondition condition in achievement.Conditions.AchievementConditions)
			{
				Statistic statToCheck = player.Statistics.FirstOrDefault(x => x.StatName.Equals(condition.CondName));

				// condition is not met by current statistic value
				if (statToCheck == null || statToCheck.StatValue < condition.CondValue)
				{
					return false;
				}
			}
			return true;
		}


		/// <summary>
		/// Unlocks the achievement by adding it into earnedAchievements table.
		/// </summary>
		/// <param name="achievement">The achievement.</param>
		/// <param name="player">The player.</param>
		private void unlockAchievement(TAchievement achievement, Player player) {
			EarnedAchievement newlyEarnedAchievement = new EarnedAchievement();
			newlyEarnedAchievement.IsJustEarned = true;
			newlyEarnedAchievement.PlayerId = player.PlayerId;
			newlyEarnedAchievement.UnlockedAt = gameServer.Game.currentGameTime.Value;
			newlyEarnedAchievement.AchievementId = achievement.AchievementID;

			// add the earned achievement to DB
			IEarnedAchievementDAO earnedDao = gameServer.Persistence.GetEarnedAchievementDAO();
			earnedDao.InsertEarnedAchievement(newlyEarnedAchievement);

			player.EarnedAchievements.Add(newlyEarnedAchievement);

			// reward player with some experiences 
			gameServer.Statistics.IncrementExperiences(player, ExperienceLevels.XP_FOR_ACHIEVEMENT_GET);
		}

	}
}
