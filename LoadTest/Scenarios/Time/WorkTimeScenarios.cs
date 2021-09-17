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

        private Scenario Find(FindWorkTimesFilter filter, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _workTimeController.Find(filter), expected));

            return ScenarioBuilder
                .CreateScenario("find_worktimes", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Edit(Guid worktimeId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _workTimeController.Edit(worktimeId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_worktime", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public WorkTimeScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _workTimeController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? worktimeId = JsonConvert
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
                .WithReportFileName("correct_edit")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            NBomberRunner
                .RegisterScenarios(
                    Find(
                        new FindWorkTimesFilter { SkipCount = 0, TakeCount = 20 },
                        HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_worktimes")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
