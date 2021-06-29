using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Shared.Models;

namespace SystemInfo.Shared.Requests {
    public class CreateSystemSpecsRequest {
        [Required]
        public string MachineName { get; set; }

        [Required]
        public string OperatingSystemVersion { get; set; }

        [Required]
        public bool IsOperatingSystem64bits { get; set; }

        [Required]
        public int TotalMemoryInGigaBytes { get; set; }

        [Required]
        public string ProcessorName { get; set; }

        public int ProcessorCount { get; set; }

        [Required]
        [RegularExpression("^[0-9]{9}$")]
        public string EnterpriseRNC { get; set; }

        public List<WindowsAccountDetails> WindowsAccounts { get; set; }
    }
}
