using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Domain;
using SystemInfo.Shared.Models;

namespace SystemInfo.Models.Mappers {
    public static class HardDisktMapper {

        public static HardDiskDetails ToHardDiskDetails(this HardDisk hardDisk) {
            return new HardDiskDetails() {
                Label = hardDisk.Label,
                FreeSpaceInGigabytes = hardDisk.FreeSpaceInGigabytes,
                SizeInGigabytes = hardDisk.SizeInGigabytes
            };
        }

        public static HardDisk ToHardDisk(this HardDiskDetails hardDiskDetails) {
            return new HardDisk() {
                Label = hardDiskDetails.Label,
                FreeSpaceInGigabytes = hardDiskDetails.FreeSpaceInGigabytes ,
                SizeInGigabytes = hardDiskDetails.SizeInGigabytes
            };
        }

    }
}
