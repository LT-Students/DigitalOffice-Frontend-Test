using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Users.Enums;
using DigitalOffice.LoadTesting.Models.Users.Models;
using DigitalOffice.LoadTesting.Models.Users.Requests.User;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Communication;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DigitalOffice.LoadTesting.Services.User
{
    public class UsersScenarios : BaseScenarioCreator
    {
        private readonly UsersController _usersController;
        private readonly PositionController _positionController;
        private readonly DepartmentController _departmentController;
        private readonly OfficeController _officeController;
        private readonly RolesController _rolesController;

        private Scenario Get(Guid userId, HttpStatusCode expected)
        {
            var correct = Step.Create("get", async context => CreateResponse(await _usersController.Get(userId), expected));

            return ScenarioBuilder
                .CreateScenario("get_user", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Find(int skipCount, int takeCount, HttpStatusCode expected)
        {
            var correct = Step.Create("find", async context => CreateResponse(await _usersController.Find(skipCount, takeCount), expected));

            return ScenarioBuilder
                .CreateScenario("find_users", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }


        private Scenario Edit(Guid userId, List<(string property, string newValue)> changes, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context => 
                CreateResponse(await _usersController.Edit(userId, changes), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_users", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Create(CreateUserRequest request, bool generateUniqueEmail, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
            {
                if (generateUniqueEmail)
                {
                    request.Communications.First().Value = $"test{Guid.NewGuid()}@gmail.com";
                }

                return CreateResponse(await _usersController.Create(request), expected);
            });
            
            return ScenarioBuilder
                .CreateScenario("create_user", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public UsersScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _usersController = new(settings.Token);
            _rolesController = new(settings.Token);
            _positionController = new(settings.Token);
            _departmentController = new(settings.Token);
            _officeController = new(settings.Token);
        }

        public override void Run()
        {
            #region warmup

            Guid? userId = JsonConvert
                .DeserializeObject<FindResultResponse<UserInfo>>(_usersController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                ?.Body
                ?.FirstOrDefault()
                ?.Id;

            Guid? positionId = JsonConvert
                .DeserializeObject<FindResultResponse<PositionInfo>>(_positionController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            Guid? departmentId = JsonConvert
                .DeserializeObject<FindResultResponse<DepartmentInfo>>(_departmentController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            Guid? officeId = JsonConvert
                .DeserializeObject<FindResultResponse<OfficeInfo>>(_officeController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            Guid? roleId = JsonConvert
                .DeserializeObject<FindResultResponse<RoleInfo>>(_rolesController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                ?.Body
                ?.FirstOrDefault()
                ?.Id;

            List<(string property, string newValue)> changesForUser = new()
            {
                (nameof(EditUserRequest.About), "smth new"),
            };

            if (positionId.HasValue)
            {
                changesForUser.Add((nameof(EditUserRequest.PositionId), positionId.ToString()));
            }

            if (departmentId.HasValue)
            {
                changesForUser.Add((nameof(EditUserRequest.DepartmentId), departmentId.ToString()));
            }

            if (officeId.HasValue)
            {
                changesForUser.Add((nameof(EditUserRequest.OfficeId), officeId.ToString()));
            }

            if (roleId.HasValue)
            {
                changesForUser.Add((nameof(EditUserRequest.RoleId), roleId.ToString()));
            }

            #endregion

            NBomberRunner
                .RegisterScenarios(Find(0, 20, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/find_user")
                .WithReportFileName("correct_find")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            if (userId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(Get(userId.Value, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/get_user")
                .WithReportFileName("get")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            if (positionId.HasValue && officeId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(
                    Create(
                        new CreateUserRequest()
                        {
                            FirstName = "Load",
                            LastName = "Load",
                            IsAdmin = false,
                            Status = UserStatus.Sick,
                            DepartmentId = departmentId,
                            PositionId = positionId.Value,
                            OfficeId = officeId.Value,
                            RoleId = roleId,
                            Password = "123456789",
                            Rate = 1,
                            StartWorkingAt = DateTime.UtcNow,
                            Communications = new List<CreateCommunicationRequest>()
                            {
                                new()
                                {
                                    Type = CommunicationType.Email
                                }
                            }
                        },
                        true,
                        HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_user")
                .WithReportFileName("create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            NBomberRunner
                .RegisterScenarios(
                    Edit(
                        userId.Value,
                        changesForUser,
                        HttpStatusCode.OK))
                .WithReportFolder($"{_path}/edit_user")
                .WithReportFileName("edit")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
