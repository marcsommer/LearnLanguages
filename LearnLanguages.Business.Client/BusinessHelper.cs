using System;
using LearnLanguages.Business.Security;

namespace LearnLanguages.Business
{
  public static class BusinessHelper
  {
    public static string GetCurrentUsername()
    {
      var currentUsername = ((CustomIdentity)(Csla.ApplicationContext.User.Identity)).Name;
      return currentUsername;
    }

    public static Guid GetCurrentUserId()
    {
      var currentUserId = ((CustomIdentity)(Csla.ApplicationContext.User.Identity)).UserId;
      return currentUserId;
    }
  }
}
