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

namespace SpaceTraffic.Dao
{
    public class EarnedAchievementDAO : AbstractDAO, IEarnedAchievementDAO
    {
        public List<EarnedAchievement> GetEarnedAchievements()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.EarnedAchievements.ToList<EarnedAchievement>();
            }
        }

        public List<EarnedAchievement> GetEarnedAchievementsByPlayerId(int playerId)
        {
            using (var contextDB = CreateContext())
            {
                return (from x in contextDB.EarnedAchievements
                        where x.PlayerId.Equals(playerId)
                        select x).ToList<EarnedAchievement>();
            }
        }

        public EarnedAchievement GetEarnedAchievementById(int earnedAchievementId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.EarnedAchievements.FirstOrDefault(x => x.EarnedAchievementsId.Equals(earnedAchievementId));
            }
        }

		public List<EarnedAchievement> GetUnviewedAchievementsByPlayerId(int playerId)
		{
			using (var contextDB = CreateContext())
			{
				return (from x in contextDB.EarnedAchievements
						where x.PlayerId.Equals(playerId)
						where x.IsJustEarned.Equals(true)
						select x).ToList<EarnedAchievement>();
			}
		}

        public bool InsertEarnedAchievement(EarnedAchievement earnedAchievement)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add earned achievement to context
                    contextDB.EarnedAchievements.Add(earnedAchievement);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveEarnedAchievementById(int earnedAchievementId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var statisticsTab = contextDB.EarnedAchievements.FirstOrDefault(x => x.EarnedAchievementsId.Equals(earnedAchievementId));

                    // remove earned achievement from context
                    contextDB.EarnedAchievements.Remove(statisticsTab);
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool UpdateEarnedAchievementById(EarnedAchievement earnedAchievement)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var earnedAchievementsTab = contextDB.EarnedAchievements.FirstOrDefault(x => x.EarnedAchievementsId.Equals(earnedAchievement.EarnedAchievementsId));

                    earnedAchievementsTab.AchievementId = earnedAchievement.AchievementId;
                    earnedAchievementsTab.UnlockedAt = earnedAchievement.UnlockedAt;
                    earnedAchievementsTab.PlayerId = earnedAchievement.PlayerId;
                    earnedAchievementsTab.IsJustEarned = earnedAchievement.IsJustEarned;
                    // save context to database
                    contextDB.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

		
	}
}