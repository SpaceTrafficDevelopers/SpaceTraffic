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
using System.Xml;
using System.IO;

using SpaceTraffic.Game;
using SpaceTraffic.Data;

using SpaceTraffic.Tools.StarSystemEditor.Exceptions;
using SpaceTraffic.Tools.StarSystemEditor.Data;

namespace SpaceTraffic.Tools.StarSystemEditor.Data
{
    /// <summary>
    /// Docasne reseni nacitani galaxii, v dalsi iteraci bude vyuzito nacitani z hlavniho projektu
    /// </summary>
    /// <remarks>Tato trida je zde jen docasne kvuli vstupnim datum, v iteraci 2 bude kompletne naimplementovan pomoci SpaceTraffic.Game.GalaxyMap</remarks>
    [Obsolete("Use GalaxyMap instead")]
    public class InternalGalaxyMap : GalaxyMap
    {
        /// <summary>
        /// Serazeny seznam s hvezdnymi systemy dle nazvu jako jejich klice
        /// </summary>
        public SortedList<string, StarSystem> starSystems = new SortedList<string, StarSystem>();

        /// <summary>
        /// XML Loader
        /// </summary>
        /// <param name="starSystemName">Cesta k souboru s mapou</param>
        public static StarSystem LoadStarSystem(String starSystemName)
        {
            StreamDataProvider provider = new StreamDataProvider(".//Assets");
            provider.Initialize();
            StarSystemLoader loader = new StarSystemLoader();
            StarSystem starSystem = loader.LoadStarSystem(starSystemName, provider);
            return starSystem;
        }
        /// <summary>
        /// Metoda pro nacteni galaxie
        /// </summary>
        /// <param name="galaxyName">Jmeno galaxie</param>
        /// <returns>Mapa galaxie</returns>
        public static GalaxyMap LoadGalaxy(String galaxyName)
        {
            StreamDataProvider provider = new StreamDataProvider(".//Assets");
            provider.Initialize();
            GalaxyMapLoader loader = new GalaxyMapLoader();
            GalaxyMap galaxyMap = loader.LoadGalaxyMap(galaxyName, provider);

            if (galaxyMap.Count == 0) Console.WriteLine("Nezdarilo se otevrit zadny ze zadanych souboru!");
            return galaxyMap;
        }

        /// <summary>
        /// Metoda pro zjisteni zda se v galaxii nachazi hledany system
        /// </summary>
        /// <param name="starSystemName">Jmeno starsystemu</param>
        /// <returns>Informace zda byl nebo nalezen hledany system</returns>
        public bool ContainsStarSystem(String starSystemName) 
        {
            if (starSystems == null) return false;
            return this.starSystems.ContainsKey(starSystemName);
        }

        /// <summary>
        /// Metoda ktera vrati hledany starsystem pokud existuje
        /// </summary>
        /// <param name="starSystemName">Jmeno starsystemu</param>
        /// <returns>Hledany system</returns>
        public StarSystem GetStarSystem(String starSystemName)
        {
            if (this.ContainsStarSystem(starSystemName))
            {
                return this.starSystems[starSystemName];
            }
            throw new ArgumentException("Tento starsystem se v galaxii nenachazi!");
        }
    }
}
