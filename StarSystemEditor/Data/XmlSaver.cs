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
using System.Xml.Linq;
using System.Xml.Schema;
using System.IO;
using System.Globalization;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;
using SpaceTraffic.Utils;
using SpaceTraffic.Entities.PublicEntities;
using Microsoft.Win32;
using System.Xml;
using System.Diagnostics;

namespace SpaceTraffic.Tools.StarSystemEditor.Data
{
    /// <summary>
    /// Trida starajici se o praci s XML pro potreby editoru
    /// </summary>
    public static class XmlSaver
    {
        /// <summary>
        /// Cesta k souboru
        /// </summary>
        private static String filePath;
        /// <summary>
        /// Property pro cestu k souboru
        /// </summary>
	    public static String FilePath
	    {
		    get
            {
                if(filePath == null)
                {
                    filePath = "." + Path.PathSeparator;
                }
                return filePath;
            }
		    set { filePath = value;}
	    }
	    /// <summary>
	    /// Metoda pro vytvoreni xml souboru objektu ulozenych v pameti
	    /// </summary>
	    /// <param name="map">Mapa pro ulozeni</param>
        public static void CreateXml(GalaxyMap map)
        {
            PrepareSaveFolder();
            CreateGalaxyMapXml(map);
        }
        /// <summary>
        /// Metoda pro vytvoreni xml s mapou starsystemu
        /// </summary>
        /// <param name="map">Mapa pro ulozeni</param>
        private static void CreateGalaxyMapXml(GalaxyMap map)
        {
            //Log("dialog");
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = map.MapName;
            dlg.DefaultExt = ".xml";
            dlg.Filter = "GalaxyMap XML File(.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.IgnoreComments = true;
                // Open document 
                string filename = dlg.FileName;
                FileStream fileStream = new FileStream(".\\saveddata\\Map\\" + map.MapName + ".xml", FileMode.Create);

                XNamespace defaultNamespace = XNamespace.Get("SpaceTrafficData");
                XElement doc = new XElement(
                    new XElement(defaultNamespace + "stdata",
                        new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                        new XAttribute(XNamespace.Xmlns + "html", "http://www.w3.org/2002/08/xhtml/xhtml1-strict.xsd"),
                        new XAttribute(XNamespace.Xmlns + "st", "SpaceTrafficData"),
                        new XAttribute("version", "1.2")
                    )
                );

                XElement root = new XElement("galaxy");
                root.Add(new XAttribute("name", map.MapName));

                XElement starSystems = new XElement("starSystems");
                XElement wormholes = new XElement("wormholes");
                int id = 0;
                // kolekce dvojici endpointu
                SortedList<int, WormholeEndpoint> connections = new SortedList<int, WormholeEndpoint>();
                foreach (StarSystem starSystem in map.GetStarSystems())
                {
                    XElement item = new XElement("starsystem");
                    item.Add(new XAttribute("name", starSystem.Name));
                    starSystems.Add(item);
                    CreateStarSystemXml(starSystem);
                    
                    foreach (WormholeEndpoint wormholeEndpoint in starSystem.WormholeEndpoints)
                    {
                        if (wormholeEndpoint.IsConnected)
                        {
                            // rozdil hash funkci da unikatni klic pro kazdou wormhole dvojici
                            int klic = Math.Abs(wormholeEndpoint.GetHashCode() - wormholeEndpoint.Destination.GetHashCode());
                            if (!connections.ContainsKey(klic))
                            {
                                connections.Add(klic, wormholeEndpoint);
                            }

                        }
                    }
                }
                foreach (WormholeEndpoint endpoint in connections.Values)
                {
                    XElement wormhole = null;
                    wormhole = WormholesToElement(id, endpoint);
                    wormholes.Add(wormhole);
                    id++;
                }
                root.Add(starSystems);

                root.Add(wormholes);

                doc.Add(root);

                doc.Save(fileStream);
                fileStream.Close();
            }
        }
        /// <summary>
        /// Metoda pro vytvoreni elementu ze spojeni mezi starsystemy
        /// </summary>
        /// <param name="id">id Xelementu wormhole</param>
        /// <param name="wormholeEndpoint">Spojeni pro zpracovani</param>
        /// <returns>Zpracovany element</returns>
        private static XElement WormholesToElement(int id, WormholeEndpoint wormholeEndpoint)
        {
            XElement wormhole = new XElement("wormhole");
            wormhole.Add(new XAttribute("id", id));
            XElement endpoint = new XElement("endpoint");
            endpoint.Add(new XAttribute("system", wormholeEndpoint.StarSystem.Name));
            endpoint.Add(new XAttribute("id", wormholeEndpoint.Id));
            wormhole.Add(endpoint);
            endpoint = new XElement("endpoint");
            endpoint.Add(new XAttribute("system", wormholeEndpoint.Destination.StarSystem.Name));
            endpoint.Add(new XAttribute("id", wormholeEndpoint.Destination.Id));
            wormhole.Add(endpoint);
            

            return wormhole;
        }
        /// <summary>
        /// Metoda pro vytvoreni elementu ze starsystemu
        /// </summary>
        /// <param name="starSystem">Starsystem pro zpracovani</param>
        public static void CreateStarSystemXml(StarSystem starSystem)
        {
            
                FileStream fileStream = new FileStream(".\\saveddata\\Map\\" + starSystem.Name + ".xml", FileMode.Create);

                XNamespace defaultNamespace = XNamespace.Get("SpaceTrafficData");
                XElement doc = new XElement(
                    new XElement(defaultNamespace + "stdata",
                        new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                        new XAttribute(XNamespace.Xmlns + "html", "http://www.w3.org/2002/08/xhtml/xhtml1-strict.xsd"),
                        new XAttribute(XNamespace.Xmlns + "st", "SpaceTrafficData"),
                        new XAttribute("version", "1.2")
                    )
                );

                XElement root = new XElement("starsystem");
                root.Add(new XAttribute("name", starSystem.Name));
                root.Add(new XAttribute("x", starSystem.MapPosition.X));
                root.Add(new XAttribute("y", starSystem.MapPosition.Y));

                XElement star = new XElement("star");
                star.Add(new XAttribute("name", starSystem.Star.Name));
                star.Add(TrajectoryToElement(starSystem.Star.Trajectory));
                star.Add(DetailsToElement(starSystem.Star.Details));
                root.Add(star);

                XElement planets = new XElement("planets");
                foreach (Planet planet in starSystem.Planets)
                {
                    XElement planetElement = new XElement("planet");
                    planetElement.Add(new XAttribute("altName", planet.AlternativeName));
                    planetElement.Add(new XAttribute("name", planet.Name));
                    planetElement.Add(TrajectoryToElement(planet.Trajectory));
                    planetElement.Add(DetailsToElement(planet.Details));
                    planets.Add(planetElement);
                }
                root.Add(planets);

                XElement wormholeEndpoints = new XElement("wormholeEndpoints");
                foreach (WormholeEndpoint endpoint in starSystem.WormholeEndpoints)
                {
                    wormholeEndpoints.Add(EndpointToElement(endpoint));
                }
                root.Add(wormholeEndpoints);

                doc.Add(root);

                doc.Save(fileStream);
                fileStream.Close();
            
        }
        /// <summary>
        /// Metoda pro vytvoreni elementu z trajektorie
        /// </summary>
        /// <param name="trajectory">Trajektorie pro zpracovani</param>
        /// <returns>Element s trajektorii</returns>
        private static XElement TrajectoryToElement(Trajectory trajectory)
        {
            XElement trajectoryElement = new XElement("trajectory");
            XElement trajectoryShapeElement = null;
            if (trajectory is CircularOrbit)
            {
                CircularOrbit circOrbit = (CircularOrbit)trajectory;
                trajectoryShapeElement = new XElement("circularOrbit");
                trajectoryShapeElement.Add(new XAttribute("direction", circOrbit.Direction.ToString().ToLower()));
                trajectoryShapeElement.Add(new XAttribute("period", circOrbit.PeriodInSec));
                String initialAngle = Math.Round(MathUtil.RadianToDegree(circOrbit.InitialAngleRad)).ToString("0.0", CultureInfo.InvariantCulture);
                trajectoryShapeElement.Add(new XAttribute("initialAngle", initialAngle));
                trajectoryShapeElement.Add(new XAttribute("radius", circOrbit.Radius));
            }
            else if (trajectory is EllipticOrbit)
            {
                EllipticOrbit elliOrbit = (EllipticOrbit)trajectory;
                trajectoryShapeElement = new XElement("ellipticOrbit");
                trajectoryShapeElement.Add(new XAttribute("direction", elliOrbit.Direction.ToString().ToLower()));
                trajectoryShapeElement.Add(new XAttribute("period", elliOrbit.PeriodInSec));
                trajectoryShapeElement.Add(new XAttribute("a", elliOrbit.A));
                trajectoryShapeElement.Add(new XAttribute("b", elliOrbit.B));
                String angle = Math.Round(MathUtil.RadianToDegree(elliOrbit.RotationAngleInRad)).ToString("0.0", CultureInfo.InvariantCulture);
                trajectoryShapeElement.Add(new XAttribute("angle", angle));
                String initialAngle = Math.Round(MathUtil.RadianToDegree(elliOrbit.InitialAngleRad)).ToString("0.0", CultureInfo.InvariantCulture);
                trajectoryShapeElement.Add(new XAttribute("initialAngle", initialAngle));
            }
            else if (trajectory is Stacionary) 
            {
                Stacionary stacOrbit = (Stacionary)trajectory;
                trajectoryShapeElement = new XElement("stacionary");
                trajectoryShapeElement.Add(new XAttribute("x", stacOrbit.X));
                trajectoryShapeElement.Add(new XAttribute("y", stacOrbit.Y));
            }
            trajectoryElement.Add(trajectoryShapeElement);
            return trajectoryElement;

        }
        /// <summary>
        /// Metoda pro zpracovani popisu objektu
        /// </summary>
        /// <param name="info">Popis objektu</param>
        /// <returns>Element s popisem</returns>
        private static XElement DetailsToElement(CelestialObjectInfo info)
        {
            XElement infoElement = new XElement("details");
 
            String gravity = info.Gravity.ToString("0.########E+000", CultureInfo.InvariantCulture);
            infoElement.Add(new XElement("gravity", gravity));
            String mass = info.Mass.ToString("0.########E+000", CultureInfo.InvariantCulture);
            infoElement.Add(new XElement("mass", mass));
            infoElement.Add(new XElement("description", info.Description));

            return infoElement;
        }
        /// <summary>
        /// Metoda pro vytvoreni elementu z koncoveho bodu cervi diry
        /// </summary>
        /// <param name="endpoint">Cervi dira ke zpracovani</param>
        /// <returns>Zpracovany element</returns>
        private static XElement EndpointToElement(WormholeEndpoint endpoint)
        {
            XElement wheElement = new XElement("wormholeEndpoint");
            
            wheElement.Add(new XAttribute("id", endpoint.Id));
            wheElement.Add(TrajectoryToElement(endpoint.Trajectory));
            
            return wheElement;
        }
        /// <summary>
        /// Metoda ktera prida nove vytvoreny star system do xml galaxy mapy
        /// </summary>
        /// <param name="name"></param>
        public static void AddStarSystemToGalaxy(string name, string galaxyname)
        {
            XDocument doc = XDocument.Load(".\\saveddata\\maps\\" + galaxyname + ".xml");
            XElement starSystems = doc.Descendants("starSystems").Single();
            XElement item = new XElement("starsystem");
            item.Add(new XAttribute("name", name));
            starSystems.Add(item);
            doc.Save(".\\saveddata\\maps\\" + galaxyname + ".xml");
        }
        /// <summary>
        /// Metoda pro pripravu slozek pro ukladani dat
        /// </summary>
        private static void PrepareSaveFolder()
        {
            Directory.CreateDirectory(".//saveddata//starsystems");
            Directory.CreateDirectory(".//saveddata//maps");
        }
    }
}
