using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
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

namespace MonitorTool
{
    /// <summary>
    /// Interaction logic for SystemSoftware.xaml
    /// </summary>
    public partial class SystemSoftware : UserControl
    {
        string MachineName = System.Environment.MachineName;
        public SystemSoftware()
        {
           
            InitializeComponent();
        
             Thread SoftwaresThread = new Thread(GetAllSoft);
             GetOSInfo();
            GetServices();
            SoftwaresThread.IsBackground = true;
            SoftwaresThread.Start();
        }

       

        private void GetOSInfo()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_OperatingSystem");
            var providers = wmi.GetInstances();
            foreach (var provider in providers)
            {
                string OsName =provider["Caption"].ToString();
                string OsVersion = provider["Version"].ToString();
                int OsUsers = Convert.ToInt32(provider["NumberOfUsers"]);
                string SysDir= provider["SystemDirectory"].ToString();
                OSNameTB.Text = OsName;
                OSVersionTB.Text = OsVersion;
                OSUsersTB.Text = OsUsers.ToString();
                SysDirTB.Text = SysDir;
            }

        }
       private void GetAllSoft()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_Product");
            var providers = wmi.GetInstances();

            foreach (var provider in providers)
            {

                this.Dispatcher.Invoke(() =>
                {
                    AllSoftLB.Items.Add(provider["Name"].ToString());

                });


            }
            this.Dispatcher.Invoke(() =>
            {
                LoadingLabel.Visibility = Visibility.Collapsed;
            });
        }


        private void GetServices()
        {
            System.Management.ManagementClass wmi = new System.Management.ManagementClass("Win32_Service");
            var providers = wmi.GetInstances();

            foreach (var provider in providers)
            {
              string Name= provider["Name"].ToString();
                AllServicesLB.Items.Add(Name);
            }
        }
    }
}
