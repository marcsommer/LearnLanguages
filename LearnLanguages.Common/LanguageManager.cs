using System.Collections.Generic;

namespace LearnLanguages
{
  public static class LanguageManager
  {
    static LanguageManager()
    {
      Languages = new List<string>();
      PopulateLanguages();
    }

    private static void PopulateLanguages()
    {
      if (!PopulateLanguagesFromExternalSource())
        PopulateLanguagesWithDefaults();
      
    }

    private static void PopulateLanguagesWithDefaults()
    {
      Languages.Clear();
      Languages.Add(CommonResources.LanguageEarthian);
      Languages.Add(CommonResources.LanguageEnglish);
      Languages.Add(CommonResources.LanguageSpanish);
      Languages.Add(CommonResources.LanguageGerman);
      Languages.Add(CommonResources.LanguageItalian);
      Languages.Add(CommonResources.LanguageFrench);
    }

    public static string DefaultLanguage = "Earthian";
    public static List<string> Languages;
    public static bool IsLanguage(string candidateStr)
    {
      return Languages.Contains(candidateStr);
    }

    public static bool PopulateLanguagesFromExternalSource()
    {
      //todo: languages in an xml file so can add whenever
      return false;
    }
  }
}
