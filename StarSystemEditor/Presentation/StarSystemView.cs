using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Zobrazovac Star Systemu na Galaxy mape
    /// </summary>
    public class StarSystemView : View
    {
        /// <summary>
        /// Vykreslovany objekt
        /// </summary>
        public StarSystem StarSystem { get; private set; }
        /// <summary>
        /// Property pro jmeno
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Propery pro vykreslovanou pozici
        /// </summary>
        public Point2d Position { get; private set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="starSystem">Planeta k zobrazeni</param>
        public StarSystemView(StarSystem starSystem)
        {
            this.StarSystem = starSystem;
            this.Position = starSystem.MapPosition;
            this.Name = starSystem.Name;
        }
        /// <summary>
        /// Metoda vracejici grafiku objektu
        /// </summary>
        /// <returns>Grafika objektu</returns>
        public override System.Windows.Shapes.Ellipse GetShape()
        {
            double starSystemRadius = Editor.dataPresenter.GetStarSystemRadius();
            double ratio = Editor.dataPresenter.ObjectSizeRatio;
            //Editor.Log(Editor.dataPresenter.ObjectSizeRatio.ToString());
            starSystemRadius *= ratio;
            Ellipse systemShape = new Ellipse();
            systemShape.Width = 2 * starSystemRadius;
            systemShape.Height = 2 * starSystemRadius;
            systemShape.Fill = Brushes.Yellow;
            // odstrani mezery v nazvu
            systemShape.Name = StarSystem.Name.ToString().Replace(" ", "");
            return systemShape;
        }
        /// <summary>
        /// Metoda vracejici zobrazovany objekt
        /// </summary>
        /// <returns>Zobrazovany objekt</returns>
        public override object GetLoadedObject()
        {
            return this.StarSystem;
        }
        /// <summary>
        /// Metoda vracejici jmeno objektu
        /// </summary>
        /// <returns>Jmeno objektu</returns>
        public override string GetName()
        {
            return this.Name;
        }
        /// <summary>
        /// Metoda pro ziskani velikosti objektu
        /// </summary>
        /// <returns>Velikost objektu</returns>
        public override System.Windows.Size GetSize()
        {
            Size size = new Size();
            size.Width = Editor.dataPresenter.GetStarSystemRadius();
            size.Height = size.Width;
            return size;
        }
    }
}
