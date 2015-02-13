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
    /// Trida starajici se o zobrazeni cervich der
    /// </summary>
    public class EndpointView : View
    {
        /// <summary>
        /// Instance zobrazovaneho WormholeEndpoint
        /// </summary>
        public WormholeEndpoint WormholeEndpoint { get; private set; }
        /// <summary>
        /// Jmeno pouzivane pro vyhledavani v seznamech
        /// </summary>
        public string IdentityName { get; set; }
        /// <summary>
        /// Pozice pro vykresleni
        /// </summary>
        public Point2d Position { get; private set; }
        /// <summary>
        /// Zobrazovac trajektorie
        /// </summary>
        public TrajectoryView TrajectoryView { get; private set; }
        /// <summary>
        /// Konstuktor zobrazovace
        /// </summary>
        /// <param name="endpoint"></param>
        public EndpointView(WormholeEndpoint endpoint)
        {
            this.WormholeEndpoint = endpoint;
        }
        /// <summary>
        /// Metoda objekt cervi diry pro zobrazeni
        /// </summary>
        /// <returns>Grafika pro zobrazeni</returns>
        public override Ellipse GetShape() 
        {
            double planetRadius = Editor.dataPresenter.GetPlanetRadius();
            double ratio = Editor.dataPresenter.ObjectSizeRatio;
            planetRadius *= ratio;
            Ellipse endpointShape = new Ellipse();
            endpointShape.Width = 2 * planetRadius;
            endpointShape.Height = 2 * planetRadius;
            endpointShape.Fill = Brushes.Blue;
            IdentityName = "Wormhole[" + WormholeEndpoint.Id + "]";
            Point2d point = TrajectoryView.Trajectory.CalculatePosition(Editor.Time);
            point.X *= Editor.dataPresenter.ObjectSizeRatio;
            point.Y *= Editor.dataPresenter.ObjectSizeRatio;
            point.X += Editor.dataPresenter.DrawingAreaSize - planetRadius;
            point.Y += Editor.dataPresenter.DrawingAreaSize - planetRadius;
            Position = point;
            return endpointShape;
        }
        /// <summary>
        /// Metoda volajici zobrazovac trajektorie a vracejici objekt pro zobrazeni
        /// </summary>
        /// <returns>Grafika pro zobrazeni</returns>
        public Ellipse GetTrajectoryShape()
        {
            TrajectoryView = new TrajectoryView(this.WormholeEndpoint.Trajectory);
            Ellipse trajectory = TrajectoryView.GetShape();
            trajectory.Stroke = Brushes.Teal;
            return trajectory;
        }
        /// <summary>
        /// Vraci instanci zobrazovaneho objektu
        /// </summary>
        /// <returns>Instance zobrazovaneho objektu</returns>
        public override Object GetLoadedObject() 
        {
            return this.WormholeEndpoint;
        }
        /// <summary>
        /// Metoda vracejici jmeno objektu
        /// </summary>
        /// <returns>Jmeno objektu</returns>
        public override String GetName()
        {
            return "Wormhole[" + WormholeEndpoint.Id +"]";
        }
        /// <summary>
        /// Metoda vracejici rozmer vykreslovaneho objektu
        /// </summary>
        /// <returns>Rozmer grafiky</returns>
        public override Size GetSize()
        {
            Size size = new Size();
            size.Width = Editor.dataPresenter.GetPlanetRadius();
            size.Height = size.Width;
            return size;
        }
    }
}
