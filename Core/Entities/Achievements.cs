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
	/// <summary>
	/// The class Achievements contains all achievements
	/// </summary>
	[System.Serializable()]
	[System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "SpaceTrafficData")]
	[System.Xml.Serialization.XmlRoot(ElementName = "Achievements", IsNullable = false, Namespace = "SpaceTrafficData")]
	public class Achievements
	{
		#region "Fields and Properties"

		/// <summary>
		/// List of factories/>
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "Achievement")]
		public List<TAchievement> Items { get; set; }

		#endregion

		#region "Constructors"

		public Achievements()
		{
			Items = new List<TAchievement>();
		}

		#endregion

		#region "Private Methods"
		#endregion

		#region "Public Methods"

		public TAchievement GetAchievement(int achievementId)
		{
			if (Items.Exists(p => p.AchievementID == achievementId))
			{
				return Items.First(p => p.AchievementID == achievementId);
			}

			return null;
		}

		#endregion
	}

	/// <summary>
	/// Description of a single achievement
	/// </summary>
	[System.Serializable()]
	[System.Xml.Serialization.XmlType(Namespace = "SpaceTrafficData")]
	public class TAchievement
	{
		#region "Fields and Properties"

		[System.Xml.Serialization.XmlAttribute(AttributeName = "achievementID")]
		public int AchievementID { get; set; }

		[System.Xml.Serialization.XmlElement(ElementName = "Name")]
		public string Name { get; set; }

		[System.Xml.Serialization.XmlElement(ElementName = "Conditions")]
		//public SpaceTraffic.Utils.Collections.SerializableDictionary<String, int> conditions = new SpaceTraffic.Utils.Collections.SerializableDictionary<String, int>();

		public HashSet<TConditions> conditions { set; get; }


		#endregion

		#region "Constructors"

		/// <summary>
		/// 
		/// </summary>
		public TAchievement()
		{
		}

		#endregion

		#region "Private Methods"
		#endregion

		#region "Public Methods"
		/// <summary>
		/// 
		/// </summary>
		public void Link()
		{
			try
			{
				foreach (TConditions condition in conditions)
				{
					Statistics.events[condition.CondName] += Change;
				}
			}
			catch (KeyNotFoundException exception)
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Error("Achievement and stats liking failed:{0}", exception.Message, exception);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="arg"></param>
		public void Change(object sender, string arg)
		{
			Logger logger = LogManager.GetCurrentClassLogger();
			logger.Debug("{0}: {1} - {2}", Name, (sender as Statistics).StatisticsId, arg);
		}
		#endregion
	}


	/// <summary>
	/// Description of achievement's conditions
	/// </summary>
	[System.Serializable()]
	[System.Xml.Serialization.XmlType(Namespace = "SpaceTrafficData")]
	public class TConditions
	{
		#region "Fields and Properties"
		[System.Xml.Serialization.XmlElement(ElementName = "CondName")]
		public string CondName { set; get; }

		[System.Xml.Serialization.XmlElement(ElementName = "CondValue")]
		public int CondValue { set; get; }

		#endregion

		#region "Constructors"

		public TConditions()
		{

		}
		#endregion

	}



}
