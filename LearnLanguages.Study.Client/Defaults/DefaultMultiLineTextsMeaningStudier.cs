using System;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;

namespace LearnLanguages.Study
{
  /// <summary>
  /// This MeaningStudier manages a collection of SingleMultiLineTextMeaningStudiers.  Currently, this
  /// cycles through each of the MLTs and calls its Study method.  In the future, this is where we will
  /// adjust priorities to different MLTs, depending on priority of user, and how strongly we want to study 
  /// a certain MLT, due to its projected 'memory trace health' or whatever the paradigm we are using.
  /// </summary>
  [Export(typeof(Interfaces.IMeaningStudier<MultiLineTextList>))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class DefaultMultiLineTextsMeaningStudier : MultiLineTextsMeaningStudierBase
  {
    public DefaultMultiLineTextsMeaningStudier()
    {
      _Studiers = new Dictionary<MultiLineTextEdit, DefaultSingleMultiLineTextMeaningStudier>();
    }

    private int _CurrentMultiLineTextIndex = -1;

    /// <summary>
    /// Collection of Studiers, indexed by the MLT that they study.
    /// </summary>
    private Dictionary<MultiLineTextEdit, DefaultSingleMultiLineTextMeaningStudier> _Studiers { get; set; }


    #region Methods

    protected override void StudyImpl()
    {
      var multiLineTextIndex = GetNextMultiLineTextIndex();
      var currentMultiLineText = _StudyTarget[multiLineTextIndex];
      var studier = _Studiers[currentMultiLineText];
      studier.Study();
    }

    private int GetNextMultiLineTextIndex()
    {
      //cycle through the _StudyTarget's MLTs, and get the corresponding studier for this MLT.
      _CurrentMultiLineTextIndex++;
      if (_CurrentMultiLineTextIndex > (_StudyTarget.Count - 1))
        _CurrentMultiLineTextIndex = 0;

      return _CurrentMultiLineTextIndex;
      
    }

    public double GetPercentKnown()
    {
      throw new NotImplementedException();
    }
    
    #endregion
  }
}
