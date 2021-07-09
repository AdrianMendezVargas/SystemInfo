using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Models.Mappers;
using SystemInfo.Services;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Models;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;
using SystemInfo.Wpf.Configuration;

namespace SystemInfo.Wpf.Services {
    public class EnterpriseServiceClient : BaseServiceClient {
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
                return await SaveEnterpriseLocalAsync(enterpriseRequest);
            }

            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.PostAsJsonAsync(_configuration["Api:Enterprise:Create"] , enterpriseRequest);
                if (!httpResponse.IsSuccessStatusCode) {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound) {
                        return NotFoundEndpointOperationResponse<EmptyOperationResponse>();
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.InternalServerError) {
                        return InternalServerErrorOperationResponse<EnterpriseDetails>();
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.Unauthorized) {
                        return UnauthorizedOperationResponse<EnterpriseDetails>();
                    }
                    var result = await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResponse>();
                    return result;
                }
                return await httpResponse.Content.ReadFromJsonAsync<OperationResponse<EnterpriseDetails>>();

            } catch (HttpRequestException requestException) {

                if (requestException.InnerException.GetType() == typeof(SocketException)) {
                    return await SaveEnterpriseLocalAsync(enterpriseRequest);
                }
                return UnkownHostOperationResponse<EmptyOperationResponse>();
            }

        }

        public async Task<OperationResponse<EnterpriseDetails>> GetEnterpriseAsync(string enterpriseRnc) {
            if (!MainWindow.IsServerOnline) {
                return await GetEnterpriseFromLocalAsync(enterpriseRnc);
            }

            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.GetAsync(_configuration["Api:Enterprise:GetByRnc"] + $"/{enterpriseRnc}");

                if (!httpResponse.IsSuccessStatusCode) {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound) {
                        return NotFoundEndpointOperationResponse<EnterpriseDetails>();
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.InternalServerError) {
                        return InternalServerErrorOperationResponse<EnterpriseDetails>();
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.Unauthorized) {
                        return UnauthorizedOperationResponse<EnterpriseDetails>();
                    }
                    var result = await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResponse>();
                    return new OperationResponse<EnterpriseDetails>() {
                        OperationResult = result.OperationResult ,
                        Message = result.Message ,
                        Record = new EnterpriseDetails()
                    };
                }
                return await httpResponse.Content.ReadFromJsonAsync<OperationResponse<EnterpriseDetails>>();

            } catch (HttpRequestException requestException) {

                if (requestException.InnerException.GetType() == typeof(SocketException)) {
                    return await GetEnterpriseFromLocalAsync(enterpriseRnc);
                }
                return UnkownHostOperationResponse<EnterpriseDetails>();
            }

        }

        public async Task<OperationResponse<List<EnterpriseDetails>>> GetAllEnterprisesAsync() {
            if (!MainWindow.IsServerOnline) {
                return NoServerConnectionOperationResponse<List<EnterpriseDetails>>();
            }

            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.GetAsync(_configuration["Api:Enterprise:GetAll"]);

                if (!httpResponse.IsSuccessStatusCode) {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound) {
                        return NotFoundEndpointOperationResponse<List<EnterpriseDetails>>();
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.InternalServerError) {
                        return InternalServerErrorOperationResponse<List<EnterpriseDetails>>();
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.Unauthorized) {
                        return UnauthorizedOperationResponse<List<EnterpriseDetails>>();
                    }
                    var result = await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResponse>();
                    return new OperationResponse<List<EnterpriseDetails>>() {
                        OperationResult = result.OperationResult ,
                        Message = result.Message ,
                        Record = null
                    };
                }
                return await httpResponse.Content.ReadFromJsonAsync<OperationResponse<List<EnterpriseDetails>>>();

            } catch (HttpRequestException requestException) {

                if (requestException.InnerException.GetType() == typeof(SocketException)) {
                    return NoServerConnectionOperationResponse<List<EnterpriseDetails>>();
                }
                return UnkownHostOperationResponse<List<EnterpriseDetails>>();
            }

        }

        private async Task<EmptyOperationResponse> SaveEnterpriseLocalAsync(CreateEnterpriseRequest enterpriseRequest) {
            var offlineOperationResponse = await offlineEnterpriseService.CreateAsync(enterpriseRequest);
            offlineOperationResponse.Message = offlineOperationResponse.Message.Insert(0 , "OFFLINE: ");
            return offlineOperationResponse;
        }

        private async Task<OperationResponse<EnterpriseDetails>> GetEnterpriseFromLocalAsync(string enterpriseRnc) {
            var offlineOperationResponse = await offlineEnterpriseService.GetByRncAsync(enterpriseRnc);
            offlineOperationResponse.Message = offlineOperationResponse.Message.Insert(0 , "OFFLINE: ");

            return new OperationResponse<EnterpriseDetails>() {
                OperationResult = offlineOperationResponse.OperationResult ,
                Message = offlineOperationResponse.Message ,
                Record = offlineOperationResponse.Record.ToEnterpriseDetails()
            };
        }

    }
}
