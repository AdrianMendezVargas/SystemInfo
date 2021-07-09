using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Wpf.Services {
    public class BaseServiceClient {

        protected OperationResponse<T> NoServerConnectionOperationResponse<T>() {
            return new OperationResponse<T> {
                OperationResult = ServiceResult.Unknown ,
                Message = "No se pudo contactar el servidor" ,
                Record = default
            };
        }

        protected OperationResponse<T> InternalServerErrorOperationResponse<T>() {
            return new OperationResponse<T> {
                OperationResult = ServiceResult.Unknown ,
                Message = "Ah ocurrido un error en el servidor" ,
                Record = default
            };
        }

        protected OperationResponse<T> UnkownHostOperationResponse<T>() {
            return new OperationResponse<T>() {
                OperationResult = ServiceResult.Unknown ,
                Message = "El host no ha sido encontrado.\n" +
                          "Configure lo en App.config " ,
                Record = default
            };
        }

        protected OperationResponse<T> NotFoundEndpointOperationResponse<T>() {
            return new OperationResponse<T> {
                OperationResult = ServiceResult.Unknown ,
                Message = "No se encontró el endpoint especificado." +
                          "\nConfigure lo en App.Config" ,
                Record = default
            };
        }

        protected OperationResponse<T> UnauthorizedOperationResponse<T>() {
            return new OperationResponse<T> {
                OperationResult = ServiceResult.Unauthorized ,
                Message = "No esta autorizado para usar el servidor." +
                          "\nConfigure la contraseña en App.Config" ,
                Record = default
            };
        }

    }
}
