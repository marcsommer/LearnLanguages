using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LearnLanguages.Common
{
  public static class LearnLanguagesExtensions
  {
    public static List<string> ParseIntoLines(this string str, bool removeEmptyEntries = true)
    {
      if (string.IsNullOrEmpty(str))
        return new List<string>();

      var lineDelimiter = CommonResources.LineDelimiter;
      lineDelimiter = lineDelimiter.Replace("\\r", "\r");
      lineDelimiter = lineDelimiter.Replace("\\n", "\n");
      var lines = new List<string>(str.Split(new string[] { lineDelimiter }, StringSplitOptions.RemoveEmptyEntries));
      return lines;
    }

    public static List<string> ParseIntoWords(this string str)
    {
      var splitIntoWordsPattern = CommonResources.RegExSplitPatternWords;
      splitIntoWordsPattern = splitIntoWordsPattern.Replace(@"\\", @"\");
      var words = new List<string>(Regex.Split(str, splitIntoWordsPattern));
      return words;
    }

    public static int CountWords(this string str)
    {
      return str.ParseIntoWords().Count;
    }
  }
}
