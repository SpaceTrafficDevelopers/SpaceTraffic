using System.Xml;
using SpaceTraffic.Game;
using System.Collections.Generic;
using System.IO;
using NLog;
using System.Diagnostics;
using System.Text;
using System;


namespace SpaceTraffic.Data
{
    /// <summary>
    /// This static class is used to generate Star system and it's subordinate object like planets and stars from XML file. 
    /// It use extension methods from PT2XmlParser.
    /// </summary>
    public class GalaxyMapLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GalaxyMap LoadGalaxyMap(string mapName, IGalaxyMapDataStreamProvider dataService)
        {
            using (Stream stream = dataService.GetGalaxyMapStream(mapName))
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.IgnoreComments = true;
                using (XmlReader reader = XmlReader.Create(stream, readerSettings))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    XmlNode galaxyNode = doc.GetElementsByTagName("galaxy")[0];

                    GalaxyMap map = new GalaxyMap();
                    map.MapName = galaxyNode.Attributes["name"].Value.ToString();
                    IList<string> starSystemNames = null;
                    IList<GalaxyMapConnection> connections = null;
                    foreach (XmlNode childNode in galaxyNode.ChildNodes)
                    {
                        switch (childNode.Name.ToLowerInvariant())
                        {
                            case "starsystems":
                                starSystemNames = childNode.ParseStarSystems();
                                break;
                            case "wormholes":
                                connections = childNode.ParseWormholes();
                                break;
                            default:
                                throw new XmlException("Unexpected childNode.");
                        }
                    }
                    Debug.Assert(starSystemNames != null, "starSystemNames is null");
                    Debug.Assert(connections != null, "connections is null");


                    this.LoadStarSystems(starSystemNames, dataService, map);
                    this.ApplyConnections(connections, map);

                    return map;
                }
            }
        }

        /// <summary>
        /// Loads the star systems defined in the map.
        /// </summary>
        /// <param name="starSystems">The list of star systems.</param>
        /// <param name="streamProvider">Provider of map data streams.</param>
        /// <param name="map">The map.</param>
        private void LoadStarSystems(IList<string> starSystems, IGalaxyMapDataStreamProvider streamProvider, GalaxyMap map)
        {
            StarSystemLoader loader = new StarSystemLoader();
            StarSystem starSystem;
            foreach (string starSystemName in starSystems)
            {
                
                starSystem = loader.LoadStarSystem(starSystemName, streamProvider);
                if (map.ContainsKey(starSystem.Name))
                {
                    throw new XmlException("Duplicate star system name in starsystems");
                }
                else
                {
                    map.Add(starSystem);
                }
            }
        }

        /// <summary>
        /// Applies the connections of wormhole endpoints.
        /// </summary>
        /// <param name="connections">The connections.</param>
        /// <param name="map">The galaxy map.</param>
        private void ApplyConnections(IList<GalaxyMapConnection> connections, GalaxyMap map)
        {
            foreach (GalaxyMapConnection connection in connections)
            {
                logger.Info("Connectiong: {0}", connection);
                connection.ConnectWormhole(map);
            }
        }
    }
}
