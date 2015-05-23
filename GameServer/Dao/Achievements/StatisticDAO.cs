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
    public class StatisticDAO : AbstractDAO, IStatisticDAO
    {

        public List<Statistic> GetStatistic()
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Statistics.ToList<Statistic>();
            }
        }

        public List<Statistic> GetStatisticsByPlayerId(int playerId)
        {
            using (var contextDB = CreateContext())
            {
                return (from x in contextDB.Statistics
                        where x.PlayerId.Equals(playerId)
                        select x).ToList<Statistic>();
            }
        }

        public Statistic GetStatisticById(int statisticId)
        {
            using (var contextDB = CreateContext())
            {
                return contextDB.Statistics.FirstOrDefault(x => x.StatisticsId.Equals(statisticId));
            }
        }

        public bool InsertStatistic(Statistic statistic)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    // add statistic to context
                    contextDB.Statistics.Add(statistic);
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

        public bool RemoveStatisticById(int statisticId)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var statisticsTab = contextDB.Statistics.FirstOrDefault(x => x.StatisticsId.Equals(statisticId));
                    // remove statistic from context
                    contextDB.Statistics.Remove(statisticsTab);
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

        public bool UpdateStatisticById(Statistic statistic)
        {
            using (var contextDB = CreateContext())
            {
                try
                {
                    var statisticsTab = contextDB.Statistics.FirstOrDefault(x => x.StatisticsId.Equals(statistic.StatisticsId));

                    statisticsTab.StatName = statistic.StatName;
                    statisticsTab.StatValue = statistic.StatValue;
                    statisticsTab.PlayerId = statistic.PlayerId;
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

