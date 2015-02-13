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
        /// Reakce na tlacitko expandovat/skryt (+/-)
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.starSystemListBox.Visibility == Visibility.Collapsed)
            {
                this.starSystemListExpander.Content = "-";
                this.starSystemListBox.Visibility = Visibility.Visible;
            }
            else
            {
                this.starSystemListExpander.Content = "+";
                this.starSystemListBox.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Inicializace comboboxu
        /// </summary>
        private void starSystemListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (Editor.dataPresenter == null) return;
            Editor.dataPresenter.StarSystemListLoader();
            this.starSystemListBox.Content = Editor.dataPresenter.GetStarSystemList();
            this.starSystemObjectTreeBox.Content = Editor.dataPresenter.GetStarSystemObjectTree();
        }
        /// <summary>
        /// Inicializace
        /// </summary>
        private void starSystemSingleSelector_Loaded(object sender, RoutedEventArgs e)
        {
            if (Editor.dataPresenter == null) return;
            this.starSystemSingleSelector.SelectionChanged += new SelectionChangedEventHandler(Editor.dataPresenter.StarSystemSelectorChange);
            this.starSystemSingleSelector.ItemsSource = Editor.LoadStarSystemNames();
        }
    }
}
