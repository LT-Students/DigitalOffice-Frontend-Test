using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Project.Enums;
using DigitalOffice.LoadTesting.Models.Project.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Rights.Responses;
using DigitalOffice.LoadTesting.Models.Users.Models;
using DigitalOffice.LoadTesting.Models.Users.Requests.User;
using DigitalOffice.LoadTesting.Services.Project;
using DigitalOffice.LoadTesting.Services.User;
using LT.DigitalOffice.LoadTesting.LoadTests;
using LT.DigitalOffice.LoadTesting.Models.Common.Enums;
using LT.DigitalOffice.LoadTesting.Models.Company.Responses;
using LT.DigitalOffice.LoadTesting.Models.Project.Requests.User;
using LT.DigitalOffice.LoadTesting.Models.Rights.Models;
using LT.DigitalOffice.LoadTesting.Models.Rights.Requests;
using LT.DigitalOffice.LoadTesting.Services.Department;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LT.DigitalOffice.LoadTesting.Helpers
{
  public class EntitiesCreator : BaseScenarioCreatorAsync
  {
    private readonly int _initRate = 1;
    private readonly TimeSpan _initDuration = TimeSpan.FromSeconds(50);

    private readonly UsersController _usersController;
    private readonly DepartmentController _departmentController;
    private readonly RolesController _rolesController;
    private readonly RightsController _rightsController;
    private readonly CompanyController _companyController;
    private readonly ProjectController _projectController;

    const string avatarContent = "iVBORw0KGgoAAAANSUhEUgAAAHQAAABgCAIAAABDv8H9AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAAQ3SURBVHhe7ZstVOswFIDrmKMSB3IHBYJzJpHISmTlJBI3WYlETiInd1DISSQSOTmJ5H2PXHL6+ro/lrs14X5qS7q0+XJ7mzRn2aehhslVxOQqYnIVMbmKmFxFTK4iJlcRk6uIyVXE5CpichUxuYqYXEVMriImVxGTq4jJVcTkKmJyFTG5iphcRUyuIiZXEZOriMlVxOQqYnIVMbmKmNzATKfT6+vr8XjMZ5MbmJOTkyzLer0en4PJfXl58SP2O1ksFnd3d5h1UBJMLmZdo/P5XIp+B/SXkCrLMs9zZwBub2+pCib35ubGtfv4+ChFKYJK7lH6OBqN6HK/33e9rkP5x8cHBweTy+i5pglhKYqW9/d3DAIGgaikU4PBwHVwBWdnZ/XEGEwuGYcs7s7BBUlpV2nVB+76t4JeE6pVVc1mM2n9m2ByYTgcygm75/f19ZWnzc/01SE2aYSm6CBjQ7NygjZCyiXR+MwLXfDrnGJErmkzLi4uMFgUxd+oHo0mk8laj62ElAsd8bvWaSh9qwksFxp+uWip0GeFU+b2ZK19XgyElwt1v8SIm5fo0TWnHhW5wHzQTx4eHh6kNCidderRkgs4dV3FcsBlW/edehTlkg3ICa7bbjm4CxE59SjKBTosAn6aHGJ06tGVC8SsyMiy6XQqpeuI2qlHXW49OeR5/vb2JhVtpOHUoy4XWMijxjlC3GKxkIpvEnPq2YdcmM1mfmbG0sjNfFN16tmTXHh6ehJzWXZ+fn56eipfaqTh1LM/ucTp1dWVWPyXxJx61OWuuPfh8vLy+flZDk0OLbnMCpY5PT4+Pjo6ki9Z1u/3/3/NnAaB5bLMZbHQurNUv/c5rP7mDO7v77Xf7+yfMHLxMh6PG74cK/IpP6nvmDIkq2fB0bGrXBZdZVn6aZaHEtZmk8lEjltCI4T5VUqbxz+Uy7qAG5moFCs1mMYSkv+vFFaA0PrwFEWx1c87y9Zyma623v7c1FVVIV2O2xISgl8lA8OWwMxsU7nLQtWlVOZbctwOkLg5hbT7BaMldXGyXu6yUKWQKjkoHCTx+hAeaoszCEvlusl/a6gSXz++/TeBhEvilvPF7LdFLsmudfLP03/zF7I7Qoqo3y6R+m2RW48aQDS5L+Am2IYk4LdFLt2gM+5JddhHdux+1z/QDkvUfrsuF+L1G4FciNRvHHIhRr/RyIXo/MYkFxp+WeZQInXdIzK50PA7GAxUl4u7EJ9caPjN83zti+ODEKVcBzlX7H7RwRQcsVxgAdnlV2hxy4X5fN7ZV2jRy4VGCi7LsiO7GCnIhYZfIF0o/V1gcxKRC/gtikLUftHr9ep/Ft0/6ch1uA0Uv5fMB6k4BKnJdVRV5eQOh0MpOgRpyu0IJlcRk6uIyVXE5CpichUxuYqYXEVMriImVxGTq4jJVcTkKmJyFTG5iphcNT4//wB4lLqpi9HRMQAAAABJRU5ErkJggg==";

    private Scenario CreateUsers(
      Guid? departmentId,
      List<Guid> rolesIds,
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
              RoleId = rolesIds?[Random.Shared.Next(rolesIds.Count)],
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
        .CreateScenario("init_users", correct)
        .WithoutWarmUp()
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_initRate, _initDuration)
        });
    }

    private Scenario CreateProjects(
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
      },
      timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("create_project", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.InjectPerSec(_initRate, _initDuration)
        });
    }

    public EntitiesCreator(ScenarioStartSettings settings) : base(settings)
    {
      _usersController = new(settings.Token);
      _departmentController = new(settings.Token);
      _rolesController = new(settings.Token);
      _rightsController = new(settings.Token);
      _companyController = new(settings.Token);
      _projectController = new(settings.Token);
    }

    public override async Task Run()
    {
      List<int> rights = JsonConvert.DeserializeObject<OperationResultResponse<List<RightInfo>>>(
        await (await _rightsController.GetRightsList()).Content.ReadAsStringAsync())?
        .Body?.Select(x => x.RightId).ToList();

      foreach (var right in rights)
      {
        await _rolesController.Create(new()
        {
          Localizations = new List<CreateRoleLocalizationRequest>()
          {
            new()
            {
              Locale = "ru",
              Name = CreatorUniqueName.Generate()
            }
          },
          Rights = new() { right }
        });
      }

      List<Guid> rolesIds = JsonConvert.DeserializeObject<FindResultResponse<RoleInfo>>(await
       (await _rolesController.Find(0, int.MaxValue)).Content.ReadAsStringAsync())
        .Body?.Select(x => x.Id).ToList();

      Guid? departmentId = JsonConvert.DeserializeObject<OperationResultResponse<Guid?>>(await
        (await _departmentController.Create(
          new()
          {
            Name = "NewDepartmentForTests",
            ShortName = "NewTe",
            Users = new()
          }))?
        .Content.ReadAsStringAsync())?.Body;

      Guid? companyId = JsonConvert.DeserializeObject<OperationResultResponse<CompanyResponse>>(await
        (await _companyController.Get())?.Content.ReadAsStringAsync())?.Body?.Id;

      NBomberRunner
        .RegisterScenarios(CreateUsers(departmentId: departmentId, rolesIds: rolesIds, companyId: companyId))
        .Run();

      List<UserInfo> users = JsonConvert
        .DeserializeObject<FindResultResponse<UserInfo>>(await
          (await _usersController.Find(
            skipCount: 0,
            takeCount: int.MaxValue,
            isActive: true)).Content.ReadAsStringAsync())?.Body;

      NBomberRunner
        .RegisterScenarios(CreateProjects(
          departmentId: departmentId,
          users: users.Select(u => new UserRequest()
          {
            UserId = u.Id,
            Role = ProjectUserRoleType.Employee
          }).ToList()))
        .Run();

      Console.WriteLine("success");
    }
  }
}
