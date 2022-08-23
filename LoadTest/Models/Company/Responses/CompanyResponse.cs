using DigitalOffice.LoadTesting.Models.Company.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Company.Responses
{
  public record CompanyResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Tagline { get; set; }
    public string Contacts { get; set; }
    public ImageConsist Logo { get; set; }
    public List<OfficeInfo> Offices { get; set; }
  }
}
