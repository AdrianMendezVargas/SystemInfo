using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Management;

namespace SystemInfo {
    class Program {

        static void Main(string[] args) {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var appSettings = ConfigurationManager.AppSettings;
            Console.WriteLine("RNC: " + appSettings["RNC"]);

            Console.WriteLine("Current user: " + Environment.UserName);
            Console.WriteLine("User domain name: " + Environment.UserDomainName);
            Console.WriteLine("Machine name: " + Environment.MachineName);
            Console.WriteLine("Current directory: " + Environment.CurrentDirectory);

            Console.WriteLine("OS Version: " + GetOSFullname());
            string OSArchitecture = Environment.Is64BitOperatingSystem ? "64Bits" : "32Bits";
            Console.WriteLine("OS Architecture: " + OSArchitecture);

            Console.WriteLine("RAM: " + GetTotalMemoryInGigaBytes() + "GB");

            Console.WriteLine("Processor count: " + Environment.ProcessorCount);
            //Console.WriteLine("System directory: " + Environment.SystemDirectory);
            //Console.WriteLine("Stack trace: " + Environment.StackTrace);
            //Console.WriteLine("System memory page: " + Environment.SystemPageSize + "Bytes");
            using (var mos = new ManagementObjectSearcher("Root\\CIMV2" , "")) {
                Console.WriteLine("CPU: " + GetCpuInfo(mos));
                Console.WriteLine("\nActive user accounts:");
                GetActiveUserAccounts(mos).ForEach((account) => {
                    Console.WriteLine("\t" + account);
                });
            }
            PrintDiskInfo();

            stopWatch.Stop();
            Console.WriteLine("\n\nElapsed time: " + stopWatch.Elapsed.TotalMilliseconds + "ms");
            Console.ReadKey();
        }       
        static void PrintDiskInfo() {
            var computer = new Microsoft.VisualBasic.Devices.Computer();
            foreach (var drive in computer.FileSystem.Drives) {
                if (drive.IsReady) {
                    Console.WriteLine("\nDisk info:");
                    Console.WriteLine("\tName: " + drive.Name);
                    Console.WriteLine("\t\tTotalSize: {0}GB",GetTotalSizeInGigaBytes(drive));
                    Console.WriteLine("\t\tFreeSpace: {0}GB",GetAvailableFreeSpaceInGigaBytes(drive));
                }
            }

            long GetTotalSizeInGigaBytes(System.IO.DriveInfo drive) {
                return drive.TotalSize / 1024L / 1024L / 1024L;
            }

            long GetAvailableFreeSpaceInGigaBytes(System.IO.DriveInfo drive) {
                return drive.AvailableFreeSpace / 1024L / 1024L / 1024L;
            }
        }

        static int GetTotalMemoryInGigaBytes() {
            return Convert.ToInt32(Math.Ceiling(new ComputerInfo().TotalPhysicalMemory / 1024m / 1024m / 1024m));
        }
        static string GetOSPlatform() {     
            return new ComputerInfo().OSPlatform;
        }
        static string GetOSVersion() {
            return new ComputerInfo().OSVersion;
        }
        static string GetOSFullname() {
            return new ComputerInfo().OSFullName;
        }
        private static string GetCpuInfo(ManagementObjectSearcher mos) {
            mos.Query = new ObjectQuery("SELECT Name FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get()) {
                try {
                    return mo.Properties["Name"].Value.ToString();
                } catch (Exception) { }
            }
            return "unknown";
        }
        private static List<string> GetActiveUserAccounts(ManagementObjectSearcher mos) {
            var activeAccounts = new List<string>();
            mos.Query = new ObjectQuery("SELECT Name FROM Win32_UserAccount WHERE Disabled = false");
            foreach (ManagementObject mo in mos.Get()) {
                try {
                    var nameProperty = mo.Properties["Name"];
                    activeAccounts.Add(nameProperty.Value.ToString());

                } catch (Exception) {}
            }
            return activeAccounts;
        }
    }
}
