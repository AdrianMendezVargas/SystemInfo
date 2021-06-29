using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Models.Domain {
    public class SystemSpecs : Record {

        [Key]
        public int Id { get; set; }
        public string MachineName { get; set; }
        public string OperatingSystemVersion { get; set; }
        public bool IsOperatingSystem64bits { get; set; }
        public int TotalMemoryInGigaBytes { get; set; }
        public string ProcessorName { get; set; }
        public int ProcessorCount { get; set; }
        public string EnterpriseRNC { get; set; }
        public virtual Enterprise Enterprise { get; set; }

        public virtual List<WindowsAccount> WindowsAccounts { get; set; } = new List<WindowsAccount>();

    }
}
