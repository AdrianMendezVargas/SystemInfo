using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SystemInfo.Shared.Requests {
    public class CreateEnterpriseRequest {
        [Required]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "El formato del RNC es incorrecto")]
        public string RNC { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsValid() {
            if (!Regex.IsMatch(RNC, "^[0-9]{9}$")) {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Name)) {
                return false;
            }
            return true;
        }
    }
}
