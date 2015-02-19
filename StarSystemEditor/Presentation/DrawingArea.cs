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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using SpaceTraffic.Game;
using SpaceTraffic.Tools.StarSystemEditor.Entities;

namespace SpaceTraffic.Tools.StarSystemEditor.Presentation
{
    /// <summary>
    /// Vykreslovac starsystemu
    /// </summary>
    public class DrawingArea
    {
        private static Canvas _canvas;
        /// <summary>
        /// Property pro ziskani platna
        /// </summary>
        public static Canvas Canvas { 
            get
            {
                if (!Prepared) PrepareCanvas();
                return _canvas;
            }
            private set
            {
                _canvas = value;
            }
        }
        /// <summary>
        /// Stav zobrazovace
        /// </summary>
        public static bool Prepared = false;

        /// <summary>
        /// Metoda pro inicializace vykreslovace
        /// </summary>
        private static void PrepareCanvas() 
        {
            Editor.Log("Preparing canvas");
            _canvas = new Canvas();
            Prepared = true;
            Canvas.Name = "starSystemRenderer";
            Canvas.Height = 500;
            Canvas.Width = 500;
            Canvas.Margin = new Thickness(0, 5, 0, 0);
            Canvas.VerticalAlignment = VerticalAlignment.Top;
            Canvas.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.ClipToBounds = true;
            Canvas.Background = Brushes.Black;
            
        }
        /// <summary>
        /// Metoda pro pridani grafiky k zobrazeni
        /// </summary>
        /// <param name="view">Objekt pro zobrazeni</param>
        public static void AddShapeView(View view) 
        {
            if (!Prepared) PrepareCanvas();
            Shape shape = view.GetShape();
            shape.Tag = view.GetLoadedObject();
            shape.Name = view.GetLoadedObject().ToString();
            Canvas.SetLeft(shape, (Editor.dataPresenter.DrawingAreaSize - view.GetSize().Width));
            Canvas.SetTop(shape, (Editor.dataPresenter.DrawingAreaSize - view.GetSize().Height)); 
            Canvas.Children.Add(shape);
        }
        /// <summary>
        /// Metoda zobrazujici informace o starsystemu
        /// </summary>
        public static void ShowStarSystemInfo()
        {
            if (!Prepared) PrepareCanvas();
            GroupBox starSystemInfoBox = new GroupBox();
            starSystemInfoBox.Header = "StarSystem info";
            starSystemInfoBox.BorderBrush = Brushes.White;
            starSystemInfoBox.BorderThickness = new Thickness(1);
            starSystemInfoBox.Foreground = Brushes.White;

            TextBlock infoText = new TextBlock();
            if (Editor.dataPresenter.SelectedStarSystem == null)
            {
                infoText.Foreground = Brushes.White;
                infoText.Name = "loadedStarSystemData";
                infoText.Text = "NO SYSTEM LOADED";
            }
            else
            {
                infoText.Foreground = Brushes.White;
                infoText.Name = "loadedStarSystemData";
                infoText.Text = Editor.dataPresenter.SelectedStarSystem.Name.ToString();
                infoText.Text += "\n" + "Planet count: " + Editor.dataPresenter.SelectedStarSystem.Planets.Count;
                infoText.Text += "\n" + "Wormholes count: " + Editor.dataPresenter.SelectedStarSystem.WormholeEndpoints.Count;
            }
            

            starSystemInfoBox.Content = infoText;

            Canvas.SetLeft(starSystemInfoBox, 10);
            Canvas.SetTop(starSystemInfoBox, 10);
            
            Canvas.Children.Add(starSystemInfoBox);
        }
    }
}
