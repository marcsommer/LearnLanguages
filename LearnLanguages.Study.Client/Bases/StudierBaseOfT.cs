using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public abstract class StudierBase<J, T> : IStudier<J, T>
    where J : IStudyJobInfo<T>
  {
    public StudierBase()
    {
      HasStudied = false;
    }

    protected J _StudyJobInfo { get; set; }
    protected IOfferExchange _OfferExchange { get; set; }

    public void Study(J studyJobInfo, IOfferExchange offerExchange)
    {
      if (studyJobInfo == null)
        throw new ArgumentNullException("studyTarget");
      if (offerExchange == null)
        throw new ArgumentNullException("offerExchange");

      //if our old study target isn't empty and the new study target isn't the same one
      //then we haven't studied this before.
      //or the same reasoning for offer exchange
      if ( (_StudyJobInfo != null && (studyJobInfo.Id != _StudyJobInfo.Id)) ||
           (_OfferExchange != null && (offerExchange.OfferExchangeId != _OfferExchange.OfferExchangeId)) )
        HasStudied = false;

      _StudyJobInfo = studyJobInfo;
      _OfferExchange = offerExchange;

      StudyImpl();
      HasStudied = true;
    }

    public virtual bool StudyAgain()
    {
      if (_StudyJobInfo == null || _OfferExchange == null || !HasStudied)
        return false;
      
      Study(_StudyJobInfo, _OfferExchange);
      return true;
    }

    public bool HasStudied { get; protected set; }

    /// <summary>
    /// Should take into account if HasStudied before.
    /// </summary>
    protected abstract void StudyImpl();

    public abstract string GetStudyContext();
  }
}
