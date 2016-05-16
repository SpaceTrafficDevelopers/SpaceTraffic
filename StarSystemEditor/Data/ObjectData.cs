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

        /// <summary>
        /// Constructor creating graphics for selected object editor
        /// </summary>
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
            else if(Editor.dataPresenter.SelectedObject is PlanetView)
            {
                Planet selectedPlanet = (Editor.dataPresenter.SelectedObject as PlanetView).Planet;

                Grid grid = createGrid();
                
                Label planetName = createLabel("Planet Name:", 0, 0);
                Label planetAltName = createLabel("Planet Alt Name:", 0, 1);
                Label period = createLabel("Period:", 0, 2);
                Label direction = createLabel("Direction:", 0, 3);
                Label mass = createLabel("Mass:", 0, 4);
                Label gravity = createLabel("Gravity:", 0, 5);
                
                Label description = new Label();
                description.MinWidth = 60;
                description.Content = "Description:";
                description.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumnSpan(description, 2);
                Grid.SetRow(description, 6);

                TextBox planetName_text = createTextBox(0, 1, 0);
                planetName_text.Text = selectedPlanet.Name;

                TextBox planetAltName_text = createTextBox(1, 1, 1);
                planetAltName_text.Text = selectedPlanet.AlternativeName;

                TextBox period_text = createTextBox(2, 1, 2);
                period_text.Text = (selectedPlanet.Trajectory as OrbitDefinition).PeriodInSec.ToString();

                ComboBox direction_box = createDirectionComboBox();
                if ((selectedPlanet.Trajectory as OrbitDefinition).Direction == SpaceTraffic.Game.Geometry.Direction.CLOCKWISE)
                    direction_box.SelectedIndex = 0;
                else direction_box.SelectedIndex = 1;
                
                TextBox mass_text = createTextBox(4, 1, 4);
                mass_text.Text = selectedPlanet.Details.Mass.ToString();

                TextBox gravity_text = createTextBox(5, 1, 5);
                gravity_text.Text = selectedPlanet.Details.Gravity.ToString();

                TextBox description_text = createDescriptionTextBox();
                description_text.Text = selectedPlanet.Details.Description;
                
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

        private TextBox createDescriptionTextBox()
        {
            TextBox description_text = new TextBox();
            description_text.MinWidth = 240;
            description_text.SelectionChanged += selection_changed;
            description_text.Tag = 6;
            description_text.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            description_text.Background = Brushes.White;
            description_text.AcceptsReturn = true;
            description_text.AcceptsTab = true;
            description_text.TextWrapping = TextWrapping.Wrap;
            description_text.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumnSpan(description_text, 2);
            Grid.SetRow(description_text, 7);
            return description_text;
        }

        /// <summary>
        /// creates direction combobox
        /// </summary>
        /// <returns>combobox</returns>
        private ComboBox createDirectionComboBox()
        {
            ComboBox direction_box = new ComboBox();
            direction_box.MinWidth = 60;
            ComboBoxItem clockwise = new ComboBoxItem();
            clockwise.Content = "Clockwise";
            ComboBoxItem counter_clockwise = new ComboBoxItem();
            counter_clockwise.Content = "Counterclockwise";
            direction_box.Items.Add(clockwise);
            direction_box.Items.Add(counter_clockwise);
            direction_box.HorizontalAlignment = HorizontalAlignment.Left;
            direction_box.SelectionChanged += selection_changed;
            direction_box.Tag = 3;
            Grid.SetColumn(direction_box, 1);
            Grid.SetRow(direction_box, 3);
            return direction_box;
        }
        /// <summary>
        /// creates label
        /// </summary>
        /// <param name="content">Text on label</param>
        /// <param name="Column">column in grid</param>
        /// <param name="Row">row in grid</param>
        /// <returns></returns>
        private Label createLabel(String content, int Column, int Row)
        {
            Label label = new Label();
            label.MinWidth = 60;
            label.Content = content;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(label, Column);
            Grid.SetRow(label, Row);
            return label;
        }

        /// <summary>
        /// creates TextBox
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="Column">column in grid</param>
        /// <param name="Row">column in grid</param>
        /// <returns></returns>
        private TextBox createTextBox(int tag, int Column, int Row)
        {
            TextBox textBox = new TextBox();
            textBox.MinWidth = 60;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.SelectionChanged += selection_changed;
            textBox.Tag = tag;
            Grid.SetColumn(textBox, Column);
            Grid.SetRow(textBox, Row);
            return textBox;
        }

        /// <summary>
        /// creates Grid
        /// </summary>
        /// <returns>grid</returns>
        private Grid createGrid()
        {
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

            return grid;
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
