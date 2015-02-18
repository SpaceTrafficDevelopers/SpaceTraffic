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
