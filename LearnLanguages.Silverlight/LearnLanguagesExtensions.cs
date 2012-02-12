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
using System.Collections.Generic;

namespace LearnLanguages.Silverlight
{
  public static class LearnLanguagesExtensions
  {
    public static List<string> ParseIntoLines(this string str, bool removeEmptyEntries = true)
    {
      if (string.IsNullOrEmpty(str))
        return new List<string>();

      var lineDelimiter = ViewViewModelResources.LineDelimiter;
      lineDelimiter = lineDelimiter.Replace("\\r", "\r");
      lineDelimiter = lineDelimiter.Replace("\\n", "\n");
      var lines = new List<string>(str.Split(new string[] { lineDelimiter }, StringSplitOptions.RemoveEmptyEntries));
      return lines;
    }
  }
}
