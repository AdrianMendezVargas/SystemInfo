using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Shared.Requests {
    public class TokenRequest {
        [Required]
        public string Password { get; set; }
    }
}
