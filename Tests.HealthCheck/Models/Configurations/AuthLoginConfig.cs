namespace Tests.HealthCheck.Models.Configurations
{
    public class AuthLoginConfig
    {
        public const string SectionName = "AuthSettings";

        public string UriString { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}