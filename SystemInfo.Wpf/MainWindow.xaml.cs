using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using SystemInfo.Services;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Wpf.Services;

namespace SystemInfo.Wpf {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public CreateSystemSpecsRequest SystemSpecsRequest { get; set; }

        private bool _isEditingEnterprise;
        private ComputerInfo _computerInfo;
        private SystemSpecsServiceClient _specsClient;

        public MainWindow() {
            InitializeComponent();
            SystemSpecsRequest = new CreateSystemSpecsRequest();
            _computerInfo = new ComputerInfo();
            _specsClient = new SystemSpecsServiceClient();

            this.DataContext = SystemSpecsRequest;
            GetSystemSpecs();
        }

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);
        }

        private void GetSystemSpecs() {
            SystemSpecsRequest.MachineName = Environment.MachineName;
            SystemSpecsRequest.OperatingSystemVersion = GetOSFullname();
            SystemSpecsRequest.IsOperatingSystem64bits = Environment.Is64BitOperatingSystem;
            SystemSpecsRequest.TotalMemoryInGigaBytes = GetTotalMemoryInGigaBytes();
            SystemSpecsRequest.ProcessorCount = Environment.ProcessorCount;

            using (var mos = new ManagementObjectSearcher("Root\\CIMV2" , "")) {
                SystemSpecsRequest.ProcessorName = GetCpuName(mos);
                SystemSpecsRequest.WindowsAccounts = GetActiveUserAccounts(mos);
            }
            RefreshDataContext();
        }

        private void RefreshDataContext() {
            DataContext = null;
            DataContext = SystemSpecsRequest;
            osArchTextbox.Text = SystemSpecsRequest.IsOperatingSystem64bits ? "x64" : "x32";
        }

        string GetOSFullname() {
            return _computerInfo.OSFullName;
        }

        int GetTotalMemoryInGigaBytes() {
            return Convert.ToInt32(Math.Ceiling(_computerInfo.TotalPhysicalMemory / 1024m / 1024m / 1024m));
        }

        private string GetCpuName(ManagementObjectSearcher mos) {
            mos.Query = new ObjectQuery("SELECT Name FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get()) {
                try {
                    return mo.Properties["Name"].Value.ToString();
                } catch (Exception) { }
            }
            return "unknown";
        }

        private List<WindowsAccountDetails> GetActiveUserAccounts(ManagementObjectSearcher mos) {
            var activeAccounts = new List<WindowsAccountDetails>();
            mos.Query = new ObjectQuery("SELECT Name FROM Win32_UserAccount WHERE Disabled = false");
            foreach (ManagementObject mo in mos.Get()) {
                try {
                    activeAccounts.Add(new WindowsAccountDetails() {
                        Username = mo.GetPropertyValue("Name").ToString()
                });

                } catch (Exception) { }
            }
            return activeAccounts;
        }

        private void EditButton_Click(object sender , RoutedEventArgs e) {
            if (_isEditingEnterprise) {
                if (!IsRncValid()) {
                    MessageBox.Show("RNC invalido.");
                    return;
                }
                SetEditingEnterprise(false);
            } else {
                SetEditingEnterprise(true);
            }

        }

        private bool IsRncValid() {
            return Regex.IsMatch(EnterpriseRncTextBox.Text , "^[0-9]{9}$");
        }

        private void SetEditingEnterprise(bool state) {
            _isEditingEnterprise = state;
            EditButton.Content = state ? "Accept" : "Edit";
            EnterpriseNameTextBox.IsEnabled = state;
            EnterpriseRncTextBox.IsEnabled = state;
            refreshButton.IsEnabled = !state;
            saveButton.IsEnabled = !state;
        }

        private void refreshButton_Click(object sender , RoutedEventArgs e) {
            GetSystemSpecs();
        }

        private async void saveButton_Click(object sender , RoutedEventArgs e) {
            if (!IsRncValid()) {
                MessageBox.Show("RNC invalido.");
                return;
            }

            saveButton.IsEnabled = false;
            var result = await _specsClient.SaveSystemSpecsAsync(SystemSpecsRequest);
            saveButton.IsEnabled = true;
            MessageBox.Show(result.Message);
        }
    }
}
