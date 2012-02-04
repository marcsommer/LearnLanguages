using System;
using System.Net;
using LearnLanguages.Business;

namespace LearnLanguages.Delegates
{
  /// <summary>
  /// Callback that just sends exception data.
  /// </summary>
  /// <param name="exception"></param>
  public delegate void ExceptionCheckCallback(Exception exception);
}
