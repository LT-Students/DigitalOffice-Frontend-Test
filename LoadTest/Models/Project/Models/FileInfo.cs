using LT.DigitalOffice.LoadTesting.Models.Project.Enums;
using System;

namespace DigitalOffice.LoadTesting.Models.Project.Models
{
  public record FileInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public FileAccessType Access { get; set; }
  }
}
