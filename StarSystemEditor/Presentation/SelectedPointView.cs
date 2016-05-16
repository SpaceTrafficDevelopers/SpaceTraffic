using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceTraffic.Game.Geometry;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using SpaceTraffic.Game;
using SpaceTraffic.Utils;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Zobrazovac bodu pro editaci na vybranem objektu
    /// </summary>
    public class SelectedPointView : View
    {
        #region properties
        /// <summary>
        /// pridruzeny objekt - planeta/wormhole/trajektorie
        /// </summary>
        public object AssociatedObject { get; private set; }
        /// <summary>
        /// Property pro jmeno
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Velikost bodu na canvasu
        /// </summary>
        public double Size { get; private set; }
        /// <summary>
        /// Property pro pozici bodu, predana konstruktorem z datapresenteru
        /// </summary>
        public Point2d Point{ get; private set;}
        /// <summary>
        /// Propery pro vykreslovanou pozici
        /// </summary>
        public Point2d Position { get; private set; }
        #endregion

        #region konstanty
        private const double DEFAULT_POINT_SIZE = 8;
        private const double DEFAULT_STARSYSTEM_POINT_SIZE = 20;
        #endregion

        #region konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="associatedObject">objekt kteremu se dela bod</param>
        /// <param name="point">misto bodu</param>
        public SelectedPointView(object associatedObject, Point2d point)
        {
            this.AssociatedObject = associatedObject;
            this.Point = point;
            this.Name = "point";
        }
        #endregion

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The centre point of rotation.</param>
        /// <param name="angleInDegrees">The rotation angle in degrees.</param>
        /// <returns>Rotated point</returns>
        static Point2d RotatePoint(Point2d pointToRotate, Point2d centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point2d
            {
                X =
                    // (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    //  (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }

        /// <summary>
        /// Metoda vracejici seznam dvou bodu.
        /// Oba jsou na trajektorii vybraneho objektu.
        /// První bod je v pravo pro ovladani sirky, druhý nahore pro vysku. 
        /// </summary>
        /// <param name="celestialObjectView">view objectu behajici po orbite(planeta/wormhole)</param>
        /// <returns></returns>
        public List<SelectedPointView> getTrajectoryPoint(CelestialObjectView celestialObjectView)
        {
            List<SelectedPointView> points = new List<SelectedPointView>();
            Ellipse trajectory = celestialObjectView.GetTrajectoryView().GetShape();
            if (celestialObjectView.GetTrajectoryView().Trajectory is EllipticOrbit)
            {
                
                EllipticOrbit orbit = ((EllipticOrbit)celestialObjectView.GetTrajectoryView().Trajectory);
                double rotationAngle = MathUtil.RadianToDegree(orbit.RotationAngleInRad);
                // stred canvasu, ne elipsy - pro rotaci
                Point2d center = new Point2d(Editor.dataPresenter.DrawingAreaSize, Editor.dataPresenter.DrawingAreaSize);
                //bod sirky
                double x = celestialObjectView.GetTrajectoryView().Position.X +
                    orbit.Cx * Editor.dataPresenter.ObjectSizeRatio + orbit.A * 2 * Editor.dataPresenter.ObjectSizeRatio;
                double y = celestialObjectView.GetTrajectoryView().Position.Y + trajectory.Height / 2.0;
                Point2d point = new Point2d(x, y);
                Point2d newPoint = RotatePoint(point, center, -rotationAngle);
                SelectedPointView pointView = new SelectedPointView(celestialObjectView.GetTrajectoryView(), newPoint);
                //add points to list of points, needed for hit testing
                points.Add(pointView);
                // bod vysky
                double x2 = x - trajectory.Width / 2.0;
                double y2 = y - trajectory.Height / 2.0;
                point.X = x2;
                point.Y = y2;
                // draw trajectory minor axis dragging point 
                newPoint = RotatePoint(point, center, -rotationAngle);
                pointView = new SelectedPointView(celestialObjectView.GetTrajectoryView(), newPoint);
                //add points to list of points, needed for hit testing
                points.Add(pointView);
                //bod stredu - posouvaci
                double x3 = x2;
                double y3 = y2 + trajectory.Height / 2.0;
                point.X = x3;
                point.Y = y3;
                // draw trajectory minor axis dragging point 
                newPoint = RotatePoint(point, center, -rotationAngle);
                pointView = new SelectedPointView(celestialObjectView.GetTrajectoryView(), newPoint);
                //add points to list of points, needed for hit testing
                points.Add(pointView);
            }
            if (celestialObjectView.GetTrajectoryView().Trajectory is CircularOrbit)
            {
                CircularOrbit orbit = ((CircularOrbit)celestialObjectView.GetTrajectoryView().Trajectory);
                // bod sirky
                double x = celestialObjectView.GetTrajectoryView().Position.X + trajectory.Width;
                double y = celestialObjectView.GetTrajectoryView().Position.Y + trajectory.Height / 2.0;
                Point2d point = new Point2d(x, y);
                SelectedPointView pointView = new SelectedPointView(celestialObjectView.GetTrajectoryView(), point);
                //add points to list of points, needed for hit testing
                points.Add(pointView);
                // bod vysky
                double x2 = x - trajectory.Width / 2.0;
                double y2 = y - trajectory.Height / 2.0;
                point.X = x2;
                point.Y = y2;
                pointView = new SelectedPointView(celestialObjectView.GetTrajectoryView(), point);
                //add points to list of points, needed for hit testing
                points.Add(pointView);
                //bod stredu - posouvaci
                double x3 = x2;
                double y3 = y2 + trajectory.Height / 2.0;
                point.X = x3;
                point.Y = y3;
                pointView = new SelectedPointView(celestialObjectView.GetTrajectoryView(), point);
                //add points to list of points, needed for hit testing
                points.Add(pointView);
            }
            return points;
        }

        /// <summary>
        /// Vraci ellipsu pro vykresleni na platno
        /// </summary>
        /// <returns>ellipsa</returns>
        public override Ellipse GetShape()
        {
            Ellipse ellipse = new Ellipse();
            double ratio = Editor.dataPresenter.ObjectSizeRatio;
            ellipse.Fill = Brushes.Blue;
            if(AssociatedObject is StarSystemView)
                this.Size = DEFAULT_STARSYSTEM_POINT_SIZE;
            else
                this.Size = DEFAULT_POINT_SIZE;
            ellipse.Width = Size*ratio;
            ellipse.Height = Size*ratio;
            Name = "Point" + 0;
            Point2d pos = new Point2d();
            pos.X = Point.X - this.Size/2.0 * ratio;
            pos.Y = Point.Y - this.Size/2.0 * ratio;
            Position = pos;
            return ellipse;
        }

        /// <summary>
        /// objekt kteremu vykreslujeme bod
        /// </summary>
        /// <returns>objekt</returns>
        public override object GetLoadedObject()
        {
            return this.AssociatedObject;
        }
        /// <summary>
        /// jmeno objektu
        /// </summary>
        /// <returns>jmeno</returns>
        public override string GetName()
        {
            return this.Name;
        }
        /// <summary>
        /// velikost bodu
        /// </summary>
        /// <returns>velikost</returns>
        public override Size GetSize()
        {
            Size size = new Size();
            size.Width = Size;
            size.Height = Size;
            return size;
        }
    }
}
