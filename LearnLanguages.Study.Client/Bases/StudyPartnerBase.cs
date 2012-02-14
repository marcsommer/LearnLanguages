using System;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Simple IStudyPartner base class.  You do not HAVE to descend from this, it just provides some 
  /// very basic plumbing.
  /// </summary>
  public abstract class StudyPartnerBase : Interfaces.IStudyPartner
  {
    protected MultiLineTextList _MultiLineTexts { get; set; }
    protected IOfferExchange _OfferExchange { get; set; }

    public void StudyMultiLineTexts(MultiLineTextList multiLineTexts, IOfferExchange offerExchange)
    {
      if (multiLineTexts == null)
        throw new ArgumentNullException("multiLineTexts");
      if (offerExchange == null)
        throw new ArgumentNullException("offerExchange");

      _MultiLineTexts = multiLineTexts;
      _OfferExchange = offerExchange;

      StudyMultiLineTextsImpl();
    }

    protected abstract void StudyMultiLineTextsImpl();

    //public bool HasStudied { get; protected set; }

    //public virtual bool StudyAgain()
    //{
    //  if (_StudyTarget == null || _OfferExchange == null || !HasStudied)
    //    return false;

    //  Study(_StudyTarget, _OfferExchange);
    //  return true;
    //}

  }
}
