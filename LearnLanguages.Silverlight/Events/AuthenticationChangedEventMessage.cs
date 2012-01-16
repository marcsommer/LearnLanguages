namespace LearnLanguages.Silverlight.Events
{
  public class AuthenticationChangedEventMessage : Interfaces.IAuthenticationChangedEventMessage
  {
    public string CurrentPrincipalName
    {
      get { return Csla.ApplicationContext.User.Identity.Name; }
    }

    public bool IsAuthenticated
    {
      get { return Csla.ApplicationContext.User.Identity.IsAuthenticated; }
    }

    public bool IsInRole(string role)
    {
      bool isInRole = Csla.ApplicationContext.User.IsInRole(role);
      return isInRole;
    }
  }
}
