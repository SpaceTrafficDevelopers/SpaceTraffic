﻿/**
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
using SpaceTraffic.Game;
using SpaceTraffic.Entities;

namespace SpaceTraffic.Services.Contracts
{
    [ServiceContract]
    public interface IGameService
    {
        [OperationContract]
        IList<WormholeEndpointDestination> GetStarSystemConnections(string starSystem);

        [OperationContract]
        IList<StarSystem> GetGalaxyMap(string galaxyMap);

		[OperationContract]
		SpaceTraffic.Entities.Base GetBaseByName(string planetName);

		[OperationContract]
		IList<SpaceTraffic.Entities.Base> GetAllBases();

        [OperationContract]
        int PerformAction(int playerId, string actionName, params object[] actionArgs);

        [OperationContract]
        object GetActionResult(int playerId, int actionCode);

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
