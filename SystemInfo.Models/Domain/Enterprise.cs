using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Models.Domain {
    public class Enterprise : Record {

        [Key]
        public string RNC { get; set; }
        public string Name { get; set; }

        public virtual List<SystemSpecs> SystemSpecs { get; set; }
    }
}
