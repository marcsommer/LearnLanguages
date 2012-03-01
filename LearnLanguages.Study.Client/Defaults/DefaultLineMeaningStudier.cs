using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;
using Caliburn.Micro;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Study.Advisors;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Contains a single Line's information about its aggregate phrases.  It maintains its own 
  /// knowledge of PhraseTexts "KnownPhraseTexts", and exposes methods for marking known/unknown,
  /// asking if IsKnown(phraseText), and performing the actual aggregation.  This includes 
  /// populating aggregate phrase texts with the OriginalAggregateSize, as well as aggregating
  /// adjacent known phrase texts, according to its own knowledge.
  /// </summary>
  public class DefaultLineMeaningStudier : StudierBase<LineEdit>
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

    private object _IsReadyLock = new object();
    private bool _isReady = false;
    protected bool _IsReady
    {
      get
      {
        lock (_IsReadyLock)
        {
          return _isReady;
        }
      }
      set
      {
        lock (_IsReadyLock)
        {
          _isReady = value;
        }
      }
    }

    #endregion

    #region Methods

    public override void GetNextStudyItemViewModel(Common.Delegates.AsyncCallback<StudyItemViewModelArgs> callback)
    {
      //IF OUR _LASTSTUDIEDINDEX IS MAXED OUT AT AGGREGATE PHRASE TEXTS LIST, THEN WE
      //HAVE COMPLETED ONE ITERATION THROUGH OUR PHRASE TEXTS.  
      if (_LastStudiedIndex >= (AggregatePhraseTexts.Count - 1))
      {
        //NOW WE WILL AGGREGATE OUR ADJACENT KNOWN TEXTS AND REPOPULATE OUR PHRASE STUDIERS
        //WITH ONLY THOSE PHRASES THAT WE DON'T KNOW
        var maxIterations = int.Parse(StudyResources.DefaultMaxIterationsAggregateAdjacentKnownPhraseTexts);
        AggregateAdjacentKnownPhraseTexts(maxIterations);
        PopulateStudiersWithUnknownAggregatePhraseTexts((e) =>
        {
          if (_Studiers.Count == 0)
            throw new Exception("todo: figure out how to communicate that this line is 100% known/what to do");

          //WE NOW HAVE COMPLETELY NEW STUDIERS, ALREADY INITIALIZED, SO WE JUST NEED TO RUN 
          //THE FIRST STUDIER AND UPDATE OUR LAST STUDIED INDEX
          _Studiers[0].GetNextStudyItemViewModel(callback);
          _LastStudiedIndex = 0;
        });
      }
      else
      {
        //WE ARE CONTINUING A STUDY CYCLE, SO RUN THE NEXT ONE AND UPDATE OUR LAST STUDIED INDEX.
        var indexToStudy = _LastStudiedIndex + 1;
        _Studiers[indexToStudy].GetNextStudyItemViewModel((s, r) =>
          {
            if (r.Error != null)
              throw r.Error;

            _LastStudiedIndex++;
            callback(this, r);
          });
      }
    }

    private void PopulateStudiersWithUnknownAggregatePhraseTexts(ExceptionCheckCallback callback)
    {
      _Studiers.Clear();
      _LastStudiedIndex = -1;

      var phraseTextsCriteria = 
        new Business.Criteria.PhraseTextsCriteria(_Target.Phrase.Language.Text, AggregatePhraseTexts);
      PhraseList.NewPhraseList(phraseTextsCriteria, (s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var phraseList = r.Object;
          List<PhraseEdit> unknownPhraseEdits = new List<PhraseEdit>();
          for (int i = 0; i < AggregatePhraseTexts.Count; i++)
          {
            var results = from phrase in phraseList
                          where phrase.Text == AggregatePhraseTexts[i]
                          select phrase;
            unknownPhraseEdits.Insert(i, results.First());
          }

          //WE NOW HAVE AN ORDERED LIST OF KNOWN AND UNKNOWN AGGREGATE PHRASEEDITS.
          //WE NEED TO FIND, AND KEEP IN RELATIVE ORDER, ONLY THE UNKNOWN PHRASEEDITS.
          //EACH PHRASE EDIT IS THE SAME INDEX AS ITS PHRASETEXT COUNTERPART.

          //WE NEED TO FIND THE COUNT OF UNKNOWN TEXTS FIRST, TO INCREMENT OUR COUNTER FOR ASYNC FUNCTIONALITY.
          int unknownCount = 0;
          for (int i = 0; i < AggregatePhraseTexts.Count; i++)
          {
            var phraseText = AggregatePhraseTexts[i];
            if (!IsPhraseKnown(phraseText))
              unknownCount++;
          }

          //UNKNOWNRELATIVEORDER: UNKNOWN IN THAT OUR PHRASE IS UNKNOWN.  RELATIVE ORDER MEANS
          //THE ORDER/POSITION OF THIS PHRASE RELATIVE TO THE OTHER _UNKNOWN_ PHRASES.
          var unknownRelativeOrder = -1;
          int initializedCount = 0;
          for (int i = 0; i < AggregatePhraseTexts.Count; i++)
          {
            var phraseText = AggregatePhraseTexts[i];
            if (IsPhraseKnown(phraseText))
              continue;

            //PHRASE IS UNKNOWN, SO INC OUR RELATIVE ORDER AND ADD THE STUDIER TO STUDIERS, INITIALIZING EACH STUDIER.
            unknownRelativeOrder++;
            var studier = new DefaultPhraseMeaningStudier();
            studier.InitializeForNewStudySession(unknownPhraseEdits[i], (e) =>
              {
                if (e != null)
                  throw e;

                _Studiers.Add(unknownRelativeOrder, studier);
                initializedCount++;
                //IF WE HAVE INITIALIZED ALL OF OUR UNKNOWN PHRASES, THEN WE ARE DONE AND CAN CALL CALLBACK.
                if (initializedCount == unknownCount)
                  callback(null);
              });

          }
        });

      
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

      var lineText = _Target.Phrase.Text;
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

        //IF WE FOUND A NEW AGGREGATE, THEN WE MAY YET FIND MORE IN ANOTHER ITERATION, 
        //SO WE ARE STILL LOOKING.
        if (foundNewAggregate)
          stillLooking = true;
        else
          //IF WE DIDN'T FIND A NEW AGGREGATE, THEN WE SEARCHED THE ENTIRE LIST
          //OF AGGREGATE PHRASE TEXTS AND NO ADJACENT PHRASE TEXTS WERE KNOWN.
          //SO, WE HAVE AGGREGATED AS MUCH AS WE CAN, AND WE ARE NO LONGER LOOKING.
          stillLooking = false;

        //ITERATION COUNTER FOR ESCAPING OUT OF THIS WHILE LOOP.
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
    public double GetPercentKnown()
    {
      if (_Target == null)
        return 0;

      var listKnown = new List<string>();
      var listUnknown = new List<string>();
      for (int i = 0; i < AggregatePhraseTexts.Count; i++)
      {
        var phraseText = AggregatePhraseTexts[i];
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

    public override void InitializeForNewStudySession(LineEdit target, ExceptionCheckCallback completedCallback)
    {
      _IsReady = false;
      _Target = target;
      AggregateSize = int.Parse(StudyResources.DefaultMeaningStudierAggregateSize);
      AggregatePhraseTexts = new List<string>();
      KnownPhraseTexts = new List<string>();
      PopulateAggregatePhraseTexts(AggregateSize);
      //CURRENTLY NO NEED TO AGGREGATE ADJACENT KNOWN PHRASE TEXTS BECAUSE WE START FROM UNKNOWN STATE.
      //ONCE WE START USING AN EXTERNAL KNOWLEDGE STATE, THEN WE CAN AGGREGATE 
      //AggregateAdjacentKnownPhraseTexts(
      //  int.Parse(StudyResources.DefaultMaxIterationsAggregateAdjacentKnownPhraseTexts));
      PopulateStudiersWithUnknownAggregatePhraseTexts((e) =>
        {
          if (e != null)
            throw e;

          completedCallback(null);
        });
    }
  }
}
