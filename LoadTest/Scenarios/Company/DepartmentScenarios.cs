using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Company.Models;
using DigitalOffice.LoadTesting.Models.Company.Requests.Department;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.User;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Company
{
    public class DepartmentScenarios : BaseScenarioCreator
    {
        private readonly DepartmentController _departmentController;

        private Scenario Get(Guid departmentId, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context =>
                CreateResponse(await _departmentController.Get(departmentId), expected));

            return ScenarioBuilder
                .CreateScenario("get_department", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Find(int skipCount, int takeCount, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _departmentController.Find(skipCount, takeCount), expected));

            return ScenarioBuilder
                .CreateScenario("find_departments", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }


        private Scenario Edit(Guid departmentId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _departmentController.Edit(departmentId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_department", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreateDepartmentRequest request, bool generateUniqueName, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
            {
                if (generateUniqueName)
                {
                    request.Name = CreatorUniqueName.Generate();
                }

                return CreateResponse(await _departmentController.Create(request), expected);
            });

            return ScenarioBuilder
                .CreateScenario("create_department", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public DepartmentScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _departmentController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? departmentId = JsonConvert
                .DeserializeObject<FindResultResponse<DepartmentInfo>>(_departmentController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            if (departmentId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(Get(departmentId.Value, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/get_department")
                .WithReportFileName("correct_get")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

                NBomberRunner
                    .RegisterScenarios(
                        Edit(
                            departmentId.Value,
                            new()
                            {
                                (nameof(EditDepartmentRequest.Description), "load test")
                            },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/edit_department")
                    .WithReportFileName("correct_edit")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();
            }

            NBomberRunner
                .RegisterScenarios(Find(0, 20, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_department")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(Create(new(), true, HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_department")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
