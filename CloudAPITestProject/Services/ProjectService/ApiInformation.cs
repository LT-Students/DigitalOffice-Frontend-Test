using CloudAPITestProject.Services.Abstract;
using RestSharp;

namespace CloudAPITestProject.Services.ProjectService
{
    public class ApiInformation : ApiInformationCheck
    {
        protected override IRestResponse SendRequest()
        {
            var client = new RestClient("https://project.ltdo.xyz/apiinformation");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
