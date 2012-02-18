using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using Caliburn.Micro;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Contains a single Line's information about its aggregate phrases.  It maintains its own 
  /// knowledge of PhraseTexts "KnownPhraseTexts", and exposes methods for marking known/unknown,
  /// asking if IsKnown(phraseText), and performing the actual aggregation.  This includes 
  /// populating aggregate phrase texts with the OriginalAggregateSize, as well as aggregating
  /// adjacent known phrase texts, according to its own knowledge.
  /// </summary>
  public class DefaultLineMeaningStudier : 
    StudierBase<StudyJobInfo<LineEdit, IViewModelBase>, LineEdit, IViewModelBase>,
    IHandle<IStatusUpdate<PhraseEdit, IViewModelBase>>
  {
    #region Ctors and Init

    public DefaultLineMeaningStudier()
    {
      _Studiers = new Dictionary<int, DefaultPhraseMeaningStudier>();
      Exchange.Ton.SubscribeToStatusUpdates(this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Dictionary of phrase meaning studiers, indexed by their position in this line.
    /// </summary>
    private Dictionary<int, DefaultPhraseMeaningStudier> _Studiers { get; set; }

    /// <summary>
    /// This AggregateSize this object was created with.
    /// </summary>
    public int AggregateSize { get; private set; }

    /// <summary>
    /// The Phrase's order in the list is their order in the line.
    /// </summary>
    public List<string> AggregatePhraseTexts { get; private set; }

    /// <summary>
    /// Contains list of known phrase texts according to this object.  This does not reference
    /// any outside source for knowledge.  It only is an internal reference.
    /// </summary>
    public List<string> KnownPhraseTexts { get; private set; }

    /// <summary>
    /// The index of the last studied phrase in AggregatePhraseTexts
    /// </summary>
    private int _LastStudiedIndex { get; set; }
    #endregion

    #region Methods

    //public bool StudyAgain()
    //{
    //  if (IsNotFirstRun)
    //  {
    //    DoImpl();
    //    return true;
    //  }
    //  else
    //    return false;
    //}

    protected override void DoImpl()
    {
      if (!IsNotFirstRun) //IS FIRST RUN...ALREADY TODOING REFACTOR
      {
        AggregateSize = int.Parse(StudyResources.DefaultMeaningStudierAggregateSize);
        AggregatePhraseTexts = new List<string>();
        KnownPhraseTexts = new List<string>();
        PopulateAggregatePhraseTexts(AggregateSize);
        //CURRENTLY NO NEED TO AGGREGATE BECAUSE WE START FROM UNKNOWN STATE.
        //ONCE WE START USING AN EXTERNAL KNOWLEDGE STATE, THEN WE CAN AGGREGATE 
        //IF THAT IS HOW WE HAVE IT SET UP
        PopulateStudiersWithUnknownAggregatePhraseTexts();
      }
      else
      {
        //IF OUR _LASTSTUDIEDINDEX IS MAXED OUT AT AGGREGATE PHRASE TEXTS LIST, THEN WE
        //HAVE COMPLETED ONE ITERATION THROUGH OUR PHRASE TEXTS.  
        if (_LastStudiedIndex >= (AggregatePhraseTexts.Count - 1))
        {
          //NOW WE WILL AGGREGATE OUR ADJACENT KNOWN TEXTS AND REPOPULATE OUR PHRASE STUDIERS
          //WITH ONLY THOSE PHRASES THAT WE DON'T KNOW
          var maxIterations = int.Parse(StudyResources.DefaultMaxIterationsAggregateAdjacentKnownPhraseTexts);
          AggregateAdjacentKnownPhraseTexts(maxIterations);
          PopulateStudiersWithUnknownAggregatePhraseTexts();
          if (_Studiers.Count == 0)
            throw new Exception("todo: figure out how to communicate that this line is 100% known/what to do");
        }
      }

      //WE NOW HAVE OUR STUDIERS.  GET OUR INDEX TO STUDY, POPULATE THAT STUDIER, AND DO IT.
      var indexToStudy = _LastStudiedIndex + 1;
      //var phraseText = _Studiers[indexToStudy].PhraseText;
      var phraseText = AggregatePhraseTexts[indexToStudy];
      var language = this._StudyJobInfo.Target.Phrase.Language;
        
      PhraseEdit.NewPhraseEdit(language.Text, (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var phrase = r.Object;
          phrase.Text = phraseText;
          var criteria = (StudyJobCriteria)_StudyJobInfo.Criteria;


          //CREATE THE JOB INFO
          var studyJobInfo = new StudyJobInfo<PhraseEdit, IViewModelBase>(phrase, 
                                                                          criteria.Language, 
                                                                          _StudyJobInfo.ExpirationDate, 
                                                                          criteria.ExpectedPrecision);
          //I'M INCREMENTING THIS BEFORE EXECUTION.  PROBABLY CAN DO THIS AFTERWARDS, 
          //BUT I WANT TO BE SURE THIS IS INCREMENTED BEFORE THE STUDIER DOES ANYTHING.
          _LastStudiedIndex++;

          //INVOKE THE STUDIER TO DO THE JOB, THIS DOES NOT USE THE EXCHANGE...BUT!!
          //BUT THIS DOES LISTEN FOR COMPLETION USING THE EXCHANGE.
          var studier = _Studiers[indexToStudy];
          studier.Do(studyJobInfo);

          //THIS DOES LISTEN FOR COMPLETION USING THE EXCHANGE, SO GOTO HANDLE<JOB<PHRASEEDIT, IVIEWMODEL>...
        });
      
    }

    private void PopulateStudiersWithUnknownAggregatePhraseTexts()
    {
      _Studiers.Clear();
      _LastStudiedIndex = -1;

      //unknownRelativeOrder: unknown in that our phrase is unknown.  Relative order means
      //the order/position of this phrase relative to the other _unknown_ phrases.
      var unknownRelativeOrder = -1;
      for (int i = 0; i < AggregatePhraseTexts.Count; i++)
      {
        var phraseText = AggregatePhraseTexts[i];
        if (IsPhraseKnown(phraseText))
          continue;

        var studier = new DefaultPhraseMeaningStudier();
        //studier.PhraseText = phraseText;
        //studier.Language = _StudyJobInfo.Target.Phrase.Language;
        unknownRelativeOrder++;
        _Studiers.Add(unknownRelativeOrder, studier);
      }
    }

    /// <summary>
    /// Parses the Line.Text into phrase texts, each phrase with a word count of aggregateSize, excepting
    /// the final phrase which may have fewer words.
    /// </summary>
    /// <param name="aggregateSize">the number of words contained in each aggregate phrase text.</param>
    private void PopulateAggregatePhraseTexts(int aggregateSize)
    {
      if (aggregateSize == 0)
        throw new ArgumentOutOfRangeException("aggregateSize");

      AggregatePhraseTexts.Clear();

      var lineText = _StudyJobInfo.Target.Phrase.Text;
      //lets say lineText is 20 words long (long line).  our aggregateSize is 2.  
      //We will end up with 20/2 = 10 aggregate phrases.  
      //Now, say aggregate size is 7.  We will have 20 / 7 = 2 6/7, rounded up = 3 
      //phrases of lengths 7, 7, and 6.  So, in our for loop, we will iterate 
      //Count/AggregateSize rounded up number of times.

      var words = lineText.ParseIntoWords();
      var wordCount = words.Count;
      var aggregateCount = (wordCount - 1) / aggregateSize + 1; //equivalent to Count/AggregateSize rounded up

      //our first aggregate phrases up to the very last one will be of size aggregate size
      //but our last one is the remainder and may contain anywhere from 1 to aggregateSize
      //words in it.  so we must be mindful of this.
      for (int i = 0; i < aggregateCount; i++)
      {
        var aggregatePhraseText = "";
        for (int j = 0; j < aggregateSize; j++)
        {
          var wordIndex = (i * aggregateSize) + j;
          if (wordIndex >= words.Count)
            break;//we have reached the end of the last phrase
          var currentWord = words[wordIndex];

          if (string.IsNullOrEmpty(aggregatePhraseText))
            aggregatePhraseText = currentWord;
          else
            aggregatePhraseText += " " + currentWord;
        }

        if (!string.IsNullOrEmpty(aggregatePhraseText))
          AggregatePhraseTexts.Insert(i, aggregatePhraseText);

        //remove any phrases that are empty.  this shouldn't happen, but we'll log it if we find one.
        var count = AggregatePhraseTexts.Count;
        //counting down from bottom of index because we will be removing items if needed.
        for (int k = count - 1; k >= 0; k--)
        {
          if (string.IsNullOrEmpty(AggregatePhraseTexts[k]))
          {
            Services.Log(StudyResources.WarningMsgEmptyAggregatePhraseTextFound, 
                         LogPriority.Medium, 
                         LogCategory.Warning);
            AggregatePhraseTexts.RemoveAt(k);
          }
        }
      }
    }

    /// <summary>
    /// Using this object's internal knowledge base, this will combine adjacent known aggregates 
    /// into their largest sizes possible.  It will iterate through the phrase texts a maximum of
    /// maxIterations.
    /// </summary>
    private void AggregateAdjacentKnownPhraseTexts(int maxIterations)
    {
      //FIRST, GIVE US A FRESH START...I'M NOT ENTIRELY SURE THIS IS NECESSARY.
      PopulateAggregatePhraseTexts(AggregateSize);

      //SET UP A LOOP THAT ITERATES THROUGH THE LIST OF AGGREGATE PHRASES
      //
      //var maxIterations = 100; //escape for our while loop
      var iterations = 0;
      var stillLooking = true;
      while (stillLooking && (iterations < maxIterations))
      {
        //This is "starting" as in before our next for loop which may modify AggregatePhraseTexts.Count.
        var startingPhraseCount = AggregatePhraseTexts.Count;
        var foundNewAggregate = false;

        //We do this until count - 1, because we are indexing i and i+1.
        for (int i = 0; i < startingPhraseCount - 1; i++)
        {
          var phraseA = AggregatePhraseTexts[i];
          var phraseB = AggregatePhraseTexts[i + 1];
          if (IsPhraseKnown(phraseA) && IsPhraseKnown(phraseB))
          {
            var newAggregate = phraseA + " " + phraseB;

            AggregatePhraseTexts[i] = newAggregate;
            AggregatePhraseTexts.RemoveAt(i + 1);
            foundNewAggregate = true;
            break;
          }
        }

        //if we found a new aggregate, then we may yet find more in another iteration, 
        //so we are still looking.
        if (foundNewAggregate)
          stillLooking = true;
        else
          //if we didn't find a new aggregate, then we searched the entire list
          //of aggregate phrase texts and no adjacent phrase texts were known.
          //so, we have aggregated as much as we can, and we are no longer looking.
          stillLooking = false;

        //iteration counter for escaping out of this while loop.
        iterations++;
      }
    }

    /// <summary>
    /// Searches within all KnownPhraseTexts for the given phraseText.  
    /// Returns true if known, else false.
    /// If you are asking about an aggregate phrase of which you know each of the pieces,
    /// BUT we have NOT marked the entire aggregate phrase as known, this will return false.
    /// Knowing the pieces is not necessarily knowing the whole.
    /// </summary>
    private bool IsPhraseKnown(string phraseText)
    {
      var isKnown = (from knownPhraseText in KnownPhraseTexts
                     where knownPhraseText.Contains(phraseText)
                     select knownPhraseText).Count() > 0;

      return isKnown;
    }

    /// <summary>
    /// Marks this phraseText internally as known.  This does not mark any outside knowledge base.
    /// </summary>
    private void MarkPhraseKnown(string phraseText)
    {
      if (!IsPhraseKnown(phraseText))
        KnownPhraseTexts.Add(phraseText);
    }

    /// <summary>
    /// Marks this phraseText internally as UNknown.  This does not mark any outside knowledge base.
    /// </summary>
    private void MarkPhraseUnknown(string phraseText)
    {
      if (IsPhraseKnown(phraseText))
        KnownPhraseTexts.Remove(phraseText);
    }

    /// <summary>
    /// Calculates the PercentKnown of this line.
    /// </summary>
    /// <returns></returns>
    public double GetLinePercentKnown()
    {
      if (_StudyJobInfo == null)
        return 0;

      var listKnown = new List<string>();
      var listUnknown = new List<string>();
      foreach (var phraseText in AggregatePhraseTexts)
      {
        var phraseTextDistinctWords = phraseText.ParseIntoWords().Distinct();
        if (IsPhraseKnown(phraseText))
          listKnown.AddRange(phraseTextDistinctWords);
        else
          listUnknown.AddRange(phraseTextDistinctWords);
      }

      int knownWordsCount = listKnown.Count;
      int unknownWordsCount = listUnknown.Count;
      int totalWordsCount = knownWordsCount + unknownWordsCount;
      if (totalWordsCount == 0)
        throw new Exception("knownWordsCount + unknownWordsCount == 0.  This means that we have an aggregate phrase of zero words in our LineAggregateInfo.  This shouldn't happen.");

      double percentKnown = (double)knownWordsCount / (double)(totalWordsCount);
      return percentKnown;
    }

    #endregion

    public void Handle(IStatusUpdate<PhraseEdit, IViewModelBase> message)
    {
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE ONLY CARE ABOUT MESSAGES PUBLISHED BY PHRASE MEANING STUDIERS
      if (
           (message.Publisher != null) && 
           !(message.Publisher is DefaultPhraseMeaningStudier)
         )
        return;

      ////WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      //if (message.PublisherId == Id)
      //  return;


      //TODO: CHECK TO SEE IF THIS IS ONE OF THIS OBJECT'S UPDATES.  RIGHT NOW THEY ALL WILL BE, BUT THIS OBJECT SHOULD TRACK ITS OPEN JOBS.

      //THIS IS ONE OF THIS OBJECT'S UPDATES, SO BUBBLE IT BACK UP WITH THIS JOB'S INFO

      //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
      if (message.Status == CommonResources.StatusCompleted && 
          message.JobInfo.Product != null &&
          _StudyJobInfo != null)
      {
        _StudyJobInfo.Product = message.JobInfo.Product;
      }
      
      //CREATE THE BUBBLING UP UPDATE
      var statusUpdate = new StatusUpdate<LineEdit, IViewModelBase>(message.Status, null, null, 
        null, _StudyJobInfo, Id, this, StudyResources.CategoryStudy, null);
        
      //PUBLISH TO BUBBLE UP
      Exchange.Ton.Publish(statusUpdate);
    }
  }
}
