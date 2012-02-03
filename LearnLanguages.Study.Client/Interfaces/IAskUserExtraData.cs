using System;
using System.Net;

namespace LearnLanguages.Study.Interfaces
{
  public interface IAskUserExtraData
  {
    void AskUserExtraData();
    event EventHandler AskUserExtraDataCompleted;
  }
}
