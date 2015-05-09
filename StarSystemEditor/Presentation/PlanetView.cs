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
    public class PlanetView : CelestialObjectView
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
        public override Point2d Position { get; set; }
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
            this.Name = planet.AlternativeName.ToString().Replace(" ", "");
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
            Name = Planet.AlternativeName.ToString().Replace(" ", "");
            planetShape.Name = Name;//Planet.AlternativeName.ToString().Replace(" ", "");
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
        public override Ellipse GetTrajectoryShape()
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
            return this.Name;
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

        /// <summary>
        /// Metoda vracejici TrajectoryView tohoto objektu
        /// </summary>
        /// <returns>TrajectoryView instance</returns>
        public override TrajectoryView GetTrajectoryView()
        {
            return TrajectoryView;
        }
        /// <summary>
        /// Metoda nastavujici TrajectoryView - potrebna pro zmenu mezi kruhovou a eliptickou orbitou
        /// </summary>
        /// <param name="view"></param>
        public override void SetTrajectoryView(TrajectoryView view)
        {
            this.TrajectoryView = view;
        }

    }
}
