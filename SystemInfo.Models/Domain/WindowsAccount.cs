using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Models.Domain {
    public class WindowsAccount {

        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public int SystemSpecsId { get; set; }
        public virtual SystemSpecs SystemSpecs { get; set; }

    }
}
