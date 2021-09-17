using System;
using DigitalOffice.LoadTesting.Models.Users.Enums;

namespace DigitalOffice.LoadTesting.Models.Users.Models
{
    public record EducationInfo
    {
        public Guid Id { get; set; }
        public string UniversityName { get; set; }
        public string QualificationName { get; set; }
        public FormEducation FormEducation { get; set; }
        public DateTime AdmissionAt { get; set; }
        public DateTime? IssueAt { get; set; }
    }
}
