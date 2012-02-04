using System;
using System.Net;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IAskUserExtraData
  {
    void AskUserExtraData();
    event EventHandler AskUserExtraDataCompleted;
  }
}
