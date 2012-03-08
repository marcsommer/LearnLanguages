using System;
using System.Linq;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;
using Caliburn.Micro;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study
{
  /// <summary>
  /// This studies the meaning of a single MultiLineTextEdit object.  It utilizes a LineMeaningStudier
  /// for each line in the MLT.  It uses its knowledge to determine the active number of lines to study.  
  /// Once it has this number, it cycles through the lines, studying each one until known, then de-activates
  /// that line, and activates another.    Once all lines are known, then StudyAgain() will return false.
  /// 
  /// **It does NOT aggregate Lines currently, though I should refactor the concept of a line into an 
  /// OrderedPhrase, but I'll have to do that some other time.  Once that happens, we can think of all 
  /// "MultiLineTexts" as "OrderedTexts", which is what all phrases are really anyway, with individual words
  /// as phrase with only one text to order.  I digress.  This should more than suffice for now.
  /// 
  /// **Also, the concept of active lines is not necessary as well.  This is a concession I'm making 
  /// for a strategy for choosing the next LineStudier.  Again, this would be better with an OrderedText,
  /// but the general idea is that choosing a line to study should be abstracted and the choice should be 
  /// reliant upon other factors, like when each was studied last, how well they are known, and 
  /// a whole slew of other factors...perfect for a neural net to figure out.  Alas, for now it must 
  /// just be active lines.
  /// </summary>
  public class DefaultSingleMultiLineTextMeaningStudier : StudierBase<MultiLineTextEdit>
  {
    #region Ctors and Init
    
    public DefaultSingleMultiLineTextMeaningStudier()
      : base()
    {
      _LineStudiers = new Dictionary<int, DefaultLineMeaningStudier>();
      _LastActiveLineStudiedIndex = -1;
      _KnowledgeThreshold = double.Parse(StudyResources.DefaultKnowledgeThreshold);
    }

    public DefaultSingleMultiLineTextMeaningStudier(int startingAggregateSize)
      : this()
    {
      _AggregateSize = startingAggregateSize;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets and Sets the aggregate size for phrases in units of words per phrase.  So, 
    /// if AggregateSize == 2, then a phrase "the quick brown fox jumped over the moon"
    /// is parsed into "the quick" "brown fox" "jumped over" "the moon".  If 3, "the quick brown"
    /// "fox jumped over" "the moon", and so forth.  I may end up adding some noise to this, so it 
    /// will be something like 2 2 2 1 2 3 or something, but for now it's just this size or less 
    /// (as in the second example of 3, where the last phrase is just "the moon" (2).
    /// </summary>
    private int _AggregateSize { get; set; }

    /// <summary>
    /// How many lines to cycle through while studying.  Much like a buffer.  When a line is learned,
    /// then it gets removed from the ActiveLines and a different line replaces it.
    /// </summary>
    private int _ActiveLinesCount { get; set; }

    private int _LastActiveLineStudiedIndex { get; set; }

    private double _KnowledgeThreshold { get; set; }

    /// <summary>
    /// LineStudiers indexed by each's corresponding line number.
    /// </summary>
    private Dictionary<int, DefaultLineMeaningStudier> _LineStudiers { get; set; }
    
    #endregion

    #region Methods

    private DefaultLineMeaningStudier  ChooseNextLineStudier(out int nextLineNumber)
    {
      List<int> unknownLineNumbers = new List<int>();

      foreach (var studier in _LineStudiers)
      {
        var studierLineNumber = studier.Key;
        var studierPercentKnown = studier.Value.GetPercentKnown();
        if (studierPercentKnown > this._KnowledgeThreshold)
        {
          //line is known
          continue;
        }

        //line is unknown
        unknownLineNumbers.Add(studierLineNumber);
        ////if line is lowest so far, then update lowestsofar to this line number
        //if (studierLineNumber < lowestSoFar)
        //  lowestSoFar = studierLineNumber;
      }

      //all lines are known if count == 0, so we have no studier to choose.
      if (unknownLineNumbers.Count == 0)
      {
        nextLineNumber = -1;
        return null;
      }

      //THIS IS DIFFICULT, BECAUSE WE'RE HANDLING INDEXES OF INDEXES.
      //WE ARE LOOKING FOR THE LINE NUMBER OF THE NEXT STUDIER (THE INDEX FOR THAT STUDIER).
      //THIS LINE NUMBER IS IN THE UNKNOWN_LINE_NUMBERS, WHICH WE REFERENCE BY USING _*THAT*_
      //LINE NUMBER'S INDEX.  IF THAT INDEX IS GREATER THAN OUR ACTIVE LINE COUNT (WE CANNOT STUDY
      //MORE THAN ACTIVE_LINE_COUNT LINES AT A TIME)
      unknownLineNumbers.Sort();
      var nextLineNumberIndex = _LastActiveLineStudiedIndex + 1;
      if (nextLineNumberIndex >= unknownLineNumbers.Count ||
          nextLineNumberIndex >= _ActiveLinesCount)
        nextLineNumberIndex = 0;

      nextLineNumber = unknownLineNumbers[nextLineNumberIndex];
      var nextStudierToUse = _LineStudiers[nextLineNumber];

      return nextStudierToUse;
    }

    private void UpdateKnowledge()
    {
      //todo: MLTMeaning...dynamically set active lines count from either some sort of calculator class
      _ActiveLinesCount = int.Parse(StudyResources.DefaultMeaningStudierActiveLinesCount);

      //todo: MLTMeaning...dynamically set knowledge threshold to user's specifications or other.
      _KnowledgeThreshold = double.Parse(StudyResources.DefaultKnowledgeThreshold);
    }

    protected void ChooseAggregateSize()
    {
      //todo: MLTMeaning...dynamically choose aggregate size
      _AggregateSize = int.Parse(StudyResources.DefaultMeaningStudierAggregateSize);
    }

    protected virtual void PopulateLineStudiers(ExceptionCheckCallback completedCallback)
    {
      _LineStudiers.Clear();
      foreach (var line in _Target.Lines)
      {
        var lineStudier = new DefaultLineMeaningStudier();
        _LineStudiers.Add(line.LineNumber, lineStudier);
      }

      //WE NOW HAVE LINE STUDIERS POPULATED WITH UNINITIALIZED LINE STUDIERS.
      //WE WILL NOW INITIALIZE EACH LINE STUDIER
      int linesInitializedCount = 0;
      foreach (var lineStudierEntry in _LineStudiers)
      {
        var line = _Target.Lines[lineStudierEntry.Key];
        lineStudierEntry.Value.InitializeForNewStudySession(line, (e) =>
          {
            linesInitializedCount++;

            //IF WE HAVE INITIALIZED ALL OF OUR LINES, THEN OUR INITIALIZATION POPULATION IS COMPLETE
            if (linesInitializedCount == _Target.Lines.Count)
              completedCallback(null);
          });
      }
    }

    /// <summary>
    /// Calculates the percent known of all lines of the MLT study target.
    /// </summary>
    /// <returns></returns>
    public double GetPercentKnown()
    {
      var lineCount = _Target.Lines.Count;
      double totalPercentKnownNonNormalized = 0.0d;
      double maxPercentKnownNonNormalized = 100 * lineCount;
      foreach (var lineInfo in _LineStudiers)
      {
        var linePercentKnown = lineInfo.Value.GetPercentKnown();
        totalPercentKnownNonNormalized += linePercentKnown;
      }

      var totalPercentKnownNormalized = totalPercentKnownNonNormalized / maxPercentKnownNonNormalized;
      return totalPercentKnownNormalized;
    }
    
    #endregion

    public override void InitializeForNewStudySession(MultiLineTextEdit target, 
                                                      ExceptionCheckCallback completedCallback)
    {
      _Target = target;
      _LastActiveLineStudiedIndex = -1;

      //ACTIVATE EACH LINE FOR USE IN HISTORY PUBLISHER
      foreach (var line in target.Lines)
      {
        var activatedLineEvent = new History.Events.ActivatedLineEvent(line);
        History.HistoryPublisher.Ton.PublishEvent(activatedLineEvent);
      }

      //EXECUTES CALLBACK WHEN POPULATE IS COMPLETED
      PopulateLineStudiers((e) =>
        {
          if (e != null)
            throw e;

          completedCallback(null);
        });
    }

    public override void GetNextStudyItemViewModel(AsyncCallback<StudyItemViewModelArgs> callback)
    {
      UpdateKnowledge();
      ChooseAggregateSize();
      var nextLineNumber = -1;
      DefaultLineMeaningStudier nextStudier = ChooseNextLineStudier(out nextLineNumber);
      if (nextStudier == null || nextLineNumber < 0)
        throw new Exception("todo: all lines are studied, publish completion event or something");

      LineEdit nextLineEdit = _Target.Lines[nextLineNumber];
      if (nextLineEdit.LineNumber != nextLineNumber)
      {
        var results = from line in _Target.Lines
                      where line.LineNumber == nextLineNumber
                      select line;

        nextLineEdit = results.FirstOrDefault();
        if (nextLineEdit == null)
          throw new Exception("cannot find line with corresponding line number");
      }

      nextStudier.GetNextStudyItemViewModel((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          callback(this, r);
        });
      _LastActiveLineStudiedIndex = nextLineEdit.LineNumber;
    }
  }
}
