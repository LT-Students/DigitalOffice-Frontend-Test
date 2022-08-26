using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Scenarios.Company;
using DigitalOffice.LoadTesting.Scenarios.Project;
using DigitalOffice.LoadTesting.Scenarios.Rights;
using DigitalOffice.LoadTesting.Scenarios.Time;
using DigitalOffice.LoadTesting.Services.Auth;
using DigitalOffice.LoadTesting.Services.User;
using LT.DigitalOffice.LoadTesting.LoadTests.Admin;
using LT.DigitalOffice.LoadTesting.LoadTests.Company;
using System;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting
{
  class Program
  {
    private static async Task<string> GetTokenAsync(AuthController controller, string login, string password)
    {
      return (await controller.Auth(login, password)).AccessToken;
    }

    static async Task Main(string[] args)
    {
      AuthController controller = new AuthController();

      var settings = new ScenarioStartSettings()
      {
        Path = "C:\\Temp\\NB\\200rps",
        During = TimeSpan.FromSeconds(60),
        WarmUpTime = TimeSpan.FromSeconds(5),
        ResponseTimeout = TimeSpan.FromMilliseconds(30000),
        Rate = 200,
        Token = await GetTokenAsync(controller, "N1ki26", "Admin2022!")
      };

      AdminScenarious adminScenarious = new(settings);
      adminScenarious.Run();

      RightsScenarios rightsScenarios = new(settings);
      RolesScenarios rolesScenarios = new(settings);

      rightsScenarios.Run();
      await rolesScenarios.RunAsync();

      StatScenarios statScenarios = new(settings);
      await statScenarios.RunAsync();

      UsersScenarios users = new(settings);
      await users.RunAsync();

      settings.Token = await GetTokenAsync(controller, "N1ki26", "Admin2022!");

      ImportScenarios importScenarios = new(settings);
      LeaveTimeScenarios leaveTimeScenarios = new(settings);
      WorkTimeMonthLimitScenarios workTimeMonthLimitScenarios = new(settings);
      WorkTimeScenarios workTimeScenarios = new(settings);

      await leaveTimeScenarios.RunAsync();
      workTimeMonthLimitScenarios.Run();
      workTimeScenarios.Run();

      settings.Token = await GetTokenAsync(controller, "N1ki26", "Admin2022!");

      ProjectScenarios projectScenarios = new(settings);
      await projectScenarios.RunAsync();

      CompanyScenarios companyScenarios = new(settings);
      ContractSubjectScenarios contractSubjectScenarios = new(settings);
      
      companyScenarios.Run();
      contractSubjectScenarios.Run();
    }
  }
}
