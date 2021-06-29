using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Services {
    public abstract class BaseService {

        protected OperationResponse<T> Error<T>(string message , T record) {
            return new OperationResponse<T> {
                Message = message ,
                Record = record ,
                IsSuccess = false
            };
        }

        protected OperationResponse<T> Success<T>(string message , T record) {
            return new OperationResponse<T> {
                Message = message ,
                Record = record ,
                IsSuccess = true
            };
        }

    }
}
