using System;
using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;

namespace LearnLanguages.Study
{
  public abstract class StudyJobInfoBase<T> : IStudyJobInfo<T>
  {
    public StudyJobInfoBase(T studyTarget, LanguageEdit language, DateTime jobExpirationDate)
    {
      Id = Guid.NewGuid();
      StudyTarget = studyTarget;
      Language = language;
      JobExpirationDate = jobExpirationDate;
      //System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
    }

    public Guid Id { get; private set; }
    public T StudyTarget { get; private set; }
    public LanguageEdit Language { get; private set; }
    public DateTime JobExpirationDate { get; private set; }

    public abstract string GetStudyContext();
  }
}
