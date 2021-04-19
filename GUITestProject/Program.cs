using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GUITestProject
{
    public class Program
    {
        private static ILogger<Program> _logger;
        private static List<(string ServiceName, string Url)> _servicesInfo;
        private static List<Process> _runningServices = new();

        private static string ServicesPath => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\"));

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

            var serviceUrls = configuration
                .GetSection(nameof(ServiceUrls))
                .Get<ServiceUrls>();

            _servicesInfo = serviceUrls
                .GetType()
                .GetProperties()
                .Select(x => (ServiceName: x.Name, Url: (string)x.GetValue(serviceUrls)))
                .ToList();
        }

        private static void CloneOrFetchServices()
        {
            foreach (var (ServiceName, Url) in _servicesInfo)
            {
                var pathToService = $"{ServicesPath}Services\\{ServiceName}";

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
        }

        private static void BuidServices()
        {
            foreach (var (ServiceName, Url) in _servicesInfo)
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "build --configuration Release",
                        WorkingDirectory = $"{ServicesPath}Services\\{ServiceName}",
                        CreateNoWindow = false
                    }
                };

                RunProcess(process).WaitForExit();
            };
        }

        private static void RunServices()
        {
            foreach (var (ServiceName, Url) in _servicesInfo)
            {
                var pathToExe = $"{ServicesPath}Services\\{ServiceName}" +
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

                _runningServices.Add(RunProcess(process));
            };  
        }

        private static void KillServicesProcesses()
        {
            _runningServices.ForEach(x => x.Kill());
            _runningServices.Clear();
        }

        private static Process RunProcess(Process process)
        {
            process.StartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = "Development";

            var command = $"{process.StartInfo.FileName} {process.StartInfo.Arguments}";

            try
            {
                process.Start();

                _logger.LogInformation($"Command '{command}' successfully invoked for proccess named {process.ProcessName}");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Command '{command}' failed invoked for proccess named {process.ProcessName}");

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

            CloneOrFetchServices();
            BuidServices();
            RunServices();

            Console.WriteLine("Enter a key to kill all processes.");
            Console.ReadKey();

            KillServicesProcesses();
        }
    }
}
