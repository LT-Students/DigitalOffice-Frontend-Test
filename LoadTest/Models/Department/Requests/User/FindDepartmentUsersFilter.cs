using LT.DigitalOffice.LoadTesting.Models.Common;

namespace LT.DigitalOffice.LoadTesting.Models.Department.Requests.User
{
  public record FindDepartmentUsersFilter : BaseFindFilter
  {
    public bool? IsActive { get; set; }
    public bool? AscendingSort { get; set; }
    public bool? DepartmentUserRoleAscendingSort { get; set; }
    public bool IncludeAvatars { get; set; }
    public bool IncludePositions { get; set; }
  }
}
