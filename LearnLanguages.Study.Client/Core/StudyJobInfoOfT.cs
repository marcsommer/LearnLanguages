using System;
using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public class StudyJobInfo<T> : IJobInfo<T>
  {
    /// <summary>
    /// Creates a new StudyJobInfo object with the given "immutable" parameters.
    /// ExpectedPrecision defaults to 0, ie anything will do.
    /// </summary>
    public StudyJobInfo(T target, LanguageEdit language, DateTime expirationDate, double expectedPrecision)
    {
      Id = Guid.NewGuid();
      Target = target;
      ExpirationDate = expirationDate;
      Criteria = new StudyJobCriteria(language, expectedPrecision);
    }

    public StudyJobInfo(T target, LanguageEdit language, DateTime expirationDate, object criteria)
    {
      Id = Guid.NewGuid();
      Target = target;
      ExpirationDate = expirationDate;
      Criteria = new StudyJobCriteria(language);
    }

    public Guid Id { get; private set; }
    
    public T Target { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public object Criteria { get; private set; }

    public static DateTime NoExpirationDate
    {
      get
      {
        return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
      }
    }

  }
}
