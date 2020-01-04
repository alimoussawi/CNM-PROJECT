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

namespace MonitorTool
{
    /// <summary>
    /// Interaction logic for SystemHardware.xaml
    /// </summary>
    public partial class SystemHardware : UserControl
    {
        string MachineName = System.Environment.MachineName;

        public SystemHardware()
        {
            InitializeComponent();
            GetCpuInfo();
            GetAllNetAdapters();
            GetBoardInfo();
            GetFanInfo();
            GetBatteryInfo();
            GetDiskInfo();
            GetControllerInfo();
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

        private void GetCpuInfo()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_ComputerSystem");
            var providers = wmi.GetInstances();
            foreach(var provider in providers)
            {
                string SystemFamily = provider["SystemFamily"].ToString();
                string SystemType = provider["SystemType"].ToString();
                string ComModel = provider["Model"].ToString();
                CpuFamily.Text = SystemFamily;
                CpuType.Text = SystemType;
                ComputerModel.Text = ComModel;
            }
        }
        private void GetBoardInfo()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_Processor");
            var providers = wmi.GetInstances();
            foreach (var provider in providers)
            {
                string CpuInfo = provider["Name"].ToString();
                string Manufac = provider["Manufacturer"].ToString();
                int CoreNumber = Convert.ToInt32(provider["NumberOfCores"]);
                CpuFullInfo.Text = CpuInfo;
                Manufacturer.Text = Manufac;
                NumberOfCores.Text=CoreNumber.ToString();
            }
        }



        private void GetFanInfo()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_Fan");
            var providers = wmi.GetInstances();
            foreach (var provider in providers)
            {
                string FanSts = provider["Status"].ToString();
                FanStatus.Text = FanSts;
              
            }

        }
        private void GetBatteryInfo()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_Battery");
            var providers = wmi.GetInstances();
            foreach (var provider in providers)
            {
                int BatterySts =Convert.ToInt16(provider["BatteryStatus"]);
                string sts;
                switch (BatterySts)
                {
                    case 1:
                        sts = "discharging";
                        break;
                    case 2:
                        sts = "unknown";
                        break;
                    case 3:
                        sts = "full";
                        break;
                    case 4:
                        sts = "low";
                        break;
                    case 5:
                        sts = "critical";
                        break;
                    case 6:
                        sts = "charging and high";
                        break;
                    case 7:
                        sts = "charging and low";
                        break;
                    default:
                        sts = "not available";
                            break;
                }

                BatteryStatus.Text = sts;

            }

        }
        private void GetControllerInfo()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_USBController");
            var providers = wmi.GetInstances();
            foreach (var provider in providers)
            {
                string Name = provider["Name"].ToString();
                AllUsbPortsLB.Items.Add(Name);
            }
                
        }

        private void GetDiskInfo()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_DiskDrive");
            var providers = wmi.GetInstances();
            foreach (var provider in providers)
            {
                string Name = provider["Caption"].ToString();
                string Type = provider["MediaType"].ToString();
                long Size = Convert.ToInt64(provider["Size"])/1000000000;
                string sts = provider["Status"].ToString();
                string BPS = provider["BytesPerSector"].ToString()+" "+"Bytes";
                DiskName.Text = Name;
                DiskType.Text = Type;
                DiskSize.Text = Size.ToString()+" "+"GB";
                DiskSatus.Text = sts;
                BytePerSector.Text = BPS;
            }

                
        }

    }
}

