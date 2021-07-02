using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Mappers;
using SystemInfo.Services;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;
using SystemInfo.Wpf.Configuration;

namespace SystemInfo.Wpf.Services {
    public class EnterpriseServiceClient {
        //TODO: Create an baseServiceClient class to handle the exceptions

        private readonly IEnterpriseService offlineEnterpriseService;
        private readonly HttpClient httpClient;
        private readonly ClientConfiguration _configuration;

        public EnterpriseServiceClient() {
            offlineEnterpriseService = OfflineBussinessServicesContainer.EnterpriseService;
            _configuration = new ClientConfiguration();
            httpClient = _configuration.GetHttpClient();
        }

        public async Task<EmptyOperationResponse> SaveEnterpriseAsync(CreateEnterpriseRequest enterpriseRequest) {
            if (!MainWindow.IsServerOnline) {
                return await SaveInLocalAsync(enterpriseRequest);
            }

            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.PostAsJsonAsync(_configuration["Api:Enterprise:Create"] , enterpriseRequest);
                if (!httpResponse.IsSuccessStatusCode) {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound) {
                        return NotFoundEndpointEmptyOperationResponse();
                    }
                    var result = await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResponse>();
                    return result;
                }
                return await httpResponse.Content.ReadFromJsonAsync<OperationResponse<EnterpriseDetails>>();

            } catch (HttpRequestException requestException) {

                if (requestException.InnerException.GetType() == typeof(SocketException)) {
                    return await SaveInLocalAsync(enterpriseRequest);
                }
                return NotFoundHostEmptyOperationResponse();
            }

        }

        private async Task<EmptyOperationResponse> SaveInLocalAsync(CreateEnterpriseRequest enterpriseRequest) {
            var offlineOperationResponse = await offlineEnterpriseService.CreateAsync(enterpriseRequest);
            offlineOperationResponse.Message = offlineOperationResponse.Message.Insert(0 , "OFFLINE: ");
            return offlineOperationResponse;
        }

        public async Task<OperationResponse<EnterpriseDetails>> GetEnterpriseAsync(string enterpriseRnc) {
            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.GetAsync(_configuration["Api:Enterprise:GetByRnc"] + $"/{enterpriseRnc}");
                
                if (!httpResponse.IsSuccessStatusCode) {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound) {
                        return NotFoundOperationResponse();
                    }

                    var result = await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResponse>();
                    return new OperationResponse<EnterpriseDetails>() {
                        IsSuccess = result.IsSuccess ,
                        Message = result.Message ,
                        Record = new EnterpriseDetails()
                    };
                }
                return await httpResponse.Content.ReadFromJsonAsync<OperationResponse<EnterpriseDetails>>();

            } catch (HttpRequestException requestException) {

                if (requestException.InnerException.GetType() == typeof(SocketException)) {
                    var offlineOperationResponse = await offlineEnterpriseService.GetByRncAsync(enterpriseRnc);
                    offlineOperationResponse.Message = offlineOperationResponse.Message.Insert(0 , "OFFLINE: ");

                    return new OperationResponse<EnterpriseDetails>() {
                        IsSuccess = offlineOperationResponse.IsSuccess ,
                        Message = offlineOperationResponse.Message ,
                        Record = offlineOperationResponse.Record.ToEnterpriseDetails()
                    };
                }
                return UnkownHostOperationResponse();
            }

        }

        private static OperationResponse<EnterpriseDetails> NotFoundOperationResponse() {
            return new OperationResponse<EnterpriseDetails>() {
                IsSuccess = false ,
                Message = "No se encontró el endpoint especificado." +
                                                  "\nConfigure lo en App.Config" ,
                Record = new EnterpriseDetails()
            };
        }

        private static OperationResponse<EnterpriseDetails> UnkownHostOperationResponse() {
            return new OperationResponse<EnterpriseDetails>() {
                IsSuccess = false ,
                Message = "El host no ha sido encontrado.\n" +
                                          "Configure lo en App.config " ,
                Record = new EnterpriseDetails()
            };
        }

        private EmptyOperationResponse NotFoundEndpointEmptyOperationResponse() {
            return new EmptyOperationResponse {
                IsSuccess = false ,
                Message = "No se encontró el endpoint especificado." +
                        "\nConfigure lo en App.Config"
            };
        }

        private EmptyOperationResponse NotFoundHostEmptyOperationResponse() {
            return new EmptyOperationResponse {
                IsSuccess = false ,
                Message = "No se encontró el host especificado." +
                        "\nConfigure lo en App.Config"
            };
        }

    }
}
