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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;
using SpaceTraffic.Game.Geometry;
using System.Windows.Media;
using SpaceTraffic.Utils;

namespace SpaceTraffic.Tools.StarSystemEditor
{    
    /// <summary>
    /// Editor class
    /// </summary>
    public static class Editor
    {
        #region Properties
        //editors section
        /// <summary>
        /// Instance of StarSystem Editor
        /// </summary>
        public static StarSystemEditorEntity StarSystemEditor { get; private set; }
        /// <summary>
        /// Instance of Planet editor
        /// </summary>
        public static PlanetEditorEntity PlanetEditor { get; private set; }
        /// <summary>
        /// Instance of Star Editor
        /// </summary>
        public static StarEditorEntity StarEditor { get; private set; }
        /// <summary>
        /// Instance of Wormhole Editor
        /// </summary>
        public static WormholeEditorEntity WormholeEditor { get; private set; }
        /// <summary>
        /// Instance of Circular Orbit editor
        /// </summary>
        public static CircleEditorEntity CircleOrbitEditor { get; private set; }
        /// <summary>
        /// Instance of Elliptic Orbit Editor
        /// </summary>
        public static EllipseEditorEntity EllipseOrbitEditor { get; private set; }
        /// <summary>
        /// Galaxy Map property
        /// </summary>
        public static GalaxyMap GalaxyMap { get; private set; }
        /// <summary>
        /// Name of all generated objects property
        /// </summary>
        public static Names Names { get; private set; }
        /// <summary>
        /// Galaxy Name
        /// </summary> 
        public static String GalaxyName { get; set; }
        /// <summary>
        /// Property pro kontrolu zda je editor nacten
        /// </summary>
        public static bool IsLoaded { get; private set; }
        /// <summary>
        /// Simulation time
        /// </summary>
        public static int Time { get; set; }
        /// <summary>
        /// Name displayed on button in top left corner of editor, used to determine wheter galaxy or star system is shown
        /// </summary>
        public static string ButtonName = "Galaxy Map";
        #endregion
        #region Constants
        /// <summary>
        /// Filepath to names file
        /// </summary>
        public const String NAMESFILEPATH = "Assets\\names.txt";
        /// <summary>
        /// Filepath to galaxy map file
        /// </summary>
        public const String GALAXYFILEPATH = "Assets\\map\\GalaxyMap2.xml";
        #endregion
        /// <summary>
        /// Datapresenter used to for showing GUI
        /// </summary>
        public static DataPresenter dataPresenter;
        /// <summary>
        /// logger instance
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// method for cleaning of editor
        /// </summary>
        public static void FlushEditors() 
        {
            StarSystemEditor = new StarSystemEditorEntity();
            PlanetEditor = new PlanetEditorEntity();
            StarEditor = new StarEditorEntity();
            WormholeEditor = new WormholeEditorEntity();
            CircleOrbitEditor = new CircleEditorEntity();
            EllipseOrbitEditor = new EllipseEditorEntity();
        }
        /// <summary>
        /// Method preloading the editor
        /// </summary>
        public static void Preload()
        {
            
            Log("Initializin editor");
            Names = new Names(NAMESFILEPATH);
            GalaxyMap = new GalaxyMap();
            dataPresenter = new DataPresenter();
            //create instance of all editors
            StarSystemEditor = new StarSystemEditorEntity();
            PlanetEditor = new PlanetEditorEntity();
            StarEditor = new StarEditorEntity();
            WormholeEditor = new WormholeEditorEntity();
            CircleOrbitEditor = new CircleEditorEntity();
            EllipseOrbitEditor = new EllipseEditorEntity();
            IsLoaded = false;
            Time = 0;
            Log("Initialization complete");

            
        }
        /// <summary>
        /// Method loading galaxy for editor purposes
        /// </summary>
        /// <param name="galaxyName">Galaxy Name</param>
        /// <param name="filePath">filepath to galaxy file</param>
        public static void LoadGalaxy(String galaxyName, String filePath)
        {
            //attempting to laod galaxy
            Log("Loading galaxy " + galaxyName);
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

                Log("Galaxy " + GalaxyName + " loaded!");
            }
            catch (Exception ex)
            {
                Log("Loading of galaxy: " + GalaxyName + "failed! " + ex.Message);
            }
        }
        /// <summary>
        /// Method creating new star system
        /// </summary>
        /// <returns>Star system</returns>
        public static void NewSystem(string name, int planetCount, int wormholeCount, string type)
        {
            StarSystemCreator starSystemCreator = new StarSystemCreator();
            StarSystem system = starSystemCreator.createSystem(name, planetCount, wormholeCount, type);
            GalaxyMap.Add(system);
            
            ListView view = dataPresenter.GetStarSystemList();
            view.Items.Clear(); // clears loaded starsystems
            dataPresenter.StarSystemListLoader();
        }
        /// <summary>
        /// Method adding planet to selected starsystem
        /// </summary>
        public static bool newPlanet()
        {
            return StarSystemCreator.addPlanet(Editor.dataPresenter.SelectedStarSystem);
        }

        /// <summary>
        /// method adding wormhole endpoint to selected starsystem
        /// </summary>
        public static bool newWormhole()
        {
            return StarSystemCreator.addWormhole(Editor.dataPresenter.SelectedStarSystem);
        }

        /// <summary>
        /// method return list of names of starsystems
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
        /// Method writing to logger and console
        /// </summary>
        /// <param name="text"></param>
        public static void Log(String text)
        {
            Console.WriteLine("LOG: " + text);
            logger.Info(text);
        }
        /// <summary>
        /// Method for loading galaxy file, process it, and save it to editor
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
                    
                    String MapName = galaxyNode.Attributes["name"].Value.ToString();
                    fs.Close();
                    // cuts filename of galaxy map file name
                    int index = dlg.FileName.LastIndexOf("\\");
                    string filepath = dlg.FileName.Remove(index);
                    index = filepath.LastIndexOf("\\");
                    filepath = filepath.Remove(index);
                    
                    LoadGalaxy(MapName, filepath);
                }
            }
        }

        /// <summary>
        /// Method for processing of star system file
        /// </summary>
        /// <param name="path">Starsystem filepath</param>
        /// <param name="settings">loading settings</param>
        /// <returns>Instance of Star System</returns>
        private static StarSystem LoadStarSystem(String path, XmlReaderSettings settings)
        {
            FileStream starSystemStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            XmlReader starSystemReader = XmlReader.Create(starSystemStream, settings);
            XmlDocument docStarSystem = new XmlDocument();
            docStarSystem.Load(starSystemReader);
            XmlNode starSystemNode = docStarSystem.GetElementsByTagName("starsystem")[0];
            StarSystem parsedStarSystem = starSystemNode.ParseStarSystem();
            starSystemStream.Close();
            return parsedStarSystem;
        }

        /// <summary>
        /// Method preparing folders
        /// </summary>
        private static void prepareDataFolder()
        {
            Directory.CreateDirectory(".//Assets//");
            Directory.CreateDirectory(".//Assets//map");
        }

        /// <summary>
        /// Selects element on canvas
        /// </summary>
        /// <param name="source">element on canvas clicked</param>
        public static void selectEntity(object source)
        {
            if (!Editor.dataPresenter.selected)
            {
                if (source is Shape)
                {
                    Shape selectedShape = (Shape)source;
                    if (selectedShape.Tag is StarSystemView)
                    {
                        Editor.dataPresenter.DrawStarSystemPoint(selectedShape.Tag as StarSystemView);
                    }
                    else
                    Editor.dataPresenter.DrawPoints((View)selectedShape.Tag);
                }
            }
        }
    }

}
