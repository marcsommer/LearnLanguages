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
    //uses default study partner base behavior for delegation at the moment.

  }
}
