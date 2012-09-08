
using System;
namespace LearnLanguages
{
  //HACK:  COMMONRESOURCES SHOULD BE IN RESX
  public static class CommonResources
  {

    #region Keys

    public static string LoggerKey { get { return "Logger"; } }
    public static string EventAggregatorKey { get { return "EventAggregator"; } }
    public static string MainObjectKey { get { return "MainObject"; } }

    #endregion

    #region Error Messages

    public static string ErrorMsgInvalidResult { get { return "Result<T> cannot have T == Exception and IsSuccess == true."; } }
    public static string ErrorMsgNotInjected(string partNotInjected)
    {
      return string.Format("The Dependency Injection mechanism could not find the part: {0}",
                            partNotInjected);
    }
    public static string ErrorMsgNotFound(string whatWasNotFound)
    {
      return string.Format("The following was not found: \n{0}", whatWasNotFound);
    }

    #endregion

    #region Result Messages

    public static string ResultUndefined { get { return "Result == Undefined"; } }
    public static string ResultUndefinedWithInfo { get { return "Result == Undefined, Additional Info Created."; } }
    public static string ResultSuccess { get { return "Result == Success"; } }
    public static string ResultSuccessWithInfo { get { return "Result == Success, Additional Info Created."; } }
    public static string ResultFailure { get { return "Result == Failure"; } }
    public static string ResultFailureWithInfo { get { return "Result == Failure, Additional Info Created."; } }

    #endregion

    #region InfoKeys (key for use with result additional info)

    public static string InfoKeyExceptionObject { get { return "ExceptionObject"; } }
    public static string InfoKeyInfoMessage { get { return "InfoMessage"; } }

    #endregion 
  
    #region Default Languages

    public static string LanguageEarthian { get { return "Earthian"; } }

    public static string LanguageEnglish { get { return "English"; } }

    public static string LanguageSpanish { get { return "Spanish"; } }

    public static string LanguageGerman { get { return "German"; } }

    public static string LanguageItalian { get { return "Italian"; } }

    public static string LanguageFrench { get { return "French"; } }

    #endregion

    #region Statuses

    public static string StatusAdding { get { return "Adding"; } }
    public static string StatusAdded { get { return "Added"; } }
    public static string StatusCompleted { get { return "Completed"; } }
    public static string StatusDeleting { get { return "Deleting"; } }
    public static string StatusDeleted { get { return "Deleted"; } }
    public static string StatusStarted { get { return "Started"; } }
    public static string StatusUnspecified { get { return "Unspecified"; } }
    public static string StatusUpdating { get { return "Updating"; } }
    public static string StatusUpdated { get { return "Updated"; } }
    public static string StatusInitialized { get { return "Initialized"; } }
    public static string StatusCanceled { get { return "Canceled"; } }
    public static string StatusCanceling { get { return "Canceling"; } }
    public static string StatusAborted { get { return "Aborted"; } }
    public static string StatusAborting { get { return "Aborting"; } }
    public static string StatusProgressed { get { return "Progressed"; } }
    public static string StatusErrorEncountered { get { return "ErrorEncountered"; } }
    public static string StatusOther { get { return "Other"; } }


    #endregion

    //need to check if this still works, so throwing exception
    public static string LineDelimiter { get { return @"\r"; } }

    //need to check if this still works, so throwing exception
    public static string RegExSplitPatternWords { get { return @"\\W+"; } }

    //public static char[] SplitWordsDelimiters = { ' ', '\\', '/', ':', ';', '<', '>', ',', '.', '\n', '\r', '?', '[', ']', '{', '}', '|', '~', '`', '!', '#', '$', '%', '^', '&', '*', '(', ')', '+', '=', '"', '@' };

    public static string SplitWordsDelimiterString = @" \/:;<>,.?[]{}|~`!#@$%^&*()+=""\\r\\n";

    public static string ShowGridLines { get { return false.ToString(); } }

    //Only numbers, letters, and underscores
    public static string UsernameValidationRegex = @"^[a-zA-Z0-9_]*$";

    //Must contain digit, and an alpha, and can only contain digits, alphas, and !@#$%^&*() special characters
    public static string PasswordValidationRegex = @"^(?=.*\d+)(?=.*[a-zA-Z])[0-9a-zA-Z!@#$%^&*()]{6,15}$";
  }
}
