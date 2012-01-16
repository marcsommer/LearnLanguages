namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IAuthenticationChangedEventMessage
  {
    string CurrentPrincipalName { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
  }
}
