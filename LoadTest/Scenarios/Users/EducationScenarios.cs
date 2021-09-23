using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Users.Enums;
using DigitalOffice.LoadTesting.Models.Users.Models;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Education;
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
    public class EducationScenarios : BaseScenarioCreator
    {
        private readonly EducationController _educationController;
        private readonly UsersController _usersController;

        private Scenario Create(CreateEducationRequest request, out List<Guid> createdEducations, HttpStatusCode expected)
        {
            List<Guid> createdEducationsIds = new();

            var correct = Step.Create("create", async context =>
            {
                var response = await _educationController.Create(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    createdEducationsIds.Add(JsonConvert.DeserializeObject<OperationResultResponse<Guid>>(await response.Content.ReadAsStringAsync()).Body);
                }

                return CreateResponse(response, expected);
            });

            createdEducations = createdEducationsIds;

            return ScenarioBuilder
                .CreateScenario("create_education", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Edit(Guid educationId, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _educationController.Edit(educationId, new()
                {
                    (nameof(EditEducationRequest.QualificationName), "LoadTest"),
                    (nameof(EditEducationRequest.UniversityName), "LoadTest")
                }), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_education", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Remove(List<Guid> educationsIds, HttpStatusCode expected)
        {
            var correct = Step.Create("remove", async context =>
                {
                    Guid? educationId = educationsIds.FirstOrDefault();
                    if (educationId == null)
                    {
                        return Response.Ok();
                    }

                    educationsIds.RemoveAt(0);

                    return CreateResponse(await _educationController.Remove(educationId.Value), expected);
                });

            return ScenarioBuilder
                .CreateScenario("remove_education", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public EducationScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _usersController = new(settings.Token);
            _educationController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? userId = JsonConvert
                .DeserializeObject<FindResultResponse<UserInfo>>(
                    _usersController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            List<Guid> createdEducationsIds = null;

            if (userId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(Create(
                    new CreateEducationRequest()
                    {
                        UserId = userId.Value,
                        FormEducation = FormEducation.FullTime,
                        AdmissionAt = DateTime.UtcNow,
                        IssueAt = DateTime.UtcNow,
                        QualificationName = "LoadTest",
                        UniversityName = "LoadTest"
                    },
                    out createdEducationsIds,
                    HttpStatusCode.OK))
                .WithReportFolder($"{_path}/create_education")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            if (createdEducationsIds == null || !createdEducationsIds.Any())
            {
                return;
            }

            NBomberRunner
                .RegisterScenarios(Edit(createdEducationsIds.First(), HttpStatusCode.OK))
                .WithReportFolder($"{_path}/edit_education")
                .WithReportFileName("correct_edit")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(Remove(createdEducationsIds, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/remove_education")
                .WithReportFileName("correct_remove")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
