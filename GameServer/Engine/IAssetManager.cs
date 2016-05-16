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
using SpaceTraffic.Data;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Minigame;

namespace SpaceTraffic.Engine
{
    public interface IAssetManager : IGalaxyMapDataStreamProvider, IGoodsDataStreamProvider,  IDisposable
    {
        /// <summary>
        /// Vrací cestu k assetům
        /// </summary>
        string AssetRootPath { get; }

        //void Initialize();

        /// <summary>
        /// Vrací cestu k adresáři s mapou.
        /// </summary>
        string MapDirectoryPath { get; }

        /// <summary>
        /// Vrací cestu k souboru s mapou.
        /// </summary>
        /// <param name="name">Jméno mapy.</param>
        /// <returns>Úplnou cestu k adresáři s mapou daného jména.</returns>
        string GetMapFilePath(string name);

        //GalaxyMap LoadGalaxyMap(string galaxyMapName);

        /// <summary>
        /// Method for loading logos for LogoQuiz.
        /// </summary>
        /// <returns>list of logo or null</returns>
        List<Logo> LoadLogos();

        /// <summary>
        /// Method for loading email templates for MailClient
        /// </summary>
        /// <returns>Dictionary of templates or null</returns>
        Dictionary<string, string> LoadEmailTemplates();
    }
}
