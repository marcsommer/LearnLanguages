using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public abstract class MultiLineTextsOrderStudierBase : IOrderStudier<MultiLineTextList>
  {
    protected MultiLineTextList _StudyTarget { get; set; }
    protected IOfferExchange _OfferExchange { get; set; }

    public void Study(MultiLineTextList studyTarget, IOfferExchange offerExchange)
    {
      if (studyTarget == null)
        throw new ArgumentNullException("studyTarget");
      if (offerExchange == null)
        throw new ArgumentNullException("offerExchange");

      _StudyTarget = studyTarget;
      _OfferExchange = offerExchange;

      StudyImpl();
    }

    protected abstract void StudyImpl();
  }
}
