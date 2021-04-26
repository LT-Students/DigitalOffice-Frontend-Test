using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloudAPITestProject
{
    public class Request
    {
        public string Name { get; }
        public string Description { get; }
        public string Uri { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new();
        public Dictionary<string, string> Query { get; set; } = new();
        public object Body { get; set; }
        public HttpMethod HttpMethod { get; set; }

        public Request(string name, string description, string uri, HttpMethod httpMethod)
        {
            Name = name;
            Description = description;
            Uri = uri;
            HttpMethod = httpMethod;
        }

        public HttpResponseMessage SendRequest()
        {
            var message = CreateRequestMessage();

            var httpClient = new HttpClient();
            return httpClient.SendAsync(message).Result;
        }

        private HttpRequestMessage CreateRequestMessage()
        {
            var message = new HttpRequestMessage();
            message.Method = HttpMethod;

            StringBuilder builder = new StringBuilder(Uri);
            builder.Append('?');

            foreach (var pair in Query)
            {
                builder.Append(pair.Key);
                builder.Append('=');
                builder.Append(pair.Value);
                builder.Append('&');
            }
            if (Query.Any())
            {
                builder.Remove(builder.Length - 1, 1);
            }

            message.RequestUri = new Uri(builder.ToString());

            foreach (var header in Headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            if (Body != null)
            {
                var json = JsonSerializer.Serialize(Body);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return message;
        }
    }
}
