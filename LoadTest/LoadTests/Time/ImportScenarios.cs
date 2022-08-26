using DigitalOffice.LoadTesting.Models;
using DigitalOffice.LoadTesting.Models.Responses.Templates;
using DigitalOffice.LoadTesting.Models.Time.Filters;
using DigitalOffice.LoadTesting.Services.Time;
using LT.DigitalOffice.LoadTesting.LoadTests;
using LT.DigitalOffice.LoadTesting.Models.Department.Models;
using LT.DigitalOffice.LoadTesting.Services.Department;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DigitalOffice.LoadTesting.Scenarios.Time
{
  public class ImportScenarios : BaseScenarioCreatorAsync
  {
    private readonly ImportController _importController;
    private readonly DepartmentController _departmentController;

    private Scenario Get(ImportStatFilter filter, HttpStatusCode expected)
    {
      var correct = Step.Create("get", async context => CreateResponse(await _importController.Get(filter), expected), timeout: _responseTimeout);

      return ScenarioBuilder
        .CreateScenario("get_import", correct)
        .WithWarmUpDuration(_warmUpTime)
        .WithLoadSimulations(new[]
        {
          Simulation.KeepConstant(_rate, _during)
        });
    }

    public ImportScenarios(ScenarioStartSettings settings)
      : base(settings)
    {
      _importController = new(settings.Token);
      _departmentController = new(settings.Token);
    }

    public override async Task RunAsync()
    {
      Guid? departmentId = JsonConvert
        .DeserializeObject<FindResultResponse<DepartmentInfo>>(await
          (await _departmentController.Find(0, 1))?.Content.ReadAsStringAsync())?
        .Body?.FirstOrDefault()?.Id;

      if (departmentId.HasValue)
      {
        //TODO rework
        NBomberRunner
          .RegisterScenarios(
            Get(new()
            {
              DepartmentId = departmentId.Value,
              Year = DateTime.UtcNow.Year,
              Month = DateTime.UtcNow.Month
            },
            HttpStatusCode.OK))
          .WithReportFolder($"{_path}/get_import")
          .WithReportFileName("get_import")
          .WithReportFormats(ReportFormat.Txt, ReportFormat.Html)
          .Run();
      }
    }
  }
}
