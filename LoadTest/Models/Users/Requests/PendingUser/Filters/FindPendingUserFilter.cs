using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Users.Requests.PendingUser.Filters
{
  public record FindPendingUserFilter : BaseFindFilter
  {
    public bool IncludeCommunication { get; set; }
    public bool IncludeCurrentAvatar { get; set; }
  }
}
