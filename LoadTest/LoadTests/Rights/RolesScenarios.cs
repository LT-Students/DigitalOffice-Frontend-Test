using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Rights.Models;
using DigitalOffice.LoadTesting.Services.User;
using LT.DigitalOffice.LoadTesting.LoadTests;
using LT.DigitalOffice.LoadTesting.Models.Rights.Requests;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Scenarios.Rights
{
  public class RolesScenarios : BaseScenarioCreatorAsync
  {
    private readonly RolesController _rolesController;

    private Scenario Get(List<Guid> rolesIds, string locale = "ru", HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("get", async context =>
        CreateResponse(await _rolesController.Get(rolesIds[Random.Shared.Next(rolesIds.Count)], locale), expected), timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("get_role", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Find(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("find", async context =>
        CreateResponse(await _rolesController.Find(
          skipCount: Random.Shared.Next(50),
          takeCount: Random.Shared.Next(int.MaxValue)),
        expected),
        timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("find_roles", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Create(List<int> rights, HttpStatusCode expected = HttpStatusCode.Created)
    {
      var correct = Step.Create("create", async context =>
        CreateResponse(await _rolesController.Create(new()
        {
          Localizations = new List<CreateRoleLocalizationRequest>()
          { 
            new()
            {
              Locale = "ru",
              Name = CreatorUniqueName.Generate()
            }
          },
          Rights = rights
        }),
        expected));

      return ScenarioBuilder
        .CreateScenario("create_roles", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public RolesScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _rolesController = new(settings.Token);
    }

    public override async Task RunAsync()
    {
      List<Guid> rolesIds = JsonConvert
        .DeserializeObject<FindResultResponse<RoleInfo>>(await
          (await _rolesController.Find(0, 1))?.Content.ReadAsStringAsync())?
        .Body?.Select(x => x.Id).ToList();

      if (rolesIds is not null && rolesIds.Any())
      {
        NBomberRunner
        .RegisterScenarios(Get(rolesIds))
        .WithReportFolder($"{_path}/get_role")
        .WithReportFileName("get_roles")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
      }

      NBomberRunner
        .RegisterScenarios(Find())
        .WithReportFolder($"{_path}/find_roles")
        .WithReportFileName("find_roles")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
    }
  }
}
