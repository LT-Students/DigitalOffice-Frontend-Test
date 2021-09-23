using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Company.Models;
using DigitalOffice.LoadTesting.Models.Company.Requests.Position;
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
    public class PositionScenarios : BaseScenarioCreator
    {
        private readonly PositionController _positionController;

        private Scenario Get(Guid positionId, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context => 
                CreateResponse(await _positionController.Get(positionId), expected));

            return ScenarioBuilder
                .CreateScenario("get_position", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Find(int skipCount, int takeCount, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context => 
                CreateResponse(await _positionController.Find(skipCount, takeCount), expected));

            return ScenarioBuilder
                .CreateScenario("find_positions", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }


        private Scenario Edit(Guid userId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _positionController.Edit(userId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_position", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreatePositionRequest request, bool generateUniqueName, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
            {
                if (generateUniqueName)
                {
                    request.Name = CreatorUniqueName.Generate();
                }

                return CreateResponse(await _positionController.Create(request), expected);
            });

            return ScenarioBuilder
                .CreateScenario("create_position", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public PositionScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _positionController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? positionId = JsonConvert
                .DeserializeObject<FindResultResponse<PositionInfo>>(_positionController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            if (positionId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(Get(positionId.Value, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/get_position")
                .WithReportFileName("correct_get")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

                NBomberRunner
                    .RegisterScenarios(
                        Edit(
                            positionId.Value,
                            new()
                            {
                                (nameof(EditPositionRequest.Description), "load test")
                            },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/edit_position")
                    .WithReportFileName("correct_edit")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();
            }

            NBomberRunner
                .RegisterScenarios(Find(0, 20, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_position")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(Create(new(), true, HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_position")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
