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

namespace Playerlog.Windows
{
    public partial class FirstUseWindow : Window
    {
        MainWindow _parent;

        public FirstUseWindow(MainWindow parent)
        {
            _parent = parent;

            InitializeComponent();
        }

        #region Window-Events

        #region Clicked-Events

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton1.IsChecked == false &&
                radioButton2.IsChecked == false)
            {
                radioButton1.BorderBrush = Brushes.Red;
                radioButton2.BorderBrush = Brushes.Red;
                return;
            }

            if (_parent.cW == null || _parent.cW.IsLoaded) _parent.cW = new ChartWindow(_parent);
            _parent.cW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _parent.cW.Show();

            Utils.Registry.SetValue(Utils.Registry.Key_firstuse, "true");
            Utils.Registry.SetValue(Utils.Registry.Key_mode, radioButton1.IsChecked == true ? "0" : "1");

            this.Close();
        }

        #endregion Clicked-Events

        #endregion Window-Events
       
    }
}
