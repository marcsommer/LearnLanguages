using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using System;
using LearnLanguages.Business;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudyJobInfo<TStudyTarget> 
  {
    Guid Id { get; }
    TStudyTarget StudyTarget { get; }
    LanguageEdit Language { get; }
    DateTime JobExpirationDate { get; }
    IOfferExchange OfferExchange { get; }
    double ExpectedPrecision { get; }
  }
}
