using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;

namespace SystemInfo.Models.Mappers {
    public static class SystemSpecsMapper {
        public static SystemSpecs ToSystemSpecs(this CreateSystemSpecsRequest saveRequest) {

            var systemSpec = new SystemSpecs() {
                EnterpriseRNC = saveRequest.EnterpriseRNC ,
                IsOperatingSystem64bits = saveRequest.IsOperatingSystem64bits ,
                OperatingSystemVersion = saveRequest.OperatingSystemVersion ,
                MachineName = saveRequest.MachineName ,
                ProcessorCount = saveRequest.ProcessorCount ,
                ProcessorName = saveRequest.ProcessorName ,
                TotalMemoryInGigaBytes = saveRequest.TotalMemoryInGigaBytes ,
            };

            saveRequest.WindowsAccounts.ForEach(a => {
                systemSpec.WindowsAccounts.Add(new WindowsAccount() { Username = a.Username});
            });

            return systemSpec;
        }

        public static SystemSpecsDetails ToSystemSpecsDetails(this SystemSpecs systemSpecs) {

            var systemSpecsDetails = new SystemSpecsDetails() {
                EnterpriseRNC = systemSpecs.EnterpriseRNC ,
                IsOperatingSystem64bits = systemSpecs.IsOperatingSystem64bits ,
                OperatingSystemVersion = systemSpecs.OperatingSystemVersion ,
                MachineName = systemSpecs.MachineName ,
                ProcessorCount = systemSpecs.ProcessorCount ,
                ProcessorName = systemSpecs.ProcessorName ,
                TotalMemoryInGigaBytes = systemSpecs.TotalMemoryInGigaBytes ,
            };

            systemSpecs.WindowsAccounts.ForEach(a => {
                systemSpecsDetails.WindowsAccounts.Add(a.ToWindowsAccountDetails());
            });

            return systemSpecsDetails;
        }



    }
}
