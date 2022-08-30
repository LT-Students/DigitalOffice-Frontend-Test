using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.User.Filters
{
  public record FindUsersFilter : BaseFindFilter
  {
    public bool? IsAscendingSort { get; set; }
    public string FullNameIncludeSubstring { get; set; }
    public bool? IsActive { get; set; }
    public bool IncludeCurrentAvatar { get; set; }
    public bool IncludeCommunications { get; set; }
  }
}
