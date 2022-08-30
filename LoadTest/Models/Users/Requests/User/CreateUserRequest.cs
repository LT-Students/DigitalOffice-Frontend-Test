using LT.DigitalOffice.LoadTesting.Models.Users.Requests.Avatar;
using LT.DigitalOffice.LoadTesting.Models.Users.Requests.Communication;
using LT.DigitalOffice.LoadTesting.Models.Users.Requests.UserCompany;
using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.User
{
  public record CreateUserRequest
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public bool IsAdmin { get; set; }
    public string About { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? BusinessHoursFromUtc { get; set; }
    public DateTime? BusinessHoursToUtc { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? OfficeId { get; set; }
    public Guid? PositionId { get; set; }
    public Guid? RoleId { get; set; }
    public string Password { get; set; }
    public CreateUserCompanyRequest UserCompany { get; set; }
    public CreateAvatarRequest AvatarImage { get; set; }
    public CreateCommunicationRequest Communication { get; set; }
  }
}
