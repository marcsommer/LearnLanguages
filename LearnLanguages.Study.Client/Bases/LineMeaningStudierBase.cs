using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public abstract class LineMeaningStudierBase : IMeaningStudier<LineEdit>
  {
    public LineMeaningStudierBase()
    {
      HasStudied = false;
    }

    protected LineEdit _StudyTarget { get; set; }
    protected IOfferExchange _OfferExchange { get; set; }

    public void Study(LineEdit studyTarget, IOfferExchange offerExchange)
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

    public virtual bool StudyAgain()
    {
      if (_StudyTarget == null || _OfferExchange == null || !HasStudied)
        return false;
      
      Study(_StudyTarget, _OfferExchange);
      return true;
    }

    public bool HasStudied { get; protected set; }

    /// <summary>
    /// Should take into account if HasStudied before.
    /// </summary>
    protected abstract void StudyImpl();
  }
}
