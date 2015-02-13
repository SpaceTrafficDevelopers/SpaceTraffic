using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Zobrazovac hvezdy
    /// </summary>
    public class StarView : View
    {
        /// <summary>
        /// Vykreslovany objekt
        /// </summary>
        public Star Star { get; private set; }
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="star">Hvezda k vykresleni</param>
        public StarView(Star star)
        {
            this.Star = star;
        }
        /// <summary>
        /// Metoda vracejici grafiku objektu
        /// </summary>
        /// <returns>Grafika objektu</returns>
        public override Ellipse GetShape() 
        {
            double starRadius = Editor.dataPresenter.GetStarRadius();
            double ratio = Editor.dataPresenter.ObjectSizeRatio;
            //Editor.Log(Editor.dataPresenter.ObjectSizeRatio.ToString());
            starRadius *= ratio;
            Ellipse planetShape = new Ellipse();
            planetShape.Width = 2 * starRadius;
            planetShape.Height = 2 * starRadius;
            planetShape.Fill = Brushes.Yellow;
            planetShape.Name = Star.Name.ToString().Replace(" ", "");
            return planetShape;
        }
        /// <summary>
        /// Metoda vracejici zobrazovany objekt
        /// </summary>
        /// <returns>Zobrazovany objekt</returns>
        public override Object GetLoadedObject()
        {
            return this.Star;
        }
        /// <summary>
        /// Metoda vracejici jmeno objektu
        /// </summary>
        /// <returns>Jmeno objektu</returns>
        public override String GetName()
        {
            return this.Star.Name;
        }
        /// <summary>
        /// Metoda pro ziskani velikosti objektu
        /// </summary>
        /// <returns>Velikost objektu</returns>
        public override Size GetSize()
        {
            Size size = new Size();
            size.Width = Editor.dataPresenter.GetStarRadius();
            size.Height = size.Width;
            return size;
        }
    }
}
