using CloudAPITestProject.Models.Request;
using System.Net.Http;

namespace CloudAPITestProject
{
    class Program
    {
        private static ServiceUrls _servicesInfo;

        static void Main(string[] args)
        {
            var request = new Request(
                "CheckService_Login",
                "Login with wrong login",
                "https://auth.ltdo.xyz/auth/login",
                HttpMethod.Post);

            var login = new Login { LoginData = "login", Password = "Password" };

            request.Body = login;
                //"{\n\"LoginData\": \"login\",\n\"Password\": \"password\"\n}";

            var result = request.SendRequest();
        }
    }
}
