using DigitalOffice.LoadTesting.Helpers;
using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Message.Requests.Channel;
using DigitalOffice.LoadTesting.Services;
using DigitalOffice.LoadTesting.Services.Message;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using System;
using System.Net;

namespace DigitalOffice.LoadTesting.Scenarios.Message
{
    public class ChannelScenarios : BaseScenarioCreator
    {
        private readonly ChannelController _channelController;

        private Scenario Create(CreateChannelRequest request, bool generateUniqueName, HttpStatusCode expected)
        {
            var correct = Step.Create("create", async context =>
            {
                if (generateUniqueName)
                {
                    request.Name = CreatorUniqueName.Generate();
                }

                return CreateResponse(await _channelController.Create(request), expected);
            });

            return ScenarioBuilder
                .CreateScenario("create_channel", correct)
                .WithWarmUpDuration(_warmUpTime)
                .WithLoadSimulations(new[]
                {
                    Simulation.InjectPerSec(_rate, _during)
                });
        }

        public ChannelScenarios(ScenarioStartSettings settings)
            : base(settings)
        {
            _channelController = new(settings.Token);
        }

        public override void Run()
        {
            NBomberRunner
                .RegisterScenarios(
                    Create(
                        new(),
                        true,
                        HttpStatusCode.Created))
                .WithReportFolder($"{_path}/create_channel")
                .WithReportFileName("correct_create")
                .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
                .Run();
        }
    }
}
