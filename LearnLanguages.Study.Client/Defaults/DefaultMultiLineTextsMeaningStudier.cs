using System;
using Caliburn.Micro;
using LearnLanguages.Business;
using System.ComponentModel.Composition;

namespace LearnLanguages.Study
{
  /// <summary>
  /// 
  /// </summary>
  [Export(typeof(Interfaces.IMeaningStudier<MultiLineTextList>))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class DefaultMultiLineTextsOrderStudier : Interfaces.IMeaningStudier<MultiLineTextList>
  {
    public void Study(MultiLineTextList studyTarget, IOfferExchange offerExchange)
    {
      throw new NotImplementedException();
    }
  }
}
