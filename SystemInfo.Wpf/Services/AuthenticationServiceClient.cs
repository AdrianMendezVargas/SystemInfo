using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Shared.Enums;
using SystemInfo.Shared.Requests;
using SystemInfo.Shared.Responses;
using SystemInfo.Wpf.Configuration;

namespace SystemInfo.Wpf.Services {
    public class AuthenticationServiceClient : BaseServiceClient {

        private readonly HttpClient httpClient;
        private readonly ClientConfiguration _ClientConfiguration;

        public AuthenticationServiceClient() {
            _ClientConfiguration = new ClientConfiguration();
            httpClient = _ClientConfiguration.GetHttpClient();
        }

        public async Task<OperationResponse<TokenResponse>> RequestToken() {
            if (!MainWindow.IsServerOnline) {
                return NoServerConnectionOperationResponse<TokenResponse>();   
            }

            HttpResponseMessage httpResponse;
            try {
                var tokenRequest = new TokenRequest() {
                    Password = ConfigurationManager.AppSettings["ApiPassword"]
                };

                httpResponse = await httpClient.PostAsJsonAsync(_ClientConfiguration["Api:Authentication"], tokenRequest);

                if (!httpResponse.IsSuccessStatusCode) {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound) {
                        return NotFoundEndpointOperationResponse<TokenResponse>();
                    }
                    if (httpResponse.StatusCode == HttpStatusCode.InternalServerError) {
                        return InternalServerErrorOperationResponse<TokenResponse>();
                    }

                    var result = await httpResponse.Content.ReadFromJsonAsync<EmptyOperationResponse>();
                    return new OperationResponse<TokenResponse>() {
                        OperationResult = result.OperationResult ,
                        Message = result.Message ,
                        Record = null
                    };
                }
                return await httpResponse.Content.ReadFromJsonAsync<OperationResponse<TokenResponse>>();

            } catch (HttpRequestException requestException) {

                if (requestException.InnerException.GetType() == typeof(SocketException)) {
                    return NoServerConnectionOperationResponse<TokenResponse>();
                }
                return UnkownHostOperationResponse<TokenResponse>();
            }

        }

    }
}
