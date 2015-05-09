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

        //Closing handler
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // activate main window
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
            //activate main window
            this.Owner.Focusable = true;
            Close();
        }

        private void planetcount_text_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkDigit(sender, e);
        }

        private void wormholecount_text_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkDigit(sender, e);
        }

        /// <summary>
        /// checks content of textbox for positive digits
        /// </summary>
        /// <param name="sender">textbox</param>
        /// <param name="e">args</param>
        private void checkDigit(object sender, TextChangedEventArgs e)
        {
            try
            {
                int planetCount = Convert.ToInt16((sender as TextBox).Text);
                if (planetCount <= 0)
                {
                    MessageBox.Show("You must enter positive number");
                    // clears textbox content
                    (sender as TextBox).Text = "" + 5;
                }
            }
            catch (FormatException exception)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch((sender as TextBox).Text, "[^0-9]"))
                {
                    MessageBox.Show("You must enter positive number");
                    Editor.Log(exception.ToString());
                    // clears textbox content
                    (sender as TextBox).Text = "" + 5;
                }
            }
        }
    }
}
