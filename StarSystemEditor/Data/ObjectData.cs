using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using SpaceTraffic.Game;
using SpaceTraffic.Tools.StarSystemEditor.Presentation;
using System.Windows;
using SpaceTraffic.Game.Geometry;
using System.Windows.Media;

namespace SpaceTraffic.Tools.StarSystemEditor.Data
{
    class ObjectData
    {
        private FrameworkElement loadedObjectData = null;

        /// <summary>
        /// Getter pro ziskani nacitaneho objektu
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetLoadedObjectData()
        {
            return this.loadedObjectData;
        }

        public ObjectData()
        {
            if (Editor.dataPresenter.SelectedObject == null)
            {
                if (!(Editor.dataPresenter.SelectedObject is PlanetView))
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Name = "loadedObjectData";
                    textBlock.Text = "NO PLANET LOADED";
                    this.loadedObjectData = textBlock;
                }
            }
            else
            {
                Planet selectedPlanet = (Editor.dataPresenter.SelectedObject as PlanetView).Planet;
                
                Grid grid = new Grid();
                grid.Width = 250;
                grid.MaxWidth = 250;
                grid.MinHeight = 350;
                grid.HorizontalAlignment = HorizontalAlignment.Left;
                grid.VerticalAlignment = VerticalAlignment.Top;

                // Define the Columns
                ColumnDefinition labels = new ColumnDefinition();
                ColumnDefinition textFields = new ColumnDefinition();
                grid.ColumnDefinitions.Add(labels);
                grid.ColumnDefinitions.Add(textFields);

                // Define the Rows
                RowDefinition PlanetName = new RowDefinition();
                RowDefinition PlanetAltName = new RowDefinition();
                RowDefinition Period = new RowDefinition();
                RowDefinition Direction = new RowDefinition();
                RowDefinition Mass = new RowDefinition();
                RowDefinition Gravity = new RowDefinition();
                RowDefinition DescriptionLabel = new RowDefinition();
                RowDefinition DescriptionText = new RowDefinition();
                DescriptionText.MinHeight = 200;
                grid.RowDefinitions.Add(PlanetName);
                grid.RowDefinitions.Add(PlanetAltName);
                grid.RowDefinitions.Add(Period);
                grid.RowDefinitions.Add(Direction);
                grid.RowDefinitions.Add(Mass);
                grid.RowDefinitions.Add(Gravity);
                grid.RowDefinitions.Add(DescriptionLabel);
                grid.RowDefinitions.Add(DescriptionText);

                Label planetName = new Label();
                planetName.MinWidth = 60;
                planetName.Content = "Planet Name:";
                planetName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(planetName, 0);
                Grid.SetRow(planetName, 0);

                Label planetAltName = new Label();
                planetAltName.MinWidth = 60;
                planetAltName.Content = "Planet Alt Name:";
                planetAltName.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(planetAltName, 0);
                Grid.SetRow(planetAltName, 1);

                Label period = new Label();
                period.MinWidth = 60;
                period.Content = "Period:";
                period.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(period, 0);
                Grid.SetRow(period, 2);

                Label direction = new Label();
                direction.MinWidth = 60;
                direction.Content = "Direction:";
                direction.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(direction, 0);
                Grid.SetRow(direction, 3);

                Label mass = new Label();
                mass.MinWidth = 60;
                mass.Content = "Mass:";
                mass.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(mass, 0);
                Grid.SetRow(mass, 4);

                Label gravity = new Label();
                gravity.MinWidth = 60;
                gravity.Content = "Gravity:";
                gravity.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(gravity, 0);
                Grid.SetRow(gravity, 5);

                Label description = new Label();
                description.MinWidth = 60;
                description.Content = "Description:";
                description.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumnSpan(description, 2);
                Grid.SetRow(description, 6);

                TextBox planetName_text = new TextBox();
                planetName_text.MinWidth = 60;
                planetName_text.Text = selectedPlanet.Name;
                planetName_text.HorizontalAlignment = HorizontalAlignment.Left;
                planetName_text.SelectionChanged += selection_changed;
                planetName_text.Tag = 0;
                Grid.SetColumn(planetName_text, 1);
                Grid.SetRow(planetName_text, 0);

                TextBox planetAltName_text = new TextBox();
                planetAltName_text.MinWidth = 60;
                planetAltName_text.Text = selectedPlanet.AlternativeName;
                planetAltName_text.HorizontalAlignment = HorizontalAlignment.Left;
                planetAltName_text.SelectionChanged += selection_changed;
                planetAltName_text.Tag = 1;
                Grid.SetColumn(planetAltName_text, 1);
                Grid.SetRow(planetAltName_text, 1);

                TextBox period_text = new TextBox();
                period_text.MinWidth = 60;
                period_text.Text = (selectedPlanet.Trajectory as OrbitDefinition).PeriodInSec.ToString();
                period_text.SelectionChanged += selection_changed;
                period_text.Tag = 2;
                period_text.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(period_text, 1);
                Grid.SetRow(period_text, 2);

                ComboBox direction_box = new ComboBox();
                direction_box.MinWidth = 60;
                ComboBoxItem clockwise = new ComboBoxItem();
                clockwise.Content = "Clockwise";
                ComboBoxItem counter_clockwise = new ComboBoxItem();
                counter_clockwise.Content = "Counterclockwise";
                direction_box.Items.Add(clockwise);
                direction_box.Items.Add(counter_clockwise);
                direction_box.HorizontalAlignment = HorizontalAlignment.Left;
                if ((selectedPlanet.Trajectory as OrbitDefinition).Direction == SpaceTraffic.Game.Geometry.Direction.CLOCKWISE)
                    direction_box.SelectedIndex = 0;
                else direction_box.SelectedIndex = 1;
                direction_box.SelectionChanged += selection_changed;
                direction_box.Tag = 3;
                Grid.SetColumn(direction_box, 1);
                Grid.SetRow(direction_box, 3);

                TextBox mass_text = new TextBox();
                mass_text.MinWidth = 60;
                mass_text.SelectionChanged += selection_changed;
                mass_text.Tag = 4;
                mass_text.Text = selectedPlanet.Details.Mass.ToString();
                mass_text.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(mass_text, 1);
                Grid.SetRow(mass_text, 4);

                TextBox gravity_text = new TextBox();
                gravity_text.MinWidth = 60;
                gravity_text.SelectionChanged += selection_changed;
                gravity_text.Tag = 5;
                gravity_text.Text = selectedPlanet.Details.Gravity.ToString();
                gravity_text.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(gravity_text, 1);
                Grid.SetRow(gravity_text, 5);

                TextBox description_text = new TextBox();
                description_text.MinWidth = 240;
                description_text.SelectionChanged += selection_changed;
                description_text.Tag = 6;
                description_text.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                description_text.Background = Brushes.White;
                description_text.AcceptsReturn = true;
                description_text.AcceptsTab = true;
                description_text.TextWrapping = TextWrapping.Wrap;
                description_text.Text = selectedPlanet.Details.Description;
                description_text.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumnSpan(description_text, 2);
                Grid.SetRow(description_text, 7);

                grid.Children.Add(planetName);
                grid.Children.Add(planetAltName);
                grid.Children.Add(planetName_text);
                grid.Children.Add(planetAltName_text);
                grid.Children.Add(period);
                grid.Children.Add(direction);
                grid.Children.Add(mass);
                grid.Children.Add(gravity);
                grid.Children.Add(description);
                grid.Children.Add(period_text);
                grid.Children.Add(direction_box);
                grid.Children.Add(mass_text);
                grid.Children.Add(gravity_text);
                grid.Children.Add(description_text);

                this.loadedObjectData = grid;
            }
        }

        /// <summary>
        /// metoda kterou zavola event handler z object info pri selection changed
        /// </summary>
        /// <param name="sender">textbox u object info</param>
        /// <param name="e">parametry eventu</param>
        private void selection_changed(object sender, RoutedEventArgs e)
        {
            Planet selectedPlanet = (Editor.dataPresenter.SelectedObject as PlanetView).Planet;
            switch ((int)((sender as FrameworkElement).Tag))
            {
                case 0:
                    selectedPlanet.Name = (sender as TextBox).Text;
                    break;
                case 1:
                    selectedPlanet.AlternativeName = (sender as TextBox).Text;
                    break;
                case 2:
                    try
                    {
                        double perioda = Convert.ToDouble((sender as TextBox).Text);
                        (selectedPlanet.Trajectory as OrbitDefinition).PeriodInSec = perioda;
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
                case 3:
                    if ((sender as ComboBox).SelectedIndex == 0)
                        (selectedPlanet.Trajectory as OrbitDefinition).Direction =
                            SpaceTraffic.Game.Geometry.Direction.CLOCKWISE;
                    else
                        (selectedPlanet.Trajectory as OrbitDefinition).Direction =
                            SpaceTraffic.Game.Geometry.Direction.COUNTERCLOCKWISE;
                    break;
                case 4:
                    try
                    {
                        // pokud je predposledni E, a posledni + nebo -
                        if ((sender as TextBox).Text.Length >= 2)
                        {
                            if ((sender as TextBox).Text.ElementAt((sender as TextBox).Text.Length - 2).Equals('E'))
                            {
                                if ((sender as TextBox).Text.ElementAt((sender as TextBox).Text.Length - 2).Equals('+') ||
                                    (sender as TextBox).Text.ElementAt((sender as TextBox).Text.Length - 2).Equals('-'))
                                    break;
                            }
                        }
                        // pokud je posledni E
                        if ((sender as TextBox).Text.Length >= 1)
                        {
                            if ((sender as TextBox).Text.ElementAt((sender as TextBox).Text.Length - 1).Equals('E'))
                                break;
                        }
                        double mass = Convert.ToDouble((sender as TextBox).Text);
                        // detail je treba vytvorit znovu, nejde proste priradit jeden jeho parametr
                        selectedPlanet.Details =
                            new CelestialObjectInfo(selectedPlanet.Details.Gravity, mass, selectedPlanet.Details.Description);
                    }
                    catch (FormatException exception)
                    {

                        if (System.Text.RegularExpressions.Regex.IsMatch((sender as TextBox).Text, "[^0-9]"))
                        {
                            MessageBox.Show("Mass must be a number");
                            (sender as TextBox).Clear();
                        }
                    }
                    break;
                case 5:
                    try
                    {
                        double gravity = Convert.ToDouble((sender as TextBox).Text);
                        // detail je treba vytvorit znovu, nejde proste priradit jeden jeho parametr
                        selectedPlanet.Details = new CelestialObjectInfo(
                            gravity, selectedPlanet.Details.Mass, selectedPlanet.Details.Description);
                    }
                    catch (FormatException exception)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch((sender as TextBox).Text, "[^0-9]"))
                        {
                            MessageBox.Show("Period must be a number");
                            // vycistime text
                            (sender as TextBox).Clear();
                        }
                    } break;
                case 6:
                    // detail je treba vytvorit znovu, nejde proste priradit jeden jeho parametr
                    selectedPlanet.Details = new CelestialObjectInfo(
                        selectedPlanet.Details.Gravity, selectedPlanet.Details.Mass, (sender as TextBox).Text);
                    break;
            }
        }
    }

    
}
