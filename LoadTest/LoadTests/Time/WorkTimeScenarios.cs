using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Time.Filters;
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
  public class WorkTimeScenarios : BaseScenarioCreator
  {
    private readonly WorkTimeController _workTimeController;

    private Scenario Find(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context =>
        CreateResponse(
          await _workTimeController.Find(new()
          {
            SkipCount = Random.Shared.Next(50),
            TakeCount = Random.Shared.Next(int.MaxValue)
          }), expected),
          timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("find_worktimes", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Edit(Guid worktimeId, List<(string property, string newValue)> changes, HttpStatusCode expected)
    {
      var correct = Step.Create("edit", async context =>
        CreateResponse(await _workTimeController.Edit(worktimeId, changes), expected), timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("edit_worktime", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public WorkTimeScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _workTimeController = new(settings.Token);
    }

    public override void Run()
    {
      /*Guid? worktimeId = JsonConvert
        .DeserializeObject<FindResultResponse<WorkTimeResponse>>(
        _workTimeController.Find(new FindWorkTimesFilter { SkipCount = 0, TakeCount = 1 }).Result.Content.ReadAsStringAsync().Result)
        .Body
        .FirstOrDefault()
        ?.WorkTime.Id;

      if (worktimeId.HasValue)
      {
        NBomberRunner
        .RegisterScenarios(
          Edit(
            worktimeId.Value,
            new()
            {
              (nameof(EditWorkTimeRequest.Description), "load test")
            },
            HttpStatusCode.OK))
        .WithReportFolder($"{_path}/edit_worktime")
        .WithReportFileName("edit_worktime")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
      }*/

      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_worktimes")
        .WithReportFileName("find_worktimes")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
