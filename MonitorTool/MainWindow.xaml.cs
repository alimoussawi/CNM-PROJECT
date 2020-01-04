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
using System.Management;
using System.Windows.Threading;

namespace MonitorTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer ClockTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            ClockTimer.Tick += new EventHandler(Timer_Tick);
            ClockTimer.Interval = new TimeSpan(0, 0, 0);
            ClockTimer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            CurrentTimeTextBox.Text = date.ToLongTimeString().ToString() + "  " + date.ToShortDateString().ToString(); 
        }

        private void SystemHardwareButton_Click(object sender, RoutedEventArgs e)
        {
            ActivateItem.Content = new SystemHardware();
        }

        private void SystemSoftwareButton_Click(object sender, RoutedEventArgs e)
        {
            ActivateItem.Content = new SystemSoftware();
        }

        private void PerfomanceMonitoringButton_Click(object sender, RoutedEventArgs e)
        {
            ActivateItem.Content = new PerfomanceMonitoring();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
