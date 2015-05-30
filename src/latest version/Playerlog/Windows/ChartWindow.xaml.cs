using System;
using System.Collections.Generic;
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
using Playerlog.Utils;

namespace Playerlog.Windows
{
    public partial class ChartWindow : Window
    {
        ControlWindow cW;
        MainWindow mW;

        Double division_factor = 1000;
        Double pointSize = 6.0, lineSize = 2.0;

        public ChartWindow(MainWindow _mW)
        {
            mW = _mW;

            InitializeComponent();
        }

        #region Window-Events

        private void Window_Closed(object sender, EventArgs e)
        {
            if (cW != null && cW.IsLoaded) cW.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mW.Close();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (cW != null && cW.IsLoaded && Properties.Settings.Default.stick_window)
            {
                cW.Top = this.Top;
                cW.Left = this.Left + this.Width;

                cW.Height = this.Height;
                cW.Width = cW.defaultWidth;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (cW != null && cW.IsLoaded && Properties.Settings.Default.stick_window)
            {
                cW.Top = this.Top;
                cW.Left = this.Left + this.Width;

                cW.Height = this.Height;
                cW.Width = cW.defaultWidth;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (cW != null && cW.IsLoaded && Properties.Settings.Default.stick_window)
            {
                if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    MaximizeToSecondaryMonitor(cW);
                }
                else
                {
                    cW.WindowState = System.Windows.WindowState.Normal;
                    cW.Top = this.Top;
                    cW.Left = this.Left + this.Width;

                    cW.Height = this.Height;
                    cW.Width = cW.defaultWidth;
                }
            }
        }

        #region Loaded-Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (cW == null || !cW.IsLoaded) cW = new ControlWindow(this);
            cW.Top = this.Top;
            cW.Left = this.Left + this.Width;
            cW.Height = this.Height;
            cW.Width = cW.defaultWidth;

            cW.Show();
        }

        #endregion Loaded-Events

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

        public void SetChartPlayer(Player player)
        {
            LogCollection log_timeline = DBConnect.instance.FetchPlayerLog(player.uid);

            if (log_timeline == null) return;

            Int32 mode = 0;
            Int32.TryParse(Utils.Registry.GetValue(Utils.Registry.Key_mode), out mode);

            if (log_timeline.maxCash > 1000 * 1) division_factor = 1;
            //if (log_timeline.maxCash > 1000 * 10) division_factor = 10;
            //if (log_timeline.maxCash > 1000 * 100) division_factor = 100;
            if (log_timeline.maxCash > 1000 * 1000) division_factor = 1000;

            verticalTitle.Content = String.Format("Value in $ (1:{0})", division_factor.ToString());
            plotHeader.Content = String.Format("Log viewer: '{0}'", player.uname);

            DateTime[] dates = new DateTime[log_timeline.Count];
            for (int i = 0; i < log_timeline.Count; i++) { dates[i] = log_timeline[i].updatetime; }

            double[] cash = new double[log_timeline.Count];
            for (int i = 0; i < log_timeline.Count; i++) { cash[i] = log_timeline[i].cash / division_factor; }

            double[] bankCash = new double[log_timeline.Count];
            for (int i = 0; i < log_timeline.Count; i++) { bankCash[i] = log_timeline[i].bank / division_factor; }

            double[] totalCash = new double[log_timeline.Count];
            for (int i = 0; i < log_timeline.Count; i++) { totalCash[i] = log_timeline[i].totalcash / division_factor; }

            var datesDataSource = new EnumerableDataSource<DateTime>(dates);
            datesDataSource.SetXMapping(x => dateAxis.ConvertToDouble(x));

            var cashDataSource = new EnumerableDataSource<double>(cash);
            cashDataSource.SetYMapping(y => y);
            var bankDataSource = new EnumerableDataSource<double>(bankCash);
            bankDataSource.SetYMapping(y => y);
            var totalCashDataSource = new EnumerableDataSource<double>(totalCash);
            totalCashDataSource.SetYMapping(y => y);

            CompositeDataSource compositeDataSource1 = new
              CompositeDataSource(datesDataSource, cashDataSource);
            CompositeDataSource compositeDataSource2 = new
              CompositeDataSource(datesDataSource, bankDataSource);
            CompositeDataSource compositeDataSource3 = new
              CompositeDataSource(datesDataSource, totalCashDataSource);

            plotter.Children.RemoveAll(typeof(LineGraph));
            plotter.Children.RemoveAll(typeof(MarkerPointsGraph));

            if (Properties.Settings.Default.show_cash) plotter.AddLineGraph(compositeDataSource1,
              new Pen(Brushes.Red, lineSize),
              new CirclePointMarker { Size = pointSize, Fill = Brushes.Red },
              new PenDescription(mode == 0 ? "Cash" : "Cash difference"));

            if (Properties.Settings.Default.show_bank) plotter.AddLineGraph(compositeDataSource2,
              new Pen(Brushes.Blue, lineSize),
              new CirclePointMarker { Size = pointSize, Fill = Brushes.Blue },
              new PenDescription(mode == 0 ? "Bank" : "Bank difference"));

            if (Properties.Settings.Default.show_total) plotter.AddLineGraph(compositeDataSource3,
              new Pen(Brushes.DarkGreen, lineSize),
              new CirclePointMarker { Size = pointSize, Fill = Brushes.DarkGreen },
              new PenDescription("Total cash"));
            
            plotter.Legend.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            plotter.Legend.VerticalAlignment = System.Windows.VerticalAlignment.Top;
           
            plotter.Viewport.FitToView();
        }

        #endregion Functions
    }
}
