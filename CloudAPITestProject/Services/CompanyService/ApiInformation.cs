using CloudAPITestProject.Models;
using CloudAPITestProject.Services.Abstract;
using CloudAPITestProject.Services.Interfaces;
using RestSharp;
using System.Net;

namespace CloudAPITestProject.Services.CompanyService
{
    public class ApiInformation : ApiInformationCheck
    {
        protected override IRestResponse SendRequest()
        {
            var client = new RestClient("https://company.ltdo.xyz/apiinformation");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
