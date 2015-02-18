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
using System.Web;

namespace SpaceTraffic.GameUi.GameServerClient
{
    /// <summary>
    /// This class encapsulates whole idea of access of standalone GameServer application, called from this web frontend.
    /// It provides instance of client class, which provides access to GameServer app.
    /// </summary>
    public static class GameServerClientFactory
    {
        private static IGameServerClient clientIntance;

        //TODO: read from configuration
        private static Type _ClientType = typeof(WCFGameServerClient);

        /// <summary>
        /// Gets or sets the type of the client.
        /// </summary>
        /// <value>
        /// The type of the client. Must be implementation of <see>IGameServerClient</see>.
        /// </value>
        public static Type ClientType
        {
            get { return _ClientType; }
            set
            {
                if(! value.IsSubclassOf(typeof(IGameServerClient)))
                    throw new ArgumentException("Given type must implement IGameServerClient");
                _ClientType = value;
                clientIntance = null;
            }
        }

        /// <summary>
        /// Gets the client instance.
        /// Instance is singleton.
        /// </summary>
        /// <returns></returns>
        public static IGameServerClient GetClientInstance()
        {
            if (clientIntance == null)
            {
                CreateGameServerClientInstance();
            }
            return clientIntance;
        }


        private static void CreateGameServerClientInstance()
        {
            //TODO: Instance based on configuration
            clientIntance = (IGameServerClient) Activator.CreateInstance(ClientType); 
        }
    }
}