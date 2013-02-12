using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LearnLanguages.Common
{
  public static class CommonHelper
  {
    public static bool PasswordIsValid(string password)
    {
      if (string.IsNullOrEmpty(password))
        return false;

      var isValid = Regex.IsMatch(password, CommonResources.PasswordValidationRegex);
      return isValid;
    }

    public static bool UsernameIsValid(string username)
    {
      string dummy = "";
      var isValid = UsernameIsValid(username, out dummy);
      return isValid;
    }

    public static bool UsernameIsValid(string username, out string errorDescription)
    {
      errorDescription = null;

      if (string.IsNullOrEmpty(username))
      {
        errorDescription = CommonResources.ErrorMsgInvalidUsernameEmpty;
        return false;
      }

      if (username.Length < int.Parse(CommonResources.MinUsernameLength) ||
          username.Length > int.Parse(CommonResources.MaxUsernameLength))
      {
        errorDescription = CommonResources.ErrorMsgInvalidUsernameLength;
        return false;
      }

      if (!Regex.IsMatch(username, CommonResources.UsernameValidationRegex))
      {
        errorDescription = CommonResources.ErrorMsgInvalidUsernameComposition;
        return false;
      }

      return true;
    }


    /// <summary>
    ///ANY CODE RUNNING ON THE SERVER SHOULD ONLY
    ///BE RUN WHEN THE CURRENT USER IS AUTHENTICATED
    ///(EXCEPT CODE INVOLVING THE AUTHENTICATION PROCESS ;)
    /// </summary>
    public static void CheckAuthentication()
    {
      if (!Csla.ApplicationContext.User.Identity.IsAuthenticated)
        throw new Common.Exceptions.UserNotAuthenticatedException();
    }

    public static bool CurrentUserIsAuthenticated()
    {
      return Csla.ApplicationContext.User.Identity.IsAuthenticated;
    }


    /// <summary>
    /// Waits for the given time in a background thread (executed with a Task)
    /// </summary>
    /// <returns>Task (async void)</returns>
    public static async Task WaitAsync(int timeToWaitInMs)
    {
      var waitTask = new Task(() => System.Threading.Thread.Sleep(timeToWaitInMs));
      waitTask.Start();
      await waitTask;
    }
  }
}
