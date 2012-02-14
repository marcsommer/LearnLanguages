using System;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;

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
  public class DefaultSingleMultiLineTextMeaningStudier : 
    StudierBase<MeaningStudyJobInfo<MultiLineTextEdit>, MultiLineTextEdit>
  {
    #region Ctors and Init
    
    public DefaultSingleMultiLineTextMeaningStudier()
      : base()
    {
      _LineStudiers = new Dictionary<int, DefaultLineMeaningStudier>();
    }

    public DefaultSingleMultiLineTextMeaningStudier(int startingAggregateSize)
      : this()
    {
      AggregateSize = startingAggregateSize;
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
    public int AggregateSize { get; set; }

    /// <summary>
    /// How many lines to cycle through while studying.  Much like a buffer.  When a line is learned,
    /// then it gets removed from the ActiveLines and a different line replaces it.
    /// </summary>
    public int ActiveLinesCount { get; set; }

    /// <summary>
    /// LineStudiers indexed by each's corresponding line number.
    /// </summary>
    private Dictionary<int, DefaultLineMeaningStudier> _LineStudiers { get; set; }
    
    #endregion

    #region Methods

    protected override void StudyImpl()
    {
      if (!HasStudied)
      {
        PopulateLineStudiers();
      }

      UpdateKnowledge();
      ChooseAggregateSize();
      DefaultLineMeaningStudier nextStudier = ChooseNextLineStudier();
      StartNextStudier();
      //TODO: post Q & A offer to offer exchange.
    }

    private DefaultLineMeaningStudier ChooseNextLineStudier()
    {
      throw new NotImplementedException();
    }

    private void UpdateKnowledge()
    {
      //for now, this just sets the active lines
      //todo: MLTMeaning...dynamically set active lines count from either some sort of calculator class
      ActiveLinesCount = int.Parse(StudyResources.DefaultMeaningStudierActiveLinesCount);
    }

    protected void ChooseAggregateSize()
    {
      //todo: MLTMeaning...dynamically choose aggregate size
      AggregateSize = int.Parse(StudyResources.DefaultMeaningStudierAggregateSize);
    }

    protected virtual void PopulateLineStudiers()
    {
      _LineStudiers.Clear();
      foreach (var line in _StudyJobInfo.StudyTarget.Lines)
      {
        var lineInfo = new DefaultLineMeaningStudier(line, AggregateSize);
        _LineStudiers.Add(line.LineNumber, lineInfo);
      }
    }

    /// <summary>
    /// Calculates the percent known of all lines of the MLT study target.
    /// </summary>
    /// <returns></returns>
    public double GetPercentKnown()
    {
      var lineCount = _StudyTarget.Lines.Count;
      double totalPercentKnownNonNormalized = 0.0d;
      double maxPercentKnownNonNormalized = 100 * lineCount;
      foreach (var lineInfo in _LineStudiers)
      {
        var linePercentKnown = lineInfo.Value.GetLinePercentKnown();
        totalPercentKnownNonNormalized += linePercentKnown;
      }

      var totalPercentKnownNormalized = totalPercentKnownNonNormalized / maxPercentKnownNonNormalized;
      return totalPercentKnownNormalized;
    }

    public override string GetStudyContext()
    {
      return StudyResources.StudyContextMeaning;
    }
    
    #endregion
  }
}
