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

using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor.Entities
{
    /// <summary>
    /// Interface ktery by mely implementovat veskere editory pracujici s presunutelnymi objekty
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// Metoda pro presunuti objektu na danou pozici
        /// </summary>
        /// <param name="coordX">Souradnice X</param>
        /// <param name="coordY">Souradnice Y</param>
        void MoveTo(double coordX, double coordY);
        /// <summary>
        /// Metoda pro presunuti objektu na danou pozici
        /// </summary>
        /// <param name="coords">Souradnice</param>
        void MoveTo(Point2d coords);
    }
}
