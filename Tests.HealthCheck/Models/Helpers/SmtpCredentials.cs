namespace Tests.HealthCheck.Models.Helpers
{
    internal static class SmtpCredentials
    {
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static bool EnableSsl { get; set; }
        public static string Email { get; set; }
        public static string Password { get; set; }
    }
}
