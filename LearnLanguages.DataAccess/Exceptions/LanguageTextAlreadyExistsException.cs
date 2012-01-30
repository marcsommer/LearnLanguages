using System;
using Csla.Serialization;

namespace LearnLanguages.DataAccess.Exceptions
{
  [Serializable]
  public class LanguageTextAlreadyExistsException : Exception
  {
    public LanguageTextAlreadyExistsException(string languageText)
      : base(string.Format(DalResources.ErrorMsgLanguageTextAlreadyExistsException, languageText))
    {
      LanguageText = languageText; 
    }

    public LanguageTextAlreadyExistsException(string errorMsg, string languageText)
      : base(errorMsg)
    {
      LanguageText = languageText;
    }

    public LanguageTextAlreadyExistsException(Exception innerException, string languageText)
      : base(string.Format(DalResources.ErrorMsgLanguageTextAlreadyExistsException, languageText), innerException)
    {
      LanguageText = languageText;
    }

    public string LanguageText { get; private set; }
  }
}
