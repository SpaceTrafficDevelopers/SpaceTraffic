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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using NLog;
using SpaceTraffic.Data;
using SpaceTraffic.Game;
using SpaceTraffic.Tools.StarSystemEditor.Data;
using SpaceTraffic.Tools.StarSystemEditor.Entities;
using SpaceTraffic.Tools.StarSystemEditor.Presentation;

namespace SpaceTraffic.Tools.StarSystemEditor
{    
    /// <summary>
    /// Trida s editory
    /// </summary>
    public static class Editor
    {
        #region Properties
        //editors section
        /// <summary>
        /// Instance editoru starsystemu
        /// </summary>
        public static StarSystemEditorEntity StarSystemEditor { get; private set; }
        /// <summary>
        /// Intance editoru planet
        /// </summary>
        public static PlanetEditorEntity PlanetEditor { get; private set; }
        /// <summary>
        /// Instance editoru hvezd
        /// </summary>
        public static StarEditorEntity StarEditor { get; private set; }
        /// <summary>
        /// Instance editoru cercich der
        /// </summary>
        public static WormholeEditorEntity WormholeEditor { get; private set; }
        /// <summary>
        /// Instance editoru kruhovych orbit
        /// </summary>
        public static CircleEditorEntity CircleOrbitEditor { get; private set; }
        /// <summary>
        /// Instance editoru eliptickych orbit
        /// </summary>
        public static EllipseEditorEntity EllipseOrbitEditor { get; private set; }
        /// <summary>
        /// Instance editoru bodovych drah
        /// </summary>
        public static StacionaryEditorEntity StacionaryOrbitEditor { get; private set; }
        /// <summary>
        /// Property se soucasnou mapou galaxie
        /// </summary>
        public static GalaxyMap GalaxyMap { get; private set; }
        /// <summary>
        /// Property s mnozinou moznych jmen pro nove generovane objekty
        /// </summary>
        public static Names Names { get; private set; }
        /// <summary>
        /// Jmeno galaxie
        /// </summary> 
        public static String GalaxyName { get; set; }
        /// <summary>
        /// Property pro kontrolu zda je editor nacten
        /// </summary>
        public static bool IsLoaded { get; private set; }
        /// <summary>
        /// Cas simulace
        /// </summary>
        public static int Time { get; set; }
        #endregion
        #region Constants
        /// <summary>
        /// Cesta k souboru se jmeny
        /// </summary>
        public const String NAMESFILEPATH = "Assets\\names.txt";
        /// <summary>
        /// Cesta k souboru s mapou galaxie
        /// </summary>
        public const String GALAXYFILEPATH = "Assets\\map\\GalaxyMap2.xml";
        #endregion
        /// <summary>
        /// Datapresenter slouzici pro zobrazeni GUI
        /// </summary>
        public static DataPresenter dataPresenter;
        /// <summary>
        /// Instance loggeru
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Metoda pro vycisteni editoru
        /// </summary>
        public static void FlushEditors() 
        {
            StarSystemEditor = new StarSystemEditorEntity();
            PlanetEditor = new PlanetEditorEntity();
            StarEditor = new StarEditorEntity();
            WormholeEditor = new WormholeEditorEntity();
            CircleOrbitEditor = new CircleEditorEntity();
            EllipseOrbitEditor = new EllipseEditorEntity();
            StacionaryOrbitEditor = new StacionaryEditorEntity();
        }
        /// <summary>
        /// Metoda starajici se o pripravu editoru
        /// </summary>
        public static void Preload()
        {
            
            Log("Inicializuji editor");
            Names = new Names(NAMESFILEPATH);
            GalaxyMap = new GalaxyMap();
            dataPresenter = new DataPresenter();
            //vytvorim instance vsech editoru
            StarSystemEditor = new StarSystemEditorEntity();
            PlanetEditor = new PlanetEditorEntity();
            StarEditor = new StarEditorEntity();
            WormholeEditor = new WormholeEditorEntity();
            CircleOrbitEditor = new CircleEditorEntity();
            EllipseOrbitEditor = new EllipseEditorEntity();
            StacionaryOrbitEditor = new StacionaryEditorEntity();
            IsLoaded = false;
            Time = 0;
            Log("Inicializace dokoncena");

            
        }
        /// <summary>
        /// Metoda pro nacteni galaxie pro potreby editoru
        /// </summary>
        /// <param name="galaxyName">Jmeno galaxie</param>
        /// <param name="filePath">Cesta ke galaxii</param>
        public static void LoadGalaxy(String galaxyName, String filePath) 
        {
            //pokusim se nacist mapu
            Log("Nacitam galaxii " + galaxyName);
            try
            {
                StreamDataProvider provider = new StreamDataProvider(filePath);
                provider.Initialize();
                GalaxyMapLoader loader = new GalaxyMapLoader();
                try
                {
                    GalaxyMap = loader.LoadGalaxyMap(galaxyName, provider);
                    GalaxyName = galaxyName;
                    IsLoaded = true;
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    GalaxyName = "";
                }

                Log("Galaxie " + GalaxyName + " nactena!");
            }
            catch (Exception ex)
            {
                Log("Nacitani galaxie " +GalaxyName +"se nezdarilo! " + ex.Message);
            }

            //XmlSaver.CreateXml(GalaxyMap);
        }
        /// <summary>
        /// Metoda vracejici seznam jmen starsystemu nactene galaxie
        /// </summary>
        /// <returns>Seznam jmen</returns>
        public static List<String> LoadStarSystemNames()
        {
            List<String> list = new List<string> { };
            foreach (StarSystem system in GalaxyMap)
            {
                list.Add(system.Name.ToString());
            }
            return list;
        }
        /// <summary>
        /// Metoda zapisujici informace jak do logu tak do konzole
        /// </summary>
        /// <param name="text"></param>
        public static void Log(String text)
        {
            Console.WriteLine("LOG: " + text);
            logger.Info(text);
        }
        /// <summary>
        /// Metoda pro nacteni souboru s galaxii, jeho zpracovani a pak ulozeni do pameti
        /// </summary>
        public static void LoadGalaxyFile()
        {
            prepareDataFolder();
            //Log("dialog");
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "GalaxyMap XML File(.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.IgnoreComments = true;
                // Open document 
                string filename = dlg.FileName;
                FileStream fs = new FileStream(filename, FileMode.Open);
                //Editor.Log(dlg.InitialDirectory);
                GalaxyMap map = new GalaxyMap();
                List<string> starSystemNamesList = new List<string> { };
                List<string> loadList = new List<string> { };
                using (XmlReader reader = XmlReader.Create(fs, readerSettings))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    XmlNode galaxyNode = doc.GetElementsByTagName("galaxy")[0];
                    if (galaxyNode == null)
                    {
                        throw new FileFormatException("This file doesn't contain galaxy map!");
                    }
                    
                    map.MapName = galaxyNode.Attributes["name"].Value.ToString();
                    
                    foreach (XmlNode childNode in galaxyNode.ChildNodes)
                    {
                        switch (childNode.Name.ToLowerInvariant())
                        {
                            case "starsystems":

                                foreach (XmlNode starSystemNode in childNode.ChildNodes)
                                {
                                    string starSystemFileName = starSystemNode.Attributes["name"].Value.ToString();
                                    starSystemNamesList.Add(starSystemFileName);
                                    //Editor.Log(starSystemFileName);
                                    if (File.Exists(dlg.FileName.Replace(dlg.SafeFileName, "") + Path.DirectorySeparatorChar + starSystemFileName + ".xml"))
                                    {
                                        Editor.Log("parsing");
                                        loadList.Add(starSystemFileName);
                                    }
                                    else
                                    {
                                        StarSystem starSystem = new StarSystem();
                                        starSystem.Name = starSystemFileName;
                                        Star star = new Star();
                                        star.Name = starSystemFileName;
                                        starSystem.Star = star;
                                        map.Add(starSystem);
                                    }
                                }
                                break;
                            case "wormholes":
                                //TODO: Implementation
                                //connections = childNode.ParseWormholes();
                                break;
                            default:
                                throw new XmlException("Unexpected childNode.");
                        }
                    }  
                }
                fs.Close();
                foreach (String starSystemName in loadList)
                {
                    String starSystemPath = (dlg.FileName.Replace(dlg.SafeFileName, "") + starSystemName + ".xml");
                    map.Add(LoadStarSystem(starSystemPath, readerSettings));
                }
                Editor.GalaxyMap = map;
            }
        }
        /// <summary>
        /// Metoda pro zpracovani souboru se starsystemem
        /// </summary>
        /// <param name="path">Cesta k starsystemu</param>
        /// <param name="settings">Nastaveni loadingu</param>
        /// <returns>Instance zpracovaneho starsystemu</returns>
        private static StarSystem LoadStarSystem(String path, XmlReaderSettings settings)
        {
            //Editor.Log(path);
            FileStream starSystemStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //Editor.Log(starSystemStream.CanRead.ToString());
            XmlReader starSystemReader = XmlReader.Create(starSystemStream, settings);
            XmlDocument docStarSystem = new XmlDocument();
            docStarSystem.Load(starSystemReader);
            XmlNode starSystemNode = docStarSystem.GetElementsByTagName("starsystem")[0];
            //Editor.Log(starSystemNode.FirstChild.ToString());
            StarSystem parsedStarSystem = starSystemNode.ParseStarSystem();
            //Editor.Log("parsing complete, adding: " + parsedStarSystem.Name);
            starSystemStream.Close();
            return parsedStarSystem;
        }
        /// <summary>
        /// Metoda pripracujici slozky pro nacitani dat
        /// </summary>
        private static void prepareDataFolder()
        {
            Directory.CreateDirectory(".//Assets//");
            Directory.CreateDirectory(".//Assets//map");
        }
    }

}
