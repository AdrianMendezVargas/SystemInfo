﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SystemInfo.Models.Domain;
using SystemInfo.Models.Mappers;
using SystemInfo.Services;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Extensions;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;
using SystemInfo.Wpf.Configuration;
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
        private AuthenticationServiceClient _authenticationClient;
        private PreferencesService _preferencesService;

        public MainWindow() {
            InitializeComponent();
            SystemSpecsRequest = new CreateSystemSpecsRequest();
            _computerInfo = new ComputerInfo();

            InitServices();
            DataContext = SystemSpecsRequest;
            GetSystemSpecsAsync();
            InitAsyncTasks();
        }

        private async void InitAsyncTasks() {
            await SetTokenAsync();
            await SetConnectionStatusAsync();
            await CheckPendingChangesAsync();
        }

        private void InitServices() {
            _authenticationClient = new AuthenticationServiceClient();
            _connectionClient = new ConnectionServiceClient();
            _preferencesService = OfflineBussinessServicesContainer.PreferencesService;
            
            _specsClient = new SystemSpecsServiceClient();
            _enterpriseClient = new EnterpriseServiceClient();
        }

        public async Task<bool> SetTokenAsync() {
            string exprirationDateString = (await _preferencesService.Get(PreferencesKeys.TokenExpiration))?.Value;
            if (!string.IsNullOrWhiteSpace(exprirationDateString)) {

                DateTime expirationDate = Convert.ToDateTime(exprirationDateString);

                if (expirationDate > DateTime.UtcNow) {  //There is not need to request a new token
                    bool tokenReady = await UseLocalToken(expirationDate);
                    if (tokenReady) {
                        return tokenReady;
                    } else {
                        return await RequestNewToken();
                    }
                }
            }

            return await RequestNewToken();
        }

        private async Task<bool> RequestNewToken() {
            await SetConnectionStatusAsync();
            var result = await _authenticationClient.RequestToken();
            if (result.OperationResult == ServiceResult.Success) {
                return await SaveTokenAndUpdateServices(result.Record);

            }
            return false;
        }

        private async Task<bool> UseLocalToken(DateTime expirationDate) {
            string token = (await _preferencesService.Get(PreferencesKeys.Token))?.Value;
            ConfigurationManager.AppSettings["Api:Token"] = token ?? "";
            return token != null;
        }

        private async Task<bool> SaveTokenAndUpdateServices(TokenResponse tokenResponse) {

            bool tokenSaved = await _preferencesService.Save(new PreferencesKeyValues() {
                Key = PreferencesKeys.Token ,
                Value = tokenResponse.Token
            });

            bool tokenExpiratioinSaved = await _preferencesService.Save(new PreferencesKeyValues() {
                Key = PreferencesKeys.TokenExpiration ,
                Value = tokenResponse.ExpirationDate.ToString("G")
            });

            ConfigurationManager.AppSettings["Api:Token"] = tokenResponse.Token;
            InitServices();
            return tokenSaved && tokenExpiratioinSaved;
        }

        private async Task CheckPendingChangesAsync() {
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
            SystemSpecsRequest.HardDisks = GetHardDisks();
            using (var mos = new ManagementObjectSearcher("Root\\CIMV2" , "")) {
                SystemSpecsRequest.ProcessorName = GetCpuName(mos);
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

        private List<HardDiskDetails> GetHardDisks() {
            var computer = new Computer();
            var hardDiskList = new List<HardDiskDetails>();

            foreach (var drive in computer.FileSystem.Drives) {
                if (drive.IsReady) {
                    hardDiskList.Add(new HardDiskDetails() {
                        Label = drive.Name ,
                        FreeSpaceInGigabytes = GetAvailableFreeSpaceInGigaBytes(drive) ,
                        SizeInGigabytes = GetTotalSizeInGigaBytes(drive)
                    });
                }
            }

            int GetTotalSizeInGigaBytes(System.IO.DriveInfo drive) {
                return Convert.ToInt32(drive.TotalSize / 1024L / 1024L / 1024L);
            }

            int GetAvailableFreeSpaceInGigaBytes(System.IO.DriveInfo drive) {
                return Convert.ToInt32(drive.AvailableFreeSpace / 1024L / 1024L / 1024L);
            }

            return hardDiskList;
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
                await SetTokenAsync();
                var result = await _enterpriseClient.GetEnterpriseAsync(SystemSpecsRequest.EnterpriseRNC);
                EditButton.IsEnabled = true;

                if (result.OperationResult == ServiceResult.Success) {
                    EnterpriseNameTextBox.Text = result.Record?.Name;
                    SetEditingEnterprise(false);
                    return;
                }

                if (result.OperationResult == ServiceResult.Unauthorized) {
                    EnterpriseNameTextBox.Text = string.Empty;
                    EnterpriseRncTextBox.Text = string.Empty;
                    SetEditingEnterprise(false);
                    MessageBox.Show(result.Message);
                    return;
                }

                if (result.OperationResult == ServiceResult.NotFound) {
                    var answer = MessageBox.Show("Esta empresa no existe, desea crearla?" , "Error" , MessageBoxButton.YesNo);
                    if (answer == MessageBoxResult.No) {
                        EnterpriseNameTextBox.Text = string.Empty;
                        return;
                    }

                    if (!isValidEnterprise()) {
                        MessageBox.Show("Introduzca un nombre para la empresa y vuelva a intentarlo.");
                        return;
                    }
                    var enterpriseResult = await _enterpriseClient.SaveEnterpriseAsync(new CreateEnterpriseRequest() {
                        Name = EnterpriseNameTextBox.Text.Trim() ,
                        RNC = SystemSpecsRequest.EnterpriseRNC
                    });

                    if (enterpriseResult.OperationResult == ServiceResult.Success) {
                        SetEditingEnterprise(false);
                    }

                    MessageBox.Show(enterpriseResult.Message);
                    return;
                }

                MessageBox.Show(result.Message);

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
            editStackpanel.Visibility = state ? Visibility.Collapsed : Visibility.Visible;
            acceptStackpanel.Visibility = !state ? Visibility.Collapsed : Visibility.Visible;
            EnterpriseNameTextBox.IsEnabled = state;
            EnterpriseRncTextBox.IsEnabled = state;
            refreshButton.IsEnabled = !state;
            saveButton.IsEnabled = !state;
        }

        private async void refreshButton_Click(object sender , RoutedEventArgs e) {
            refreshButton.IsEnabled = false;
            GetSystemSpecsAsync();
            await CheckPendingChangesAsync();
            await SetConnectionStatusAsync();
            refreshButton.IsEnabled = true;

        }

        private async void saveButton_Click(object sender , RoutedEventArgs e) {
            if (!SystemSpecsRequest.EnterpriseRNC.IsRncValid()) {
                MessageBox.Show("RNC invalido.");
                return;
            }

            saveButton.IsEnabled = false;
            await SetTokenAsync();
            var result = await _specsClient.SaveSystemSpecsAsync(SystemSpecsRequest);
            saveButton.IsEnabled = true;
            MessageBox.Show(result.Message);
        }

        private async void syncButton_Click(object sender , RoutedEventArgs e) {
            try {
                syncButton.IsEnabled = false;
                await SyncChangesAsync();
                syncButton.IsEnabled = true;
                await CheckPendingChangesAsync();
                if (!HasPendingChanges) {
                    MessageBox.Show("Los cambios fueron sincronizados");
                } else {
                    MessageBox.Show("Algunos cambios no fueron sincronizados. \nRevise la conexión a Internet y la contraseña del api.");
                }
            } catch (Exception) {
                syncButton.IsEnabled = true;
            }
        }

        private async Task SyncChangesAsync() {
            var readyToRemoveEnterprises = new List<Enterprise>();

            var offlineEnterpriseService = OfflineBussinessServicesContainer.EnterpriseService;
            var offlineSystemSpecsService = OfflineBussinessServicesContainer.SystemSpecsService;

            var offlineDbContext = OfflineBussinessServicesContainer.GetOfflineDbContext();

            var pendingEnterprises = await offlineDbContext.Enterprises
                .IgnoreAutoIncludes()
                .Include(e => e.SystemSpecs).ThenInclude(s => s.HardDisks)
                .ToListAsync();

            foreach (var enterprise in pendingEnterprises) {
                var enterpriseResult = await _enterpriseClient.SaveEnterpriseAsync(enterprise.ToCreateEntepriseRequest());
                if (enterpriseResult.OperationResult == ServiceResult.InvalidData
                    || enterpriseResult.OperationResult == ServiceResult.Unknown) {
                    //TODO:Save in invalidEnterprises Table
                    continue;
                }

                foreach (var systemSpec in enterprise.SystemSpecs) {
                    var systemSpecsResult = await _specsClient.SaveSystemSpecsAsync(systemSpec.ToCreateSystemSpecRequest());
                    if (enterpriseResult.OperationResult == ServiceResult.InvalidData
                    || enterpriseResult.OperationResult == ServiceResult.Unknown) {
                        //TODO:Save in invalidSystemSpecs Table
                        continue;
                    }
                }

                if (enterpriseResult.OperationResult == ServiceResult.Success) {
                    readyToRemoveEnterprises.Add(enterprise);
                }
            }

            try {
                offlineDbContext.Enterprises.RemoveRange(readyToRemoveEnterprises);
                await offlineDbContext.SaveChangesAsync();
            } catch (Exception) {
                MessageBox.Show("Error al remover los cambios pendientes");
            } finally {
                await offlineDbContext.DisposeAsync();
            }
        }
    }
}
