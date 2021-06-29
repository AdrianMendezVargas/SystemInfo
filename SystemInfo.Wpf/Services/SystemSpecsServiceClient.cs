using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Services;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;

namespace SystemInfo.Wpf.Services {
    public class SystemSpecsServiceClient {

        private readonly ISystemSpecsService offlineSpecService;
        private readonly IEnterpriseService offlineEnterpriseService;
        private readonly HttpClient httpClient;

        public SystemSpecsServiceClient() {
            offlineSpecService = OfflineBussinessServicesContainer.SystemSpecsService;
            offlineEnterpriseService = OfflineBussinessServicesContainer.EnterpriseService;
            httpClient = new HttpClient();
        }

        public async Task<EmptyOperationResponse> SaveSystemSpecsAsync(CreateSystemSpecsRequest specsRequest) {
            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.PostAsJsonAsync(ConfigurationManager.AppSettings["Api:CreateSystemSpecs"] , specsRequest);
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
