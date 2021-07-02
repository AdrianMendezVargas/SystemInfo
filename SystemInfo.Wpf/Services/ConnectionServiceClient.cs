using System.Net.Http;
using System.Threading.Tasks;
using SystemInfo.Wpf.Configuration;

namespace SystemInfo.Wpf.Services {
    public class ConnectionServiceClient {
        private readonly HttpClient _httpClient;
        private readonly ClientConfiguration _configuration;

        public ConnectionServiceClient() {
            _configuration = new ClientConfiguration();
            _httpClient = _configuration.GetHttpClient();
        }

        public async Task<bool> IsConnectionEstablished() {
            HttpResponseMessage httpResponse;
            try {
                httpResponse = await _httpClient.GetAsync(_configuration["Api:Connection:Verify"]);
                if (!httpResponse.IsSuccessStatusCode) {
                    return false;
                }
                return true;

            } catch (HttpRequestException) {
                return false;
            }
        }

    }
}
