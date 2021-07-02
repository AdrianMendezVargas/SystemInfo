using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Wpf.Configuration {
    public class ClientConfiguration {
        public string this[string name] {
            get => BaseAdress + ConfigurationManager.AppSettings[name];
            set => ConfigurationManager.AppSettings[name] = value;
        }
        public string BaseAdress => ConfigurationManager.AppSettings["Api:BaseAdress"];

        public HttpClient GetHttpClient() {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender , cert , chain , sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }

    }
}
