using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemInfo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Repository;
using SystemInfo.Models.Domain;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Models;

namespace SystemInfo.Services.Tests {
    [TestClass()]
    public class SystemSpecsServiceTests {

        private IUnitOfWork Unit;
        private ISystemSpecsService SystemSpecsService;

        [TestInitialize]
        public void Initialize() {
            var db = DbContextHelper.GetSeedInMemoryDbContext();
            Unit = new EfUnitOfWork(db);
            SystemSpecsService = new SystemSpecsService(Unit);

        }

        [TestMethod()]
        public async Task CreateSystemSpecs_ShouldCreateTheSpecsAndCreateTheWindowsAccounts() {
            var systemSpecRequest = new CreateSystemSpecsRequest {
                EnterpriseRNC = "123456789" ,
                IsOperatingSystem64bits = true ,
                OperatingSystemVersion = "Windows 11" ,
                MachineName = "DESKTOP-NUEVA" ,
                ProcessorCount = 4 ,
                ProcessorName = "Intel i7" ,
                TotalMemoryInGigaBytes = 8 ,
                WindowsAccounts = new List<WindowsAccountDetails>(){
                    new WindowsAccountDetails {
                        Username = "Visitante"
                    },
                    new WindowsAccountDetails {
                        Username = "Recepcion"
                    }
                }
            };

            var result = await SystemSpecsService.CreateAsync(systemSpecRequest);

            Assert.IsTrue(result.IsSuccess);

        }

        [TestMethod()]
        public async Task CreateSystemSpecs_NotExistingRnc_ShouldNotCreateTheSpecs() {
            var systemSpecRequest = new CreateSystemSpecsRequest {
                EnterpriseRNC = "999999999" ,
                IsOperatingSystem64bits = true ,
                OperatingSystemVersion = "Windows 11" ,
                MachineName = "DESKTOP-HKMAUJO" ,
                ProcessorCount = 4 ,
                ProcessorName = "Intel i7" ,
                TotalMemoryInGigaBytes = 8 ,
                WindowsAccounts = new List<WindowsAccountDetails>(){
                    new WindowsAccountDetails {
                        Username = "Visitante"
                    },
                    new WindowsAccountDetails {
                        Username = "Recepcion"
                    }
                }
            };

            var result = await SystemSpecsService.CreateAsync(systemSpecRequest);

            Assert.IsFalse(result.IsSuccess);

        }

        [TestMethod()]
        public async Task CreateSystemSpecs_ValidRncWithAlreadyExistingMachineName_ShouldNotCreateTheSpecs() {
            var systemSpecRequest = new CreateSystemSpecsRequest {
                EnterpriseRNC = "123456789" ,
                IsOperatingSystem64bits = true ,
                OperatingSystemVersion = "Windows 11" ,
                MachineName = "DESKTOP-HKMAUJO" ,
                ProcessorCount = 4 ,
                ProcessorName = "Intel i7" ,
                TotalMemoryInGigaBytes = 8 ,
                WindowsAccounts = new List<WindowsAccountDetails>(){
                    new WindowsAccountDetails {
                        Username = "Visitante"
                    },
                    new WindowsAccountDetails {
                        Username = "Recepcion"
                    }
                }
            };

            var result = await SystemSpecsService.CreateAsync(systemSpecRequest);

            Assert.IsFalse(result.IsSuccess);

        }

    }
}