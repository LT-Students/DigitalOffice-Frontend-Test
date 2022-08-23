using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Project.Requests.User.Filters
{
  public record FindProjectUsersFilter : BaseFindFilter
  {
    public bool? IsActive { get; set; }
    public bool? AscendingSort { get; set; }
    public bool IncludeAvatars { get; set; }
    public bool IncludePositions { get; set; }
  }
}
