using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
using System.Windows.Threading;
using System.Diagnostics;
namespace MonitorTool
{
    /// <summary>
    /// Interaction logic for PerfomanceMonitoring.xaml
    /// </summary>
    public partial class PerfomanceMonitoring : UserControl
    {
        DispatcherTimer Timer = new DispatcherTimer();
        PerformanceCounter CPUPerfomance = new PerformanceCounter();
        PerformanceCounter MemPerfomance = new PerformanceCounter();
        PerformanceCounter NetPerfomance = new PerformanceCounter();
        PerformanceCounter DiskPerfomance = new PerformanceCounter();
        string MachineName = System.Environment.MachineName;

        public PerfomanceMonitoring()
        {
            InitializeComponent();
            GetAllNetAdapters();
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            CPUPerfomance.CategoryName = "Processor";
            CPUPerfomance.CounterName = "% Processor Time";
            CPUPerfomance.InstanceName = "_Total";
            ///////////////////////////////////////////
            MemPerfomance.CategoryName = "Memory";
            MemPerfomance.CounterName = "% Committed Bytes In Use";
            /////////////////////////////////////////////
            NetPerfomance.CategoryName = "Network Adapter";
            NetPerfomance.CounterName = "Bytes Total/sec";
            
            ///////////////////////////////////////////////
            DiskPerfomance.CategoryName = "LogicalDisk";
            DiskPerfomance.CounterName = "% Disk Time";
            DiskPerfomance.InstanceName = "_Total";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty( NetPerfomance.InstanceName))
            {
               
                GetNetUtil();
               
            }
            GetCpuUtil();
            GetMemUtil();
            GetDiskUtil();
        }
        private void GetAllNetAdapters()
        {
            var netQuery = new SelectQuery("Win32_NetworkAdapter");
            var mgmtScope = new ManagementScope("\\\\" + MachineName + "\\root\\cimv2");
            mgmtScope.Connect();
            var mgmtSrchr = new ManagementObjectSearcher(mgmtScope, netQuery);
            foreach (var net in mgmtSrchr.Get())
            {
                NetAdaptersList.Items.Add(net.GetPropertyValue("Description").ToString());


            }
        }

        private void GetDiskUtil()
        {
            float DiskUtil = DiskPerfomance.NextValue();
            DiskUtilPB.Value = DiskUtil;
            DiskUtilTB.Text = DiskUtil.ToString();
        }
        private void GetNetUtil()
        {
            try
            {
                float NetUtil = NetPerfomance.NextValue();


                NetUtilTB.Text = NetUtil.ToString();
            }
            catch(Exception e)
            {
                NetUtilTB.Text = "UNAVAILABLE";
            }
            }
        private void GetCpuUtil()
        {
            float CPUUtil = CPUPerfomance.NextValue();
            if (CPUUtil > 65)
            {
                ProcessorUtilPB.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ProcessorUtilPB.Foreground = new SolidColorBrush(Colors.Green);
            }
            ProcessorUtilPB.Value = CPUUtil;

            CpuUtilTB.Text = CPUUtil.ToString();
        }
        private void GetMemUtil()
        {
            float MemUtil = MemPerfomance.NextValue();
            if (MemUtil > 65)
            {
                MemUtilPB.Foreground= new SolidColorBrush(Colors.Red);
            }
            else
            {
                MemUtilPB.Foreground = new SolidColorBrush(Colors.Green);
            }
            MemUtilPB.Value = MemUtil;
            MemUtilTB.Text = MemUtil.ToString();
        }

        private void NetAdaptersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NetPerfomance.InstanceName = NetAdaptersList.SelectedItem.ToString();
        }
    }
}
