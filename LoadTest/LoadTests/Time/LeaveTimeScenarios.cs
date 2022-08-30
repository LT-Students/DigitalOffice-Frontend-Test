using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Time.Responses;
using DigitalOffice.LoadTesting.Services.Time;
using LT.DigitalOffice.LoadTesting.LoadTests;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Scenarios.Time
{
  public class LeaveTimeScenarios : BaseScenarioCreatorAsync
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
          }), expected),
          timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("find_leavetimes", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Edit(Guid leaveTimeId, List<(string property, string newValue)> changes, HttpStatusCode expected)
    {
      var correct = Step.Create("edit", async context =>
        CreateResponse(await _leaveTimeController.Edit(leaveTimeId, changes), expected), timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("edit_leavetime", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public LeaveTimeScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _leaveTimeController = new(settings.Token);
    }

    public override async Task RunAsync()
    {
      Guid? leaveTimeId = JsonConvert
        .DeserializeObject<FindResultResponse<LeaveTimeResponse>>(await
          (await _leaveTimeController.Find(new() { SkipCount = 0, TakeCount = 1 }))?.Content.ReadAsStringAsync())?
        .Body?.FirstOrDefault()?.LeaveTime.Id;

      /*if (leaveTimeId.HasValue)
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
        .WithReportFileName("edit_leaveTime")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
      }*/

      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_leavetimes")
        .WithReportFileName("find_leaveTimes")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
