using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Data;
using SpaceTraffic.Game;

namespace SpaceTraffic.Engine
{
    public interface IAssetManager : IGalaxyMapDataStreamProvider, IDisposable
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
    }
}
