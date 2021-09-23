using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Message.Models.Workspace;
using DigitalOffice.LoadTesting.Models.Message.Requests.Workspace;
using DigitalOffice.LoadTesting.Models.Message.Requests.Workspace.Filters;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Message;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Message
{
    public class WorkspaceScenarios : BaseScenarioCreator
    {
        private readonly WorkspaceController _workspaceController;

        private Scenario Get(GetWorkspaceFilter filter, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context =>
                CreateResponse(await _workspaceController.Get(filter), expected));

            return ScenarioBuilder
                .CreateScenario("get_workspace", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Find(FindWorkspaceFilter filter, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _workspaceController.Find(filter), expected));

            return ScenarioBuilder
                .CreateScenario("find_workspaces", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }


        private Scenario Edit(Guid workspaceId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _workspaceController.Edit(workspaceId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_workspace", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreateWorkspaceRequest request, bool generateUniqueName, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
            {
                if (generateUniqueName)
                {
                    request.Name = CreatorUniqueName.Generate();
                }

                return CreateResponse(await _workspaceController.Create(request), expected);
            });

            return ScenarioBuilder
                .CreateScenario("create_workspace", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public WorkspaceScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _workspaceController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? workspaceId = JsonConvert
                .DeserializeObject<FindResultResponse<ShortWorkspaceInfo>>(
                    _workspaceController.Find(new FindWorkspaceFilter { SkipCount = 0, TakeCount = 1 }).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            if (workspaceId.HasValue)
            {
                NBomberRunner
                    .RegisterScenarios(
                        Get(
                            new GetWorkspaceFilter { WorkspaceId = workspaceId.Value },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/get_workspace")
                    .WithReportFileName("correct_get")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();

                NBomberRunner
                    .RegisterScenarios(
                        Edit(
                            workspaceId.Value,
                            new()
                            {
                                (nameof(EditWorkspaceRequest.Description), "load test")
                            },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/edit_workspace")
                    .WithReportFileName("correct_edit")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();
            }

            NBomberRunner
                .RegisterScenarios(
                    Find(
                        new FindWorkspaceFilter { SkipCount = 0, TakeCount = 20 },
                        HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_workspaces")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(
                    Create(
                        new(), 
                        true,
                        HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_workspace")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
