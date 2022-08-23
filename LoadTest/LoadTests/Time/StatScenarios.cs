using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Time;
using LT.DigitalOffice.LoadTesting.Models.Department.Models;
using LT.DigitalOffice.LoadTesting.Services.Department;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Time
{
  public class StatScenarios : BaseScenarioCreator
  {
    private readonly StatController _statController;
    private readonly DepartmentController _departmentController;

    private Scenario Find(
      List<Guid> departmentsIds = null,
      Guid? projectId = null,
      HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context => CreateResponse(
        await _statController.Find(new()
        {
          SkipCount = Random.Shared.Next(50),
          TakeCount = Random.Shared.Next(int.MaxValue),
          Year = DateTime.UtcNow.Year,
          Month = DateTime.UtcNow.Month,
          DepartmentsIds = departmentsIds,
          ProjectId = projectId
        }), expected));

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
      List<Guid> departmentsIds = JsonConvert
        .DeserializeObject<FindResultResponse<DepartmentInfo>>(
          _departmentController.Find(0, int.MaxValue).Result.Content.ReadAsStringAsync().Result)?
        .Body?.Select(x => x.Id).ToList();

      NBomberRunner
        .RegisterScenarios(Find(departmentsIds: departmentsIds))
        .WithReportFolder($"{_path}/find_stat")
        .WithReportFileName("correct_find")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
