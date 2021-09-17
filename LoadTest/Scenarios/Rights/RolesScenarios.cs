using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Rights.Responses;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.User;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Rights
{
    public class RolesScenarios : BaseScenarioCreator
    {
        private readonly RolesController _rolesController;

        private Scenario Get(Guid roleId, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context =>
                CreateResponse(await _rolesController.Get(roleId), expected));

            return ScenarioBuilder
                .CreateScenario("get_role", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Find(int skipCount, int takeCount, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _rolesController.Find(skipCount, takeCount), expected));

            return ScenarioBuilder
                .CreateScenario("find_roles", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public RolesScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _rolesController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? roleId = JsonConvert
                .DeserializeObject<FindResponse>(_rolesController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Roles
                .FirstOrDefault()
                ?.Id;

            if (roleId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(Get(roleId.Value, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/get_role")
                .WithReportFileName("correct_get")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            NBomberRunner
                .RegisterScenarios(Find(0, 20, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_roles")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
