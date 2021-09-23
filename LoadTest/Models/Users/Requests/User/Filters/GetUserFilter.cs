using System;

namespace DigitalOffice.LoadTesting.Models.Users.Requests.User.Filters
{
    public class GetUserFilter
    {
        public Guid UserId { get; set; }
        public bool IncludeCommunications { get; set; } = true;
        public bool IncludeCertificates { get; set; } = true;
        public bool IncludeAchievements { get; set; } = true;
        public bool IncludeDepartment { get; set; } = true;
        public bool IncludePosition { get; set; } = true;
        public bool IncludeOffice { get; set; } = true;
        public bool IncludeRole { get; set; } = true;
        public bool IncludeSkills { get; set; } = true;
        public bool IncludeEducations { get; set; } = true;
        public bool IncludeImages { get; set; } = true;
        public bool IncludeProjects { get; set; } = true;
    }
}
