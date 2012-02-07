using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
