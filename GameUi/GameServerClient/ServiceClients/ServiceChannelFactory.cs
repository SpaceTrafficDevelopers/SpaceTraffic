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
using System.ServiceModel;
using SpaceTraffic.Services.Contracts;

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{

    /// <summary>
    /// Creates channels of given service.
    /// </summary>
    public static class ServiceChannelFactory
    {
        /*TODO: Optimalization: caching channels, http://www.codeproject.com/KB/silverlight/SynchronousSilverlight.aspx#ManagingChannelsInAnEfficientManner 
         and http://www.devproconnections.com/article/net-framework2/wcf-proxies-to-cache-or-not-to-cache
        */

        /// <summary>
        /// Gets the client channel.
        /// </summary>
        /// <typeparam name="T">Service contract interface</typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>Opened channel to the service.</returns>
        public static IClientChannel GetClientChannel<T>(string endpoint)
        {
            return (IClientChannel)CreateChannelFactory<T>(endpoint).CreateChannel();
        }

        private static ChannelFactory<T> CreateChannelFactory<T>(string endpoint)
        {
            return new ChannelFactory<T>(endpoint);
        }
    }
}