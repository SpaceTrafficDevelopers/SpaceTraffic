using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Xml;

using SpaceTraffic.Data;
using SpaceTraffic.Game;

namespace SpaceTraffic.Tools.StarSystemEditor.Data
{
    /// <summary>
    /// Trida s jadra SpaceTraffic slouzici pro standartni praci s xml soubory
    /// </summary>
    public class StreamDataProvider : IGalaxyMapDataStreamProvider
    {
        /// <summary>
        /// Konstanta urcujici format souboru
        /// </summary>
        public const string MAP_FILE_EXTENSION = ".xml";
        /// <summary>
        /// Vychozi cesta k nacitanym souborum
        /// </summary>
        private string _mapDirecoryPath = ".\\";
        /// <summary>
        /// Property s korenovou cestou
        /// </summary>
        public string RootPath { get; private set; }
        /// <summary>
        /// Property pro overeni zda je streamprovider pripraveny
        /// </summary>
        protected bool IsInitialized { get; private set; }
        /// <summary>
        /// Property pro cestu k mapam
        /// </summary>
        public string MapDirectoryPath
        {
            get
            {
                Debug.Assert(this.IsInitialized, "Not initialized");
                return this._mapDirecoryPath;
            }
            private set
            {
                Debug.Assert(!String.IsNullOrWhiteSpace(value), "MapDirectoryPath cannot be null, empty or whitespace");
                this._mapDirecoryPath = value;
            }
        }
        /// <summary>
        /// Streamdata provider
        /// </summary>
        /// <param name="assetsRootPath">Cesta do slozky s Assety</param>
        public StreamDataProvider(string assetsRootPath)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(assetsRootPath), "assetsRootPath cannot be null, empty or whitespace");
            this.RootPath = assetsRootPath;
            this.IsInitialized = false;
        }
        /// <summary>
        /// Inicializace streamdata provideru
        /// </summary>
        public void Initialize()
        {
            Debug.Assert(!this.IsInitialized, "Already initialized");

            DirectoryInfo assetsRootDir = new DirectoryInfo(this.RootPath);
            if (!assetsRootDir.Exists)
            {
                throw new DirectoryNotFoundException("Assets root directory not found: " + assetsRootDir.FullName);
            }
            this.RootPath = assetsRootDir.FullName;

            this.SetMapPath();

            this.IsInitialized = true;
        }
        /// <summary>
        /// Metoda nastavujici cestu k mape
        /// </summary>
        private void SetMapPath()
        {
            string path = Path.Combine(this.RootPath, "Map");
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Assets directory 'Map' not found: " + path);
            }
            this.MapDirectoryPath = path;
        }
        /// <summary>
        /// Getter pro cestu k mapam
        /// </summary>
        /// <param name="filename">Jmeno mapy</param>
        /// <returns>Cestu k pame</returns>
        public string GetMapFilePath(string filename)
        {
            Debug.Assert(this.IsInitialized, "Not initialized");

            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("Name cannot be null or empty string.");

            Debug.Assert(filename.Contains('\\') == false, "Name contains \\");

            string path = Path.Combine(this.MapDirectoryPath, filename);
            return path;
        }
        /// <summary>
        /// Metoda pro ziskani streamu starsystemu
        /// </summary>
        /// <param name="starSystemName">Jmeno starsystemu</param>
        /// <returns>Stream starsystemu</returns>
        public Stream GetStarSystemStream(string starSystemName)
        {
            return this.GetMapDataStream(starSystemName);
        }
        /// <summary>
        /// Metoda pro ziskani streamu galaxie
        /// </summary>
        /// <param name="mapName">Jmeno galaxie</param>
        /// <returns>Stream galaxie</returns>
        public Stream GetGalaxyMapStream(string mapName)
        {
            return this.GetMapDataStream(mapName);
        }
        /// <summary>
        /// Metoda pro ziskani streamu dat mapy
        /// </summary>
        /// <param name="filenameWithoutExtension">Cesta k souboru</param>
        /// <returns>Stream s daty</returns>
        private Stream GetMapDataStream(string filenameWithoutExtension)
        {
            string filename = this.GetMapFilePath(filenameWithoutExtension + MAP_FILE_EXTENSION);

            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            return stream;
        }
        /// <summary>
        /// Metoda pro nacteni mapy galaxie
        /// </summary>
        /// <param name="galaxyMapName"></param>
        /// <returns></returns>
        public GalaxyMap LoadGalaxyMap(string galaxyMapName)
        {
            GalaxyMap galaxyMap = null;
            try
            {
                GalaxyMapLoader mapLoader = new GalaxyMapLoader();
                galaxyMap = mapLoader.LoadGalaxyMap(galaxyMapName, this);
                galaxyMap.Lock();
            }
            catch (XmlException ex)
            {
                throw ex;
            }
            catch (GalaxyMapBuildingException ex)
            {
                throw ex;
            }
            return galaxyMap;
        }
    }
}
