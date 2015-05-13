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
	/// <summary>
	/// The class Factories contains all factory
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
		[System.Xml.Serialization.XmlElement(ElementName = "achievement")]
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

		public TAchievement GetAchievement(string achievementId)
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
		public string AchievementID { get; set; }

		#endregion

		#region "Constructors"

		public TAchievement()
		{

		}

		#endregion

		#region "Private Methods"
		#endregion

		#region "Public Methods"
		#endregion
	}
/**
	/// <summary>
	/// Description general information about the factory
	/// </summary>
	[System.Serializable()]
	[System.Xml.Serialization.XmlType(Namespace = "SpaceTrafficData")]
	public class TFactoryGeneralInformation
	{
		#region "Fields and Properties"

		/// <summary>
		/// Name of the factory.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "name")]
		public string Name { get; set; }
		/// <summary>
		/// Needed rank to buy.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "rankToBay")]
		public int RankToBay { get; set; }
		/// <summary>
		/// Purchase price of the factory.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "price")]
		public decimal Price { get; set; }
		/// <summary>
		/// Description of the factory.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "description")]
		public string Description { get; set; }
		#endregion

		#region "Constructors"

		public TFactoryGeneralInformation() { }

		#endregion

	}

	/// <summary>
	/// Description resources for produce
	/// </summary>
	[System.Serializable()]
	[System.Xml.Serialization.XmlType(Namespace = "SpaceTrafficData")]
	public class TFactoryResources
	{
		#region "Fields and Properties"

		/// <summary>
		/// List of resources
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "resource")]
		public List<TFactoryResource> Resources { get; set; }
		#endregion

		#region "Constructors"

		public TFactoryResources()
		{
			this.Resources = new List<TFactoryResource>();
		}

		#endregion

	}

	/// <summary> 
	/// Description resource for produce
	/// </summary>
	[System.Serializable()]
	[System.Xml.Serialization.XmlType(Namespace = "SpaceTrafficData")]
	public class TFactoryResource
	{
		#region "Fields and Properties"

		[System.Xml.Serialization.XmlIgnore()]
		public int ID { get; set; }
		/// <summary>
		/// Information about production cycle: needed product.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "product")]
		public string ProductID { get; set; }
		/// <summary>
		/// Information about production cycle: needed quantity by cycle.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "quantityByCycle")]
		public int QuantityByCycle { get; set; }

		#endregion

		#region "Constructors"

		public TFactoryResource(string productID, int quantityByCycle)
		{
			this.ProductID = productID;
			this.QuantityByCycle = quantityByCycle;
		}

		public TFactoryResource() : this(string.Empty, 0) { }

		#endregion

		#region "Private Methods"
		#endregion

		#region "Public Methods"
		#endregion
	}

	/// <summary>
	/// Description produced product and production cycle
	/// </summary>
	[System.Serializable()]
	[System.Xml.Serialization.XmlType(Namespace = "SpaceTrafficData")]
	public class TFactoryManufacturing
	{
		#region "Fields and Properties"

		/// <summary>
		/// Manufacturing: product.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "product")]
		public string ProductID { get; set; }
		/// <summary>
		/// Manufacturing: cycle-time.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "cycle-time")]
		public int CycleTime { get; set; }
		/// <summary>
		/// Manufacturing: quantity by cycle.
		/// </summary>
		[System.Xml.Serialization.XmlElement(ElementName = "quantityByCycle")]
		public int QuantityByCycle { get; set; }

		#endregion

		#region "Constructors"

		public TFactoryManufacturing() { }

		#endregion
	}*/

}
