using System.Collections.Generic;
using System;
using System.Xml;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;
using SpaceTraffic.Xml;

namespace SpaceTraffic.Data
{
    public static class StarSystemXmlHelper
    {

        /// <summary>
        /// Extension method for parsing StarSystem instance from XmlNode.
        /// </summary>
        /// <param name="starSystemNode">Xml node starsystem</param>
        /// <returns>Instance of StarSystem filled with data from xml.</returns>
        public static StarSystem ParseStarSystem(this XmlNode starSystemNode)
        {
            StarSystem starSystem = new StarSystem();
            starSystem.Name = starSystemNode.Attributes["name"].Value;
            int positionX = starSystemNode.Attributes["x"].IntValue();
            int positionY = starSystemNode.Attributes["y"].IntValue();
            starSystem.MapPosition = new Point2d(positionX, positionY);
            foreach (XmlNode childNode in starSystemNode.ChildNodes)
            {
                switch (childNode.Name.ToLowerInvariant())
                {
                    case "star":
                        starSystem.Star = childNode.ParseStar(starSystem);
                        break;
                    case "planets":
                        starSystem.Planets.AddAll(childNode.ParsePlanets(starSystem));
                        break;
                    case "wormholeendpoints":
                        starSystem.WormholeEndpoints.AddAll(childNode.ParseWormholeEndpoints(starSystem));
                        break;
                    default:
                        throw new XmlException("Unexpected space object.");
                }
            }
            return starSystem;
        }

        /// <summary>
        /// Extension method for parsing star.
        /// </summary>
        /// <param name="starNode">Node with a star</param>
        /// <param name="starSystem">Star system where the star belongs to.</param>
        /// <returns>Star from xml.</returns>
        public static Star ParseStar(this XmlNode starNode, StarSystem starSystem)
        {
            String name = starNode.Attributes["name"].Value;
            String altName = (starNode.Attributes["altName"] != null) ? starNode.Attributes["altName"].Value : "";
            CelestialObjectInfo details = starNode.LastChild.ParseDetails();
            Trajectory trajectory = starNode.FirstChild.ParseTrajectory();

            Star star = new Star(name, altName, details, starSystem, trajectory);

            return star;
        }

        /// <summary>
        /// Extension method for parsing planets.
        /// </summary>
        /// <param name="planetsNode">Node with planets</param>
        /// <param name="starSystem">Star system where the planet belongs to.</param>
        /// <returns>List of planets from xml.</returns>
        public static List<Planet> ParsePlanets(this XmlNode planetsNode, StarSystem starSystem)
        {
            List<Planet> planets = new List<Planet>();
            foreach (XmlNode childNode in planetsNode.ChildNodes)
            {
                String name = childNode.Attributes["name"].Value;
                String altName = (childNode.Attributes["altName"] != null) ? childNode.Attributes["altName"].Value : "";
                CelestialObjectInfo details = childNode.LastChild.ParseDetails();
                Trajectory trajectory = childNode.FirstChild.ParseTrajectory();

                planets.Add(new Planet(name, altName, details, starSystem, trajectory));
            }
            
            return planets;
        }

        /// <summary>
        /// Extension method for parsing wormhole endpoints.
        /// </summary>
        /// <param name="planetsNode">Node with wormhole endpoints</param>
        /// /// <param name="starSystem">Star system where the wormholeEndpoint belongs to.</param>
        /// <returns>List of wormhole endpoints from xml.</returns>
        public static List<WormholeEndpoint> ParseWormholeEndpoints(this XmlNode wormholeEndpointsNode, StarSystem starSystem)
        {
            List<WormholeEndpoint> wormholeEndpoints = new List<WormholeEndpoint>();
            foreach (XmlNode childNode in wormholeEndpointsNode.ChildNodes)
            {
                int id = Convert.ToInt32(childNode.Attributes["id"].Value);
                Trajectory trajectory = childNode.FirstChild.ParseTrajectory();
                wormholeEndpoints.Add(new WormholeEndpoint(id, starSystem, trajectory));
            }

            return wormholeEndpoints;
        }

        /// <summary>
        /// Extension method for parsing trajectory.
        /// </summary>
        /// <param name="trajectoryNode">Node with trajectory</param>
        /// <returns>Trajectory from xml.</returns>
        public static Trajectory ParseTrajectory(this XmlNode trajectoryNode)
        {
            
            Direction direction;

            if (trajectoryNode.FirstChild.Attributes["direction"] == null)
            {
                direction = Direction.CLOCKWISE;
            }
            else
            {
                string directionString = trajectoryNode.FirstChild.Attributes["direction"].Value;
                if(directionString.Equals("clockwise"))
                {
                    direction =  Direction.CLOCKWISE;
                }
                else
                {
                    direction = Direction.COUNTERCLOCKWISE;
                }
            }

            
            Trajectory trajectory = null;
            switch (trajectoryNode.FirstChild.Name.ToLowerInvariant())
            {
                case "stacionary":
                    int x = trajectoryNode.FirstChild.Attributes["x"].IntValue();
                    int y = trajectoryNode.FirstChild.Attributes["y"].IntValue();
                    trajectory = new Stacionary(x, y);
                    break;
                case "circularorbit":
                    {
                        int radius = trajectoryNode.FirstChild.Attributes["radius"].IntValue();
                        double initialAngle = trajectoryNode.FirstChild.Attributes["initialAngle"].DoubleValue();
                        int period = trajectoryNode.FirstChild.Attributes["period"].IntValue();
                        trajectory = new CircularOrbit(radius, period, direction, initialAngle);
                    }
                    break;
                case "ellipticorbit":
                    {
                        int a = trajectoryNode.FirstChild.Attributes["a"].IntValue();
                        int b = trajectoryNode.FirstChild.Attributes["b"].IntValue();
                        double rotationAngle = trajectoryNode.FirstChild.Attributes["angle"].DoubleValue();
                        double initialAngle = trajectoryNode.FirstChild.Attributes["initialAngle"].DoubleValue();
                        int period = trajectoryNode.FirstChild.Attributes["period"].IntValue();
                        trajectory = new EllipticOrbit(new Point2d(0.0, 0.0), a, b, rotationAngle, period, direction, initialAngle);
                    }
                    break;
                default:
                    throw new XmlException("Unexpected trajectory.");
            }
            return trajectory;
        }

        /// <summary>
        /// Extension method for parsing trajectory.
        /// </summary>
        /// <param name="trajectoryNode">Node with trajectory</param>
        /// <returns>Trajectory from xml.</returns>
        public static CelestialObjectInfo ParseDetails(this XmlNode detailsNode)
        {
            CelestialObjectInfo details = new CelestialObjectInfo();

            foreach (XmlNode childNode in detailsNode.ChildNodes)
            {
                //Console.WriteLine(childNode.InnerText.ToString());
                switch (childNode.Name.ToLowerInvariant())
                {
                    case "gravity":
                        details.Gravity = Double.Parse(childNode.InnerText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                        //Console.WriteLine("parsed gravity:{1} to {0}", details.Gravity, childNode.InnerText);
                        break;
                    case "mass":
                        details.Mass = Double.Parse(childNode.InnerText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture);
                        //Console.WriteLine("parsed mass:{1} to {0}", details.Mass, childNode.InnerText);
                        break;
                    case "description":
                        details.Description = childNode.InnerText.Trim();
                        //Console.WriteLine("parsed desc:{1} to {0}", details.Description, childNode.InnerText.Trim());
                        break;
                    default:
                        throw new XmlException("Unexpected detail of celestial object.");
                }
            }
            return details;
        }
    }
}
