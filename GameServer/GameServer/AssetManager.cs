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
using SpaceTraffic.Game.Minigame;
using SpaceTraffic.Data.Minigame;
using SpaceTraffic.Data.EmailClient;

namespace SpaceTraffic.GameServer
{
    public class AssetManager : IAssetManager
    {
        public const string FILE_EXTENSION = ".xml";
        private string _mapDirectoryPath = ".\\";

        private string _goodsDirectoryPath = ".\\";

        private string _economicLevelsDirectoryPath = ".\\";

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

        public string EconomicLevelsDirectoryPath
        {
            get
            {
                Debug.Assert(this.IsInitialized, "Not initialized");
                return this._economicLevelsDirectoryPath;
            }
            private set
            {
                Debug.Assert(!String.IsNullOrWhiteSpace(value), "EconomicLevelsDirectoryPath cannot be null, empty or whitespace");
                this._economicLevelsDirectoryPath = value;
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
            this.SetEconomicLevelsPath();

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

        /// <summary>
        /// Sets economic levels path to EconomicLevels directory.
        /// Throws DirectoryNotFoundException when EconomicLevels directory isn't found.
        /// </summary>
        private void SetEconomicLevelsPath()
        {
            string path = Path.Combine(this.AssetRootPath, "EconomicLevels");
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Assets directory 'EconomicLevels' not found: " + path);
            }
            this.EconomicLevelsDirectoryPath = path;
            logger.Info("Assets Economic levels directory path: {0}", path);
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

        /// <summary>
        /// Gets the data stream for economic level file specified by file name (without extension).
        /// Throws ArgumentNullException when the economicLevelsFileName is null or empty.
        /// </summary>
        /// <param name="economicLevelsFileName">Name of the economic level file (without extension).</param>
        /// <returns>Stream with economic level file.</returns>
        public Stream GetEconomicLevelStream(string economicLevelsFileName)
        {
            Debug.Assert(this.IsInitialized, "Not initialized");

            if (String.IsNullOrWhiteSpace(economicLevelsFileName))
                throw new ArgumentNullException("Name cannot be null or empty string.");

            Debug.Assert(economicLevelsFileName.Contains('\\') == false, "Name contains \\");

            string filename = Path.Combine(this.EconomicLevelsDirectoryPath, economicLevelsFileName + FILE_EXTENSION);

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
        /// <param name="economicLevelsFileName">Goods file name (without extension).</param>
        /// <returns>List of goods.</returns>
        public IList<IGoods> LoadGoods(string goodsFileName)
        {
            GoodsLoader goodsLoader = new GoodsLoader();

            return goodsLoader.LoadGoods(goodsFileName, this);
        }

        /// <summary>
        /// Loading economic levels from economic levels file by file name.
        /// </summary>
        /// <param name="goodsFileName">economic level file name (without extension).</param>
        /// <returns>List of economic level.</returns>
        public IList<EconomicLevel> LoadEconomicLevels(string economicLevelsFileName)
        {
            EconomicLevelLoader economicLevelLoader = new EconomicLevelLoader();

            return economicLevelLoader.LoadEconomicLevels(economicLevelsFileName, this);
        }

		/// <summary>
		/// Loads the achievements from xml.
		/// </summary>
		public Entities.Achievements LoadAchievements()
		{
			string fileName = Path.Combine(this.AssetRootPath, "Achievements", "Achievements.xml");
			logger.Info("Loading achievements: {0}", fileName);

			Entities.Achievements achievements = AchievementsLoader.LoadAchievements(fileName);

			return achievements;
		}

		public Entities.ExperienceLevels LoadExperienceLevels()
		{
			string fileName = Path.Combine(this.AssetRootPath, "Achievements", "Levels.xml");
			logger.Info("Loading experience levels: {0}", fileName);

			Entities.ExperienceLevels experienceLevels = ExperienceLevelsLoader.LoadExperienceLevels(fileName);

			return experienceLevels;
		}

        /// <summary>
        /// Method for loading logos for LogoQuiz.
        /// </summary>
        /// <returns>list of logo or null</returns>
        public List<Logo> LoadLogos()
        {
            string fileName = Path.Combine(this.AssetRootPath, "Minigames", "LogoQuiz", "logos.xml");
			logger.Info("Loading logos for LogoQuiz: {0}", fileName);

            return LogoQuizLoader.loadLogos(fileName);
        }

        /// <summary>
        /// Method for loading email templates for MailClient
        /// </summary>
        /// <returns>Dictionary of templates or null</returns>
        public Dictionary<string, string> LoadEmailTemplates()
        {
            string pathBase = Path.Combine(this.AssetRootPath, "EmailTemplates");
            logger.Info("Loading templates for MailClient from: {0}", pathBase);

            return EmailTemplateLoader.loadAllTemplates(pathBase);
        }
    }
}
