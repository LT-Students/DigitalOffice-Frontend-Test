using CloudAPITestProject.Models;
using CloudAPITestProject.Services.Interfaces;

namespace CloudAPITestProject.Services.AuthService
{
    public class Login : ICheckEndpoint
    {
        public RequestResult Check()
        {
            return new RequestResult { IsSuccess = true };
        }

        public string Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
