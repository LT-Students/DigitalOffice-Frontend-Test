using DigitalOffice.LoadTesting.Models.Project.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.Image
{
  public record CreateImagesRequest
  {
    public Guid ProjectId { get; set; }
    public List<ImageContent> Images { get; set; }
  }
}
