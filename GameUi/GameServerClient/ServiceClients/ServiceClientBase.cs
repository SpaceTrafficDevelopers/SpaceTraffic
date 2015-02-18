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

namespace SpaceTraffic.GameUi.GameServerClient.ServiceClients
{
    /// <summary>
    /// Base class for all service clients.
    /// Provides means to aquire channels to service.
    /// Child classes should also implement service contract interface.
    /// </summary>
    /// <typeparam name="T">Service contract interface</typeparam>
    public abstract class ServiceClientBase<T>
    {
        /// <summary>
        /// Gets or sets the name of the endpoint.
        /// </summary>
        /// <value>
        /// The name of the endpoint.
        /// </value>
        public string EndpointName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientBase&lt;T&gt;"/> class.
        /// Endpoint is set to the name of service contract interface without leading "I" character.
        /// </summary>
        public ServiceClientBase()
        {
            //TODO: Endpoint name from configuration file instead of this hardcoded guessing.
            this.EndpointName = typeof(T).Name.Substring(1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        public ServiceClientBase(string endpointName)
        {
            this.EndpointName = endpointName;
        }

        /// <summary>
        /// Gets the client channel.
        /// </summary>
        /// <returns>channel instance created by <see cref="ServiceChannelFactory"/></returns>
        protected IClientChannel GetClientChannel()
        {
            return ServiceChannelFactory.GetClientChannel<T>(this.EndpointName);
        }
    }
}