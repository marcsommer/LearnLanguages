using System;
using System.ComponentModel.Composition;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{
  /// <summary>
  /// 
  /// </summary>
  [Export(typeof(Interfaces.IOrderStudier<MultiLineTextEdit>))]
  public class DefaultMultiLineTextsOrderStudier: MultiLineTextsOrderStudierBase
  {
    protected override void StudyImpl()
    {
      throw new NotImplementedException();
    }
  }
}
