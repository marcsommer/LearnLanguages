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
  public class DefaultMultiLineTextsMeaningStudier : StudierBase<StudyJobInfo<MultiLineTextList>, MultiLineTextList>
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
      var currentMultiLineText = _StudyJobInfo.StudyTarget[multiLineTextIndex];
      var studier = _Studiers[currentMultiLineText];

      //OUR CURRENT JOB ENTAILS ALL MULTILINETEXTEDITS, SO WE NEED TO CREATE 
      //A NEW JOB WITH ONLY THE ONE IN PARTICULAR WE CARE ABOUT.
      var originalJob = _StudyJobInfo;
      var newExpirationDate = 
        System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
      var newJob = new StudyJobInfo<MultiLineTextEdit>(currentMultiLineText, 
                                                       originalJob.Language, 
                                                       _OfferExchange, 
                                                       newExpirationDate);
      //newJob.OriginalJobId = originalJob.Id;
      studier.Study(newJob, _OfferExchange);
    }

    private int GetNextMultiLineTextIndex()
    {
      //cycle through the , and get the corresponding studier for this MLT.
      _CurrentMultiLineTextIndex++;
      if (_CurrentMultiLineTextIndex > (_StudyJobInfo.StudyTarget.Count - 1))
        _CurrentMultiLineTextIndex = 0;

      return _CurrentMultiLineTextIndex;
    }

    public double GetPercentKnown()
    {
      var totalLineCount = 0;
      double totalPercentKnownNonNormalized = 0.0d;
      foreach (var studier in _Studiers)
      {
        var studierLineCount = studier.Key.Lines.Count;
        totalLineCount += studierLineCount;

        var studierPercentKnown = studier.Value.GetPercentKnown();
        totalPercentKnownNonNormalized += (studierPercentKnown / 100.0d) * studierLineCount;
        //eg 50% known 10 lines
        //25% known 1000 lines.  Percent known should be just over 25% known (255/1010 to be exact);
        //totalLineCount = 1010
        //totalPercentKnownNonNormalized = (.5 * 10) + (.25 * 1000) = 255
        //totalPercentKnownNonNormalized = 255 / 1010 = .252475
      }
      var totalPercentKnownNormalized = totalPercentKnownNonNormalized / totalLineCount;

      return totalPercentKnownNormalized;
    }
    
    #endregion
  }
}
