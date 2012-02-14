using System;

namespace LearnLanguages.Study
{
  public class MeaningStudyJobInfo<T> : StudyJobInfoBase<T>
  {
    public override string GetStudyContext()
    {
      return StudyResources.StudyContextMeaning;
    }
  }
}
