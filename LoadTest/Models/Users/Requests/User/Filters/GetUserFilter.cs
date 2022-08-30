using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.User.Filters
{
  public class GetUserFilter
  {
    public Guid? UserId { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public bool IncludeAchievements { get; set; }
    public bool IncludeCurrentAvatar { get; set; } = true;
    public bool IncludeAvatars { get; set; }
    public bool IncludeCertificates { get; set; }
    public bool IncludeCommunications { get; set; } = true;
    public bool IncludeCompany { get; set; } = true;
    public bool IncludeDepartment { get; set; } = true;
    public bool IncludeEducations { get; set; }
    public bool IncludeOffice { get; set; }
    public bool IncludePosition { get; set; } = true;
    public bool IncludeProjects { get; set; } = true;
    public bool IncludeRole { get; set; } = true;
    public bool IncludeSkills { get; set; }
    public string Locale { get; set; }
  }
}
