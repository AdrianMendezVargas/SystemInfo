using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Shared.Enums;

namespace SystemInfo.Shared.Responses {
    public class EmptyOperationResponse {
        public string Message { get; set; }
        public ServiceResult OperationResult { get; set; }
    }
}
