using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Wpf.Services;

namespace SystemInfo.Wpf.Configuration {
    public class ClientConfiguration {
        private readonly PreferencesService _preferencesService;

        public ClientConfiguration() {
            _preferencesService = OfflineBussinessServicesContainer.PreferencesService;
        }
        public string this[string name] {
            get => BaseAdress + ConfigurationManager.AppSettings[name];
            set => ConfigurationManager.AppSettings[name] = value;
        }
        public string BaseAdress => ConfigurationManager.AppSettings["Api:BaseAdress"];

        public async Task<HttpClient> GetHttpClient() {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender , cert , chain , sslPolicyErrors) => { return true; };
            var client = new HttpClient(clientHandler);

            string token = (await _preferencesService.Get(PreferencesKeys.Token))?.Value;
            if (!string.IsNullOrWhiteSpace(token)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , token);
            }
            return client;
        }

    }
}
