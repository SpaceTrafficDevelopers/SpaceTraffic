using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game;
using System.Diagnostics;

namespace SpaceTraffic.Data
{
    /// <summary>
    /// Represents wormhole connection definition.
    /// Used in GalaxyMapLoader.
    /// </summary>
    public class GalaxyMapConnection
    {
        private GalaxyMapConnectionEnd[] connectionEnds = new GalaxyMapConnectionEnd[2];

        #region Indexers
        /// <summary>
        /// Gets the <see cref="SpaceTraffic.Data.GalaxyMapConnection.GalaxyMapConnectionEnd"/> with the specified i.
        /// Valid values are 0 and 1.
        /// </summary>
        public GalaxyMapConnectionEnd this[int index]
        {
            get
            {
                return connectionEnds[index];
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="GalaxyMapConnection"/> class.
        /// </summary>
        /// <param name="starSystem1Name">Name of the first star system.</param>
        /// <param name="wormholeEndpoint1Id">Id of the first wormhole endpoint.</param>
        /// <param name="starSystem2Name">Name of the second star system.</param>
        /// <param name="wormholeEndpoint2Id">Id of the second wormhole endpoint.</param>
        public GalaxyMapConnection(string starSystem1Name, int wormholeEndpoint1Id, string starSystem2Name, int wormholeEndpoint2Id)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(starSystem1Name), "starSystem1Name is null, empty or whitespace");
            Debug.Assert(!String.IsNullOrWhiteSpace(starSystem2Name), "starSystem2Name is null, empty or whitespace");
            Debug.Assert(wormholeEndpoint1Id >= 0, "wormholeEndpoint1Id < 0");
            Debug.Assert(wormholeEndpoint2Id >= 0, "wormholeEndpoint1Id < 0");

            this.connectionEnds[0] = new GalaxyMapConnectionEnd(starSystem1Name, wormholeEndpoint1Id);
            this.connectionEnds[1] = new GalaxyMapConnectionEnd(starSystem2Name, wormholeEndpoint2Id);
        }
        #endregion

        /// <summary>
        /// Connects the wormhole in given map according to connection defined in this instance.
        /// </summary>
        /// <param name="map">The map on which this connection will be realised.</param>
        public void ConnectWormhole(GalaxyMap map)
        {
            WormholeEndpoint endpoint1 = GetConnectionEndpoint(map, this[0]);
            WormholeEndpoint endpoint2 = GetConnectionEndpoint(map, this[1]);

            if (endpoint1.IsConnected)
                throw new GalaxyMapBuildingException(
                    String.Format("Endpoint already connected: {0} to {1}. Connection {2} cannot be set.",
                    endpoint1, endpoint1.Destination, this)
                );

            if (endpoint2.IsConnected)
                throw new GalaxyMapBuildingException(
                    String.Format("Endpoint already connected: {0} to {1}. Connection {2} cannot be set.",
                    endpoint2, endpoint2.Destination, this)
                );

            endpoint1.ConnectTo(endpoint2);
        }

        /// <summary>
        /// Gets the connection endpoint.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        /// <exception cref="GalaxyMapBuildingException">If endpoint cannot be found.</exception>
        private WormholeEndpoint GetConnectionEndpoint(GalaxyMap map, GalaxyMapConnectionEnd end)
        {
            StarSystem starSystem = map[end.StarSystemName];
            if (starSystem == null)
                throw new GalaxyMapBuildingException(
                    String.Format("Invalid connection: {0}, star system '{1}' not found.",
                    this, end.StarSystemName)
                );

            WormholeEndpoint endpoint = starSystem.WormholeEndpoints[end.WormholeEndpointId];
            if (endpoint == null)
                throw new GalaxyMapBuildingException(
                    String.Format("Invalid connection: {0}, wormhole endpoint '{1}' not found in star system '{2}'.",
                    this, end.WormholeEndpointId, end.StarSystemName)
                );

            return endpoint;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("'{0}:{1}'<->'{2}:{3}'",
                this.connectionEnds[0].StarSystemName, this.connectionEnds[0].WormholeEndpointId,
                this.connectionEnds[1].StarSystemName, this.connectionEnds[1].WormholeEndpointId);
        }

        #region Class GalaxyMapConnectionEnd
        /// <summary>
        /// Representation of one side of the connection.
        /// </summary>
        public class GalaxyMapConnectionEnd
        {
            #region Properties
            /// <summary>
            /// Gets the name of the star system.
            /// </summary>
            /// <value>
            /// The name of the star system.
            /// </value>
            public string StarSystemName { get; private set; }
            /// <summary>
            /// Gets the wormhole endpoint id.
            /// </summary>
            public int WormholeEndpointId { get; private set; }
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="GalaxyMapConnectionEnd"/> class.
            /// </summary>
            /// <param name="starSystemName">Name of the star system.</param>
            /// <param name="wormholeEndpointId">The wormhole endpoint id.</param>
            public GalaxyMapConnectionEnd(string starSystemName, int wormholeEndpointId)
            {
                this.StarSystemName = starSystemName;
                this.WormholeEndpointId = wormholeEndpointId;
            }
            #endregion
        }
        #endregion
    }
        


}
