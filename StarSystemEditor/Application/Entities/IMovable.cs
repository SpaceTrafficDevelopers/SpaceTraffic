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
