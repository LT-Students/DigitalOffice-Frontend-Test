using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LT.DigitalOffice.LoadTesting.Services.Admin
{
  public class GuiController
  {
    private const string GuiControllerProdUrl = "https://admin.ltdo.xyz/graphicaluserinterface/";
    private const string GuiControllerDevUrl = "http://localhost:9838/graphicaluserinterface/";

    private readonly HttpClient _httpClient;

    public GuiController()
    {
      _httpClient = new HttpClient();
#if DEBUG
      _httpClient.BaseAddress = new Uri(GUIControllerDevUrl);
#else
      _httpClient.BaseAddress = new Uri(GuiControllerProdUrl);
#endif
    }

    public Task<HttpResponseMessage> Get()
    {
      return _httpClient.GetAsync("get");
    }
  }
}
