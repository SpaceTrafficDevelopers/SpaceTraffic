﻿/**
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
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;
using SpaceTraffic.Tools.StarSystemEditor.Data;
using SpaceTraffic.Utils;
using SpaceTraffic.Tools.StarSystemEditor.Entities;
using System.Windows.Input;
using System.Threading;
using System.Diagnostics;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Trida obsluhujici hlavni logiku programu a vykresleni vsech komponent
    /// </summary>
    public class DataPresenter
    {
        #region Constants
        private const double DEFAULT_DRAWING_AREA_HALF_SIZE = 250;
        private const double DEFAULT_PLANET_RADIUS = 5;
        private const double DEFAULT_SUN_RADIUS = 20;
        private const double DEFAULT_STARSYSTEM_RADIUS = 10;
        #endregion

        #region Objects
        private DrawingArea drawingArea;
        private TextBlock galaxyInfo;
        private FrameworkElement loadedObjectData; 
        private FrameworkElement loadedWormholeData;
        private TreeView starSystemObjectTree;
        private FrameworkElement connectionList;
        private ListView starSystemList;

        #endregion

        #region Properties
        /// <summary>
        /// Property pro nastaveni velikosti zobrazeni
        /// </summary>
        public double DrawingAreaSize { get; set; }
        /// <summary>
        /// Metoda pro vypocet pomeru pro zobrazeni teles pri zvetseni
        /// </summary>
        public double ObjectSizeRatio
        {
            get { return DrawingAreaSize / DEFAULT_DRAWING_AREA_HALF_SIZE; }
            private set { }
        }
        /// <summary>
        /// Property pro vybrany objekt
        /// </summary>
        public View SelectedObject { get; set; }
        /// <summary>
        /// Property pro vybrany starsystem
        /// </summary>
        public StarSystem SelectedStarSystem { get; private set; }
        /// <summary>
        /// seznam bodu pro editaci objektu
        /// 0 - planeta/wormhole, 1 - sirka, 2 - vyska
        /// </summary>
        private List<SelectedPointView> points = new List<SelectedPointView>();
        /// <summary>
        /// indicator whether there is something selected or not
        /// </summary>
        public bool selected { get; set; }
        /// <summary>
        /// Definice delegatni metody
        /// </summary>
        public delegate void PlanetSelected();
        /// <summary>
        /// Pointer na metodu
        /// </summary>
        public PlanetSelected planetSelectionChanged { get; set; }
        /// <summary>
        /// Definice delegatni metody
        /// </summary>
        public delegate void WormholeSelected();
        /// <summary>
        /// Pointer na metodu
        /// </summary>
        public WormholeSelected wormholeSelectionChanged { get; set; }
        /// <summary>
        /// Definice delegatni metody
        /// </summary>
        public delegate void StarSystemListChanged();
        /// <summary>
        /// Pointer na metodu
        /// </summary>
        public StarSystemListChanged starSystemListChanged { get; set; }
        /// <summary>
        /// Definice delegatni metody
        /// </summary>
        public delegate void ConnectionsChanged();
        /// <summary>
        /// Pointer na metodu
        /// </summary>
        public ConnectionsChanged connectionsChanged { get; set; }
        #endregion

        #region Getters
        /// <summary>
        /// vraci seznam bodu pro editaci objektu
        /// </summary>
        /// <returns>seznam bodu</returns>
        public List<SelectedPointView> GetPoints()
        {
            return points;
        }
        /// <summary>
        /// Getter pro standartni velikost planety
        /// </summary>
        /// <returns>Velikost</returns>
        public double GetPlanetRadius()
        {
            return DEFAULT_PLANET_RADIUS;
        }
        /// <summary>
        /// Getter pro standartni velikost starSystemu
        /// </summary>
        /// <returns>Velikost</returns>
        public double GetStarSystemRadius()
        {
            return DEFAULT_STARSYSTEM_RADIUS;
        }
        /// <summary>
        /// Getter pro standartni velikost hvezdy
        /// </summary>
        /// <returns>Velikost</returns>
        public double GetStarRadius()
        {
            return DEFAULT_SUN_RADIUS;
        }
        #endregion

        #region ObjectGetters
        /// <summary>
        /// Getter pro spojeni
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetConnectionList()
        {
            if (this.connectionList == null)
            {
                this.CreateConnectionList();
            }
            return this.connectionList;
        }
        /// <summary>
        /// Getter pro ziskani vykreslovace
        /// </summary>
        /// <returns></returns>
        public DrawingArea GetDrawingArea()
        {
            if (this.drawingArea == null)
            {
                this.CreateDrawingArea();
            }
            return this.drawingArea;
        }
        /// <summary>
        /// Getter pro ziskani informaci o galaxie
        /// </summary>
        /// <returns></returns>
        public TextBlock GetGalaxyInfo()
        {
            if (this.galaxyInfo == null)
            {
                this.CreateGalaxyInfo();
            }
            return this.galaxyInfo;
        }
        /// <summary>
        /// Getter pro ziskani nacitaneho objektu
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetLoadedObjectData()
        {
            if (this.loadedObjectData == null)
            {
                this.CreateLoadedObjectData();
            }
            return this.loadedObjectData;
        }
        /// <summary>
        /// Getter pro ziskani nacitaneho objektu
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetLoadedWormholeData()
        {
            if (this.loadedWormholeData == null)
            {
                this.CreateLoadedWormholeData();
            }
            return this.loadedWormholeData;
        }
        /// <summary>
        /// Getter pro seznam starsystemu
        /// </summary>
        /// <returns>Seznam starsystemu</returns>
        public ListView GetStarSystemList()
        {
            if (this.starSystemList == null)
            {
                this.CreateStarSystemList();
            }
            return this.starSystemList;
        }
        /// <summary>
        /// Getter pro objekty k zobrazeni
        /// </summary>
        /// <returns>Strom objketu v starsystemu</returns>
        public TreeView GetStarSystemObjectTree()
        {
            if (this.starSystemObjectTree == null)
            {
                this.CreateStarSystemObjectTree();
            }
            return this.starSystemObjectTree;
        }
        
        #endregion

        #region Constructor
        /// <summary>
        /// Konstruktor
        /// </summary>
        public DataPresenter()
        {
            this.DrawingAreaSize = DEFAULT_DRAWING_AREA_HALF_SIZE;
            this.loadedObjectData = null;
            this.SelectedObject = null;
            this.SelectedStarSystem = null;

            //this.drawingArea = new DrawingArea();
        }
        #endregion
        /// <summary>
        /// Metoda starajici se o vykresleni soustavy
        /// </summary>
        public void StarSystemDrawer()
        {
            // prepise text na tlacitku zpatky na Galaxy Map
            if (Editor.ButtonName.Equals("Star System"))
                return;
            DrawingArea.Canvas.Children.Clear();
            TreeView tree = this.GetStarSystemObjectTree();
            if (SelectedStarSystem == null) return;
            //Editor.Log("aftercheck");
            StarSystem starSystem = SelectedStarSystem;
            //Editor.Log("system" + starSystem.Name);
            Editor.StarSystemEditor.LoadObject(starSystem);
            //if (this.GetDrawingArea().Canvas == null) return;
            
            StarView starView = new StarView(starSystem.Star);
            //Editor.Log("system" + starSystem.Name);
            Ellipse star = starView.GetShape();

            double sunRadius = DEFAULT_SUN_RADIUS * ObjectSizeRatio;
            star.Tag = starView;
            Canvas.SetLeft(star, (DrawingAreaSize - sunRadius));
            Canvas.SetTop(star, (DrawingAreaSize - sunRadius));
            DrawingArea.Canvas.Children.Add(star);

            DrawingArea.ShowStarSystemInfo();
            this.GetLoadedObjectData(); 
            this.GetLoadedWormholeData();

            foreach (Planet planet in starSystem.Planets)
            {
                PlanetView planetView = new PlanetView(planet);

                Ellipse trajectoryShape = planetView.GetTrajectoryShape();
                trajectoryShape.Tag = planetView.TrajectoryView;
                Canvas.SetLeft(trajectoryShape, planetView.TrajectoryView.Position.X);
                Canvas.SetTop(trajectoryShape, planetView.TrajectoryView.Position.Y);

                DrawingArea.Canvas.Children.Add(trajectoryShape);

                Ellipse planetShape = planetView.GetShape();
                planetShape.Tag = planetView;
                Canvas.SetLeft(planetShape, planetView.Position.X);
                Canvas.SetTop(planetShape, planetView.Position.Y);
                DrawingArea.Canvas.Children.Add(planetShape);

            }
            foreach (WormholeEndpoint wormhole in starSystem.WormholeEndpoints)
            {
                EndpointView endpointView = new EndpointView(wormhole);

                Ellipse trajectoryShape = endpointView.GetTrajectoryShape();
                trajectoryShape.Tag = endpointView.TrajectoryView;
                Canvas.SetLeft(trajectoryShape, endpointView.TrajectoryView.Position.X);
                Canvas.SetTop(trajectoryShape, endpointView.TrajectoryView.Position.Y);
                DrawingArea.Canvas.Children.Add(trajectoryShape);

                Ellipse endpointShape = endpointView.GetShape();
                endpointShape.Tag = endpointView;
                Canvas.SetLeft(endpointShape, endpointView.Position.X);
                Canvas.SetTop(endpointShape, endpointView.Position.Y);
                DrawingArea.Canvas.Children.Add(endpointShape);
            }
            
            // unselects selected objects - used for editing goTo Editor.selectEntity();
            foreach (SelectedPointView point in points)
            {
                removeElement(point);
            }
            points.Clear();
            this.selected = false;
            
        }

        /// <summary>
        /// Draws galaxy map in canvas
        /// </summary>
        public void DrawGalaxyMap()
        {
            if (Editor.ButtonName.Equals("Galaxy Map"))
                return;
            
            DrawingArea.Canvas.Children.Clear();
            // nejdrive wormholy aby neprekryvali grafiku systemu
            foreach (StarSystem system in Editor.GalaxyMap)
            {
                
                StarSystemView starSystemView = new StarSystemView(system);
                
                foreach (WormholeEndpoint wormhole in starSystemView.StarSystem.WormholeEndpoints)
                {
                    if (wormhole.IsConnected)
                    {
                        WormholeEndpoint destinationWormhole = wormhole.Destination;
                        Line whole = new Line();
                        whole.X1 = wormhole.StarSystem.MapPosition.X;
                        whole.X2 = destinationWormhole.StarSystem.MapPosition.X;
                        whole.Y1 = wormhole.StarSystem.MapPosition.Y;
                        whole.Y2 = destinationWormhole.StarSystem.MapPosition.Y;
                        whole.Stroke = Brushes.Teal;
                        DrawingArea.Canvas.Children.Add(whole);
                    }
                }
            }
            foreach (StarSystem system in Editor.GalaxyMap)
            {
                StarSystemView starSystemView = new StarSystemView(system);
                Ellipse starSystem = starSystemView.GetShape();
                double systemRadius = GetStarSystemRadius() * ObjectSizeRatio;
                starSystem.Tag = starSystemView;
                Canvas.SetLeft(starSystem, system.MapPosition.X - systemRadius);
                Canvas.SetTop(starSystem, system.MapPosition.Y - systemRadius);
                
                DrawingArea.Canvas.Children.Add(starSystem);
                // add name of the system
                TextBlock systemName = new TextBlock();
                systemName.Text = system.Name;
                systemName.Background = Brushes.Transparent;
                systemName.Foreground = Brushes.WhiteSmoke;
                // priblizna sirka nazvu, pouzito pro vycentrovani textu nad system
                // jedno pismeno je velke cca 5.5 pixelu
                double textWidth = system.Name.Length*5.5;
                Canvas.SetLeft(systemName, system.MapPosition.X - textWidth/2);
                Canvas.SetTop(systemName, system.MapPosition.Y - systemRadius*3);
                DrawingArea.Canvas.Children.Add(systemName);
            }
            this.selected = false;
        }

        /// <summary>
        /// finds angle between X axis going through center point, and mouse position
        /// </summary>
        /// <param name="center">central point - X axis</param>
        /// <param name="mouse">mouse position</param>
        /// <param name="mode">mode - init angle, or rotation angle</param>
        /// <returns>angle in radians</returns>
        public static double findAngle(Point2d center, Point2d mouse, int mode)
        {
            double arc;
            if (mode == 0)
                arc = Math.Atan2(-mouse.Y + center.Y, -mouse.X + center.X);
            else
                arc = Math.Atan2(mouse.Y - center.Y, -mouse.X + center.X);
            if (arc < 0) arc += Math.PI * 2;
            else if (arc > 360) arc -= Math.PI * 2;
            return arc;
        }

        /// <summary>
        /// sets new init angle for selected object
        /// </summary>
        public void editObjectInitAngle(double mouseX, double mouseY)
        {
            //gets trajectoryView of selected object
            TrajectoryView trajectoryView = (this.SelectedObject as CelestialObjectView).GetTrajectoryView();
            Point2d center = new Point2d(DrawingAreaSize*ObjectSizeRatio, DrawingAreaSize*ObjectSizeRatio);
            Point2d mouse = new Point2d(mouseX, mouseY);
            double angle = findAngle(center, mouse, 0);
            //Debug.WriteLine(MathUtil.RadianToDegree(angle));
            
            if (trajectoryView.Trajectory is CircularOrbit)
            {
                Editor.CircleOrbitEditor.LoadObject(trajectoryView.Trajectory);
                // circular orbit is -PI behind
                angle -= Math.PI;
                Editor.CircleOrbitEditor.SetInitialAngleRad(angle);
            }
            else if (trajectoryView.Trajectory is EllipticOrbit)
            {
                Editor.EllipseOrbitEditor.LoadObject(trajectoryView.Trajectory);
                // elliptic orbit may have rotation of its own, add that angle to our angle
                angle += (Editor.EllipseOrbitEditor.LoadedObject as EllipticOrbit).RotationAngleInRad;
                Editor.EllipseOrbitEditor.SetInitialAngleRad(angle);
            }
        }
        
        /// <summary>
        /// sets new width to selected object
        /// </summary>
        /// <param name="mouseX">mouse position X</param>
        /// <param name="mouseY">mouse position Y</param>
        /// <param name="modifier">shift or ctrl modifier</param>
        /// <param name="finalize">true if called from mouse up, false otherwise</param>
        private void editObjectWidth(double mouseX, double mouseY, int modifier, bool finalize)
        {
            //gets trajectoryView of selected object
            TrajectoryView trajectoryView = (this.SelectedObject as CelestialObjectView).GetTrajectoryView();
            Point2d center = new Point2d(DrawingAreaSize, DrawingAreaSize);
            Point2d mouse = new Point2d(mouseX, mouseY);
            // translate mouse position to be in line center - selected point - mouse
            Point2d mouseActualPosition = translatePoint(center, points[1].Position, mouse);
            int width = (int)distance(center, mouseActualPosition);
            if (trajectoryView.Trajectory is CircularOrbit)
            {
                Editor.CircleOrbitEditor.LoadObject(trajectoryView.Trajectory);
                if (modifier == 0)
                {
                    if (!finalize)
                        Editor.CircleOrbitEditor.PreviewSetWidth(width);
                    else
                        Editor.CircleOrbitEditor.SetWidth(width);
                }
                // shift modifier - scale
                else if (modifier == 1)
                {
                    Editor.CircleOrbitEditor.SetRadius(width);
                }
                // sets new trajectory to trajectoryView in case of changing types from circular to elliptic
                trajectoryView.SetTrajectory((Trajectory)Editor.CircleOrbitEditor.LoadedObject);
            }
            else if (trajectoryView.Trajectory is EllipticOrbit)
            {
                Editor.EllipseOrbitEditor.LoadObject(trajectoryView.Trajectory);
                EllipticOrbit orbit = (EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject;
                //points[3] holds center of ellipse point
                width = (int)distance(points[3].Point, mouseActualPosition);
                if (modifier == 0)
                {
                    if (!finalize)
                        Editor.EllipseOrbitEditor.PreviewSetWidth(width);
                    else
                        Editor.EllipseOrbitEditor.SetWidth(width);
                }
                // shift modifier - scale
                else if (modifier == 1)
                {
                    double ratio = width / (double)orbit.A;
                    Editor.EllipseOrbitEditor.Resize(ratio);
                }
                else if (modifier == 2)
                {
                    double angle = findAngle(mouse, center, 1);
                    Editor.EllipseOrbitEditor.SetRotationAngleInRad(angle);
                }
            }
        }

        /// <summary>
        /// sets new height to selected object
        /// </summary>
        /// <param name="mouseX">mouse position X</param>
        /// <param name="mouseY">mouse position Y</param>
        /// <param name="modifier">shift or ctrl modifier</param>
        /// <param name="finalize">true if called from mouse up, false otherwise</param>
        public void editObjectHeight(double mouseX, double mouseY, int modifier, bool finalize)
        {
            //gets trajectoryView of selected object
            TrajectoryView trajectoryView = (this.SelectedObject as CelestialObjectView).GetTrajectoryView();
            Point2d center = new Point2d(DrawingAreaSize, DrawingAreaSize);
            Point2d mouse = new Point2d(mouseX, mouseY);
            // translate mouse position to be in line center - selected point - mouse
            Point2d mouseActualPosition = translatePoint(center, points[2].Position, mouse);
            int height = (int)distance(center, mouseActualPosition);
            if (trajectoryView.Trajectory is CircularOrbit)
            {
                Editor.CircleOrbitEditor.LoadObject(trajectoryView.Trajectory);
                // no modifier
                if (modifier == 0)
                    Editor.CircleOrbitEditor.SetHeight(height);
                // shift - scaling
                else if (modifier == 1)
                    Editor.CircleOrbitEditor.SetRadius(height);
            }
            else if (trajectoryView.Trajectory is EllipticOrbit)
            {
                Editor.EllipseOrbitEditor.LoadObject(trajectoryView.Trajectory);
                EllipticOrbit orbit = (EllipticOrbit)Editor.EllipseOrbitEditor.LoadedObject;
                mouseActualPosition = translatePoint(points[3].Point, points[2].Point, mouse);
                height = (int)distance(points[3].Point, mouseActualPosition);
                if (modifier == 0)
                {
                    if (!finalize)
                        Editor.EllipseOrbitEditor.PreviewSetHeight(height);
                    else
                        Editor.EllipseOrbitEditor.SetHeight(height);
                }
                // shift modifier - scale
                else if (modifier == 1)
                {
                    double ratio = height / (double)orbit.B;
                    Editor.EllipseOrbitEditor.Resize(ratio);
                }
                else if (modifier == 2)
                {
                    double angle = findAngle(mouse, center, 1);
                    // translate angle by 90 degrees
                    angle -= Math.PI / 4;
                    Editor.EllipseOrbitEditor.SetRotationAngleInRad(angle);
                }
            }
        }

        /// <summary>
        /// edits objects when mouse is draging points on canvas
        /// </summary>
        /// <param name="X">mouse X position</param>
        /// <param name="Y">mouse Y position</param>
        /// <param name="index">point selected</param>
        /// <param name="modifier">shift or ctrl modifier</param>
        /// <param name="finalize">true if called from mouse up, false otherwise</param>
        public void editShape(double X, double Y, int index, int modifier, bool finalize)
        {
            if (this.SelectedObject is CelestialObjectView)
            {
                CelestialObjectView selectedEntity = (CelestialObjectView)SelectedObject;
                // planet position editing
                if (index == 0)
                {
                    // sends mouse position altered by selected point radius
                    editObjectInitAngle(X - points[index].Size/2, Y - points[index].Size/2);
                }
                // ellipse width
                else if (index == 1)
                {
                    // if selected object is wormhole, dont allow it to sway from circular orbit
                    if (selectedEntity is EndpointView)
                        if (modifier == 0)
                            modifier = 1;
                    editObjectWidth(X, Y, modifier, finalize);
                }
                //ellipse height
                else if (index == 2)
                {
                    // if selected object is wormhole, dont allow it to sway from circular orbit
                    if (selectedEntity is EndpointView)
                        if (modifier == 0)
                            modifier = 1;
                    editObjectHeight(X , Y, modifier, finalize);
                }
                // redraws edited entity on canvas
                redrawElement(selectedEntity);
                // when done with editing one segment, redraw
                if (finalize)
                {
                    deselect();
                    StarSystemDrawer();
                }
            }
            else if (this.SelectedObject is StarSystemView)
            {
                MoveStarSystem(X, Y);
            }
        }
        /// <summary>
        /// redraws edited element on canvas
        /// </summary>
        /// <param name="element">element to redraw</param>
        private void redrawElement(View element)
        {
            CelestialObjectView selectedEntity = (CelestialObjectView)SelectedObject;
            TrajectoryView trajectoryView = selectedEntity.GetTrajectoryView();
            removeElement(trajectoryView);
            Ellipse ellipse = trajectoryView.GetShape();
            ellipse.Tag = trajectoryView;
            Canvas.SetLeft(ellipse, trajectoryView.Position.X);
            Canvas.SetTop(ellipse, trajectoryView.Position.Y);
            DrawingArea.Canvas.Children.Add(ellipse);
            selectedEntity.SetTrajectoryView(trajectoryView);
            //removes and adds objects shape on canvas
            removeElement(selectedEntity);
            ellipse = selectedEntity.GetShape();
            ellipse.Tag = selectedEntity;
            Canvas.SetLeft(ellipse, selectedEntity.Position.X);
            Canvas.SetTop(ellipse, selectedEntity.Position.Y);
            DrawingArea.Canvas.Children.Add(ellipse);
            
            // adding new orbit to Planet behind PlanetView
            if (selectedEntity is PlanetView)
            {
                Planet planet = (Planet)selectedEntity.GetLoadedObject();
                planet.Trajectory = trajectoryView.Trajectory;
            }
            // odebere označené body
            foreach (SelectedPointView point in points)
            {
                removeElement(point);
            }
            points.Clear();
            DrawPoints(selectedEntity);
        }

        /// <summary>
        /// Rotates one point around another
        /// </summary>
        /// <param name="pointToRotate">The point to rotate.</param>
        /// <param name="centerPoint">The centre point of rotation.</param>
        /// <param name="angleInRadians">The rotation angle in radians.</param>
        /// <returns>Rotated point</returns>
        static Point2d RotatePoint(Point2d pointToRotate, Point2d centerPoint, double angleInRadians)
        {
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
        /// Returns point translated on rotated axis of ellipse
        /// </summary>
        /// <param name="center">center of ellipse</param>
        /// <param name="ellipse">selected dragging point</param>
        /// <param name="mouse">mouse position</param>
        /// <returns>bod na primce</returns>
        private Point2d translatePoint(Point2d center, Point2d ellipse, Point2d mouse)
        {
            Point2d v1 = new Point2d(ellipse.X - center.X, ellipse.Y - center.Y);
            // move mouse position by half of planet radius, so it is in center, not topleft corner
            Point2d v2 = new Point2d(mouse.X + DEFAULT_PLANET_RADIUS * ObjectSizeRatio - center.X,
                                     mouse.Y+DEFAULT_PLANET_RADIUS * ObjectSizeRatio - center.Y);
            double size = Math.Sqrt(v1.X * v1.X + v1.Y * v1.Y);
            v1.X = v1.X / size;
            v1.Y = v1.Y / size;
            double size2 = Math.Sqrt(v2.X * v2.X + v2.Y * v2.Y);
            // size must be positive
            if (size2 <= 0)
            {
                return v1;
            }
            v2.X = v2.X / size2;
            v2.Y = v2.Y / size2;
            double angle = v1.X * v2.X + v1.Y * v2.Y;
            //size of new vector v3
            double size3 = size2 * angle;
            Point2d v3 = new Point2d(v1.X * size3, v1.Y * size3);
            // new translated point
            v3.X = v3.X + center.X;
            v3.Y = v3.Y + center.Y;
            return v3;
        }

        /// <summary>
        /// Gets element on canvas by its tag
        /// </summary>
        /// <param name="view">View, element.Tag</param>
        /// <returns></returns>
        private Ellipse getElement(View view)
        {
            UIElementCollection list = DrawingArea.Canvas.Children;
            foreach (UIElement element in list)
            {
                if (element is GroupBox) continue;
                if (((Ellipse)element).Tag == view)
                {
                    Ellipse ellipse = element as Ellipse;
                    if (ellipse == null) continue;
                    //Editor.Log("found");
                    DrawingArea.Canvas.Children.Remove(element);
                    return ellipse;
                }
            }
            return null;
        }

        /// <summary>
        /// removes element from canvas
        /// </summary>
        /// <param name="view">View, element.Tag</param>
        private void removeElement(View view)
        {
            UIElementCollection list = DrawingArea.Canvas.Children;
            foreach (UIElement element in list)
            {
                if (element is GroupBox) continue;
                if (((Ellipse)element).Tag == view)
                {
                    Ellipse ellipse = element as Ellipse;
                    if (ellipse == null) continue;
                    DrawingArea.Canvas.Children.Remove(element);
                    break;
                }
            }
        }

        /// <summary>
        /// Draws and elipse, symbolising point which will be used to edit trajectories
        /// </summary>
        /// <param name="pointView">SelectedPoint view</param>
        private void drawPoint(SelectedPointView pointView)
        {
            Ellipse ellipse = pointView.GetShape();
            ellipse.Tag = pointView;
            Canvas.SetLeft(ellipse, pointView.Position.X );
            Canvas.SetTop(ellipse, pointView.Position.Y );
            DrawingArea.Canvas.Children.Add(ellipse);
        }

        /// <summary>
        /// Draws point on selected Star System in galaxy mode
        /// </summary>
        /// <param name="objectView">View of an object</param>
        public void DrawStarSystemPoint(View objectView)
        {
            this.SelectedObject = objectView;
            if (objectView is StarSystemView)
            {
                points.Clear();
                StarSystemView starSystemView = (StarSystemView)objectView;
                // PlanetPosition
                double x = starSystemView.Position.X;
                double y = starSystemView.Position.Y;
                Point2d point = new Point2d(x, y);
                SelectedPointView pointView = new SelectedPointView(starSystemView, point);
                // draw planet draging point
                drawPoint(pointView);
                //add points to list of points, needed for hit testing
                points.Add(pointView);

                this.selected = true;
            }
        }
        
        /// <summary>
        /// Moves StarSystem on GalaxyMap canvas
        /// </summary>
        /// <param name="X">Position X</param>
        /// <param name="Y">Position Y</param>
        private void MoveStarSystem(double X, double Y)
        {
            StarSystemView view = this.SelectedObject as StarSystemView;
            Point2d position = new Point2d((int)X, (int)Y);
            view.StarSystem.MapPosition = position;
            DrawGalaxyMap();
        }

        /// <summary>
        /// Draws points for selected entity on canvas, these points are then used to edit objects
        /// </summary>
        /// <param name="entityView">selected entity</param>
        public void DrawPoints(View entityView)
        {
            this.SelectedObject = entityView;
            if (entityView is PlanetView)
            {
                this.loadedObjectData = null;
                planetSelectionChanged();
            }
            else if (entityView is EndpointView)
            {
                this.loadedWormholeData = null;
                wormholeSelectionChanged();
            }
            // if selected object isnt planet or endpoint, return;
            else
                return;
            CelestialObjectView selectedEntity = (CelestialObjectView)entityView;
            Ellipse trajectory = selectedEntity.GetTrajectoryView().GetShape();
            // object position
            double x = selectedEntity.Position.X + DEFAULT_PLANET_RADIUS * this.ObjectSizeRatio;
            double y = selectedEntity.Position.Y + DEFAULT_PLANET_RADIUS * this.ObjectSizeRatio;
            Point2d point = new Point2d(x, y);
            SelectedPointView pointView = new SelectedPointView(selectedEntity, point);
            // draw planet draging point
            drawPoint(pointView);
            //add points to list of points, needed for hit testing
            points.Add(pointView);

            foreach (SelectedPointView p in pointView.getTrajectoryPoint(selectedEntity))
            {
                // save points on trajectory and draw them
                points.Add(p);
                // makes it so canvas doesnt draw central point
                if(points.Count <= 3)
                    drawPoint(p);
            }
            this.selected = true;
        }

        /// <summary>
        /// Deletes selected object
        /// </summary>
        public void DeleteObject()
        {
            if (this.SelectedObject == null)
            {
                if (this.SelectedStarSystem != null)
                {
                    MessageBoxResult dialogResult = MessageBox.Show(
                            "Opravdu chceš odstranit vybraný Star System?","Odstranit Star System", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        SortedList<string, StarSystem> starSystems = Editor.GalaxyMap.GetEditableStarSystems();
                        Editor.GalaxyMap.Unlock();
                        // odstranit StarSystem
                        starSystems.Remove(this.SelectedStarSystem.Name);
                        Editor.GalaxyMap.Lock();
                        deselect();
                        this.SelectedStarSystem = null;
                        starSystemList.Items.Clear();
                        starSystemObjectTree.Items.Clear();
                        //remove connections
                        this.connectionList = null;
                        connectionsChanged();
                        //refresh selectoru
                        starSystemListChanged();
                        StarSystemDrawer();
                        return;
                    }
                    else if (dialogResult == MessageBoxResult.No)
                    {
                        //do nothing
                        return;
                    }
                }
            }
            object selectedObject = this.SelectedObject.GetLoadedObject();
            if(selectedObject is Planet)
            {
                string PlanetName = (selectedObject as Planet).Name;
                if (this.SelectedStarSystem.Planets.Remove(PlanetName))
                {
                    MessageBox.Show("planeta " + PlanetName + " byla odstranena.");
                    //redraw
                    deselect();
                    StarSystemDrawer();
                    TreeDataLoader();
                }
            }
            else if (selectedObject is WormholeEndpoint)
            {
                int id = (selectedObject as WormholeEndpoint).Id;
                if ((selectedObject as WormholeEndpoint).IsConnected)
                    {
                        //zrusime destinaci cilove wormhole
                        (selectedObject as WormholeEndpoint).Destination.Destination = null;
                    }
                if (this.SelectedStarSystem.WormholeEndpoints.Remove(id))
                {
                    MessageBox.Show("wormhole" + id + " byla odstranena.");
                    //redraw
                    deselect();
                    StarSystemDrawer();
                    TreeDataLoader();
                    ConnectionsLoader();
                }
            }
        } 

        /// <summary>
        /// odvybere objekt
        /// </summary>
        public void deselect()
        {
            this.selected = false;
            this.SelectedObject = null;
            points.Clear();
            TextBlock objectData = new TextBlock();
            objectData.Text = "No object selected.";
            this.loadedObjectData = objectData;
            planetSelectionChanged();
            TextBlock wormholeData = new TextBlock();
            wormholeData.Text = "No object selected.";
            this.loadedWormholeData = wormholeData;
            wormholeSelectionChanged();

        }
        /// <summary>
        /// vzdalenost dvou bodu
        /// </summary>
        /// <param name="point1">bod1</param>
        /// <param name="point2">bod2</param>
        /// <returns>vzdalenost</returns>
        private double distance(Point2d point1, Point2d point2)
        {
            double dX = point2.X - point1.X;
            double dY = point2.Y - point1.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }
        /// <summary>
        /// selects planet based on selected object
        /// </summary>
        private void selectPlanet()
        {
            UIElementCollection list = DrawingArea.Canvas.Children;
            foreach (UIElement element in list)
            {
                if (element is GroupBox) continue;
                if (((Ellipse)element).Name.Equals(this.SelectedObject.GetName()))
                {
                    Ellipse ellipse = element as Ellipse;
                    if (ellipse == null) continue;
                    //select
                    points.Clear();
                    DrawPoints(ellipse.Tag as PlanetView);
                    //Editor.Log("found");
                    break;
                }
            }
        }

        private void selectWormhole()
        {
            UIElementCollection list = DrawingArea.Canvas.Children;
            foreach (UIElement element in list)
            {
                if (element is GroupBox) continue;
                if (((Ellipse)element).Name.Equals(this.SelectedObject.GetName()))
                {
                    Ellipse ellipse = element as Ellipse;
                    if (ellipse == null) continue;
                    //select
                    points.Clear();
                    DrawPoints(ellipse.Tag as PlanetView);
                    //Editor.Log("found");
                    break;
                }
            }
        }
        #region ObjectCreators
        private void CreateConnectionList()
        {
            Grid grid = new Grid();
            grid.MinWidth = 200;
            grid.Height = 50;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            //grid.ShowGridLines = true;

            // Define the Columns
            ColumnDefinition ID = new ColumnDefinition();
            ID.ToolTip = "ID";
            ColumnDefinition Destination = new ColumnDefinition();
            Destination.ToolTip = "Destination";
            grid.ColumnDefinitions.Add(ID);
            grid.ColumnDefinitions.Add(Destination);

            this.connectionList = grid;
        }
        private void CreateDrawingArea()
        {
            Editor.Log("creating area");
            this.drawingArea = new DrawingArea();
        }
        private void CreateGalaxyInfo()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Filename: " + Editor.GalaxyName + ".xml";
            textBlock.Text += "\n" + "StarSystem count: " + Editor.GalaxyMap.GetStarSystems().Count;
            this.galaxyInfo = textBlock;
            
        }
        private void CreateLoadedObjectData()
        {
            ObjectData objectData= new ObjectData();
            this.loadedObjectData = objectData.GetLoadedObjectData();
            planetSelectionChanged();
        }

        private void CreateLoadedWormholeData()
        {
            WormholeData wormholeData = new WormholeData();
            this.loadedWormholeData = wormholeData.GetLoadedWormholeData();
            wormholeSelectionChanged();
        }
        

        private void CreateStarSystemList()
        {
            ListView view = new ListView();
            view.MinHeight = 100;
            view.MaxHeight = 200;
            view.BorderThickness = new Thickness(0);
            view.FontSize = 14;
            this.starSystemList = view;
        }
        private void CreateStarSystemObjectTree()
        {
            TreeView tree = new TreeView();
            tree.MinHeight = 100;
            tree.MaxHeight = 250;
            tree.BorderThickness = new Thickness(0);
            tree.FontSize = 14;
            this.starSystemObjectTree = tree;
        }
        #endregion       
        
        #region ObjectLoaders
        /// <summary>
        /// MEtoda pro nacitani spojeni
        /// </summary>
        public void ConnectionsLoader()
        {
            FrameworkElement connections = this.GetConnectionList();
            StarSystem starSystem = SelectedStarSystem;
            //30 pixelu na kazdou wormhole
            (connections as Grid).Height = starSystem.WormholeEndpoints.Count * 30;
            // vycisteni gridu
            (connections as Grid).RowDefinitions.Clear();

            (connections as Grid).Children.Clear();
            foreach (WormholeEndpoint endpoint in starSystem.WormholeEndpoints)
            {
                RowDefinition Wormhole = new RowDefinition();
                (connections as Grid).RowDefinitions.Add(Wormhole);

                Label id = new Label();
                id.Width = 30;
                id.Content = endpoint.Id;
                id.HorizontalAlignment = HorizontalAlignment.Center;
                id.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(id, 0);
                // endpoint.Id poslouzi i jako index
                Grid.SetRow(id, endpoint.Id);
            
                ComboBox destination = CreateConnectionsComboBox();
                destination.Tag = endpoint;
                Grid.SetColumn(destination, 1);
                Grid.SetRow(destination, endpoint.Id);
                // index prvku v comboboxu
                int index = 0;
                // not connected index
                destination.SelectedIndex = 0;
                if (endpoint.IsConnected)
                {
                    foreach (ComboBoxItem item in destination.Items)
                    {
                        // starsystem koncove wormhole - kam vede
                        index++;
                        if (item.Tag == endpoint.Destination.StarSystem)
                        {
                            //nalezeno, priradim index
                            destination.SelectedIndex = destination.Items.IndexOf(item);//index-1;
                            break;
                        }
                    }
                }
                (connections as Grid).Children.Add(id);
                (connections as Grid).Children.Add(destination);
                // prihlaseni k event handleru
                destination.SelectionChanged += connection_changed;
            }
        }
        /// <summary>
        /// Vytvori combobox se vsemy star systemy a polozkou nepripojeno
        /// Slouzi pro zalozku Connections v loadedObjectInfo napravo v editoru
        /// </summary>
        /// <returns>ComboBox</returns>
        public ComboBox CreateConnectionsComboBox()
        {
            ComboBox connections = new ComboBox();
            connections.MinWidth = 120;
            connections.HorizontalAlignment = HorizontalAlignment.Left;
            ComboBoxItem notConnected = new ComboBoxItem();
            notConnected.Content = "Not connected";
            notConnected.Tag = null;
            connections.Items.Add(notConnected);
            foreach (StarSystem system in Editor.GalaxyMap)
            {
                if (system == this.SelectedStarSystem)
                    continue;
                ComboBoxItem item = new ComboBoxItem();
                item.Content = system.Name;
                item.Tag = system;
                connections.Items.Add(item);
            }
            return connections;
        }
        /// <summary>
        /// Metoda pro nacteni seznamu starsystemu
        /// </summary>
        public void StarSystemListLoader()
        {
            GalaxyMap map = Editor.GalaxyMap;
            ListView view = this.GetStarSystemList();
            foreach (StarSystem starSystem in map)
            {
                ListViewItem starSystemItem = new ListViewItem();
                starSystemItem.Content = starSystem.Name;
                starSystemItem.Tag = starSystem;
                starSystemItem.GotFocus += new RoutedEventHandler(SetStarSystemListFocus);
                view.Items.Add(starSystemItem);
            }
        }
        /// <summary>
        /// Metoda pro nacteni stromu s objekty starsystemu
        /// </summary>
        public void TreeDataLoader()
        {
       //     this.starSystemObjectTree = null;
           // this.CreateStarSystemObjectTree();
            TreeView tree = this.GetStarSystemObjectTree();
            //StarSystem starSystem = Editor.GalaxyMap[SelectedStarSystem];
            StarSystem starSystem = SelectedStarSystem;
            tree.Items.Clear();
            TreeViewItem baseSun = new TreeViewItem();
            baseSun.Header = "Star";
            baseSun.IsExpanded = true;
            TreeViewItem sun = new TreeViewItem();
            sun.Header = starSystem.Star.Name;
            baseSun.Items.Add(sun);

            TreeViewItem basePlanet = new TreeViewItem();
            basePlanet.Header = "Planets";
            basePlanet.IsExpanded = true;
            //Editor.Log("planet count" + starSystem.Planets.Count);
            foreach (Planet planet in starSystem.Planets)
            {
                TreeViewItem planetNode = new TreeViewItem();
                planetNode.Tag = new PlanetView(planet);
                planetNode.Header = planet.AlternativeName.ToString().Replace(" ", "");
                planetNode.GotFocus += new RoutedEventHandler(SetStarSystemTreeFocus);
                basePlanet.Items.Add(planetNode);
            }

            TreeViewItem baseWormhole = new TreeViewItem();
            baseWormhole.Header = "Wormholes";
            baseWormhole.IsExpanded = true;
            foreach (WormholeEndpoint wormhole in starSystem.WormholeEndpoints)
            {
                TreeViewItem wormholeNode = new TreeViewItem();
                wormholeNode.Tag = new EndpointView(wormhole);
                wormholeNode.Header = "Wormhole" + wormhole.Id;
                wormholeNode.GotFocus += new RoutedEventHandler(SetStarSystemTreeFocus);
                baseWormhole.Items.Add(wormholeNode);
            }
            tree.Items.Add(basePlanet);
            tree.Items.Add(baseSun);
            tree.Items.Add(baseWormhole);
        }
        #endregion 

        #region Events
        /// <summary>
        /// Udalost pri oznaceni nejakeho starsystemu
        /// </summary>
        /// <param name="sender">odesilatel</param>
        /// <param name="e">argumenty</param>
        public void SetStarSystemListFocus(object sender, RoutedEventArgs e)
        {
            deselect();
            this.SelectedStarSystem = ((e.Source as ListViewItem).Tag as StarSystem);
            this.TreeDataLoader();
            this.StarSystemDrawer();
            this.ConnectionsLoader();
            Editor.FlushEditors();
            TextBlock objectData= new TextBlock();
            objectData.Text = "No object selected.";
            this.loadedObjectData = objectData;
            planetSelectionChanged();
            
            //CreateStarSystemList();
            Editor.Log("focus");

            //Editor.dataPresenter.SetStarSystemListFocus(e.OriginalSource as ListViewItem);
        }
        /// <summary>
        /// Udalost pri oznaceni objektu ve stromu
        /// </summary>
        /// <param name="sender">odesilatel</param>
        /// <param name="e">parametry</param>
        public void SetStarSystemTreeFocus(object sender, RoutedEventArgs e)
        {
            if(Editor.ButtonName.Equals("Star System"))
                return;
            Editor.Log("got focus");
            TreeViewItem treeView = (TreeViewItem)e.Source;
            View view = (View)treeView.Tag;
            this.SelectedObject = view;
            if (view is PlanetView)
            {
                PlanetView planetView = (PlanetView)view;
                this.SelectedObject = planetView;
                planetSelectionChanged();
            }
            else if (view is EndpointView)
            {
                EndpointView endpointView = (EndpointView)view;
                this.SelectedObject = endpointView;
                wormholeSelectionChanged();
            }
            if (treeView.Items.Count == 0)
            {
                //redraw
                Editor.dataPresenter.StarSystemDrawer();

                UIElementCollection list = DrawingArea.Canvas.Children;
                foreach (UIElement element in list)
                {
                    if (element is GroupBox) continue;
                    if (((Ellipse)element).Name.Equals(this.SelectedObject.GetName()))
                    {
                        Ellipse ellipse = element as Ellipse;
                        if (ellipse == null) continue;
                        //select
                        points.Clear();
                        DrawPoints(ellipse.Tag as CelestialObjectView);
                        //Editor.Log("found");
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Udalost pri zmene vyberu starsystemu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StarSystemSelectorChange(object sender, SelectionChangedEventArgs e) 
        {
            if (e.AddedItems.Count == 0) return;
            String selectedName = e.AddedItems[0].ToString();
            foreach (ListViewItem item in this.GetStarSystemList().Items) 
            {
                if (item.Content.Equals(selectedName))
                {
                    (item as ListViewItem).IsSelected = true;
                    RoutedEventArgs args = new RoutedEventArgs();
                    e.Source = (item as ListViewItem);
                    this.SetStarSystemListFocus(sender, e);
                }
                else
                {
                    (item as ListViewItem).IsSelected = false;
                } 
            }
            //Editor.Log();

        }
        /// <summary>
        /// event ktery se zavola pri zneme destinace wormhole
        /// </summary>
        /// <param name="sender">combo box s vyberem star systemu</param>
        /// <param name="e">parametry eventu</param>
        private void connection_changed(object sender, RoutedEventArgs e)
        {
            StarSystem starSystem = this.SelectedStarSystem;
            foreach (WormholeEndpoint endpoint in starSystem.WormholeEndpoints)
            {
             ////   ((sender as ComboBox).SelectedItem as ComboBoxItem);
                // nalezli jsme nami upravovanou
                if (endpoint == ((sender as ComboBox).Tag))
                {
                    // pokud uz nekam vedla, je treba nastavit jeji parove wormhole, ze od ted nikam nevede
                    if (endpoint.IsConnected)
                    {
                        //zrusime destinaci cilove wormhole, ted uz nevede nikam ona
                        endpoint.Destination.Destination = null;
                    }
                    //pokud jsme nastavili at nevede nikam - Not connected., nema nastaveny tag, musime z cyklu ven nez ho pouzijeme
                    if (((sender as ComboBox).SelectedItem as ComboBoxItem).Tag == null)
                    {
                        endpoint.Destination = null;
                        break;
                    }
                    //star system destinace
                    StarSystem system = (StarSystem)((sender as ComboBox).SelectedItem as ComboBoxItem).Tag;
                    foreach (StarSystem s in Editor.GalaxyMap.GetStarSystems())
                    {
                        if (s == system)
                        {
                            bool connected = false;
                            // projdeme wormholy v systemu, jestli je nejaka bez cile, priradime ji tento starsystem
                            foreach (WormholeEndpoint we in s.WormholeEndpoints)
                            {
                                if (!we.IsConnected)
                                {
                                    endpoint.Destination = null;
                                    // nastavime vzajemne destinace
                                    endpoint.ConnectTo(we);
                                    // flag ze jsme uspesne propojili s uz existujici wormhole
                                    connected = true;
                                    break;
                                }
                            }
                            if (connected)
                                break;
                            // trajektorie nejvnejsi wormhole v cilovem systemu
                            CircularOrbit trajOfLastWormhole = 
                                (s.WormholeEndpoints[s.WormholeEndpoints.Count - 1].Trajectory as CircularOrbit);
                            // vytvori novou trajektorii s radiusem o 20 vetsi nez nejvetsi wormhole v systemu a o 6000 vetsi periodou
                            // 6000 perioda je odvozena od solar systemu - 10 radius = 3000 perioda
                            CircularOrbit trajectory = new CircularOrbit(
                                trajOfLastWormhole.Radius + 20,
                                (int)trajOfLastWormhole.PeriodInSec + 6000,
                                Direction.CLOCKWISE,
                                trajOfLastWormhole.InitialAngleRad);
                            WormholeEndpoint newWormhole = new WormholeEndpoint(s.WormholeEndpoints.Count, s, trajectory);
                            // prida nove wormhole destinaci nasi menenou wormhole
                            newWormhole.Destination = endpoint;
                            // prida do seznamu wormhole starsystemu
                            s.WormholeEndpoints.Add(newWormhole);
                            endpoint.Destination = newWormhole;
                            break;
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Tests if user clicked near selected point on canvas
        /// </summary>
        /// <param name="X">X mouse position</param>
        /// <param name="Y">Y mosue position</param>
        /// <returns></returns>
        public int pointHit(double X, double Y)
        {
            // translated by 5 - top margin of canvas
            Point2d pointClicked = new Point2d(X, Y-5);
            foreach (SelectedPointView pointView in points)
            {
                if (distance(pointClicked, pointView.Point) <= 10)
                {
                    return points.IndexOf(pointView);
                }
                
            }
            deselect();
            //repaint
            if (Editor.ButtonName.Equals("Star System"))
            {
                //redraw galaxy map
                DrawGalaxyMap();
            }
            else
            //redraw star system
            StarSystemDrawer();
            return -1;
        }
        

        #endregion

        #region commentedOutCode
        //public void LoadGalaxy(String galaxyName) 
        //{

        //}
        //public void ShowStarSystemInfo(Canvas canvas)
        //{
        //    GroupBox starSystemInfoBox = new GroupBox();
        //    starSystemInfoBox.Header = "StarSystem info";
        //    starSystemInfoBox.BorderBrush = Brushes.White;
        //    starSystemInfoBox.BorderThickness = new Thickness(1);
        //    starSystemInfoBox.Foreground = Brushes.White;

        //    TextBlock infoText = new TextBlock();
        //    infoText.Foreground = Brushes.White;
        //    infoText.Name = "loadedStarSystemData";
        //    infoText.Text = SelectedStarSystem.Name.ToString();
        //    infoText.Text += "\n" + "Planet count: " + SelectedStarSystem.Planets.Count;
        //    infoText.Text += "\n" + "Wormholes count: " + SelectedStarSystem.WormholeEndpoints.Count;

        //    starSystemInfoBox.Content = infoText;

        //    Canvas.SetLeft(starSystemInfoBox, 10);
        //    Canvas.SetTop(starSystemInfoBox, 10);
        //    canvas.Children.Add(starSystemInfoBox);
        //}
        //private void CreatePlanetShape(PlanetView planetView, Canvas canvas) 
        //{
        //    Trajectory planetTrajectory = planetView.Planet.Trajectory;
        //    double planetRadius = DEFAULT_PLANET_RADIUS * this.ObjectSizeRatio;
        //    double initialTime = 0;
        //    Point2d calculatedPoint = planetTrajectory.CalculatePosition(initialTime);
        //    double planetXpos = calculatedPoint.X * ObjectSizeRatio + DrawingAreaSize - planetRadius;
        //    double planetYpos = calculatedPoint.Y * ObjectSizeRatio + DrawingAreaSize - planetRadius;


        //    Ellipse planetShape = planetView.GetShape();
        //    Canvas.SetLeft(planetShape, planetXpos);
        //    Canvas.SetTop(planetShape, planetYpos);
        //    canvas.Children.Add(planetShape);
        //}
        #endregion

    }
}
