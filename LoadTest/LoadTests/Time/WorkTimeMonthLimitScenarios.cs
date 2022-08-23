using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Time.Models;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Time;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Time
{
  public class WorkTimeMonthLimitScenarios : BaseScenarioCreator
  {
    private readonly WorkTimeMonthLimitController _limitController;

    private Scenario Find(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context =>
        CreateResponse(
          await _limitController.Find(new()
          {
            SkipCount = Random.Shared.Next(50),
            TakeCount = Random.Shared.Next(int.MaxValue)
          }), expected));

      return ScenarioBuilder
        .CreateScenario("find_worktimemonthlimit", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    private Scenario Edit(Guid worktimeMonthLimitId, List<(string property, string newValue)> changes, HttpStatusCode expected)
    {
      var correct = Step.Create("edit", async context =>
        CreateResponse(await _limitController.Edit(worktimeMonthLimitId, changes), expected));

      return ScenarioBuilder
        .CreateScenario("edit_worktimemonthlimit", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    public WorkTimeMonthLimitScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _limitController = new(settings.Token);
    }

    public override void Run()
    {
      Guid? worktimeMonthLimitId = JsonConvert
        .DeserializeObject<FindResultResponse<WorkTimeMonthLimitInfo>>(
        _limitController.Find(new()).Result.Content.ReadAsStringAsync().Result)?
        .Body?
        .FirstOrDefault()?
        .Id;

      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_worktimemonthlimits")
        .WithReportFileName("find")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
