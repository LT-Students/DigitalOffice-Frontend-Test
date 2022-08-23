using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Project;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.Project.Filters;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.User;
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
  public class ProjectScenarios : BaseScenarioCreator
  {
    private readonly ProjectController _projectController;

    private Scenario Get(
      List<Guid> projectsIds,
      HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("get", async context =>
        CreateResponse(await _projectController.Get(new()
        {
          ProjectId = projectsIds[Random.Shared.Next(projectsIds.Count)]
        }), expected));

      return ScenarioBuilder
        .CreateScenario("get_projets", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    private Scenario Find(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context =>
        CreateResponse(await _projectController.Find(new()
        {
          SkipCount = Random.Shared.Next(50),
          TakeCount = Random.Shared.Next(int.MaxValue)
        }), expected));

      return ScenarioBuilder
        .CreateScenario("find_projects", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    private Scenario Create(
      Guid? departmentId = null,
      List<ImageContent> images = null,
      List<UserRequest> users = null,
      HttpStatusCode expected = HttpStatusCode.Created)
    {
      var correct = Step.Create("create", async context =>
      {
        return CreateResponse(await _projectController.Create(new()
        {
          Name = CreatorUniqueName.Generate(),
          ShortName = Guid.NewGuid().ToString(),
          StartDateUtc = DateTime.UtcNow,
          Status = ProjectStatusType.Active,
          DepartmentId = departmentId,
          ProjectImages = images ?? new(),
          Users = users ?? new()
        }), expected);
      });

      return ScenarioBuilder
        .CreateScenario("create_project", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_rate, _during)
        });
    }

    public ProjectScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _projectController = new(settings.Token);
    }

    public override void Run()
    {
      List<Guid> projectsIds = JsonConvert
        .DeserializeObject<FindResultResponse<ProjectInfo>>(
          _projectController.Find(new() { SkipCount = 0, TakeCount = int.MaxValue}).Result.Content.ReadAsStringAsync().Result)?
        .Body?.Select(x => x.Id).ToList();

      if (projectsIds is not null && projectsIds.Any())
      {
        NBomberRunner
          .RegisterScenarios(Get(projectsIds))
          .WithReportFolder($"{_path}/get_project")
          .WithReportFileName("get")
          .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
          .Run();
      }

      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_projects")
        .WithReportFileName("find")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();

      NBomberRunner
        .RegisterScenarios(Create())
        .WithReportFolder($"{_path}/create_project")
        .WithReportFileName("create")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
