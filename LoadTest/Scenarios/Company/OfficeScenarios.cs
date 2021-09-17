using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Company.Requests.Office;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.User;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Company
{
    public class OfficeScenarios : BaseScenarioCreator
    {
        private readonly OfficeController _officeController;

        private Scenario Find(int skipCount, int takeCount, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context =>
                CreateResponse(await _officeController.Find(skipCount, takeCount), expected));

            return ScenarioBuilder
                .CreateScenario("find_offices", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreateOfficeRequest request, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context => CreateResponse(await _officeController.Create(request), expected));

            return ScenarioBuilder
                .CreateScenario("create_office", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public OfficeScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _officeController = new(settings.Token);
        }

        public override void Run()
        {
            NBomberRunner
                .RegisterScenarios(Find(0, 20, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_offices")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(
                    Create(
                        new CreateOfficeRequest
                        { 
                            Address = "LoadTest",
                            City = "LoadTest",
                            Name = "LoadTest"
                        },
                        HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_office")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
