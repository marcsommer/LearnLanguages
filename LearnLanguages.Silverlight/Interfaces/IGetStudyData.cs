using System;
using System.Net;
using LearnLanguages.Common;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IGetStudyData
  {
    void AskUserExtraData(object criteria, AsyncCallback<StudyDataEdit> callback);
  }
}
