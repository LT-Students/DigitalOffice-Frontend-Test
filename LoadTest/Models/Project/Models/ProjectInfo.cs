﻿using DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models
{
    public record ProjectInfo
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public ProjectStatusType Status { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DepartmentInfo Department { get; set; }
    }
}
