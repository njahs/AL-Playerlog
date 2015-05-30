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

using Playerlog.Utils;
using Playerlog.Main;

namespace Playerlog.Windows
{
    public partial class OptionsWindow : Window
    {
        Int32 log_mode = -1;
        DBConnect.StatusResult last_status { get; set; }

        public OptionsWindow()
        {
            InitializeComponent();
        }

        #region Window-Events

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void modeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (modeComboBox.SelectedIndex == log_mode)
            {
                changeButton.IsEnabled = false;
                changeButton.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                changeButton.IsEnabled = true;
                changeButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #region Loaded-Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            String mode = Utils.Registry.GetValue(Utils.Registry.Key_mode);

            if (String.IsNullOrEmpty(mode) == false)
            {
                Int32 iMode;
                if (Int32.TryParse(mode, out iMode) && iMode <= modeComboBox.Items.Count + 1)
                {
                    modeComboBox.SelectedIndex = iMode;
                    log_mode = iMode;
                }
            }

            refreshStatus();
        }

        private void modeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            modeComboBox.Items.Add("Actual");
            modeComboBox.Items.Add("Difference");
        }

        #endregion Loaded-Events

        #region Clicked-Events

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            refreshStatus();
        }

        #region Table-Button-clicked-events

        private void tableCreBtn_Click(object sender, RoutedEventArgs e)
        {
            DBConnect.instance.Query(Properties.Resources.Query_05);
            // no matter what mode
            refreshStatus();
        }

        private void tableDelBtn_Click(object sender, RoutedEventArgs e)
        {
            DBConnect.instance.Query(Properties.Resources.Query_06);
            // no matter what mode
            refreshStatus();
        }

        private void tableClrBtn_Click(object sender, RoutedEventArgs e)
        {
            DBConnect.instance.Query(Properties.Resources.Query_07);
            // no matter what mode
            refreshStatus();
        }

        #endregion Table-Button-clicked-events

        #region Trigger-Button-clicked-events

        private void triggerCreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (log_mode == 0) // actual
            {
                DBConnect.instance.Query(String.Format("{0}\r\n{1}", Properties.Resources.Query_01, Properties.Resources.Query_02));
            }
            else
            {
                if (log_mode == 1) // difference
                {
                    DBConnect.instance.Query(String.Format("{0}\r\n{1}", Properties.Resources.Query_11, Properties.Resources.Query_12));
                }
            }

            refreshStatus();
        }

        private void triggerDelBtn_Click(object sender, RoutedEventArgs e)
        {
            DBConnect.instance.Query(String.Format("{0}\r\n{1}", Properties.Resources.Query_03, Properties.Resources.Query_04));

            refreshStatus();
        }

        #endregion Trigger-Button-clicked-events

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            if (log_mode != modeComboBox.SelectedIndex)
            {
                refreshStatus();

                if (MessageBox.Show("This will delete current logs and re-create existing triggers. Do you agree? \r\n", "Info", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes)
                {
                    if (last_status.tableStatus) // re-create table
                    {
                        // MAYBE KEEP TABLE AND NORMALIZE BANK/CASH VAR, NOT SPECIFIC LIKE 'BANKDIF'... (if this, just empty table)
                        DBConnect.instance.Query(Properties.Resources.Query_07);
                    }

                    if (last_status.triggerStatus) // re-create triggers
                    {
                        // delete triggers
                        // create new triggers w/ new queries

                        // delete
                        if (DBConnect.instance.Query(String.Format("{0}\r\n{1}", Properties.Resources.Query_03, Properties.Resources.Query_04)))
                        {
                            if (log_mode == 0) // actual
                            {
                                DBConnect.instance.Query(String.Format("{0}\r\n{1}", Properties.Resources.Query_01, Properties.Resources.Query_02));
                            }
                            else
                            {
                                if (log_mode == 1) // difference
                                {
                                    DBConnect.instance.Query(String.Format("{0}\r\n{1}", Properties.Resources.Query_11, Properties.Resources.Query_12));
                                }
                            }
                        }
                    }

                    changeButton.IsEnabled = false;
                    changeButton.Visibility = System.Windows.Visibility.Hidden;

                    log_mode = modeComboBox.SelectedIndex;
                    Utils.Registry.SetValue(Utils.Registry.Key_mode, log_mode.ToString());
                }
            }
        }

        #endregion Clicked-Events

        #endregion Window-Events

        #region Functions

        private void refreshStatus()
        {
            DBConnect.StatusResult status = DBConnect.instance.GetStatus();
            last_status = status;

            if (status.tableStatus)
            {
                tableStatus.Text = "Valid";
                tableStatus.Foreground = Brushes.Lime;
                tableCreBtn.IsEnabled = false;
                tableDelBtn.IsEnabled = true;
            }
            else
            {
                tableStatus.Text = "Invalid";
                tableStatus.Foreground = Brushes.OrangeRed;
                tableCreBtn.IsEnabled = true;
                tableDelBtn.IsEnabled = false;
            }

            if (status.triggerStatus)
            {
                triggerStatus.Text = "Valid";
                triggerStatus.Foreground = Brushes.Lime;
                triggerCreBtn.IsEnabled = false;
                triggerDelBtn.IsEnabled = true;
            }
            else
            {
                triggerStatus.Text = "Invalid";
                triggerStatus.Foreground = Brushes.OrangeRed;
                triggerCreBtn.IsEnabled = true;
                triggerDelBtn.IsEnabled = false;
            }

            tableClrBtn.IsEnabled = (status.logEntries > 0);
            currentLogs.Text = status.logEntries.ToString();
        }

        #endregion Functions
    }
}