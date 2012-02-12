using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Caliburn.Micro;
using LearnLanguages.Common.ViewModelBases;
using LearnLanguages.Business;
using LearnLanguages.Silverlight.Interfaces;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common.Args;

namespace LearnLanguages.Silverlight
{
  /// <summary>
  /// 
  /// </summary>
  [Export(typeof(IStudyPartner))]
  public class DefaultStudyPartner : IStudyPartner
  {
    #region Ctors and Init

    public DefaultStudyPartner()
    {

    }

    #endregion

    #region Properties

    #endregion

    #region Methods

    public void StudyMultiLineTexts(MultiLineTextList multiLineTexts, IEventAggregator eventAggregator)
    {
      //get study multiple songs view
      //give study multiple songs view multiLineTexts
      //study multiple songs view
    }

    #endregion

    #region Events

    #endregion

    
  }
}
