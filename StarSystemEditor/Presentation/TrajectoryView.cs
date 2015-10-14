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
using System.Windows.Shapes;
using System.Windows.Media;
using SpaceTraffic.Game.Geometry;
using SpaceTraffic.Utils;
using SpaceTraffic.Game;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Trida pro zobrazeni trajektorie
    /// </summary>
    public class TrajectoryView : View
    {
        /// <summary>
        /// Vykreslovany objekt
        /// </summary>
        public Trajectory Trajectory { get; private set; }
        /// <summary>
        /// Pozice pro vykresleni
        /// </summary>
        public Point2d Position { get; set; }
        /// <summary>
        /// Konstuktor
        /// </summary>
        /// <param name="trajectory"></param>
        public TrajectoryView(Trajectory trajectory)
        {
            this.Trajectory = trajectory;
        }
        /// <summary>
        /// Metoda vracejici grafiku pro vykresleni
        /// </summary>
        /// <returns>Grafiky pro vykresleni</returns>
        public override Ellipse GetShape()
        {
            Brush brush = Brushes.White;
            Ellipse ellipse = new Ellipse();
            double a = 0;
            double b = 0;
            double rotationAngle = 0;
            double xPos = Editor.dataPresenter.DrawingAreaSize;
            double yPos = Editor.dataPresenter.DrawingAreaSize;
            if (Trajectory is CircularOrbit)
            {
                CircularOrbit orbit = (CircularOrbit)Trajectory;
                a = orbit.Radius * Editor.dataPresenter.ObjectSizeRatio;
                b = a;
                //Point2d point = orbit.CalculatePosition(0);
                xPos -= a;
                yPos -= b;
                ellipse.Width = 2 * a;
                ellipse.Height = 2 * b;
            }
            else if (Trajectory is EllipticOrbit)
            {
                
                //EllipticOrbit orbit1 = this.startNavPoint.Location.Trajectory as EllipticOrbit;

                EllipticOrbit orbit = (EllipticOrbit)Trajectory;
                double cx = orbit.Cx * Editor.dataPresenter.ObjectSizeRatio;
                double cy = orbit.Cy * Editor.dataPresenter.ObjectSizeRatio;
                //setting ellipse
                ellipse.Width = 2 * orbit.A * Editor.dataPresenter.ObjectSizeRatio;
                ellipse.Height = 2 * orbit.B * Editor.dataPresenter.ObjectSizeRatio;
                //transformation prepare
                TransformGroup transformations = new TransformGroup();
                //translation
                TranslateTransform translate = new TranslateTransform(cx, cy);
                transformations.Children.Add(translate);
                //rotation
                rotationAngle = MathUtil.RadianToDegree(orbit.RotationAngleInRad);
                RotateTransform rotateTransform = new RotateTransform(-rotationAngle);
                rotateTransform.CenterX = (orbit.A - orbit.OrbitalEccentricity) * Editor.dataPresenter.ObjectSizeRatio;
                rotateTransform.CenterY = (orbit.B) * Editor.dataPresenter.ObjectSizeRatio;
                transformations.Children.Add(rotateTransform);
                //finishing transformation
                ellipse.RenderTransform = transformations;
                brush = Brushes.Wheat;
                //setting drawing position
                xPos -= orbit.A * Editor.dataPresenter.ObjectSizeRatio;
                yPos -= orbit.B * Editor.dataPresenter.ObjectSizeRatio;
                #region junk
                //gr.RotateTransform((float)MathUtil.RadianToDegree(orbit.RotationAngleInRad));
                //gr.TranslateTransform(cx, 0); //vycentrování elipsy
                //this.DrawEllipse(gr, orbitPen, 0, 0, orbit.A, orbit.B);
                //gr.TranslateTransform(-cx, 0); //vrácení zpět
                //gr.RotateTransform(-(float)MathUtil.RadianToDegree(orbit.RotationAngleInRad));
                //a = orbit.Cx;
                //b = orbit.Cy;
                #endregion
            }
            else
            {
                throw new ArgumentException("Invalid trajectory");
            }
            //a = (int)(a * Editor.dataPresenter.ObjectSizeRatio);
            //b = (int)(b * Editor.dataPresenter.ObjectSizeRatio);
            //a,b = semi-axis
            //placement logic
            //Editor.Log("size: " + Editor.dataPresenter.DrawingAreaSize + ", a:" + a);
            //Editor.Log("Trajectory: " + xPos + ":" + yPos);

            ellipse.Stroke = brush;
            ellipse.StrokeThickness = 1;
            Point2d drawingPoint= new Point2d(xPos, yPos);
            this.Position = drawingPoint;

            return ellipse;

        }
        /// <summary>
        /// Getter pro vykreslovany objekt
        /// </summary>
        /// <returns></returns>
        public override Object GetLoadedObject() 
        {
            return this.Trajectory;
        }
        /// <summary>
        /// Getter pro jmeno
        /// </summary>
        /// <returns>Jmeno objektu</returns>
        public override String GetName()
        {
            return Trajectory.GetType().Name;
        }

        /// <summary>
        /// Getter pro stred trajektorie
        /// </summary>
        /// <returns>Souradnice stredu trajektorie</returns>
        public Point2d GetCenter()
        {
            Point2d point = new Point2d();
            if (Trajectory is CircularOrbit)
            {
                CircularOrbit orbit = (CircularOrbit)Trajectory;
                point.X = orbit.Radius * Editor.dataPresenter.ObjectSizeRatio;
                point.Y = point.X;
            }
            else if (Trajectory is EllipticOrbit)
            {
            EllipticOrbit orbit = (EllipticOrbit)Trajectory;
            point.X = (orbit.A - orbit.OrbitalEccentricity) * Editor.dataPresenter.ObjectSizeRatio;
            point.Y = (orbit.B) * Editor.dataPresenter.ObjectSizeRatio;
            }
            else
            {
                throw new ArgumentException("Invalid trajectory");
            }

            return point;
        }
        
        /// <summary>
        /// Getter pro velikost grafiky
        /// </summary>
        /// <returns>Velikost grafiky</returns>
        public override Size GetSize()
        {
            Size size = new Size();
            if (this.Trajectory is CircularOrbit) 
            {
                size.Width = ((CircularOrbit)this.Trajectory).Radius;
                size.Height = size.Width;
            }
            else if (this.Trajectory is EllipticOrbit)
            {
                size.Width = ((EllipticOrbit)this.Trajectory).A * 2;
                size.Height = ((EllipticOrbit)this.Trajectory).B * 2;
            }
            return size;
        }
        
        /// <summary>
        /// sets trajectory
        /// </summary>
        /// <param name="trajectory"></param>
        public void SetTrajectory(Trajectory trajectory)
        {
            this.Trajectory = trajectory;
        }
    }
}
