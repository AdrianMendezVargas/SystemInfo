using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Shared.Enums {
    public enum ServiceResult {
        Success = 0,
        AlreadyExist = 1,
        InvalidData = 2,
        Unknown = 3,
        Unauthorized = 4,
        NotFound = 5
    }
}
