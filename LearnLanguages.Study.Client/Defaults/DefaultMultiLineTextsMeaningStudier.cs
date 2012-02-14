using System;
using LearnLanguages.Business;
using System.ComponentModel.Composition;
using LearnLanguages.Common.Interfaces;
using System.Collections.Generic;

namespace LearnLanguages.Study
{
  /// <summary>
  /// MeaningStudier has to know how much of the meaning we understand.  It should have already heard how much
  /// the MultiLineTextsStudier said the user understood of the MLT.  If not, it will have to assess this itself,
  /// but we will assume it heard it for now.  Using this % understood (% known), it will choose a phrase
  /// aggregate size, first publishing a thinking event.  Once the aggregate size is chosen, it will parse
  /// the MLT's unknown lines into aggregate phrases of this size and analyze how much of these phrases it thinks 
  /// the user knows.
  /// Then based on this, it will choose which phrase to present to the user.  For now, this will be in the form of 
  /// a Q & A.  Once chosen, it will publish an acting event and then publish an offer.  At this point, the 
  /// MeaningStudier is done for this iteration, though it may listen for feedback with that offer. 
  /// (future, it can start anticipating feedbacks and make ready its next offer).
  /// For the next iteration, based on the feedback it hears about the Aggregate phrase it presented to the user, 
  /// it will move that phrase to the KnownAggregatePhrases list.  If adjacent phrases are known, it will then
  /// aggregate the phrases into a larger aggregate compound (approximately 2 X AggregateSize), and add _this_
  /// aggregate phrase to the list of UnknownAggregatePhrases.  
  /// </summary>
  [Export(typeof(Interfaces.IMeaningStudier<MultiLineTextList>))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class DefaultMultiLineTextsMeaningStudier : MultiLineTextsMeaningStudierBase
  {
    #region Ctors and Init

    #endregion

    #region Fields

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
    /// These lines are actively being studied using the aggregating system.  They should be classified as 
    /// "unknown" although they are possibly partially known.
    /// </summary>
    protected List<LineEdit> ActiveLines { get; set; }

    /// <summary>
    /// These lines have yet to be entered into the aggregating system.
    /// </summary>
    protected List<LineEdit> UnknownLines { get; set; }

    /// <summary>
    /// These lines have been through the aggregating system, and are marked as unknown.
    /// </summary>
    protected List<LineEdit> KnownLines { get; set; }

    /// <summary>
    /// This list contains all aggregate phrases, including those of AggregateSize and higher order
    /// aggregate sizes (c. 2X, c. 4X, etc).
    /// </summary>
    protected List<LineAggregates> UnknownAggregatePhrases { get; set; }

    protected List<LineAggregates> KnownAggregatePhrases { get; set; }
    
    #endregion

    #region Methods

    protected override void StudyImpl()
    {

    }

    protected void UpdatePercentKnowns()
    {

    }

    protected void ChooseAggregateSize()
    {

    }

    protected virtual void BuildAggregatePhrases()
    {

    }

    public double GetPercentKnownLines()
    {
      throw new NotImplementedException();
    }

    public double GetPercentKnownWords()
    {
      throw new NotImplementedException();
    }
    
    #endregion
  }
}
