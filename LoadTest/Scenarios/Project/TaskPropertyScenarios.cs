using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using DigitalOffice.LoadTesting.Models.Project.Requests;
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
    public class TaskPropertyScenarios : BaseScenarioCreator
    {
        private readonly TaskPropertyController _propertyController;
        private readonly ProjectController _projectController;

        private Scenario Find(int skipCount, int takeCount, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _propertyController.Find(skipCount, takeCount), expected));

            return ScenarioBuilder
                .CreateScenario("find_task_property", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreateTaskPropertyRequest request, bool createUniqueNames, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
            {
                if (createUniqueNames)
                {
                    request.TaskProperties = request.TaskProperties
                        .Select(p =>
                        {
                            p.Name = $"{p.Name}{Guid.NewGuid()}";
                            return p;
                        }).ToList();
                }

                return CreateResponse(await _propertyController.Create(request), expected);
            });

            return ScenarioBuilder
                .CreateScenario("create_task_property", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public TaskPropertyScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _propertyController = new(settings.Token);
            _projectController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? projectId = JsonConvert
                .DeserializeObject<FindResultResponse<Models.Project.Models.ProjectInfo>>(
                    _projectController.Find(0, 1, new()).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            if (projectId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(
                    Create(
                        new()
                        {
                            ProjectId = projectId.Value,
                            TaskProperties = new List<TaskProperty>()
                            {
                                new TaskProperty
                                {
                                    Name = "LoadTest",
                                    PropertyType = TaskPropertyType.Type
                                }
                            }
                        },
                        true,
                        HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_task_property")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            NBomberRunner
                .RegisterScenarios(Find(0, 20, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_task_property")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
