using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpaceTraffic.Data
{
    /// <summary>
    /// Provides streams to map files of given names.
    /// </summary>
    public interface IGalaxyMapDataStreamProvider
    {
        /// <summary>
        /// Gets the data stream for star system specified by name (without extension).
        /// </summary>
        /// <param name="starSystemName">Name of the star system (without extension).</param>
        /// <returns></returns>
        Stream GetStarSystemStream(string starSystemName);

        /// <summary>
        /// Gets the data stream for galaxy map specified by name (without extension).
        /// </summary>
        /// <param name="mapName">Name of the galaxy map (without extension).</param>
        /// <returns></returns>
        Stream GetGalaxyMapStream(string mapName);
    }
}
