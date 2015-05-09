using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceTraffic.Tools.StarSystemEditor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RenameSystem : Window
    {
        public RenameSystem()
        {
            InitializeComponent();
        }

        //Closing handler
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // activate main window
            this.Owner.Focusable = true;
        }

        // rename system and star name
        private void Confirm_button_Click(object sender, RoutedEventArgs e)
        {
            string newName = this.name_text.Text;
            string oldName = Editor.dataPresenter.SelectedStarSystem.Name;
            // rename star
            Editor.dataPresenter.SelectedStarSystem.Star.Name = newName;
            // rename system
            Editor.dataPresenter.SelectedStarSystem.Name = newName;
            //refresh object tree and starsystemlist
            Editor.dataPresenter.GetStarSystemList().Items.Clear();
            Editor.dataPresenter.starSystemListChanged();
            Editor.dataPresenter.GetStarSystemObjectTree();
            //activate main window
            this.Owner.Focusable = true;
            Close();
        }
    }
}
