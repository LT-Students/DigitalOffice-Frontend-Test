using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.User;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Rights
{
  public class RightsScenarios : BaseScenarioCreator
  {
    private readonly RightsController _rightsController;

    private Scenario GetRightsList(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("get_rights_list", async context =>
        CreateResponse(await _rightsController.GetRightsList(), expected));

      return ScenarioBuilder
        .CreateScenario("get_rights_list", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    public RightsScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _rightsController = new(settings.Token);
    }

    public override void Run()
    {
      NBomberRunner
        .RegisterScenarios(GetRightsList(HttpStatusCode.OK))
        .WithReportFolder($"{_path}/get_rights_list")
        .WithReportFileName("get")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
