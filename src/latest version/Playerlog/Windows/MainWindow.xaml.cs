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

using Playerlog.Main;

namespace Playerlog.Windows
{
    public partial class MainWindow : Window
    {
        DBConnect db_handler;
        public ChartWindow cW;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Window-Events

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        #region Loaded-Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }


        #endregion Loaded-Events

        #region Clicked-Events

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

                        if (String.IsNullOrEmpty(Utils.Registry.GetValue(Utils.Registry.Key_firstuse)))
                        {
                            FirstUseWindow fuw = new FirstUseWindow(this);
                            fuw.Show();
                        }
                        else
                        {
                            if (cW == null || cW.IsLoaded) cW = new ChartWindow(this);
                            cW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            cW.Show();
                        }

                        this.Close();

                        break;
                    }
                case (DBConnect.Connection.ERROR_Connection):
                    {
                        messageBlock.Text = "Error connecting to the Server.";
                        break;
                    }
                case (DBConnect.Connection.ERROR_Credentials):
                    {
                        messageBlock.Text = "Wrong username / password.";
                        break;
                    }
                case (DBConnect.Connection.ERROR_Servername):
                    {
                        messageBlock.Text = "Wrong server";
                        break;
                    }
                default:
                    {
                        messageBlock.Text = "Unknown Error.";
                        break;
                    }
            }
        }

        #endregion Clicked-Events

        #endregion Window-Events
    }
}