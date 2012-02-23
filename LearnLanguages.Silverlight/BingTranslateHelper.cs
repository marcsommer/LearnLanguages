using System;

namespace LearnLanguages.Silverlight
{
  public static class BingTranslateHelper
  {
    public static string GetLanguageCode(string languageText)
    {
      var languageTextLower = languageText.ToLower();

      if (languageTextLower == "english")
        return "en";
      else if (languageTextLower == "spanish")
        return "es";
      else if (languageTextLower == "german")
        return "de";
      else if (languageTextLower == "italian")
        return "it";
      else if (languageTextLower == "french")
        return "fr";
      else
        return "";
    }
  }
}
