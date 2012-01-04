using System;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages
{
  public class ConsoleLogger : ILogger
  {
    public void Log(string msg, LogPriority priority, LogCategory category)
    {
      string padding = "------";
      
      Console.WriteLine(padding);
      
      Console.WriteLine(category.ToString());
      Console.WriteLine(priority.ToString());
      Console.WriteLine(msg);

      Console.WriteLine(padding);
    }
  }
}
