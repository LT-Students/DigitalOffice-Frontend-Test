using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Users.Enums;
using DigitalOffice.LoadTesting.Models.Users.Models;
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
    public class CommunicationScenarios : BaseScenarioCreator
    {
        private readonly CommunicationController _communicationController;
        private readonly UsersController _usersController;

        private Scenario Create(CreateCommunicationRequest request, out List<Guid> createdCommunication, HttpStatusCode expected)
        {
            List<Guid> createdEducationsIds = new();

            var correct = Step.Create("create", async context =>
            {
                request.Value = Guid.NewGuid().ToString();

                var response = await _communicationController.Create(request);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    createdEducationsIds.Add(
                        JsonConvert.DeserializeObject<OperationResultResponse<Guid>>(
                            await response.Content.ReadAsStringAsync()).Body);
                }

                return CreateResponse(response, expected);
            });

            createdCommunication = createdEducationsIds;

            return ScenarioBuilder
                .CreateScenario("create_communication", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Edit(Guid communicationId, HttpStatusCode expected)
        {
            var correct = Step.Create("edit", async context =>
                CreateResponse(await _communicationController.Edit(communicationId, new()
                {
                    (nameof(EditCommunicationRequest.Value), "LoadTest")
                }), expected)
            );

            return ScenarioBuilder
                .CreateScenario("edit_communication", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        private Scenario Remove(List<Guid> communicationsIds, HttpStatusCode expected)
        {
            var correct = Step.Create("remove", async context =>
                {
                    Guid? communicationId = communicationsIds.FirstOrDefault();
                    if (communicationId == null)
                    {
                        return Response.Ok();
                    }

                    communicationsIds.RemoveAt(0);

                    return CreateResponse(await _communicationController.Remove(communicationId.Value), expected);
                });

            return ScenarioBuilder
                .CreateScenario("remove_communication", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public CommunicationScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _usersController = new(settings.Token);
            _communicationController = new(settings.Token);
        }

        public override void Run()
        {
            Guid? userId = JsonConvert
                .DeserializeObject<FindResultResponse<UserInfo>>(
                    _usersController.Find(0, 1).Result.Content.ReadAsStringAsync().Result)
                .Body
                .FirstOrDefault()
                ?.Id;

            List<Guid> createdCommunicationsIds = null;

            if (userId.HasValue)
            {
                NBomberRunner
                .RegisterScenarios(Create(
                    new CreateCommunicationRequest()
                    {
                        UserId = userId,
                        Type = CommunicationType.Telegram,
                        Value = null
                    },
                    out createdCommunicationsIds,
                    HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_communication")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
            }

            if (createdCommunicationsIds == null || !createdCommunicationsIds.Any())
            {
                return;
            }

            NBomberRunner
                .RegisterScenarios(Edit(createdCommunicationsIds.First(), HttpStatusCode.OK))
                .WithReportFolder($"{_path}/edit_communication")
                .WithReportFileName("correct_edit")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();

            NBomberRunner
                .RegisterScenarios(Remove(createdCommunicationsIds, HttpStatusCode.OK))
                .WithReportFolder($"{_path}/remove_communication")
                .WithReportFileName("correct_remove")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
