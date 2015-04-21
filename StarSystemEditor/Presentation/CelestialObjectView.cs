using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Geometry;
using System.Windows.Shapes;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Abastraktni trida pro planety a wormholy, pridava moznost vratit trajektoryView
    /// </summary>
    public abstract class CelestialObjectView : View
    {
        /// <summary>
        /// Property s pozici objektu
        /// </summary>
        public abstract Point2d Position { get; set; }
        /// <summary>
        /// Abstraktni metoda pro navrat trajectoryView
        /// </summary>
        /// <returns>object TrajectoryView patrici planete nebo wormhole</returns>
        public abstract TrajectoryView GetTrajectoryView();
        /// <summary>
        /// Metoda nastavujici TrajectoryView - potrebna pro zmenu mezi kruhovou a eliptickou orbitou
        /// </summary>
        public abstract void SetTrajectoryView(TrajectoryView view);
        /// <summary>
        /// Metoda vracejici grafiku trajektorie
        /// </summary>
        /// <returns></returns>
        public abstract Ellipse GetTrajectoryShape();
    }
}
