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
using System.Windows.Navigation;
using System.Windows.Shapes;

using MySql.Data.MySqlClient;

using LogViewer.Main;

namespace LogViewer
{
    public partial class MainWindow : Window
    {
        DBConnect db_handler;
        ChartWindow cW;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e) // Connect
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.server) ||
                String.IsNullOrEmpty(Properties.Settings.Default.database) ||
                String.IsNullOrEmpty(Properties.Settings.Default.username) ||
                String.IsNullOrEmpty(Properties.Settings.Default.password))
            {
                messageBlock.Text = "Fill everything out.";
                return;
            }
            else
            {
                Properties.Settings.Default.Save();
            }

            db_handler = new DBConnect(
                Properties.Settings.Default.server,
                Properties.Settings.Default.database,
                Properties.Settings.Default.username,
                Properties.Settings.Default.password);

            switch (db_handler.CheckConnection())
            {
                case (DBConnect.Connection.Successfull):
                    {
                        messageBlock.Text = "Successfully connected!";

                        if (cW == null || cW.IsLoaded) cW = new ChartWindow(this);
                        cW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        cW.Show();

                        this.Hide();

                        break;
                    }
                case (DBConnect.Connection.ERROR_Connection):
                    {
                        messageBlock.Text = "Error connecting to the Server.";

                        break;
                    }
                case (DBConnect.Connection.ERROR_Credentials):
                    {
                        messageBlock.Text = "Invalid Username/Password.";

                        break;
                    }
                default:
                    {
                        messageBlock.Text = "Unknown Error.";

                        break;
                    }
            }
        }
    }
}
