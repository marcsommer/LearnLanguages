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

    protected override void StudyMultiLineTextsImpl()
    {
      //analyzes history events
      //depending on history may ask user for info about MLT
      //update percentage of order studying and meaning studying
      //using percentage, select next studier implementor, either order or meaning
      //initiate that studier's study method

      throw new NotImplementedException();
    }
  }
}
