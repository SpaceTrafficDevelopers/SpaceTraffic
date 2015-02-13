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