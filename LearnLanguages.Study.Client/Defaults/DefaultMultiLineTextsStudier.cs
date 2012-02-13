using System;
using System.ComponentModel.Composition;
using LearnLanguages.Study.Interfaces;

namespace LearnLanguages.Study
{
  /// <summary>
  /// 
  /// </summary>
  [Export(typeof(IMultiLineTextsStudier))]
  public class DefaultMultiLineTextsStudier : MultiLineTextsStudierBase
  {
    protected override void StudyMultiLineTextsImpl()
    {
      //analyzes history events
      //for now, we will assume there is no history for the MLT, and thus we should focus on understanding
      //meaning of the song.


      //depending on history may ask user for info about MLT
      //update percentage of order studying and meaning studying
      //using percentage, select next studier implementor, either order or meaning
      //initiate that studier's study method
    }

    private IMultiLineTextsOrderStudier

    private double MeaningPercentKnown { get; set; }
    private double OrderPercentKnown { get; set; }
  }
}
