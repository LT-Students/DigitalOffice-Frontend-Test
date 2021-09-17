using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Company.Requests.Company;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.User;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using System.Collections.Generic;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Company
{
    public class CompanyScenarios : BaseScenarioCreator
    {
        private readonly CompanyController _companyController;

        private Scenario Get(HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context =>
                CreateResponse(await _companyController.Get(), expected));

            return ScenarioBuilder
                .CreateScenario("get_company", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Edit(List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _companyController.Edit(changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_company", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public CompanyScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _companyController = new(settings.Token);
        }

        public override void Run()
        {
            NBomberRunner
                .RegisterScenarios(Get(HttpStatusCode.OK))
                .WithReportFolder($"{_path}/get_company")
                .WithReportFileName("correct_get")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(
                    Edit(
                        new()
                        {
                            (nameof(EditCompanyRequest.Description), "load test")
                        },
                        HttpStatusCode.OK))
                .WithReportFolder($"{_path}/edit_company")
                .WithReportFileName("correct_edit")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
