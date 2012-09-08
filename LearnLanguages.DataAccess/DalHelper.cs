using System.Text.RegularExpressions;

namespace LearnLanguages.DataAccess
{
  public static class DalHelper
  {
    public static bool IsInRoleToAddUser()
    {
      var isInAdminRole = Csla.ApplicationContext.User.IsInRole(DalResources.RoleAdmin);
      return isInAdminRole;
    }
  }
}
