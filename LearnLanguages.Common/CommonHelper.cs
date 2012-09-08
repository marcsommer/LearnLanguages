using System.Text.RegularExpressions;

namespace LearnLanguages.Common
{
  public static class CommonHelper
  {
    public static bool PasswordIsValid(string password)
    {
      var isValid = Regex.IsMatch(password, CommonResources.PasswordValidationRegex);
      return isValid;
    }

    public static bool UsernameIsValid(string username)
    {
      var isValid = Regex.IsMatch(username, CommonResources.UsernameValidationRegex);
      return isValid;
    }
  }
}
