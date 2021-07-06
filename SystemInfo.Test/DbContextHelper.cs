using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using SystemInfo.Models.Data;
using SystemInfo.Models.Domain;

namespace SystemInfo.Services.Tests {
    public static class DbContextHelper {
        public static ApplicationDbContext GetSeedInMemoryDbContext() {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var context = new ApplicationDbContext(options);

            #region creating users
            var enterprises = new Enterprise[] {
                new Enterprise{
                   RNC = "123456789",
                   Name = "Soluciones Almonte Gil",
                   CreatedOn = DateTime.Now
                },
                new Enterprise{
                   RNC = "987654321",
                   Name = "Universidad Catolica Nordestana",
                   CreatedOn = DateTime.Now.AddDays(-4)
                }
            };
            #endregion

            #region creating tasks
            var systemSpecs = new SystemSpecs[] {
                new SystemSpecs {
                    Id = 1,
                    EnterpriseRNC = "123456789",
                    IsOperatingSystem64bits = true,
                    OperatingSystemVersion = "Windows 11",
                    MachineName = "DESKTOP-HKMAUJO",
                    ProcessorCount = 4,
                    ProcessorName = "Intel i7",
                    TotalMemoryInGigaBytes = 8,
                    HardDisks = new List<HardDisk>(){
                        new HardDisk {
                            Id = 1,
                            Label = "Visitante"
                        },
                        new HardDisk {
                            Id = 2,
                            Label = "Recepcion"
                        }
                    },
                    CreatedOn = DateTime.UtcNow,
                },
                new SystemSpecs {
                    Id = 2,
                    EnterpriseRNC = "123456789",
                    IsOperatingSystem64bits = true,
                    OperatingSystemVersion = "Windows 10",
                    MachineName = "DESKTOP-DEV2",
                    ProcessorCount = 4,
                    ProcessorName = "Intel i9",
                    TotalMemoryInGigaBytes = 16,
                    HardDisks = new List<HardDisk>(){
                        new HardDisk {
                            Id = 3,
                            Label = "Guest"
                        },
                        new HardDisk {
                            Id = 4,
                            Label = "Admin"
                        }
                    },
                    CreatedOn = DateTime.UtcNow,
                },
                new SystemSpecs {
                    Id = 3,
                    EnterpriseRNC = "987654321",
                    IsOperatingSystem64bits = true,
                    OperatingSystemVersion = "Windows 10",
                    MachineName = "DESKTOP-UCNE",
                    ProcessorCount = 2,
                    ProcessorName = "Intel i3",
                    TotalMemoryInGigaBytes = 8,
                    HardDisks = new List<HardDisk>(){
                        new HardDisk {
                            Id = 5,
                            Label = "Administracion"
                        },
                        new HardDisk {
                            Id = 6,
                            Label = "Caja"
                        }
                    },
                    CreatedOn = DateTime.UtcNow
                }
            };
            #endregion


            context.Enterprises.AddRange(enterprises);
            context.SystemSpecs.AddRange(systemSpecs);

            context.SaveChanges();

            return context;
        }
    }
}
