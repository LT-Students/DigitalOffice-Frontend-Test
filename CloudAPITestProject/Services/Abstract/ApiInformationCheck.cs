using CloudAPITestProject.Models;
using CloudAPITestProject.Services.Interfaces;
using RestSharp;
using System.Net;

namespace CloudAPITestProject.Services.Abstract
{
    public abstract class ApiInformationCheck : ICheckEndpoint
    {
        protected abstract IRestResponse SendRequest();

        public RequestResult Check()
        {
            var result = new RequestResult { IsSuccess = true };

            var response = SendRequest();

            if (!response.IsSuccessful)
            {
                result.IsSuccess = false;
                result.Errors.Add($"Response {response.ResponseUri} is not successful");
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                result.IsSuccess = false;
                result.Errors.Add($"Status code {response.StatusCode}");
            }

            if (response.ErrorException != null)
            {
                result.IsSuccess = false;
                result.Errors.Add(response.ErrorException.ToString());
            }

            return result;
        }
    }
}
