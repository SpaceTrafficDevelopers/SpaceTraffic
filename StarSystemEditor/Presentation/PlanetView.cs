using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Zobrazovac planety
    /// </summary>
    public class PlanetView : View
    {
        /// <summary>
        /// Vykreslovany objekt
        /// </summary>
        public Planet Planet { get; private set; }
        /// <summary>
        /// Property pro jmeno
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Propery pro vykreslovanou pozici
        /// </summary>
        public Point2d Position { get; private set; }
        /// <summary>
        /// Zobrazovac trajektorie planety
        /// </summary>
        public TrajectoryView TrajectoryView { get; private set; }
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="planet">Planeta k zobrazeni</param>
        public PlanetView(Planet planet)
        {
            this.Planet = planet;
        }
        /// <summary>
        /// Metoda vracejici grafiku objektu
        /// </summary>
        /// <returns>Grafika objektu</returns>
        public override Ellipse GetShape() 
        {
            double planetRadius = Editor.dataPresenter.GetPlanetRadius();
            double ratio = Editor.dataPresenter.ObjectSizeRatio;
            planetRadius *= ratio;
            Ellipse planetShape = new Ellipse();
            planetShape.Width = 2 * planetRadius;
            planetShape.Height = 2 * planetRadius;
            planetShape.Fill = Brushes.Green;
            Name = Planet.Name.Replace(" ", "");
            planetShape.Name = Planet.AlternativeName.ToString().Replace(" ", "");
            Point2d point = TrajectoryView.Trajectory.CalculatePosition(Editor.Time);
            point.X *= Editor.dataPresenter.ObjectSizeRatio;
            point.Y *= Editor.dataPresenter.ObjectSizeRatio;
            point.X += Editor.dataPresenter.DrawingAreaSize - planetRadius;
            point.Y += Editor.dataPresenter.DrawingAreaSize - planetRadius;
            Position = point;
            return planetShape;
        }
        /// <summary>
        /// Metoda pro ziskani grafiky trajektorie
        /// </summary>
        /// <returns>Grafika trajektorie</returns>
        public Ellipse GetTrajectoryShape()
        {
            TrajectoryView = new TrajectoryView(this.Planet.Trajectory);
            Ellipse trajectory = TrajectoryView.GetShape();
            return trajectory;
        }
        /// <summary>
        /// Metoda vracejici zobrazovany objekt
        /// </summary>
        /// <returns>Zobrazovany objekt</returns>
        public override Object GetLoadedObject() 
        {
            return this.Planet;
        }
        /// <summary>
        /// Metoda vracejici jmeno objektu
        /// </summary>
        /// <returns>Jmeno objektu</returns>
        public override String GetName()
        {
            return this.Planet.AlternativeName;
        }
        /// <summary>
        /// Metoda vracejici velikost objektu
        /// </summary>
        /// <returns>Velikost objektu</returns>
        public override Size GetSize()
        {
            Size size = new Size();
            size.Width = Editor.dataPresenter.GetPlanetRadius();
            size.Height = size.Width;
            return size;
        }
    }
}
