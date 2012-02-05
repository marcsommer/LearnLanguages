using System;
using System.Net;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IGetStudyData
  {
    void AskUserExtraData(object criteria, Delegates.StudyDataCallback callback);
  }
}
