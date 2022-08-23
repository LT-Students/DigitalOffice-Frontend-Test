using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Scenarios.Company;
using DigitalOffice.LoadTesting.Scenarios.Project;
using DigitalOffice.LoadTesting.Scenarios.Rights;
using DigitalOffice.LoadTesting.Scenarios.Time;
using DigitalOffice.LoadTesting.Services.Auth;
using DigitalOffice.LoadTesting.Services.User;
using LT.DigitalOffice.LoadTesting.Helpers;
using LT.DigitalOffice.LoadTesting.LoadTests.Company;
using System;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var settings = new ScenarioStartSettings()
      {
        Path = "C:\\Temp\\NB",
        During = TimeSpan.FromSeconds(240),
        WarmUpTime = TimeSpan.FromSeconds(5),
        ResponseTimeout = TimeSpan.FromSeconds(60),
        Rate = 50,
        Token = (new AuthController()).Auth("Nikita26", "Admin2022!").Result.AccessToken
      };

      EntitiesCreator initial = new(settings);

      UsersScenarios users = new(settings);

      ImportScenarios importScenarios = new(settings);
      LeaveTimeScenarios leaveTimeScenarios = new(settings);
      StatScenarios statScenarios = new(settings);
      WorkTimeMonthLimitScenarios workTimeMonthLimitScenarios = new(settings);
      WorkTimeScenarios workTimeScenarios = new(settings);

      RightsScenarios rightsScenarios = new(settings);
      RolesScenarios rolesScenarios = new(settings);

      ProjectScenarios projectScenarios = new(settings);

      CompanyScenarios companyScenarios = new(settings);
      ContractSubjectScenarios contractSubjectScenarios = new(settings);

      //await initial.Run()

      users.Run();

      //importScenarios.Run();
      leaveTimeScenarios.Run();
      workTimeMonthLimitScenarios.Run();
      statScenarios.Run();
      workTimeScenarios.Run();
      
      rightsScenarios.Run();
      rolesScenarios.Run();
      
      projectScenarios.Run();
      
      companyScenarios.Run();
      contractSubjectScenarios.Run();
    }
  }
}
