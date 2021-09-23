using DigitalOffice.LoadTesting.Models.Users.Models;
using System.Collections.Generic;

namespace DigitalOffice.LoadTesting.Models.Users.Responses.User
{
    public record UserResponse
    {
        public UserInfo User { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public IEnumerable<CommunicationInfo> Communications { get; set; }
        public IEnumerable<CertificateInfo> Certificates { get; set; }
        public IEnumerable<UserAchievementInfo> Achievements { get; set; }
        public IEnumerable<ProjectInfo> Projects { get; set; }
        public IEnumerable<EducationInfo> Educations { get; set; }
    }
}