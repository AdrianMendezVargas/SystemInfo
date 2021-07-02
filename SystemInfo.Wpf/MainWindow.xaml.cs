using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SystemInfo.Models.Mappers;
using SystemInfo.Services;
using SystemInfo.Shared.Extensions;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Wpf.Data;
using SystemInfo.Wpf.Services;

namespace SystemInfo.Wpf {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public CreateSystemSpecsRequest SystemSpecsRequest { get; set; }
        public static bool IsServerOnline { get; set; }
        public bool HasPendingChanges { get; set; }

        private bool _isEditingEnterprise;
        private ComputerInfo _computerInfo;
        private SystemSpecsServiceClient _specsClient;
        private EnterpriseServiceClient _enterpriseClient;
        private ConnectionServiceClient _connectionClient;

        public MainWindow() {
            InitializeComponent();
            SystemSpecsRequest = new CreateSystemSpecsRequest();
            _computerInfo = new ComputerInfo();
            _specsClient = new SystemSpecsServiceClient();
            _enterpriseClient = new EnterpriseServiceClient();
            _connectionClient = new ConnectionServiceClient();

            DataContext = SystemSpecsRequest;
            GetSystemSpecsAsync();
            CheckConnectionAndChanges();
        }

        private async void CheckConnectionAndChanges() {
            await SetConnectionStatusAsync();
            await CheckPendingChanges();
        }

        private async Task CheckPendingChanges() {
            var offlineContext = OfflineBussinessServicesContainer.GetOfflineDbContext();
            if (await offlineContext.Enterprises.AnyAsync()) {
                HasPendingChanges = true;
            } else if (await offlineContext.SystemSpecs.AnyAsync()) {
                HasPendingChanges = true;
            } else {
                HasPendingChanges = false;
            }

            pendingChangesPanel.Visibility = HasPendingChanges
                ? Visibility.Visible
                : Visibility.Collapsed;

            syncButton.Visibility = IsServerOnline
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private async Task SetConnectionStatusAsync() {
            IsServerOnline = await IsConnectedToServer();

            offLineLabel.Visibility = IsServerOnline
                            ? Visibility.Collapsed
                            : Visibility.Visible;

            if (HasPendingChanges && IsServerOnline) {
                syncButton.Visibility = Visibility.Visible;
            }
        }

        private async Task<bool> IsConnectedToServer() {
            return await _connectionClient.IsConnectionEstablished();
        }

        private void GetSystemSpecsAsync() {
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

        private async void EditButton_Click(object sender , RoutedEventArgs e) {
            await VerifyAndSetEnterprise();
        }

        private async Task VerifyAndSetEnterprise() {
            if (_isEditingEnterprise) {
                if (!SystemSpecsRequest.EnterpriseRNC.IsRncValid()) {
                    MessageBox.Show("RNC invalido.");
                    return;
                }

                EditButton.IsEnabled = false;
                var result = await _enterpriseClient.GetEnterpriseAsync(SystemSpecsRequest.EnterpriseRNC);
                EditButton.IsEnabled = true;

                if (result.IsSuccess) {
                    EnterpriseNameTextBox.Text = result.Record?.Name;
                    SetEditingEnterprise(false);
                    return;
                }

                var answer = MessageBox.Show("Esta empresa no existe, desea crearla?" , "Error" , MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.No) {
                    EnterpriseNameTextBox.Text = string.Empty;
                    return;
                }

                if (!isValidEnterprise()) {
                    MessageBox.Show("Los datos de la empresa son inválidos.");
                    return;
                }
                var enterpriseResult = await _enterpriseClient.SaveEnterpriseAsync(new CreateEnterpriseRequest() {
                    Name = EnterpriseNameTextBox.Text.Trim() ,
                    RNC = SystemSpecsRequest.EnterpriseRNC
                });

                if (enterpriseResult.IsSuccess) {
                    SetEditingEnterprise(false);
                }

                MessageBox.Show(enterpriseResult.Message);
                return;

            } else {
                SetEditingEnterprise(true);
            }
        }

        private bool isValidEnterprise() {
            return SystemSpecsRequest.EnterpriseRNC.IsRncValid()
                   && !string.IsNullOrWhiteSpace(EnterpriseNameTextBox.Text);
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
            refreshButton.IsEnabled = false;
            GetSystemSpecsAsync();
            CheckConnectionAndChanges();
            refreshButton.IsEnabled = true;

        }

        private async void saveButton_Click(object sender , RoutedEventArgs e) {
            if (!SystemSpecsRequest.EnterpriseRNC.IsRncValid()) {
                MessageBox.Show("RNC invalido.");
                return;
            }

            saveButton.IsEnabled = false;
            var result = await _specsClient.SaveSystemSpecsAsync(SystemSpecsRequest);
            saveButton.IsEnabled = true;
            MessageBox.Show(result.Message);
        }

        private async void syncButton_Click(object sender , RoutedEventArgs e) {
            try {
                syncButton.IsEnabled = false;
                await SyncChangesAsync();
                syncButton.IsEnabled = true;
                await CheckPendingChanges();
                if (!HasPendingChanges) {
                    MessageBox.Show("Los cambios fueron sincronizados");
                } else {
                    MessageBox.Show("Algunos cambios no fueron sincronizados");
                }
            } catch (Exception) {
                syncButton.IsEnabled = true;
            }
        }

        private async Task SyncChangesAsync() {
            var offlineEnterpriseService = OfflineBussinessServicesContainer.EnterpriseService;
            var offlineSystemSpecsService = OfflineBussinessServicesContainer.SystemSpecsService;

            var offlineDbContext = OfflineBussinessServicesContainer.GetOfflineDbContext();

            var pendingEnterprises = await offlineDbContext.Enterprises
                .IgnoreAutoIncludes()
                .Include(e => e.SystemSpecs).ThenInclude(s => s.WindowsAccounts)
                .ToListAsync();

            foreach (var enterprise in pendingEnterprises) {
                var enterpriseResult = await _enterpriseClient.SaveEnterpriseAsync(enterprise.ToCreateEntepriseRequest());
                if (!enterpriseResult.IsSuccess) {
                    //TODO:Save in invalidEnterprises Table
                    continue;
                }

                foreach (var systemSpec in enterprise.SystemSpecs) {
                    var systemSpecsResult = await _specsClient.SaveSystemSpecsAsync(systemSpec.ToCreateSystemSpecRequest());
                    if (!systemSpecsResult.IsSuccess) {
                        //TODO:Save in invalidSystemSpecs Table
                        continue;
                    }
                }

                try {
                    offlineDbContext.Enterprises.Remove(enterprise);
                    await offlineDbContext.SaveChangesAsync();
                } catch (Exception) {
                    MessageBox.Show("Error al remover los cambios pendientes");
                } finally {
                    await offlineDbContext.DisposeAsync();
                }
            }
        }
    }
}
