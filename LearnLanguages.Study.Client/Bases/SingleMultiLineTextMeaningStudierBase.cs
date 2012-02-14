using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public abstract class SingleMultiLineTextMeaningStudierBase : IMeaningStudier<MultiLineTextEdit>
  {
    public SingleMultiLineTextMeaningStudierBase()
    {
      HasStudied = false;
    }

    protected MultiLineTextEdit _StudyTarget { get; set; }
    protected IOfferExchange _OfferExchange { get; set; }

    public void Study(MultiLineTextEdit studyTarget, IOfferExchange offerExchange)
    {
      if (studyTarget == null)
        throw new ArgumentNullException("studyTarget");
      if (offerExchange == null)
        throw new ArgumentNullException("offerExchange");

      _StudyTarget = studyTarget;
      _OfferExchange = offerExchange;

      StudyImpl();
      HasStudied = true;
    }

    public virtual void Study()
    {
      if (_StudyTarget == null)
        throw new Exception("Study attempted, but _StudyTarget == null");
      if (_OfferExchange == null)
        throw new Exception("Study attempted, but _OfferExchange == null");

      Study(_StudyTarget, _OfferExchange);
    }

    public bool HasStudied { get; protected set; }

    /// <summary>
    /// Should take into account if HasStudied before.
    /// </summary>
    protected abstract void StudyImpl();
  }
}
