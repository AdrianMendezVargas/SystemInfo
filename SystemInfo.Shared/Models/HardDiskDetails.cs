using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Shared.Models {
    public class HardDiskDetails {
        public string Label { get; set; }
        public int SizeInGigabytes { get; set; }
        public int FreeSpaceInGigabytes { get; set; }
    }
}
