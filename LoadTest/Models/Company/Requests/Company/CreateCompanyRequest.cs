using DigitalOffice.LoadTesting.Models.Company.Models;

namespace DigitalOffice.LoadTesting.Models.Company.Requests.Company
{
    public record CreateCompanyRequest
    {
        public string PortalName { get; set; }
        public SmtpInfo SmtpInfo { get; set; }
        public string CompanyName { get; set; }
        public string SiteUrl { get; set; }
        public AdminInfo AdminInfo { get; set; }
        public bool IsDepartmentModuleEnabled { get; set; }
        public string WorkDaysApiUrl { get; set; }
    }
}
