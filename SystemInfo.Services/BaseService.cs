using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Services {
    public abstract class BaseService {

        protected OperationResponse<T> Error<T>(string message , T record, ServiceResult serviceResult) {
            return new OperationResponse<T> {
                Message = message ,
                Record = record ,
                OperationResult = serviceResult
            };
        }

        protected OperationResponse<T> Success<T>(string message , T record) {
            return new OperationResponse<T> {
                Message = message ,
                Record = record ,
                OperationResult = ServiceResult.Success
            };
        }

    }
}
