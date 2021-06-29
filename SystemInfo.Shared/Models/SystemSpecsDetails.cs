using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Shared.Models {
    public class SystemSpecsDetails {
        public string MachineName { get; set; }

        public string OperatingSystemVersion { get; set; }

        public bool IsOperatingSystem64bits { get; set; }

        public int TotalMemoryInGigaBytes { get; set; }

        public string ProcessorName { get; set; }

        public int ProcessorCount { get; set; }

        public string EnterpriseRNC { get; set; }

        public List<WindowsAccountDetails> WindowsAccounts { get; set; } = new List<WindowsAccountDetails>();
    }
}
