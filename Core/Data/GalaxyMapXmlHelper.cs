using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SpaceTraffic.Game;
using SpaceTraffic.Xml;

namespace SpaceTraffic.Data
{
    public static class GalaxyMapXmlHelper
    {
        /// <summary>
        /// Extension method for StarSystems.
        /// </summary>
        /// <param name="trajectoryNode">Node with all star systems in galaxy</param>
        public static IList<string> ParseStarSystems(this XmlNode starSystemsNode)
        {
            IList<string> starSystemNames = new List<string>();
            string starSystemName = "";
            foreach (XmlNode childNode in starSystemsNode.ChildNodes)
            {
                starSystemName = childNode.Attributes["name"].Value;
                starSystemNames.Add(starSystemName);
            }
            return starSystemNames;
        }

        /// <summary>
        /// Extension method for parsing wormholes.
        /// </summary>
        /// <param name="trajectoryNode">Node with all wormholes in galaxy</param>
        public static IList<GalaxyMapConnection> ParseWormholes(this XmlNode wormholesNode)
        {
            IList<GalaxyMapConnection> connections = new List<GalaxyMapConnection>();
            GalaxyMapConnection connection;

            foreach (XmlNode childNode in wormholesNode.ChildNodes)
            {
                string firstStarSystemName = childNode.FirstChild.Attributes["system"].Value;
                string secondStarSystemName = childNode.ChildNodes[1].Attributes["system"].Value;
                int firstWormholeEndpoint = childNode.FirstChild.Attributes["id"].IntValue();
                int secondWormholeEndpoint = childNode.ChildNodes[1].Attributes["id"].IntValue();

                connection = new GalaxyMapConnection(firstStarSystemName, firstWormholeEndpoint, secondStarSystemName, secondWormholeEndpoint);
                connections.Add(connection);
                //WormholeEndpoint endpoint1 = galaxy[NameOfFirstEndpoint].WormholeEndpoints[IdOfFirstEndpoint];
                //WormholeEndpoint endpoint2 = galaxy[NameOfSecondEndpoint].WormholeEndpoints[IdOfSecondEndpoint];

                //endpoint1.Destination = endpoint2;
                //endpoint2.Destination = endpoint1;
            }
            return connections;
        }
    }
}
