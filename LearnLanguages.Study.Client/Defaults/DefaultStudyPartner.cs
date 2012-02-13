using System;
using System.ComponentModel.Composition;
using LearnLanguages.Study.Interfaces;

namespace LearnLanguages.Study
{
  /// <summary>
  /// 
  /// </summary>
  [Export(typeof(IStudyPartner))]
  public class DefaultStudyPartner : StudyPartnerBase
  {
    #region Ctors and Init

    public DefaultStudyPartner()
    {
      MultiLineTextsStudier = new DefaultMultiLineTextsStudier();
    }

    #endregion

    #region Properties

    IMultiLineTextsStudier MultiLineTextsStudier { get; set; }

    #endregion

    #region Methods

    protected override void StudyMultiLineTextsImpl()
    {
      //delegate the study call to the MLT studier
      MultiLineTextsStudier.StudyMultiLineTexts(_MultiLineTexts, _OfferExchange);
    }
    
    #endregion
  }
}
