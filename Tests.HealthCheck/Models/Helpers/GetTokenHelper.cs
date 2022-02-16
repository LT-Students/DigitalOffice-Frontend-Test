using System;
using System.IO;
using System.Net;
using System.Text;
using Tests.HealthCheck.Models.Configurations;

namespace LT.DigitalOffice.Tests.HealthCheck.Models.Helpers
{
    public static class GetTokenHelper
    {
        public static string GetToken(AuthLoginConfig authLoginConfig)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest
                .Create(authLoginConfig.UriString);

            string stringData =
                $"{{ \"LoginData\": \"{authLoginConfig.Login}\",\"Password\": \"{authLoginConfig.Password}\" }}";

            byte[] data = Encoding.Default.GetBytes(stringData);

            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json; charset=utf-8";
            httpRequest.ContentLength = data.Length;

            using Stream newStream = httpRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);

            using HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using Stream stream = httpResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);

            string response = reader.ReadToEnd();
            string[] separators = { "accessToken\":\"", "\",\"refreshToken" };
            string token = response.Split(separators, StringSplitOptions.TrimEntries)[1];

            return token;
        }
    }
}
