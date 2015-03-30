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
using System.IO;
using NLog;
using System.Diagnostics;
using SpaceTraffic.Game;
using SpaceTraffic.Data;
using System.Xml;
using SpaceTraffic.Engine;
using SpaceTraffic.Entities.Goods;

namespace SpaceTraffic.GameServer
{
    class AssetManager : IAssetManager
    {
        public const string FILE_EXTENSION = ".xml";
        private string _mapDirectoryPath = ".\\";

        private string _goodsDirectoryPath = ".\\";

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string AssetRootPath { get; private set; }

        protected bool IsInitialized { get; private set; }

        public string MapDirectoryPath
        {
            get
            {
                Debug.Assert(this.IsInitialized, "Not initialized");
                return this._mapDirectoryPath;
            }
            private set
            {
                Debug.Assert(!String.IsNullOrWhiteSpace(value), "MapDirectoryPath cannot be null, empty or whitespace");
                this._mapDirectoryPath = value;
            }
        }

        public string GoodsDirectoryPath
        {
            get
            {
                Debug.Assert(this.IsInitialized, "Not initialized");
                return this._goodsDirectoryPath;
            }
            private set
            {
                Debug.Assert(!String.IsNullOrWhiteSpace(value), "GoodsDirectoryPath cannot be null, empty or whitespace");
                this._goodsDirectoryPath = value;
            }
        }

        public AssetManager(string assetsRootPath)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(assetsRootPath), "assetsRootPath cannot be null, empty or whitespace");
            this.AssetRootPath = assetsRootPath;
            this.IsInitialized = false;
        }

        public void Initialize()
        {
            Debug.Assert(!this.IsInitialized, "Already initialized");

            DirectoryInfo assetsRootDir = new DirectoryInfo(this.AssetRootPath);
            if (!assetsRootDir.Exists)
            {
                throw new DirectoryNotFoundException("Assets root directory not found: " + assetsRootDir.FullName);
            }
            this.AssetRootPath = assetsRootDir.FullName;

            logger.Info("Assets root directory path: {0}", this.AssetRootPath);

            this.SetMapPath();
            this.SetGoodsPath();

            this.IsInitialized = true;
        }

        private void SetMapPath()
        {
            string path = Path.Combine(this.AssetRootPath, "Map");
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Assets directory 'Map' not found: " + path);
            }
            this.MapDirectoryPath = path;
            logger.Info("Assets Map directory path: {0}", path);
        }

        /// <summary>
        /// Sets goods path to Goods directory.
        /// Throws DirectoryNotFoundException when Goods directory isn't found.
        /// </summary>
        private void SetGoodsPath()
        {
            string path = Path.Combine(this.AssetRootPath, "Goods");
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Assets directory 'Goods' not found: " + path);
            }
            this.GoodsDirectoryPath = path;
            logger.Info("Assets Goods directory path: {0}", path);
        }

        public string GetMapFilePath(string filename)
        {
            Debug.Assert(this.IsInitialized, "Not initialized");

            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException("Name cannot be null or empty string.");

            Debug.Assert(filename.Contains('\\') == false, "Name contains \\");

            string path = Path.Combine(this.MapDirectoryPath, filename);
            return path;
        }

        public void Dispose()
        {
            // Nothing to do yet.
        }

        public Stream GetStarSystemStream(string starSystemName)
        {
            return this.GetMapDataStream(starSystemName);
        }

        public Stream GetGalaxyMapStream(string mapName)
        {
            return this.GetMapDataStream(mapName);
        }

        private Stream GetMapDataStream(string filenameWithoutExtension)
        {
            string filename = this.GetMapFilePath(filenameWithoutExtension + FILE_EXTENSION);

            logger.Debug("Creating file stream: {0}", filename);

            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return stream;
        }

        /// <summary>
        /// Gets the data stream for goods file specified by file name (without extension).
        /// Throws ArgumentNullException when the goodsFileName is null or empty.
        /// </summary>
        /// <param name="goodsFileName">Name of the goods file (without extension).</param>
        /// <returns>Stream with goods file.</returns>
        public Stream GetGoodsStream(string goodsFileName)
        {
            Debug.Assert(this.IsInitialized, "Not initialized");

            if (String.IsNullOrWhiteSpace(goodsFileName))
                throw new ArgumentNullException("Name cannot be null or empty string.");

            Debug.Assert(goodsFileName.Contains('\\') == false, "Name contains \\");

            string filename = Path.Combine(this.GoodsDirectoryPath, goodsFileName + FILE_EXTENSION); 

            logger.Debug("Creating file stream: {0}", filename);

            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return stream;
        }

        public GalaxyMap LoadGalaxyMap(string galaxyMapName)
        {
            logger.Info("Loading map: {0}", galaxyMapName);
            GalaxyMap galaxyMap = null;
            try
            {
                GalaxyMapLoader mapLoader = new GalaxyMapLoader();
                galaxyMap = mapLoader.LoadGalaxyMap(galaxyMapName, this);
                galaxyMap.Lock();
            }
            catch (XmlException ex)
            {
                logger.Fatal("Map loading failed: {0}", ex.Message, ex);
                throw;
            }
            catch (GalaxyMapBuildingException ex)
            {
                logger.Fatal("Map building failed: {0}", ex.Message, ex);
                throw;
            }
            return galaxyMap;
        }

        /// <summary>
        /// Loading goods from goods file by file name.
        /// </summary>
        /// <param name="goodsFileName">Goods file name (without extension).</param>
        /// <returns>List of goods.</returns>
        public IList<IGoods> LoadGoods(string goodsFileName)
        {
            GoodsLoader goodsLoader = new GoodsLoader();

            return goodsLoader.LoadGoods(goodsFileName, this);
        }
    }
}
