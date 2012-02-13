using System;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  public abstract class MultiLineTextsStudierBase : IMultiLineTextsStudier
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
  }
}
