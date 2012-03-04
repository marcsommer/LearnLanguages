using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  /// <summary>
  /// 
  /// </summary>
  public class DefaultMultiLineTextsOrderStudier : 
    StudierBase<StudyJobInfo<MultiLineTextList, IViewModelBase>, MultiLineTextList, IViewModelBase>
  {
    protected override void DoImpl()
    {
      throw new NotImplementedException();
    }
  }
}
