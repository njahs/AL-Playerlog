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

namespace LogViewer
{
    public partial class ChartWindow : Window
    {
        ControlWindow cW;
        MainWindow mW;

        public ChartWindow(MainWindow _mW)
        {
            mW = _mW;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (cW == null || !cW.IsLoaded) cW = new ControlWindow(this);
            cW.Top = this.Top;
            cW.Left = this.Left + this.Width;
            cW.Height = this.Height;
            cW.Width = cW.defaultWidth;

            cW.Show();
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

        private void Window_Closed(object sender, EventArgs e)
        {
            if (cW != null && cW.IsLoaded) cW.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mW.Close();
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
    }
}
