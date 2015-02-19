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
using SpaceTraffic.GameServer.Configuration;
using SpaceTraffic.GameServer.ServiceImpl;
using System.ServiceModel.Description;
using NLog;
using SpaceTraffic.Utils.Collections;
using SpaceTraffic.Engine;

namespace SpaceTraffic.GameServer
{
    /// <summary>
    /// Manages WCF services published by GameServer.
    /// </summary>
    class ServiceManager : IServiceManager
    {
        /// <summary>
        /// ServicesManager states.
        /// </summary>
        public enum States
        {
            CREATED,
            INITIALIZED,
            RUNNING,
            STOPPED,
            ERROR
        }

        #region Fields
        private Logger Logger = LogManager.GetCurrentClassLogger();
        
        private IList<Type> _ServiceList;
        
        private Dictionary<string, ServiceHost> hosts;
        #endregion
   

        #region Properties
        
        //TODO: Thread-safe state changes.
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public States State { get; set; }

        /// <summary>
        /// Gets or sets the list of services.
        /// List contain service contract types.
        /// </summary>
        /// <value>
        /// The list of services.
        /// </value>
        public IList<Type> ServiceList
        {
            get { return this._ServiceList; }
            set
            {
                if(this.State == States.CREATED)
                {
                    _ServiceList = value;
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Cannot change service list after initialization."));
                }
            }
             
        }
        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Logger.Trace("ENTRY ServicesManager.Initialize");

            if(this.State != States.CREATED)
                throw new InvalidOperationException(String.Format("Cannot be initialized from current state ({0}).", this.State));

            if (this.ServiceList != null)
            {
                ServiceHost host;
                this.hosts = new Dictionary<string, ServiceHost>();

                try
                {
                    foreach (Type serviceType in this.ServiceList)
                    {
                        host = new ServiceHost(serviceType);
                        hosts.Add(serviceType.Name, host);
                        
                    }
                    this.State = States.INITIALIZED;
                }
                catch (Exception)
                {
                    this.State = States.ERROR;
                    throw;
                }

            }

            Logger.Trace("EXIT ServicesManager.Initialize");
        }

        /// <summary>
        /// Starts all services managed by this instance.
        /// </summary>
        public void Start()
        {
            Logger.Trace("ENTRY ServicesManager.Start");

            // Ignore start call if already running (throw exception when debugging).
#if DEBUG
            if(this.State == States.RUNNING)
                throw new InvalidOperationException("Already started.");
#else
            if(this.State == States.RUNNING)
                return;
#endif
            if((this.State != States.INITIALIZED) && (this.State != States.STOPPED))
                throw new InvalidOperationException(String.Format("Cannot be initialized from current state ({0}).", this.State));

            //this.State = States.STARTING;

            try
            {
                ServiceHost host;
                foreach (Type serviceType in this.ServiceList)
                {
                    Logger.Info("Starting service: {0}", serviceType.Name);
                    host = this.hosts[serviceType.Name];
                    host.Open();
                    Logger.Info("Service running: {0} State={1} Endpoint='{2}' BaseAddresses={3}", serviceType.Name, host.State, host.Description.Endpoints[0].Address, new CollectionToString(host.BaseAddresses));
                }

                this.State = States.RUNNING;
            }
            catch (Exception)
            {
                //TODO: stop already started services.
                this.State = States.ERROR;
                throw;
            }
            Logger.Trace("EXIT ServicesManager.Start");
        }

        /// <summary>
        /// Stops all services managed by this instance.
        /// </summary>
        public void Stop()
        {
            Logger.Trace("ENTRY ServicesManager.Stop");

            // Ignore start call if already running (throw exception when debugging).
#if DEBUG
            if (this.State == States.STOPPED)
                throw new InvalidOperationException("Already stopped.");
#else
            if(this.State == States.RUNNING)
                return;
#endif
            if (this.State != States.RUNNING)
                throw new InvalidOperationException(String.Format("Not running ({0}).", this.State));

            //this.State = States.STOPPING;

            try
            {
                ServiceHost host;
                foreach (Type serviceType in this.ServiceList)
                {
                    Logger.Info("Stopping service: {0}", serviceType.Name);
                    host = this.hosts[serviceType.Name];
                    host.Close();
                    Logger.Info("Service stopped: {0}:{1} STATE={2}", serviceType.Name, host.Description.Endpoints[0].ToString(), host.State);
                }

                this.State = States.RUNNING;
            }
            catch (Exception)
            {
                this.State = States.ERROR;
                throw;
            }
            Logger.Trace("EXIT ServicesManager.Stop");
        }

        public void Dispose()
        {
            if (this.State == States.RUNNING)
                this.Stop();
        }


        //TODO: ServiceHostController class that will provide start/stop functionality for individual hosts with appropriate logging output.
    }
}
