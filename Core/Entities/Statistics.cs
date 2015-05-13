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
using NLog;


namespace SpaceTraffic.Entities
{
	public delegate void StatisticsChangeEventHandler(object sender, string key);

	public class Achievement
	{
		public Dictionary<string, int> conditions = new Dictionary<string, int>();

		public string Name { get; set; }

		public Achievement()
		{
		}

		public void Link()
		{
			foreach (KeyValuePair<string, int> condition in conditions)
			{
				Statistics.events[condition.Key] += Change;
			}
		}

		public void Change(object sender, string arg)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			logger.Debug("{0}: {1} - {2}", Name, (sender as Statistics).StatisticsId, arg);
		}

	}


	[DataContract(Name = "Statistics")]
	public class Statistics
	{
		[DataMember]
		public int StatisticsId { get; set; }

		[DataMember]
		public Dictionary<string, int> monitoredItems = new Dictionary<string, int>();

		public int this[string key]
		{
			get
			{
				return monitoredItems[key];
			}
			set
			{
				monitoredItems[key] = value;
				if (!events.ContainsKey(key))
				{
					events.Add(key, null);
				}
				if (events[key] != null)
				{
					events[key](this, key);
				}

			}
		}

		public static Dictionary<string, StatisticsChangeEventHandler> events = new Dictionary<string, StatisticsChangeEventHandler>();

	}
}
