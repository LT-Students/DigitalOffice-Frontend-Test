using DigitalOffice.LoadTesting.Models.Message.Requests.Channel;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.Message
{
    public class ChannelController
    {
        private const string ChannelControllerProdUrl = "https://message.ltdo.xyz/channel/";
        private const string ChannelControllerDevUrl = "http://localhost:9810/channel/";

        private readonly HttpClient _httpClient;

        public ChannelController(string accessToken)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add("token", accessToken);
#if DEBUG
            _httpClient.BaseAddress = new Uri(ChannelControllerDevUrl);
#else
            _httpClient.BaseAddress = new Uri(ChannelControllerProdUrl);
#endif
        }

        public async Task<HttpResponseMessage> Create(CreateChannelRequest request)
        {
            var httpContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

            return await _httpClient.PostAsync("create", httpContent);
        }
    }
}
