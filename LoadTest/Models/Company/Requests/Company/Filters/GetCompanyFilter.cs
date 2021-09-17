namespace DigitalOffice.LoadTesting.Models.Company.Requests.Company.Filters
{
    public record GetCompanyFilter
    {
        public bool? IncludeDepartments { get; set; } = true;
        public bool? IncludePositions { get; set; } = true;
        public bool? IncludeOffices { get; set; } = true;
        public bool? IncludeSmtpCredentials { get; set; } = true;
    }
}
