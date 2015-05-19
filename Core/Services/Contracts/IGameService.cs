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
using System.ServiceModel;
using SpaceTraffic.Entities.PublicEntities;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Services.Contracts
{
	[ServiceContract]
	public interface IGameService
	{
		[OperationContract]
		IList<SpaceShip> GetPlayersShips(int playerId);

		[OperationContract]
		IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem);

		[OperationContract]
		int PerformAction(int playerId, string actionName, params object[] actionArgs);

		[OperationContract]
		object GetActionResult(int playerId, int actionCode);

		[OperationContract]
		bool PlayerHasEnaughCredits(int playerId, long amount);

		[OperationContract]
		Entities.Achievements GetAchievements();

		[OperationContract]
		Entities.ExperienceLevels GetExperienceLevels();

        [OperationContract]
        bool PlayerHasEnaughCreditsForCargo(int playerId, int cargoLoadEntityId, int count);
        
        [OperationContract]
        bool SpaceShipHasCargoSpace(int spaceShipId, int cargoLoadEntityID, int count);

        [OperationContract]
        bool SpaceShipDockedAtBase(int spaceShipId, string starSystemName, string planetName);

        [OperationContract]
        bool PlayerHasSpaceShip(int playerId, int spaceShipId);
        
        [OperationContract]
        bool PlayerHasEnoughCargo(string buyingPlace, int cargoLoadEntityId, int cargoCount);

        [OperationContract]
        bool TraderHasEnoughCargo(int traderId, int cargoLoadEntityId, int cargoCount);

        [OperationContract]
        bool PlayerHasEnoughCargoOnSpaceShip(int spaceShipId, int cargoLoadEntityId, int cargoCount);

        
		[OperationContract]
		List<TAchievement> GetEarnedAchievements(int playerId);

		[OperationContract]
		Player GetPlayer(int playerId);

        int CreatePathPlan(int playerId, int spaceShipId);

        [OperationContract]
        int AddPlanItem(int pathPlanId, string solarSystem, bool isPlanet, string index, int sequenceNumber);
    
        [OperationContract]
        bool AddPlanAction(int planItemId, int sequenceNumber, int playerId, string actionName, params object[] actionArgs);

        [OperationContract]
        bool StartPathPlan(int pathPlanId);
	}

	[Serializable]
	public class ActionNotFoundException : Exception
	{
		public ActionNotFoundException() { }
		public ActionNotFoundException(string message) : base(message) { }
		public ActionNotFoundException(string message, Exception inner) : base(message, inner) { }
		protected ActionNotFoundException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}
