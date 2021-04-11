using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace GUITestProject
{
    public class Program
    {
        private static ILogger<Program> _logger;
        private static ServiceUrls _serviceUrls;

        #region private methods

        private static void SetLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("LT.DigitalOffice.Tests.Startup", LogLevel.Trace)
                    .AddConsole();
            });

            _logger = loggerFactory.CreateLogger<Program>();
        }

        private static void SetServiceUrls()
        {
            var pathToAppsettings = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(pathToAppsettings + "appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();

            _serviceUrls = configuration
                .GetSection(nameof(ServiceUrls))
                .Get<ServiceUrls>();
        }

        static public void CloneBuildRunRepositories()
        {
            var servicesPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\"));

            var servicesInfo = _serviceUrls
                .GetType()
                .GetProperties()
                .Select(x => (ServiceName: x.Name, Url: (string)x.GetValue(_serviceUrls)))
                .ToList();

            foreach (var (ServiceName, Url) in servicesInfo)
            {
                var pathToService = $"{servicesPath}Services\\{ServiceName}";

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        CreateNoWindow = false
                    }
                };

                if (!Directory.Exists(pathToService))
                {
                    process.StartInfo.Arguments = $"clone -b develop {Url} {pathToService}";

                    RunProcess(process).WaitForExit();
                }
                else
                {
                    process.StartInfo.Arguments = $"pull {Url}";

                    RunProcess(process).WaitForExit();
                }
            };

            foreach (var (ServiceName, Url) in servicesInfo)
            {
                var pathToService = $"{servicesPath}Services\\{ServiceName}";

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "build --configuration Release",
                        WorkingDirectory = $"{servicesPath}Services\\{ServiceName}",
                        CreateNoWindow = false
                    }
                };

                RunProcess(process).WaitForExit();
            };

            List<Process> runningServices = new();

            foreach (var (ServiceName, Url) in servicesInfo)
            {
                var pathToExe = $"{servicesPath}Services\\{ServiceName}" +
                    $"\\src\\{ServiceName}\\bin\\Release\\net5.0";

                var exeName = $"LT.DigitalOffice.{ServiceName}.exe";

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = $"{pathToExe}\\{exeName}",
                        WorkingDirectory = pathToExe,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        RedirectStandardError = true
                    }
                };

                runningServices.Add(RunProcess(process));
            };

            Thread.Sleep(new TimeSpan(0, 1, 0));

            runningServices.ForEach(x => x.Kill());

            Console.ReadKey();
        }

        private static Process RunProcess(Process process)
        {
            process.StartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Development";

            var command = $"{process.StartInfo.FileName} {process.StartInfo.Arguments}";

            try
            {
                process.Start();

                _logger.LogInformation($"Command successfully invoked: '{command}'");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Command failed invoked: '{command}'");

                _logger.LogWarning(e, e.Message);
            }

            return process;
        }

        #endregion

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            SetLogger();
            SetServiceUrls();

            CloneBuildRunRepositories();
        }
    }
}
