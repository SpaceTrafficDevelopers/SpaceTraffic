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

namespace SpaceTraffic.Tools.StarSystemEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public MainWindow()
        {
            Editor.Preload();
            Editor.LoadGalaxy("GalaxyMap2", ".//Assets");
            InitializeComponent();
            
            this.StarSystemSelectorPanel.Children.Add(new StarSystemSelector());
            //Editor.dataPresenter.StarSystemDrawer(Editor.GalaxyMap["Lervos2"], this.starSystemRenderer);
            //Editor.Log(((CircularOrbit)Editor.GalaxyMap["Vitera"].WormholeEndpoints[0].Trajectory).Radius.ToString());
        }
        /// <summary>
        /// Reakce na ukonceni programu
        /// </summary>
        private void QuitProgram(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }

        /// <summary>
        /// Reakce na zoom in
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
        /// Reakce na zoom out
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
        /// Inicializace seznamu starsystemu
        /// </summary>
        private void starSystemList_Loaded(object sender, RoutedEventArgs e)
        {
            Editor.Log("Loading starsystem list");
            Editor.dataPresenter.StarSystemListLoader();
        }
        /// <summary>
        /// Reakce na vybrani prvku z listu
        /// </summary>
        private void starSystemList_GotFocus(object sender, RoutedEventArgs e)
        {
            Editor.dataPresenter.SetStarSystemListFocus(sender, e);
        }
        /// <summary>
        /// Metoda pro prekresleni mapy
        /// </summary>
        public void ReDrawMap() 
        {
            if (Editor.IsLoaded)
            {
                Editor.dataPresenter.StarSystemDrawer();
                //Editor.dataPresenter.GetDrawingArea().ShowStarSystemInfo();
                this.SimulationTime.Content = "Simulation time: " + Editor.Time;
            }
        }
        /// <summary>
        /// Pridani 50 sekund k simulatoru
        /// </summary>
        private void buttonTime50_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time += 50;
            ReDrawMap();
        }
        /// <summary>
        /// Pridani 200 sekund k simulatoru
        /// </summary>
        private void buttonTime200_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time += 200;
            ReDrawMap();
        }
        /// <summary>
        /// Restart casu simulatoru
        /// </summary>
        private void buttonTimeReset_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time = 0;
            ReDrawMap();
        }
        /// <summary>
        /// Inicializace seznamu spojeni
        /// </summary>
        private void connectionListBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as GroupBox).Content = Editor.dataPresenter.GetConnectionList();
        }
        /// <summary>
        /// Inicializace scrollvieweru
        /// </summary>
        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            this.drawingAreaScrollViewew.Content = DrawingArea.Canvas;
        }
        /// <summary>
        /// Inicializace object data
        /// </summary>
        private void loadedObjectData_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as GroupBox).Content = Editor.dataPresenter.GetLoadedObjectData();
        }
        /// <summary>
        /// Reakce na kliknuti na load
        /// </summary>
        private void MenuLoad_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                Editor.LoadGalaxyFile();
                this.ProgramStatus.Content = "Loaded";
                DrawingArea.Canvas.Children.Clear();    
                Editor.dataPresenter = new DataPresenter();
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
        /// Reakce na kliknuti na save
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
        /// Prida 10 sekund do simulatoru
        /// </summary>
        private void buttonTime10_Click(object sender, RoutedEventArgs e)
        {
            Editor.Time += 10;
            ReDrawMap();
        }
        /// <summary>
        /// Inicializace galaxy info
        /// </summary>
        private void galaxyInfoGroup_Loaded(object sender, RoutedEventArgs e)
        {
            this.galaxyInfoGroup.Content = Editor.dataPresenter.GetGalaxyInfo();
        }
    }
}
