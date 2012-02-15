using System;
using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  public class StudyJobInfo<T> : IStudyJobInfo<T>
  {
    /// <summary>
    /// Creates a new StudyJobInfo object with the given "immutable" parameters.
    /// ExpectedPrecision defaults to 0, ie anything will do.
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="language"></param>
    /// <param name="offerExchange"></param>
    /// <param name="jobExpirationDate"></param>
    /// <param name="expectedPrecision"></param>
    public StudyJobInfo(T target, LanguageEdit language, IExchange offerExchange, 
      DateTime jobExpirationDate, double expectedPrecision)
    {
      Id = Guid.NewGuid();
      //OriginalJobId = Guid.Empty;
      Target = target;
      Language = language;
      OfferExchange = offerExchange;
      JobExpirationDate = jobExpirationDate;
      ExpectedPrecision = expectedPrecision;
    }

    //public static StudyJobInfo<TNewTarget> StudyJobInfo<TNewTarget, TOriginalJob, TOriginalJobTarget>
    //  (TOriginalJob originalJob, TNewTarget newTarget, LanguageEdit language, DateTime jobExpirationDate)
    //  where TOriginalJob : StudyJobInfo<TOriginalJobTarget>
    //{
    //  var job = new StudyJobInfo<T>(originalJob.stu, language, jobExpirationDate) 
    //  { 
    //    OriginalJobId = originalJob.Id 
    //  };
    //  //System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
    //}
    //public static TTargetJob Wrap<TOriginalJob, TTargetJob>(TOriginalJob originalJob)
    //{
    //  StudyJobInfo
    //}

    public Guid Id { get; private set; }
    //private bool _OriginalJobIdSetOnce = false;

    //private Guid _OriginalJobId = Guid.Empty;
    //public Guid OriginalJobId 
    //{
    //  get { return _OriginalJobId; }
    //  set
    //  {
    //    if (_OriginalJobIdSetOnce)
    //      throw new Exception();
    //    else
    //    {
    //      _OriginalJobIdSetOnce = true;
    //      _OriginalJobId = value;
    //    }
    //  }
    //}

    public T Target { get; private set; }
    public LanguageEdit Language { get; private set; }
    public DateTime JobExpirationDate { get; private set; }
    public IExchange OfferExchange { get; private set; }
    public double ExpectedPrecision { get; private set;}

    public static DateTime NoExpirationDate
    {
      get
      {
        return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
      }
    }
  }
}
