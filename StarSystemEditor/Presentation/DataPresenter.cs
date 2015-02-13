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
        #endregion

        #region Objects
        private DrawingArea drawingArea;
        private TextBlock galaxyInfo;
        private TextBlock loadedObjectData;
        private TreeView starSystemObjectTree;
        private ListView connectionList;
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
        #endregion

        #region Getters
        /// <summary>
        /// Getter pro standartni velikost planety
        /// </summary>
        /// <returns>Velikost</returns>
        public double GetPlanetRadius()
        {
            return DEFAULT_PLANET_RADIUS;
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
        public ListView GetConnectionList()
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
        public TextBlock GetLoadedObjectData()
        {
            if (this.loadedObjectData == null)
            {
                this.CreateLoadedObjectData();
            }
            return this.loadedObjectData;
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
            TreeView tree = this.GetStarSystemObjectTree();
            if (SelectedStarSystem == null) return;
            //Editor.Log("aftercheck");
            StarSystem starSystem = SelectedStarSystem;
            //Editor.Log("system" + starSystem.Name);
            Editor.StarSystemEditor.LoadObject(starSystem);
            //if (this.GetDrawingArea().Canvas == null) return;
            DrawingArea.Canvas.Children.Clear();
            StarView starView = new StarView(starSystem.Star);
            //Editor.Log("system" + starSystem.Name);
            Ellipse star = starView.GetShape();

            double sunRadius = DEFAULT_SUN_RADIUS * ObjectSizeRatio;
            Canvas.SetLeft(star, (DrawingAreaSize - sunRadius));
            Canvas.SetTop(star, (DrawingAreaSize - sunRadius));
            DrawingArea.Canvas.Children.Add(star);

            DrawingArea.ShowStarSystemInfo();
            this.GetLoadedObjectData();
            //new system preparation
            //drawingArea.AddShapeView(new StarView(starSystem.Star));
            
            
            //((TreeViewItem)tree.Items[0]).Items.Clear();
            foreach (Planet planet in starSystem.Planets)
            {
                PlanetView planetView = new PlanetView(planet);

                Ellipse trajectoryShape = planetView.GetTrajectoryShape();
                Canvas.SetLeft(trajectoryShape, planetView.TrajectoryView.Position.X);
                Canvas.SetTop(trajectoryShape, planetView.TrajectoryView.Position.Y);
                DrawingArea.Canvas.Children.Add(trajectoryShape);

                Ellipse planetShape = planetView.GetShape();
                
                //TreeViewItem planetNode = new TreeViewItem();
                //planetNode.Header = planet.AlternativeName;
                //planetNode.Tag = planetView;
                
                //((TreeViewItem)tree.Items[0]).Items.Add(planetNode);

                Canvas.SetLeft(planetShape, planetView.Position.X);
                Canvas.SetTop(planetShape, planetView.Position.Y);
                DrawingArea.Canvas.Children.Add(planetShape);

            }
            foreach (WormholeEndpoint wormhole in starSystem.WormholeEndpoints)
            {
                EndpointView endpointView = new EndpointView(wormhole);

                Ellipse trajectoryShape = endpointView.GetTrajectoryShape();
                Canvas.SetLeft(trajectoryShape, endpointView.TrajectoryView.Position.X);
                Canvas.SetTop(trajectoryShape, endpointView.TrajectoryView.Position.Y);
                DrawingArea.Canvas.Children.Add(trajectoryShape);

                Ellipse endpointShape = endpointView.GetShape();
                Canvas.SetLeft(endpointShape, endpointView.Position.X);
                Canvas.SetTop(endpointShape, endpointView.Position.Y);
                DrawingArea.Canvas.Children.Add(endpointShape);
                //TrajectoryView trajectoryView = new TrajectoryView(wormhole.Trajectory);
                //Ellipse ellipse = trajectoryView.GetShape();
                //ellipse.Stroke = Brushes.Teal;
                ////placement logic
                //Canvas.SetLeft(ellipse, trajectoryView.Position.X);
                //Canvas.SetTop(ellipse, trajectoryView.Position.Y);

                //canvas.Children.Add(ellipse);
            }
        }

        #region ObjectCreators
        private void CreateConnectionList()
        {
            ListView view = new ListView();
            view.BorderThickness = new Thickness(0);
            
            GridView gridView = new GridView();
            gridView.AllowsColumnReorder = false;
            
            GridViewColumn idColumn = new GridViewColumn();
            idColumn.DisplayMemberBinding = new Binding("Id");
            idColumn.Header = "#ID";
            idColumn.Width = 30;
            gridView.Columns.Add(idColumn);

            GridViewColumn destinationColumn = new GridViewColumn();
            destinationColumn.DisplayMemberBinding = new Binding("Destination");
            destinationColumn.Header = "Destination";
            destinationColumn.Width = 155;
            gridView.Columns.Add(destinationColumn);
            
            view.View = gridView;
            this.connectionList = view;
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
            TextBlock textBlock = new TextBlock();
            textBlock.Name = "loadedObjectData";
            if (this.SelectedObject == null)
            {
                textBlock.Text = "NO OBJECT LOADED";
            }
            else
            {
                textBlock.Text = this.SelectedObject.GetName();
            }
            
            this.loadedObjectData = textBlock;
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
            ListView connections = this.GetConnectionList();
            StarSystem starSystem = SelectedStarSystem;
            connections.Items.Clear();
            foreach (WormholeEndpoint endpoint in starSystem.WormholeEndpoints)
            {
                String destination = "Not connected";
                if (endpoint.IsConnected)
                {
                    destination = endpoint.Destination.StarSystem.Name;
                }
                connections.Items.Add(new { Id = endpoint.Id, Destination = destination });
            }
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
            this.GetStarSystemObjectTree().Items.Clear();
            TreeView tree = this.GetStarSystemObjectTree();
            //StarSystem starSystem = Editor.GalaxyMap[SelectedStarSystem];
            StarSystem starSystem = SelectedStarSystem;
            //tree.Items.Clear();
            TreeViewItem baseSun = new TreeViewItem();
            baseSun.Header = "Sun";
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
                planetNode.Header = planet.AlternativeName;
                planetNode.GotFocus += new RoutedEventHandler(SetStarSystemTreeFocus);
                basePlanet.Items.Add(planetNode);
            }

            tree.Items.Add(basePlanet);
            tree.Items.Add(baseSun);
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
            this.SelectedStarSystem = ((e.Source as ListViewItem).Tag as StarSystem);
            this.TreeDataLoader();
            this.StarSystemDrawer();
            this.ConnectionsLoader();
            Editor.FlushEditors();
            this.GetLoadedObjectData().Text = "NO OBJECT LOADED";
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
            Editor.Log("got focus");
            TreeViewItem treeView = (TreeViewItem)e.Source;
            View view = (View)treeView.Tag;
            this.SelectedObject = view;
            if (view is PlanetView)
            {
                PlanetView planetView = (PlanetView)view;
                this.SelectedObject = planetView;
                TextBlock loadedObjectData = Editor.dataPresenter.GetLoadedObjectData();
                loadedObjectData.Text = "Planet";
                loadedObjectData.Text += "\n" + "Name: " + planetView.Planet.Name;
                loadedObjectData.Text += "\n" + "StarSystem: " + planetView.Planet.StarSystem.Name;
                loadedObjectData.Text += "\n" + "TrajectoryType: " + planetView.Planet.Trajectory.GetType().Name.ToString();
                //planetView.Planet
            }
            if (treeView.Items.Count == 0)
            {
                //redraw
                Editor.dataPresenter.StarSystemDrawer();
                UIElementCollection list = DrawingArea.Canvas.Children;
                foreach (UIElement element in list)
                {
                    if (element is GroupBox) continue;
                    //Editor.Log((element as Ellipse).Name.ToString());
                    //if(((Ellipse)element).Tag == null)continue;
                    //Editor.Log((element as Ellipse).Name.ToString() + " vs " + this.SelectedObject.GetName());
                    if (((Ellipse)element).Name.Equals(this.SelectedObject.GetName()))
                    {
                        Ellipse ellipse = element as Ellipse;
                        if (ellipse == null) continue;
                        ellipse.Stroke = Brushes.Red;
                        ellipse.StrokeThickness = 2;
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
