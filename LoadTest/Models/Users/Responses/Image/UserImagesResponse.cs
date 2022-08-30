using DigitalOffice.LoadTesting.Models.Users.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Responses.Image
{
  public record UserImagesResponse
  {
    public Guid UserId { get; set; }
    public List<ImageInfo> Images { get; set; }
  }
}
