using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Wpf.Data {
    public class PreferencesKeyValues {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
