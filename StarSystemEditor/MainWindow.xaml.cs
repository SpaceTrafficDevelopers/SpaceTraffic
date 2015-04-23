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
using System.Windows;
using System.Windows.Controls;
using SpaceTraffic.Tools.StarSystemEditor.Data;
using SpaceTraffic.Tools.StarSystemEditor.Presentation;
using System.Windows.Input;
using SpaceTraffic.Tools.StarSystemEditor.Entities;
using System.Windows.Shapes;
using SpaceTraffic.Game;
using System.Collections.Generic;
using System;

namespace SpaceTraffic.Tools.StarSystemEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        /// <summary>
        /// detector wether left mouse button is held
        /// </summary>
        public bool mouseDown { get; set; }
        private int pointEditing { get; set; }
        private int modifier { get; set; }
        /// <summary>
        /// timer, used in MouseMove. Necessary for program not to miss MouseUp event occasionally
        /// </summary>
        DateTime start = DateTime.Now;
       
        #endregion

        /// <summary>
        /// Construktor
        /// </summary>
        public MainWindow()
        {
            Editor.Preload();
            Editor.dataPresenter.planetSelectionChanged = planetSelected;
            Editor.dataPresenter.wormholeSelectionChanged = wormholeSelected;
            Editor.LoadGalaxy("GalaxyMap2", ".//Assets");
            InitializeComponent();
            this.StarSystemSelectorPanel.Children.Add(new StarSystemSelector());
        }
        /// <summary>
        /// Reaction on Quit
        /// </summary>
        private void QuitProgram(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }
        /// <summary>
        /// Reaction on Galaxy Map button
        /// </summary>
        private void buttonGalaxyMap_Click(object sender, RoutedEventArgs e)
        {
            string content = (string)((Button)e.Source).Content;
            if (content.Equals("Galaxy Map"))
            {
                // cleaning up
                Editor.dataPresenter.deselect();
                Editor.dataPresenter.selected = false;
                List<SelectedPointView> points = Editor.dataPresenter.GetPoints();
                points.Clear();
                // shows galaxy map
                Editor.ButtonName = "Star System";
                ((Button)e.Source).Content = Editor.ButtonName;
                Editor.dataPresenter.DrawGalaxyMap();
            }
            else if (content.Equals("Star System"))
            {
                // shows star system 
                Editor.ButtonName = "Galaxy Map";
                ((Button)e.Source).Content = Editor.ButtonName;
                Editor.dataPresenter.StarSystemDrawer();
            }
        }
        /// <summary>
        /// Reaction on zoom in
        /// </summary>
        private void buttonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Editor.dataPresenter.DrawingAreaSize += 50;
            if (DrawingArea.Canvas == null) return;
            DrawingArea.Canvas.Width = Editor.dataPresenter.DrawingAreaSize * 2;
            DrawingArea.Canvas.Height = Editor.dataPresenter.DrawingAreaSize * 2;
            
            ReDrawMap();
        }
        /// <summary>
        /// Reaction on zoom out
        /// </summary>
        private void buttonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.dataPresenter.DrawingAreaSize < 200) return;
            Editor.dataPresenter.DrawingAreaSize -= 50;
            if (DrawingArea.Canvas == null) return;
            DrawingArea.Canvas.Width = Editor.dataPresenter.DrawingAreaSize * 2;
            DrawingArea.Canvas.Height = Editor.dataPresenter.DrawingAreaSize * 2;
            ReDrawMap();
        }

        /// <summary>
        /// Initialization of star system list
        /// </summary>
        private void starSystemList_Loaded(object sender, RoutedEventArgs e)
        {
            Editor.Log("Loading starsystem list");
            Editor.dataPresenter.StarSystemListLoader();
        }
        /// <summary>
        /// Reaction on selecting star system in list
        /// </summary>
        private void starSystemList_GotFocus(object sender, RoutedEventArgs e)
        {
            Editor.dataPresenter.SetStarSystemListFocus(sender, e);
        }
        /// <summary>
        /// Redraw Star System on canvas
        /// </summary>
        public void ReDrawMap() 
        {
            if (Editor.IsLoaded)
            {
                Editor.dataPresenter.deselect();
                Editor.dataPresenter.StarSystemDrawer();
                this.SimulationTime.Content = "Simulation time: " + Editor.Time;
            }
        }
        /// <summary>
        /// Adds 10 second to simulation
        /// </summary>
        private void buttonTime10_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time += 10;
            ReDrawMap();
        }
        /// <summary>
        /// Adds 50 seconds to simulation
        /// </summary>
        private void buttonTime50_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time += 50;
            ReDrawMap();
        }
        /// <summary>
        /// Adds 200 seconds to simulation
        /// </summary>
        private void buttonTime200_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time += 200;
            ReDrawMap();
        }
        /// <summary>
        /// Restart simulation time
        /// </summary>
        private void buttonTimeReset_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time = 0;
            ReDrawMap();
        }
        /// <summary>
        /// Initialization wormhole connections
        /// </summary>
        private void connectionListBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as GroupBox).Content = Editor.dataPresenter.GetConnectionList();
        }
        /// <summary>
        /// Initialization of scrollviewer
        /// </summary>
        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            this.drawingAreaScrollViewer.Content = DrawingArea.Canvas;
        }
        /// <summary>
        /// Initialization object data
        /// </summary>
        private void loadedObjectData_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as GroupBox).Content = Editor.dataPresenter.GetLoadedObjectData();
        }
        /// <summary>
        /// Initialization wormhole data
        /// </summary>
        private void loadedWormholeData_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as GroupBox).Content = Editor.dataPresenter.GetLoadedWormholeData();
        }
        /// <summary>
        /// Method called from dataPresenter, updating planet data
        /// </summary>
        private void planetSelected()
        {
           this.loadedObjectData.Content = Editor.dataPresenter.GetLoadedObjectData();   
        }

        /// <summary>
        /// Method called from dataPresenter, updating wormhole data
        /// </summary>
        private void wormholeSelected()
        {
            this.loadedWormholeData.Content = Editor.dataPresenter.GetLoadedWormholeData();
        }

        /// <summary>
        /// Reaction on New button in menu
        /// </summary>
        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewStarSystem form = new NewStarSystem();
                form.Owner = this;
                form.ShowDialog();
                this.Focusable = false;
                this.ProgramStatus.Content = "Created";
            }
            catch
            {
                this.ProgramStatus.Content = "Unable to create";
            }
        }

        /// <summary>
        /// Reaction on Load button in menu
        /// </summary>
        private void MenuLoad_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                Editor.LoadGalaxyFile();
                this.ProgramStatus.Content = "Loaded";
                DrawingArea.Canvas.Children.Clear();    
                Editor.dataPresenter = new DataPresenter();
                Editor.dataPresenter.planetSelectionChanged = planetSelected;
                Editor.dataPresenter.wormholeSelectionChanged = wormholeSelected;
                Editor.dataPresenter.GetDrawingArea();
                this.StarSystemSelectorPanel.Children.Clear();
                this.StarSystemSelectorPanel.Children.Add(new StarSystemSelector());
                this.galaxyInfoGroup.Content = null;
                this.galaxyInfoGroup.Content = Editor.dataPresenter.GetGalaxyInfo();
                //this
            }
            catch
            {
                this.ProgramStatus.Content = "Unable to load";
            }
        }
        /// <summary>
        /// Reaction on Save button in menu
        /// </summary>
        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                XmlSaver.CreateXml(Editor.GalaxyMap);
                this.ProgramStatus.Content = "Saved";
            }
            catch
            {
                this.ProgramStatus.Content = "Unable to save";
            }
        }

        /// <summary>
        /// Inicializace galaxy info
        /// </summary>
        private void galaxyInfoGroup_Loaded(object sender, RoutedEventArgs e)
        {
            this.galaxyInfoGroup.Content = Editor.dataPresenter.GetGalaxyInfo();
        }

        private void window_mouseUp(object sender, MouseButtonEventArgs e)
        {

          /*  if (mouseDown)
            {
                Point mousePos = e.GetPosition(drawingAreaScrollViewer);
                Editor.dataPresenter.editShape(mousePos.X, mousePos.Y, pointEditing, modifier);
            }*/
            mouseDown = false;
        }

        private void window_mouseMove(object sender, MouseEventArgs e)
        {
            TimeSpan timeItTook = DateTime.Now - start;
            if (timeItTook.Milliseconds < 20) return;
            start = DateTime.Now;

            Point p = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
            this.ProgramStatus.Content = "" + drawingAreaScrollViewer.PointFromScreen(p).ToString();

            //      this.ProgramStatus.Content = "" + e.GetPosition(drawingAreaScrollViewer).ToString(); ;
            if (mouseDown)
            {
                Point mousePos = e.GetPosition(drawingAreaScrollViewer);
                Editor.dataPresenter.editShape(mousePos.X, mousePos.Y, pointEditing, modifier);
            }
        }
   
     
        private void canvas_mouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Editor.dataPresenter.selected)
            {
                Point pointClicked = e.GetPosition(drawingAreaScrollViewer);
                pointEditing = Editor.dataPresenter.pointHit(pointClicked.X, pointClicked.Y);
                if (pointEditing == -1)
                {
                    mouseDown = false;
                }
                else
                    mouseDown = true;
            }
            else if (!Editor.dataPresenter.selected)
            {
                Editor.selectEntity(e.OriginalSource);
            }
        }



        private void canvas_keyModifierDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Delete){
                if(Editor.dataPresenter.SelectedStarSystem != null)
                {
                    Editor.dataPresenter.DeleteObject();  
                }
            }
            else if (e.Key == Key.LeftShift)
            {
                modifier = 1;
            }
            else if (e.Key == Key.LeftCtrl)
            {
                modifier = 2;
            }
        }

        private void canvas_keyModifierUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            modifier = 0;
        }

        private void NewPlanet_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.dataPresenter.SelectedStarSystem == null)
            {
                MessageBox.Show("Select a StarSystem first.");
                return;
            }
            if (Editor.newPlanet())
            {
                MessageBox.Show("Planet created.");
                ReDrawMap();
            }
            else MessageBox.Show("Planet was not created.");

        }

        private void NewWormhole_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.dataPresenter.SelectedStarSystem == null)
            {
                MessageBox.Show("Select a StarSystem first.");
                return;
            }
            if (Editor.newWormhole())
            {
                MessageBox.Show("Wormhole created.");
                ReDrawMap();
            }
            else MessageBox.Show("Wormhole was not created."); 
        }

    }
}
