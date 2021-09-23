using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Company.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Time.Filters;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Time;
using DigitalOffice.LoadTesting.Services.User;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Time
{
    public class ImportScenarios : BaseScenarioCreator
    {
        private readonly ImportController _importController;
        private readonly DepartmentController _departmentController;

        private Scenario Get(ImportStatFilter filter, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context => CreateResponse(await _importController.Get(filter), expected));

            return ScenarioBuilder
                .CreateScenario("get_import", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public ImportScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _importController = new(settings.Token);
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
                //TODO rework
                NBomberRunner
                    .RegisterScenarios(
                        Get(
                            new()
                            {
                                DepartmentId = departmentId.Value,
                                Year = 2021,
                                Month = 9
                            },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/get_import")
                    .WithReportFileName("correct_get")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();
            }
        }
    }
}
