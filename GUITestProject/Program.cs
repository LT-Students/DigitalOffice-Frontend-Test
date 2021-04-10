using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

            // FOR TESTS
            //servicesInfo = new List<(string, string)>
            //{
            //    ("AuthService", @"https://github.com/LT-Students/DigitalOffice-AuthService")
            //};

            Parallel.ForEach(servicesInfo, serviceInfo =>
            {
                var processName = "git";

                var branchArgument = "-b develop";

                var pathToService = $"{servicesPath}Services\\{serviceInfo.ServiceName}";

                if (!Directory.Exists(pathToService))
                {
                    RunProcess(processName, $"clone {branchArgument} {serviceInfo.Url} {pathToService}");
                }
                else
                {
                    RunProcess(processName, $"fetch {serviceInfo.Url}");
                }
            });

            Parallel.ForEach(servicesInfo, serviceInfo =>
            {
                var buildArguments = "/p:configuration=Release";

                var slnName = $"LT.DigitalOffice.{serviceInfo.ServiceName}.sln";

                var pathToSln = $"{servicesPath}Services\\{serviceInfo.ServiceName}\\{slnName}";

                //var pathToMSBuild = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319";
                var pathToMSBuild = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319";

                var mSBuild = pathToMSBuild + @"\MSBuild.exe";

                var buildPath = $"{servicesPath}Services\\{serviceInfo.ServiceName}\\src\\{serviceInfo.ServiceName}\\{serviceInfo.ServiceName}.csproj";

                //RunProcess("cmd", $"msbuild -t:build -restore {pathToSln} {buildArguments}");
                //RunProcess("cmd", $"cd {pathToMSBuild} msbuild -t:build -restore {pathToSln} {buildArguments}");
                //RunProcess(mSBuild, $"-t:restore {pathToSln}");
                //RunProcess(mSBuild, $"-t:build {pathToSln} {buildArguments}");
                //RunProcess(mSBuild, $"msbuild -t:build -restore {pathToSln} {buildArguments}");
                //RunProcess(mSBuild, $"msbuild -restore {pathToSln}");
                //RunProcess(mSBuild, $"msbuild -t:build {pathToSln} {buildArguments}");
                //RunProcess("cmd", $"cd {pathToMSBuild} msbuild -t:build -restore {pathToSln} {buildArguments}");
                //var mSBuild = pathToMSBuild + @"\MSBuild.exe";
                //RunProcess("cmd", $"{mSBuild} -t:build -restore {pathToSln} {buildArguments}");
                //RunProcess(mSBuild, $"msbuild -t:-build {buildPath} {buildArguments}");
            });

            Parallel.ForEach(servicesInfo, serviceInfo =>
            {
                var exeName = $"LT.DigitalOffice.{serviceInfo.ServiceName}.exe";

                var pathToExe = $"{servicesPath}Services\\{serviceInfo.ServiceName}" +
                    $"\\src\\{serviceInfo.ServiceName}\\bin\\Debug\\netcoreapp3.1\\{exeName}";

                //var pathToExe = $"{servicesPath}Services\\{serviceInfo.ServiceName}" +
                //    $"\\src\\{serviceInfo.ServiceName}\\bin\\Debug\\netcoreapp3.1";

                //RunProcess("cmd", $"{pathToExe}");

                //var process = new Process { StartInfo = new ProcessStartInfo { WorkingDirectory = pathToExe, FileName = exeName } };
                //process.Start();
            });

            Console.ReadKey();
        }

        private static void RunProcess(string processName, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = processName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            var command = $"{processName} {arguments}";

            try
            {
                process.Start();

                _logger.LogInformation($"Command successfully invoked: '{command}'");

                if (processName != "cmd") // TODO: How to know when to kill the cmd process? git dies by himself
                {
                    string output = process.StandardOutput.ReadToEnd();
                    if (!string.IsNullOrEmpty(output))
                    {
                        _logger.LogInformation($"The command '{command}' result: '{output}'");
                    }

                    process.WaitForExit();
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Command failed invoked: '{command}'");

                _logger.LogWarning(e, e.Message);
            }
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
