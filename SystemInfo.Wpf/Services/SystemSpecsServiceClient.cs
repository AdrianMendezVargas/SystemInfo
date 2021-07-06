using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Threading.Tasks;
using SystemInfo.Services;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;
using SystemInfo.Wpf.Configuration;

namespace SystemInfo.Wpf.Services {
    public class SystemSpecsServiceClient {

        private readonly ISystemSpecsService offlineSpecService;
        private readonly HttpClient httpClient;
        private readonly ClientConfiguration _configuration;

        public SystemSpecsServiceClient() {
            offlineSpecService = OfflineBussinessServicesContainer.SystemSpecsService;
            _configuration = new ClientConfiguration();
            httpClient = _configuration.GetHttpClient();
        }

        public async Task<EmptyOperationResponse> SaveSystemSpecsAsync(CreateSystemSpecsRequest specsRequest) {
            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.PostAsJsonAsync(_configuration["Api:SystemSpecs:Create"] , specsRequest);
                if (!httpResponse.IsSuccessStatusCode) {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound) {
                        return NotFoundEndpointEmptyOperationResponse();
                    }
                    var result = await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResponse>();
                    return result;
                }
                return await httpResponse.Content.ReadFromJsonAsync<OperationResponse<SystemSpecsDetails>>();
           
            } catch (HttpRequestException requestException) {

                if (requestException.InnerException.GetType() == typeof(SocketException)) {
                    var offlineOperationResponse = await offlineSpecService.CreateAsync(specsRequest);
                    offlineOperationResponse.Message = offlineOperationResponse.Message.Insert(0 , "OFFLINE: ");
                    return offlineOperationResponse;
                }
                return NotFoundHostEmptyOperationResponse();
            }        
        
        }

        private EmptyOperationResponse NotFoundEndpointEmptyOperationResponse() {
            return new EmptyOperationResponse {
                OperationResult = ServiceResult.Unknown ,
                Message = "No se encontró el endpoint especificado." +
                        "\nConfigure lo en App.Config"
            };
        }

        private EmptyOperationResponse NotFoundHostEmptyOperationResponse() {
            return new EmptyOperationResponse {
                OperationResult = ServiceResult.Unknown ,
                Message = "No se encontró el host especificado." +
                        "\nConfigure lo en App.Config"
            };
        }
    }
}
