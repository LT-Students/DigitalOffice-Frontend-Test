using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Services;
using LT.DigitalOffice.LoadTesting.Services.Company;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Net;

namespace LT.DigitalOffice.LoadTesting.LoadTests.Company
{
  public class ContractSubjectScenarios : BaseScenarioCreator
  {
    private readonly ContractSubjectController _contractSubjectController;

    private Scenario Find(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context =>
        CreateResponse(await _contractSubjectController.Find(new()
        {
          SkipCount = Random.Shared.Next(50),
          TakeCount = Random.Shared.Next(int.MaxValue)
        }), expected));

      return ScenarioBuilder
        .CreateScenario("find_contractSubject", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    public ContractSubjectScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _contractSubjectController = new(settings.Token);
    }

    public override void Run()
    {
      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_contractSubject")
        .WithReportFileName("find")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
