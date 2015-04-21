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
    public partial class NewStarSystem : Window
    {
        /// <summary>
        /// Konstruktor - inicializuje komponenty
        /// </summary>
        public NewStarSystem()
        {
            InitializeComponent();
            SystemType_load();
            this.Activate();
        }

        //obsluha kliknuti na krizek - zavrit
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Focusable = true;
        }

        /// <summary>
        /// Adds items to combobox System type.
        /// </summary>
        private void SystemType_load()
        {
            systemtypebox.Items.Add("Circular");
            systemtypebox.Items.Add("Elliptic");
            systemtypebox.SelectedIndex = 0;
        }

        private void Confirm_button_Click(object sender, RoutedEventArgs e)
        {
            int pcount = Convert.ToInt32(planetcount_text.Text);
            int wcount = Convert.ToInt32(wormholecount_text.Text);
            string type = systemtypebox.Text;
            Editor.NewSystem(name_text.Text, pcount, wcount, type);
            //aktivace hlavniho okna
            this.Owner.Focusable = true;
            Close();
        }
    }
}
