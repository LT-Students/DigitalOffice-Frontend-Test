using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Time;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Collections.Generic;
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
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Edit(Guid worktimeMonthLimitId, List<(string property, string newValue)> changes, HttpStatusCode expected)
    {
      var correct = Step.Create("edit", async context =>
        CreateResponse(await _limitController.Edit(worktimeMonthLimitId, changes), expected), timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("edit_worktimemonthlimit", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public WorkTimeMonthLimitScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _limitController = new(settings.Token);
    }

    public override void Run()
    {
      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_worktimemonthlimits")
        .WithReportFileName("find_monthLimits")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
