using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Scenarios.Company;
using DigitalOffice.LoadTesting.Scenarios.Message;
using DigitalOffice.LoadTesting.Scenarios.Project;
using DigitalOffice.LoadTesting.Scenarios.Rights;
using DigitalOffice.LoadTesting.Scenarios.Time;
using DigitalOffice.LoadTesting.Services.Auth;
using DigitalOffice.LoadTesting.Services.User;
using System;

namespace DigitalOffice.LoadTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new ScenarioStartSettings()
            {
                Path = "C://users/Egor/test",
                During = TimeSpan.FromSeconds(6),
                WarmUpTime = TimeSpan.FromSeconds(5),
                Rate = 20,
                Token = (new AuthController()).Auth("Egorka", "egor123").Result.AccessToken
            };

            EducationScenarios education = new(settings);
            CertificateScenarios certificate = new(settings);
            CommunicationScenarios communication = new(settings);
            UsersScenarios users = new(settings);

            PositionScenarios position = new(settings);
            DepartmentScenarios department = new(settings);
            CompanyScenarios company = new(settings);
            OfficeScenarios office = new(settings);

            WorkspaceScenarios workspace = new(settings);
            ChannelScenarios channel = new(settings);

            ProjectScenarios project = new(settings);
            TaskPropertyScenarios taskProperty = new(settings);
            TaskScenarios task = new(settings);

            RightsScenarios rights = new(settings);
            RolesScenarios roles = new(settings);

            ImportScenarios import = new(settings);
            LeaveTimeScenarios leaveTime = new(settings);
            StatScenarios stat = new(settings);
            WorkTimeDayJobScenarios workTimeDayJob = new(settings);
            WorkTimeMonthLimitScenarios workTimeMonthLimit = new(settings);
            WorkTimeScenarios workTime = new(settings);

            education.Run();
            certificate.Run();
            communication.Run();
            users.Run();

            position.Run();
            department.Run();
            company.Run();
            office.Run();

            workspace.Run();
            channel.Run();

            project.Run();
            task.Run();
            taskProperty.Run();

            rights.Run();
            roles.Run();

            import.Run();
            leaveTime.Run();
            stat.Run();
            workTime.Run();
            workTimeDayJob.Run();
            workTimeMonthLimit.Run();
        }
    }
}
