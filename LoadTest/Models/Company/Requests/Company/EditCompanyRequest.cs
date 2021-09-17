﻿namespace DigitalOffice.LoadTesting.Models.Company.Requests.Company
{
    public record EditCompanyRequest
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PortalName { get; set; }
        public string CompanyName { get; set; }
        public string SiteUrl { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public AddImageRequest Logo { get; set; }
        public bool IsDepartmentModuleEnabled { get; set; }
    }
}
