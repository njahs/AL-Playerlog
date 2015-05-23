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

using LogViewer.Main;

namespace LogViewer
{
    /// <summary>
    /// Interaction logic for ControlWindow.xaml
    /// </summary>
    public partial class ControlWindow : Window
    {
        ChartWindow cW;
        Int32 division_factor = 1000;
        Double pointSize = 6.0, lineSize = 2.0;

        ObservableCollection<Player> _players { get; set; }
        public ObservableCollection<Player> players { get { return _players; } set { if (_players != value) _players = value; } }
        public double defaultHeight = 350, defaultWidth = 650;

        public ControlWindow(ChartWindow _cW)
        {
            cW = _cW;

            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cW != null && cW.IsLoaded) cW.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            players = new ObservableCollection<Player>();

            DBConnect.instance.FetchPlayers().ForEach((x) => players.Add(x));

            PlayerView.ItemsSource = players;
            PlayerView.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshButton.IsEnabled = false;

            players.Clear();

            DBConnect.instance.FetchPlayers().ForEach((x) => players.Add(x));

            RefreshButton.IsEnabled = true;
        }

        private void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Player item = ((FrameworkElement)e.OriginalSource).DataContext as Player;

            if (item != null)
            {
                LogCollection log_timeline = DBConnect.instance.FetchPlayerLog(item.uid);

                DateTime[] dates = new DateTime[log_timeline.Count];
                for (int i = 0; i < log_timeline.Count; i++) { dates[i] = log_timeline[i].updatetime; }

                int[] cash = new int[log_timeline.Count];
                for (int i = 0; i < log_timeline.Count; i++) { cash[i] = log_timeline[i].cashdiff / division_factor; }

                int[] bankCash = new int[log_timeline.Count];
                for (int i = 0; i < log_timeline.Count; i++) { bankCash[i] = log_timeline[i].bankdiff / division_factor; }

                int[] cashTotal = new int[log_timeline.Count];
                for (int i = 0; i < log_timeline.Count; i++) { cashTotal[i] = log_timeline[i].totalcash / division_factor; }

                var datesDataSource = new EnumerableDataSource<DateTime>(dates);
                datesDataSource.SetXMapping(x => cW.dateAxis.ConvertToDouble(x));

                var cashDiffDataSource = new EnumerableDataSource<int>(cash);
                cashDiffDataSource.SetYMapping(y => y);
                var bankDiffDataSource = new EnumerableDataSource<int>(bankCash);
                bankDiffDataSource.SetYMapping(y => y);
                var cashTotalDataSource = new EnumerableDataSource<int>(cashTotal);
                cashTotalDataSource.SetYMapping(y => y);

                CompositeDataSource compositeDataSource1 = new
                  CompositeDataSource(datesDataSource, cashDiffDataSource);
                CompositeDataSource compositeDataSource2 = new
                  CompositeDataSource(datesDataSource, bankDiffDataSource);
                CompositeDataSource compositeDataSource3 = new
                  CompositeDataSource(datesDataSource, cashTotalDataSource);

                cW.plotter.Children.RemoveAll(typeof(LineGraph));
                cW.plotter.Children.RemoveAll(typeof(MarkerPointsGraph));  

                if (Properties.Settings.Default.show_cash) cW.plotter.AddLineGraph(compositeDataSource1,
                  new Pen(Brushes.Red, lineSize),
                  new CirclePointMarker { Size = pointSize, Fill = Brushes.Red },
                  new PenDescription("Cash difference"));

                if (Properties.Settings.Default.show_bank) cW.plotter.AddLineGraph(compositeDataSource2,
                  new Pen(Brushes.Blue, lineSize),
                  new CirclePointMarker { Size = pointSize, Fill = Brushes.Blue },
                  /*new TrianglePointMarker
                  {
                      Size = 10.0,
                      Pen = new Pen(Brushes.Black, 2.0),
                      Fill = Brushes.GreenYellow
                  },*/
                  new PenDescription("Bank difference"));

                if (Properties.Settings.Default.show_total) cW.plotter.AddLineGraph(compositeDataSource3,
                  new Pen(Brushes.DarkGreen, lineSize),
                  new CirclePointMarker { Size = pointSize, Fill = Brushes.DarkGreen },
                  new PenDescription("Total Cash"));

                cW.plotter.Legend.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                cW.plotter.Legend.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                cW.plotter.Viewport.FitToView();
            }
        }
    }
}
