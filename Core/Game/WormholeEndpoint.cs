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
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Game
{
    /// <summary>
    /// Represents wormhole endpoint in the star system.
    /// </summary>
    public class WormholeEndpoint: VisibleObject
    {
        #region Properties
        /// <summary>
        /// This is a local index within a system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// This indicates whether the endpoint is connected to other endpoint in a wormhole or not
        /// </summary>
        public bool IsConnected
        {
            get { return this.Destination != null; }
        }

        /// <summary>
        /// Property OtherSide points to the other endpoint in the wormhole
        /// </summary>
        public WormholeEndpoint Destination { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WormholeEndpoint"/> class.
        /// </summary>
        /// <param name="id">The id of the wormhole endpoint.</param>
        /// <param name="starSystem">The star system containing this endpoint.</param>
        /// <param name="trajectory">The trajectory of the endpoint.</param>
        public WormholeEndpoint(int id, StarSystem starSystem, Trajectory trajectory)
            : base(starSystem, trajectory)
        {
            this.Id = id;
            this.Destination = null;
        }
        #endregion

        public void ConnectTo(WormholeEndpoint targetEndpoint)
        {
            if (this.IsConnected)
                throw new InvalidOperationException("This endpoint already connected.");
            if (targetEndpoint.IsConnected)
                throw new InvalidOperationException("Target endpoint already connected.");

            this.Destination = targetEndpoint;
            targetEndpoint.Destination = this;
        }

        public override string ToString()
        {
            return String.Format("WormholeEndpoint[{0}:{1}]",this.StarSystem.Name,this.Id);
        }
    }
}
