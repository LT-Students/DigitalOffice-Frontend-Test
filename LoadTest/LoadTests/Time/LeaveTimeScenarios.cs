using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Time.Requests;
using DigitalOffice.LoadTesting.Models.Time.Responses;
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
  public class LeaveTimeScenarios : BaseScenarioCreator
  {
    private readonly LeaveTimeController _leaveTimeController;

    private Scenario Find(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context =>
        CreateResponse(
          await _leaveTimeController.Find(new()
          {
            SkipCount = Random.Shared.Next(50),
            TakeCount = Random.Shared.Next(int.MaxValue)
          }), expected));

      return ScenarioBuilder
        .CreateScenario("find_leavetimes", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    private Scenario Edit(Guid leaveTimeId, List<(string property, string newValue)> changes, HttpStatusCode expected)
    {
      var correct = Step.Create("edit", async context =>
        CreateResponse(await _leaveTimeController.Edit(leaveTimeId, changes), expected));

      return ScenarioBuilder
        .CreateScenario("edit_leavetime", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    public LeaveTimeScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _leaveTimeController = new(settings.Token);
    }

    public override void Run()
    {
      Guid? leaveTimeId = JsonConvert
        .DeserializeObject<FindResultResponse<LeaveTimeResponse>>(
          _leaveTimeController.Find(new() { SkipCount = 0, TakeCount = 1 }).Result.Content.ReadAsStringAsync().Result)?
        .Body?.FirstOrDefault()?.LeaveTime.Id;

      if (leaveTimeId.HasValue)
      {
        NBomberRunner
        .RegisterScenarios(
          Edit(
            leaveTimeId.Value,
            new()
            {
              (nameof(EditLeaveTimeRequest.Minutes), "100")
            },
            HttpStatusCode.OK))
        .WithReportFolder($"{_path}/edit_leavetime")
        .WithReportFileName("correct_edit")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
      }

      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_leavetimes")
        .WithReportFileName("correct_find")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
