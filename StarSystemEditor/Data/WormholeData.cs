using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SpaceTraffic.Tools.StarSystemEditor.Presentation;
using SpaceTraffic.Game;
using SpaceTraffic.Game.Geometry;

namespace SpaceTraffic.Tools.StarSystemEditor.Data
{
    class WormholeData
    {
        private FrameworkElement loadedWormholeData = null;

        /// <summary>
        /// Getter pro ziskani nacitane wormhole
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetLoadedWormholeData()
        {
            return this.loadedWormholeData;
        }

        public WormholeData()
        {
            if (Editor.dataPresenter.SelectedObject == null)
            {
                if (!(Editor.dataPresenter.SelectedObject is EndpointView))
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Name = "loadedWormholeData";
                    textBlock.Text = "NO WORMHOLE LOADED";
                    this.loadedWormholeData = textBlock;
                }
            }
            else
            {
                WormholeEndpoint selectedWormhole = (Editor.dataPresenter.SelectedObject as EndpointView).WormholeEndpoint;

                Grid grid = new Grid();
                grid.Width = 250;
                grid.MaxWidth = 250;
                grid.MinHeight = 60;
                grid.HorizontalAlignment = HorizontalAlignment.Left;
                grid.VerticalAlignment = VerticalAlignment.Top;

                // Define the Columns
                ColumnDefinition labels = new ColumnDefinition();
                ColumnDefinition textFields = new ColumnDefinition();
                grid.ColumnDefinitions.Add(labels);
                grid.ColumnDefinitions.Add(textFields);

                // Define the Rows
                RowDefinition Period = new RowDefinition();
                RowDefinition Direction = new RowDefinition();
                grid.RowDefinitions.Add(Period);
                grid.RowDefinitions.Add(Direction);

                Label period = new Label();
                period.MinWidth = 60;
                period.Content = "Period:";
                period.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(period, 0);
                Grid.SetRow(period, 0);

                Label direction = new Label();
                direction.MinWidth = 60;
                direction.Content = "Direction:";
                direction.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(direction, 0);
                Grid.SetRow(direction, 1);

                TextBox period_text = new TextBox();
                period_text.MinWidth = 60;
                period_text.Text = (selectedWormhole.Trajectory as OrbitDefinition).PeriodInSec.ToString();
                period_text.SelectionChanged += selection_changed;
                period_text.Tag = 0;
                period_text.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(period_text, 1);
                Grid.SetRow(period_text, 0);

                ComboBox direction_box = new ComboBox();
                direction_box.MinWidth = 60;
                ComboBoxItem clockwise = new ComboBoxItem();
                clockwise.Content = "Clockwise";
                ComboBoxItem counter_clockwise = new ComboBoxItem();
                counter_clockwise.Content = "Counterclockwise";
                direction_box.Items.Add(clockwise);
                direction_box.Items.Add(counter_clockwise);
                direction_box.HorizontalAlignment = HorizontalAlignment.Left;
                if ((selectedWormhole.Trajectory as OrbitDefinition).Direction == SpaceTraffic.Game.Geometry.Direction.CLOCKWISE)
                    direction_box.SelectedIndex = 0;
                else direction_box.SelectedIndex = 1;
                direction_box.SelectionChanged += selection_changed;
                direction_box.Tag = 1;
                Grid.SetColumn(direction_box, 1);
                Grid.SetRow(direction_box, 1);

                grid.Children.Add(period);
                grid.Children.Add(direction);
                grid.Children.Add(period_text);
                grid.Children.Add(direction_box);

                this.loadedWormholeData = grid;
            }
        }

        /// <summary>
        /// metoda kterou zavola event handler z object info pri selection changed
        /// </summary>
        /// <param name="sender">textbox u object info</param>
        /// <param name="e">parametry eventu</param>
        private void selection_changed(object sender, RoutedEventArgs e)
        {
            WormholeEndpoint selectedWormhole = (Editor.dataPresenter.SelectedObject as EndpointView).WormholeEndpoint;
            switch ((int)((sender as FrameworkElement).Tag))
            {
                case 0:
                    try
                    {
                        double perioda = Convert.ToDouble((sender as TextBox).Text);
                        (selectedWormhole.Trajectory as OrbitDefinition).PeriodInSec = perioda;
                    }
                    catch (FormatException exception)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch((sender as TextBox).Text, "[^0-9]"))
                        {
                            MessageBox.Show("Period must be a number");
                            // vycistime text
                            (sender as TextBox).Clear();
                        }
                    }
                    break;
                case 1:
                    if ((sender as ComboBox).SelectedIndex == 0)
                        (selectedWormhole.Trajectory as OrbitDefinition).Direction =
                            SpaceTraffic.Game.Geometry.Direction.CLOCKWISE;
                    else
                        (selectedWormhole.Trajectory as OrbitDefinition).Direction =
                            SpaceTraffic.Game.Geometry.Direction.COUNTERCLOCKWISE;
                    break;
            }
        }
    }
}
