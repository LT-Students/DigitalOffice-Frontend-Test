using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Users.Enums;
using DigitalOffice.LoadTesting.Models.Users.Models;
using DigitalOffice.LoadTesting.Models.Users.Requests.User;
using DigitalOffice.LoadTesting.Models.Users.Requests.User.Certificates;
using LoadTesting.Scenarios;
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
    public class CertificateScenarios : BaseScenarioCreator
    {
        private readonly CertificateController _certificateController;
        private readonly UsersController _usersController;

        private Scenario Create(CreateCertificateRequest request, out List<Guid> createdCertificates, HttpStatusCode expected)
        {
            List<Guid> createdCertificatesIds = new();

            var correct = Step.Create("create", async context =>
            {
                var response = await _certificateController.Create(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    createdCertificatesIds.Add(
                        JsonConvert.DeserializeObject<OperationResultResponse<Guid>>(
                            await response.Content.ReadAsStringAsync()).Body);
                }

                return CreateResponse(response, expected);
            });

            createdCertificates = createdCertificatesIds;

            return ScenarioBuilder
                .CreateScenario("create_certificate", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Edit(Guid CertificateId, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _certificateController.Edit(CertificateId, new()
                {
                    (nameof(EditCertificateRequest.Name), "LoadTest")
                }), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_certificate", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Remove(List<Guid> CertificatesIds, HttpStatusCode expected)
        {
            var correct = Step.Create("remove", async context =>
                {
                    Guid? CertificateId = CertificatesIds.FirstOrDefault();
                    if (CertificateId == null)
                    {
                        return Response.Ok();
                    }

                    CertificatesIds.RemoveAt(0);

                    return CreateResponse(await _certificateController.Remove(CertificateId.Value), expected);
                });

            return ScenarioBuilder
                .CreateScenario("remove_certificate", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public CertificateScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _usersController = new(settings.Token);
            _certificateController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? userId = JsonConvert
                .DeserializeObject<FindResultResponse<UserInfo>>(_usersController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            List<Guid> createdCertificatesIds = null;

            if (userId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(Create(
                    new CreateCertificateRequest()
                    {
                        Name = "LoadTest",
                        SchoolName = "LoadTest",
                        ReceivedAt = DateTime.UtcNow,
                        EducationType = EducationType.Online,
                        UserId = userId.Value,
                        Image = new AddImageRequest
                        {
                            Content = Image.Content,
                            Extension = Image.Extension,
                            Name = "LoadTest"
                        }
                    },
                    out createdCertificatesIds,
                    HttpStatusCode.OK))
                .WithReportFolder($"{_path}/create_certificate")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            if (createdCertificatesIds == null || !createdCertificatesIds.Any())
            {
                return;
            }

            NBomberRunner
                .RegisterScenarios(Edit(createdCertificatesIds.First(), HttpStatusCode.OK))
                .WithReportFolder($"{_path}/edit_certificate")
                .WithReportFileName("correct_edit")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(Remove(createdCertificatesIds, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/remove_certificate")
                .WithReportFileName("correct_remove")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
