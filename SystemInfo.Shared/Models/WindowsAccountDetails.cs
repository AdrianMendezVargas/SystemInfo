using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Shared.Models {
    public class WindowsAccountDetails {

        [Required]
        public string Username { get; set; }
    }
}
