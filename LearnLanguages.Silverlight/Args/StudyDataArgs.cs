using System;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.Args
{
  public class StudyDataArgs
  {
    public StudyDataArgs() { }
    public StudyDataArgs(StudyDataEdit studyData)
    {
      StudyData = studyData;
      Error = null;
    }
    public StudyDataArgs(Exception error)
    {
      StudyData = null;
      Error = error;
    }

    public StudyDataEdit StudyData { get; set; }
    public Exception Error { get; set; }
  }
}
