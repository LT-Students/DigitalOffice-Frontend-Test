using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Project.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Services.Project;
using DigitalOffice.LoadTesting.Services.Time;
using LT.DigitalOffice.LoadTesting.LoadTests;
using LT.DigitalOffice.LoadTesting.Services.Department;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM = LT.DigitalOffice.LoadTesting.Models.Department.Models;

namespace DigitalOffice.LoadTesting.Scenarios.Time
{
  public class StatScenarios : BaseScenarioCreatorAsync
  {
    private readonly StatController _statController;
    private readonly DepartmentController _departmentController;
    private readonly ProjectController _projectController;

    private Scenario Find(
      List<Guid> departmentsIds = null,
      List<Guid> projectsIds = null,
      HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context => CreateResponse(
        await _statController.Find(new()
        {
          SkipCount = Random.Shared.Next(50),
          TakeCount = Random.Shared.Next(int.MaxValue),
          Year = DateTime.UtcNow.Year,
          Month = DateTime.UtcNow.Month,
          DepartmentsIds = departmentsIds ?? new(),
          ProjectId = projectsIds is null || !projectsIds.Any()
            ? null
            : projectsIds[Random.Shared.Next(projectsIds.Count)]
        }), expected),
        timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("find_stat", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public StatScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _statController = new(settings.Token);
      _departmentController = new(settings.Token);
      _projectController = new(settings.Token);
    }

    public override async Task RunAsync()
    {
      List<Guid> departmentsIds = JsonConvert
        .DeserializeObject<FindResultResponse<DM.DepartmentInfo>>(await
          (await _departmentController.Find(0, int.MaxValue))?.Content.ReadAsStringAsync())?
        .Body?.Select(x => x.Id).ToList();

      List<Guid> projectsIds = JsonConvert
        .DeserializeObject<FindResultResponse<ProjectInfo>>(await
          (await _projectController.Find(new()
          {
            SkipCount = 0,
            TakeCount = int.MaxValue
          }))?.Content.ReadAsStringAsync())?.Body?.Select(x => x.Id).ToList();

      NBomberRunner
        .RegisterScenarios(Find(departmentsIds: departmentsIds))
        .WithReportFolder($"{_path}/find_stat")
        .WithReportFileName("find_stat_via_departments")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();

      NBomberRunner
        .RegisterScenarios(Find(projectsIds: projectsIds))
        .WithReportFolder($"{_path}/find_stat")
        .WithReportFileName("find_stat_via_project")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
