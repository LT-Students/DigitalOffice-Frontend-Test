using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Services;
using LT.DigitalOffice.LoadTesting.Services.Admin;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Net;

namespace LT.DigitalOffice.LoadTesting.LoadTests.Admin
{
  public class AdminScenarious : BaseScenarioCreator
  {
    private readonly AdminController _adminController;
    private readonly GuiController _guiController;

    private Scenario Get(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correctGui = Step.Create("get_gui", async context =>
        CreateResponse(await _guiController.Get(), expected), timeout: _responseTimeout);

      //var correctAdmin = Step.Create("find_serviceInfo", async context =>
      //  CreateResponse(await _adminController.Find(takeCount: Random.Shared.Next(100)), expected));

      return ScenarioBuilder
        .CreateScenario("get_gui_and_find_serviceInfo", correctGui/*, correctAdmin*/)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public AdminScenarious(ScenarioStartSettings settings)
      : base(settings)
    {
      _adminController = new();
      _guiController = new();
    }

    public override void Run()
    {
      NBomberRunner
        .RegisterScenarios(Get(HttpStatusCode.OK))
        .WithReportFolder($"{_path}/get_admin")
        .WithReportFileName("correct_get_gui_and_find_serviceInfo")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
