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
    public class StatScenarios : BaseScenarioCreator
    {
        private readonly StatController _statController;
        private readonly DepartmentController _departmentController;

        private Scenario Find(FindStatFilter filter, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context => CreateResponse(await _statController.Find(filter), expected));

            return ScenarioBuilder
                .CreateScenario("find_stat", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public StatScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _statController = new(settings.Token);
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
                        Find(
                            new()
                            {
                                DepartmentId = departmentId.Value,
                                Year = 2021,
                                Month = 9,
                                SkipCount = 0,
                                TakeCount = 20
                            },
                            HttpStatusCode.OK))
                    .WithReportFolder($"{_path}/find_stat")
                    .WithReportFileName("correct_find")
                    .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                    .Run();
            }
        }
    }
}
