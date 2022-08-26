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
    public class WorkTimeDayJobScenarios : BaseScenarioCreator
    {
        private readonly WorkTimeDayJobController _jobController;
        private readonly WorkTimeController _workTimeController;

        private Scenario Create(CreateWorkTimeDayJobRequest request, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
                CreateResponse(await _jobController.Create(request), expected));

            return ScenarioBuilder
                .CreateScenario("create_worktimesdayjob", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.KeepConstant(_rate, _during)
                });
        }

        private Scenario Edit(Guid workTimeDayJobId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _jobController.Edit(workTimeDayJobId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_worktime", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.KeepConstant(_rate, _during)
                });
        }

        public WorkTimeDayJobScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _jobController = new(settings.Token);
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

            Guid? worktimeDayJobId = JsonConvert
                .DeserializeObject<FindResultResponse<WorkTimeResponse>>(
                    _workTimeController.Find(
                        new FindWorkTimesFilter
                        { 
                            SkipCount = 0, 
                            TakeCount = 1, 
                            IncludeDayJobs = true 
                        })
                    .Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.WorkTime
                .Jobs
                ?.FirstOrDefault()
                ?.Id;

            if (worktimeId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(
                    Create(
                        new CreateWorkTimeDayJobRequest
                        {
                            Name = "LoadTest",
                            Day = 1,
                            Minutes = 100,
                            WorkTimeId = worktimeId.Value
                        },
                        HttpStatusCode.Created))
                .WithReportFolder($"{_path}/find_worktimedayjobs")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            if (worktimeDayJobId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(
                    Edit(
                        worktimeDayJobId.Value,
                        new()
                        {
                            (nameof(EditWorkTimeRequest.Description), "load test")
                        },
                        HttpStatusCode.OK))
                .WithReportFolder($"{_path}/edit_worktimedayjob")
                .WithReportFileName("correct_edit")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }
        }
    }
}
