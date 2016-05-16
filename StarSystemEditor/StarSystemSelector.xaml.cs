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

namespace SpaceTraffic.Tools.StarSystemEditor
{
    /// <summary>
    /// Logika pro zobrazovac seznamu systemu
    /// </summary>
    public partial class StarSystemSelector : UserControl
    {
        /// <summary>
        /// Konstuktor
        /// </summary>
        public StarSystemSelector()
        {
            InitializeComponent();
            Editor.dataPresenter.starSystemListChanged = StarSystemListChanged;
        }
        /// <summary>
        /// Inicializace seznamu starsystemu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void starSystemList_Loaded(object sender, RoutedEventArgs e)
        {
            Editor.Log("Loading starsystem list");
        }
     
        /// <summary>
        /// Combobox initialization
        /// </summary>
        private void starSystemListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (Editor.dataPresenter == null) return;
            Editor.dataPresenter.StarSystemListLoader();
            this.starSystemListBox.Content = Editor.dataPresenter.GetStarSystemList();
            this.starSystemObjectTreeBox.Content = Editor.dataPresenter.GetStarSystemObjectTree();
        }
        
        /// <summary>
        /// Method called from dataPresenter to refresh starsystems
        /// </summary>
        private void StarSystemListChanged()
        {
            Editor.dataPresenter.StarSystemListLoader();
            this.starSystemListBox.Content = Editor.dataPresenter.GetStarSystemList();
            this.starSystemObjectTreeBox.Content = Editor.dataPresenter.GetStarSystemObjectTree();
        }

    }
}
