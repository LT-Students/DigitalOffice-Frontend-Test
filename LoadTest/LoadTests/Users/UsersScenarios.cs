using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Users.Models;
using DigitalOffice.LoadTesting.Models.Users.Requests.User;
using LT.DigitalOffice.LoadTesting.Models.Users.Models;
using DM = LT.DigitalOffice.LoadTesting.Models.Department.Models;
using RM = DigitalOffice.LoadTesting.Models.Rights.Models;
using LT.DigitalOffice.LoadTesting.Services.Department;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LT.DigitalOffice.LoadTesting.Models.Company.Responses;
using LT.DigitalOffice.LoadTesting.Models.Common.Enums;
using LT.DigitalOffice.LoadTesting.LoadTests;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Services.User
{
  public class UsersScenarios : BaseScenarioCreatorAsync
  {
    private readonly UsersController _usersController;
    private readonly GenderController _genderController;
    private readonly RolesController _rolesController;
    private readonly DepartmentController _departmentController;
    private readonly CompanyController _companyController;

    const string avatarContent = "iVBORw0KGgoAAAANSUhEUgAAAHQAAABgCAIAAABDv8H9AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAAQ3SURBVHhe7ZstVOswFIDrmKMSB3IHBYJzJpHISmTlJBI3WYlETiInd1DISSQSOTmJ5H2PXHL6+ro/lrs14X5qS7q0+XJ7mzRn2aehhslVxOQqYnIVMbmKmFxFTK4iJlcRk6uIyVXE5CpichUxuYqYXEVMriImVxGTq4jJVcTkKmJyFTG5iphcRUyuIiZXEZOriMlVxOQqYnIVMbmKmNzATKfT6+vr8XjMZ5MbmJOTkyzLer0en4PJfXl58SP2O1ksFnd3d5h1UBJMLmZdo/P5XIp+B/SXkCrLMs9zZwBub2+pCib35ubGtfv4+ChFKYJK7lH6OBqN6HK/33e9rkP5x8cHBweTy+i5pglhKYqW9/d3DAIGgaikU4PBwHVwBWdnZ/XEGEwuGYcs7s7BBUlpV2nVB+76t4JeE6pVVc1mM2n9m2ByYTgcygm75/f19ZWnzc/01SE2aYSm6CBjQ7NygjZCyiXR+MwLXfDrnGJErmkzLi4uMFgUxd+oHo0mk8laj62ElAsd8bvWaSh9qwksFxp+uWip0GeFU+b2ZK19XgyElwt1v8SIm5fo0TWnHhW5wHzQTx4eHh6kNCidderRkgs4dV3FcsBlW/edehTlkg3ICa7bbjm4CxE59SjKBTosAn6aHGJ06tGVC8SsyMiy6XQqpeuI2qlHXW49OeR5/vb2JhVtpOHUoy4XWMijxjlC3GKxkIpvEnPq2YdcmM1mfmbG0sjNfFN16tmTXHh6ehJzWXZ+fn56eipfaqTh1LM/ucTp1dWVWPyXxJx61OWuuPfh8vLy+flZDk0OLbnMCpY5PT4+Pjo6ki9Z1u/3/3/NnAaB5bLMZbHQurNUv/c5rP7mDO7v77Xf7+yfMHLxMh6PG74cK/IpP6nvmDIkq2fB0bGrXBZdZVn6aZaHEtZmk8lEjltCI4T5VUqbxz+Uy7qAG5moFCs1mMYSkv+vFFaA0PrwFEWx1c87y9Zyma623v7c1FVVIV2O2xISgl8lA8OWwMxsU7nLQtWlVOZbctwOkLg5hbT7BaMldXGyXu6yUKWQKjkoHCTx+hAeaoszCEvlusl/a6gSXz++/TeBhEvilvPF7LdFLsmudfLP03/zF7I7Qoqo3y6R+m2RW48aQDS5L+Am2IYk4LdFLt2gM+5JddhHdux+1z/QDkvUfrsuF+L1G4FciNRvHHIhRr/RyIXo/MYkFxp+WeZQInXdIzK50PA7GAxUl4u7EJ9caPjN83zti+ODEKVcBzlX7H7RwRQcsVxgAdnlV2hxy4X5fN7ZV2jRy4VGCi7LsiO7GCnIhYZfIF0o/V1gcxKRC/gtikLUftHr9ep/Ft0/6ch1uA0Uv5fMB6k4BKnJdVRV5eQOh0MpOgRpyu0IJlcRk6uIyVXE5CpichUxuYqYXEVMriImVxGTq4jJVcTkKmJyFTG5iphcNT4//wB4lLqpi9HRMQAAAABJRU5ErkJggg==";

    private Scenario Get(List<Guid> usersIds, HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create(
        name: "get",
        execute: async context =>
          CreateResponse(await _usersController.Get(usersIds[Random.Shared.Next(usersIds.Count)]),
          expected),
        timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("get_user", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Find(HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create(
        name: "find",
        execute: async context =>
          CreateResponse(await _usersController.Find(
            skipCount: Random.Shared.Next(50),
            takeCount: Random.Shared.Next(int.MaxValue)),
          expected),
        timeout: _responseTimeout
      );

      return ScenarioBuilder
        .CreateScenario("find_users", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Edit(
      List<Guid> usersIds,
      HttpStatusCode expected = HttpStatusCode.OK)
    {
      var correct = Step.Create("edit", async context =>
        CreateResponse(await _usersController.Edit(
          userId: usersIds[Random.Shared.Next(usersIds.Count)],
          changes: new()
          {
            (nameof(EditUserRequest.MiddleName), "NewName"),
            (nameof(EditUserRequest.About), "test user")
          }), expected),
          timeout: _responseTimeout
      );

      return ScenarioBuilder
        .CreateScenario("edit_users", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    private Scenario Create(
      Guid? departmentId,
      Guid? roleId,
      Guid? companyId,
      HttpStatusCode expected = HttpStatusCode.Created)
    {
      var correct = Step.Create(
        "create",
        async context =>
        {
          return CreateResponse(await _usersController.Create(
            new CreateUserRequest()
            {
              FirstName = "Loadtest",
              LastName = "Load",
              IsAdmin = false,
              DepartmentId = departmentId,
              RoleId = roleId,
              Password = "!Str0ng7913",
              UserCompany = new()
              {
                CompanyId = companyId.HasValue ? companyId.Value : default,
                Rate = 0.5,
                ContractTermType = ContractTerm.FixedTerm,
                StartWorkingAt = DateTime.UtcNow
              },
              AvatarImage = new()
              {
                Name = Guid.NewGuid().ToString(),
                Content = avatarContent,
                Extension = ".png",
                IsCurrentAvatar = true
              },
              Communication = new()
              {
                Value = $"test{Guid.NewGuid()}@nоtreаl.tsts"
              }
            }), expected);
        },
        timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("create_user", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public UsersScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _usersController = new(settings.Token);
      _genderController = new(settings.Token);
      _rolesController = new(settings.Token);
      _departmentController = new(settings.Token);
      _companyController = new(settings.Token);
    }

    public override async Task RunAsync()
    {
      #region preparation

      List<Guid> usersIds = JsonConvert
        .DeserializeObject<FindResultResponse<UserInfo>>(await
          (await _usersController.Find(
            skipCount: 0,
            takeCount: int.MaxValue,
            isActive: true))?.Content.ReadAsStringAsync())?
          .Body?.Select(x => x.Id)?.ToList();

      List<Guid> gendersIds = JsonConvert
        .DeserializeObject<FindResultResponse<GenderInfo>>(await
          (await _genderController.Find(0, int.MaxValue))?.Content.ReadAsStringAsync())?
        .Body?.Select(x => x.Id)?.ToList();

      Guid? departmentId = JsonConvert
        .DeserializeObject<FindResultResponse<DM.DepartmentInfo>>(await
          (await _departmentController.Find(
            skipCount: 0,
            takeCount: 1,
            isActive: true))?.Content.ReadAsStringAsync())?
        .Body?.FirstOrDefault()?.Id;

      Guid? roleId = JsonConvert
        .DeserializeObject<FindResultResponse<RM.RoleInfo>>(await
          (await _rolesController.Find(
            skipCount: 0,
            takeCount: 1,
            includeDeactivated: false))?.Content.ReadAsStringAsync())?
        .Body?.FirstOrDefault()?.Id;

      Guid? companyId = JsonConvert
        .DeserializeObject<OperationResultResponse<CompanyResponse>>(await
          (await _companyController.Get())?.Content.ReadAsStringAsync())?
        .Body?.Id;

      List <(string property, string newValue)> changesForUser = new()
      {
        (nameof(EditUserRequest.About), "smth new"),
      };

      #endregion

      NBomberRunner
        .RegisterScenarios(Find(HttpStatusCode.OK))
        .WithReportFolder($"{_path}/find_user")
        .WithReportFileName("find_users")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();

      if (usersIds is not null && usersIds.Any())
      {
        NBomberRunner
        .RegisterScenarios(Get(usersIds, HttpStatusCode.OK))
        .WithReportFolder($"{_path}/get_user")
        .WithReportFileName("get_user")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();
      }

      /*NBomberRunner
        .RegisterScenarios(
          Create(
            departmentId: departmentId,
            companyId: companyId,
            roleId: roleId))
        .WithReportFolder($"{_path}/create_user")
        .WithReportFileName("create")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();

      NBomberRunner
        .RegisterScenarios(
          Edit(usersIds: usersIds))
        .WithReportFolder($"{_path}/edit_user")
        .WithReportFileName("edit")
        .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
        .Run();*/
    }
  }
}
