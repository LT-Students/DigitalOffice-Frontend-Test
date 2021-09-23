using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Company.Models;
using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models.ProjectUser;
using DigitalOffice.LoadTesting.Models.Project.Requests;
using DigitalOffice.LoadTesting.Models.Project.Requests.Filters;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Project;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Project
{
    public class ProjectScenarios : BaseScenarioCreator
    {
        private readonly ProjectController _projectController;

        private Scenario Get(GetProjectFilter filter, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context =>
                CreateResponse(await _projectController.Get(filter), expected));

            return ScenarioBuilder
                .CreateScenario("get_projet", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Find(int skipCount, int takeCount, FindProjectsFilter filter, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _projectController.Find(skipCount, takeCount, filter), expected));

            return ScenarioBuilder
                .CreateScenario("find_projects", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }


        private Scenario Edit(Guid projectId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _projectController.Edit(projectId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_project", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreateProjectRequest request, bool generateUniqueName, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
            {
                if (generateUniqueName)
                {
                    request.Name = CreatorUniqueName.Generate();
                }

                return CreateResponse(await _projectController.Create(request), expected);
            });

            return ScenarioBuilder
                .CreateScenario("create_project", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public ProjectScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _projectController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? projectId = JsonConvert
                .DeserializeObject<FindResultResponse<ProjectInfo>>(_projectController.Find(0, 1, new()).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            if (projectId.HasValue)
            {
                NBomberRunner
                    .RegisterScenarios(
                        Get(new GetProjectFilter { ProjectId = projectId.Value }, HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/get_project")
                    .WithReportFileName("correct_get")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();

                NBomberRunner
                    .RegisterScenarios(
                        Edit(
                            projectId.Value,
                            new()
                            {
                                (nameof(EditProjectRequest.ShortDescription), "load test")
                            },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/edit_project")
                    .WithReportFileName("correct_edit")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();
            }

            NBomberRunner
                .RegisterScenarios(Find(0, 20, new(), HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_projects")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(
                    Create(
                        new()
                        { 
                            Status = ProjectStatusType.Active,
                            Users = new List<ProjectUserRequest>()
                        },
                        true, 
                        HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_project")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
