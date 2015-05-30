using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.IO;
using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker

using Playerlog.Main;

namespace Playerlog.Windows
{
    public partial class ControlWindow : Window
    {
        ChartWindow cW;
        OptionsWindow oW;
        AboutWindow aW;

        ObservableCollection<Player> _players { get; set; }
        public ObservableCollection<Player> players { get { return _players; } set { if (_players != value) _players = value; } }
        public double defaultHeight = 350, defaultWidth = 650;

        public ControlWindow(ChartWindow _cW)
        {
            cW = _cW;

            InitializeComponent();
        }

        #region Window-Events

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cW != null && cW.IsLoaded) cW.Close();
            if (oW != null && oW.IsLoaded) oW.Close();
            if (aW != null && aW.IsLoaded) aW.Close();
        }

        #region Loaded-Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            players = new ObservableCollection<Player>();

            PlayerCollection collection = DBConnect.instance.FetchPlayers();
            if (collection != null)
                collection.ForEach((x) => players.Add(x));

            PlayerView.ItemsSource = players;
            PlayerView.DataContext = this;
        }

        #endregion Loaded-Events

        #region Clicked-Events

        private void Button_Click_1(object sender, RoutedEventArgs e) // Options button
        {
            if (oW == null || oW.IsLoaded == false)
            {
                oW = new OptionsWindow();
                oW.Show();
            }
            else
            {
                oW.Activate();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshButton.IsEnabled = false;

            players.Clear();

            PlayerCollection collection = DBConnect.instance.FetchPlayers();
            if (collection != null)
                collection.ForEach((x) => players.Add(x));

            RefreshButton.IsEnabled = true;
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player item = ((FrameworkElement)e.OriginalSource).DataContext as Player;

            if (item != null)
            {
                cW.SetChartPlayer(item);
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (aW == null || aW.IsLoaded == false)
            {
                aW = new AboutWindow();
                aW.Show();
            }
            else
            {
                aW.Activate();
            }
        }

        #endregion Clicked-Events

        #endregion Window-Events

        #region Functions

        /*~CR:Reed Copsey~*/
        public static void MaximizeToSecondaryMonitor(Window window)
        {
            var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                window.Left = workingArea.Left;
                window.Top = workingArea.Top;
                window.Width = workingArea.Width;
                window.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (window.IsLoaded)
                    window.WindowState = WindowState.Maximized;
            }
        }

        #endregion Functions
    }
}
