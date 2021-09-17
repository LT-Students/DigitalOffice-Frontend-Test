using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using DigitalOffice.LoadTesting.Models.Project.Requests;
using DigitalOffice.LoadTesting.Models.Project.Responses;
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
    public class TaskScenarios : BaseScenarioCreator
    {
        private readonly TaskController _taskController;
        private readonly TaskPropertyController _taskPropertyController;
        private readonly ProjectController _projectController;

        private Scenario Get(Guid taskId, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context =>
                CreateResponse(await _taskController.Get(taskId), expected));

            return ScenarioBuilder
                .CreateScenario("get_task", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Find(int skipCount, int takeCount, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _taskController.Find(skipCount, takeCount), expected));

            return ScenarioBuilder
                .CreateScenario("find_tasks", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Edit(Guid userId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _taskController.Edit(userId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_task", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreateTaskRequest request, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context => CreateResponse(await _taskController.Create(request), expected));

            return ScenarioBuilder
                .CreateScenario("create_task", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public TaskScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _taskController = new(settings.Token);
            _taskPropertyController = new(settings.Token);
            _projectController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? projectId = JsonConvert
                .DeserializeObject<FindResponse<ProjectInfo>>(_projectController.Find(0, 1, new()).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            Guid? taskId = JsonConvert
                .DeserializeObject<FindResultResponse<TaskInfo>>(_taskController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            List<TaskPropertyInfo> properties = JsonConvert
                .DeserializeObject<FindResultResponse<TaskPropertyInfo>>(
                    _taskPropertyController.Find(0, 20).Result.Content.ReadAsStringAsync().Result)
                .Body;

            Guid? typeId = properties.FirstOrDefault(p => p.PropertyType == TaskPropertyType.Type)?.Id;
            Guid? statusId = properties.FirstOrDefault(p => p.PropertyType == TaskPropertyType.Status)?.Id;
            Guid? priorityId = properties.FirstOrDefault(p => p.PropertyType == TaskPropertyType.Priority)?.Id;

            if (typeId != null && statusId != null && priorityId != null && projectId != null)
            {
                NBomberRunner
                .RegisterScenarios(
                    Create(
                        new()
                        {
                            Name = "LoadTest",
                            TypeId = typeId.Value,
                            StatusId = statusId.Value,
                            PriorityId = priorityId.Value,
                            ProjectId = projectId.Value
                        },
                        HttpStatusCode.OK))
                .WithReportFolder($"{_path}/create_task")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            if (taskId.HasValue)
            {
                NBomberRunner
                    .RegisterScenarios(Get(taskId.Value, HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/get_task")
                    .WithReportFileName("correct_get")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();

                NBomberRunner
                    .RegisterScenarios(
                        Edit(
                            taskId.Value,
                            new()
                            {
                                (nameof(EditTaskRequest.Description), "load test")
                            },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/edit_task")
                    .WithReportFileName("correct_edit")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();
            }

            NBomberRunner
                .RegisterScenarios(Find(0, 20, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_task")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
