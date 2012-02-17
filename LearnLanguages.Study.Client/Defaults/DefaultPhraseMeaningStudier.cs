using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Offer;

namespace LearnLanguages.Study
{
  /// <summary>
  /// The studier that actually "does" something.
  /// 
  /// This implementation: "Dumb" studier.  This doesn't know anything about whether 
  /// a phrase's history, it "simply" takes the phrase, produces an offer to show a Q & A about it.
  /// Listens for the ViewModel to publish a Q & A response
  /// </summary>
  public class DefaultPhraseMeaningStudier : StudierBase<StudyJobInfo<PhraseEdit>, PhraseEdit>
  {
    #region Ctors and Init
    public DefaultPhraseMeaningStudier()
    {

    }
    #endregion

    #region Properties

    public string PhraseText { get; set; }
    //public LanguageEdit Language { get; set; }

    #endregion

    #region Methods

    //public bool StudyAgain()
    //{
    //  if (HasStudied)
    //  {
    //    StudyImpl();
    //    return true;
    //  }
    //  else
    //    return false;
    //}

    protected override void DoImpl()
    {
      //FINALLY, THIS SHOULD ACTUALLY DO SOMETHING.  IT TAKES THE PHRASE THAT IT CONTAINS.
      //IF IT IS IN THE NATIVE LANGUAGE, THEN IT JUST POPS UP A NATIVE LANGUAGE STUDY QUESTION.
      //IF IT IS IN A DIFFERENT LANGUAGE, THEN IT POPS UP EITHER DIRECTION Q & A, 50% CHANCE.
      StudyDataRetriever.CreateNew((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          var retriever = r.Object;
          var nativeLanguageText = retriever.StudyData.NativeLanguageText;
          if (string.IsNullOrEmpty(nativeLanguageText))
            throw new StudyException("No native language set.");

          if (_StudyJobInfo.Target == null)
            throw new StudyException("No PhraseEdit to study, _StudyJobInfo.Target == null.");

          var phraseEdit = _StudyJobInfo.Target;
          var phraseText = phraseEdit.Text;
          if (string.IsNullOrEmpty(phraseText))
            throw new StudyException("Attempted to study empty phrase text, _StudyJobInfo.Target.Text is null or empty.");

          var languageText = phraseEdit.Language.Text;

          //WE HAVE A PHRASEEDIT WITH A LANGUAGE AND WE HAVE OUR NATIVE LANGUAGE.
          //IF THE TWO LANGUAGES ARE DIFFERENT, THEN WE CREATE A TRANSLATION Q & A.
          //IF THE TWO LANGUAGES ARE THE SAME, THEN WE CREATE A STUDY NATIVE LANGUAGE PHRASE Q & A.

          bool languagesAreDifferent = (languageText != nativeLanguageText);
          if (languagesAreDifferent)
          {
            //DO A TRANSLATION Q & A
          }
          else
          {
            //DO A NATIVE LANGUAGE Q & A
          }
        });
    }

    ///// <summary>
    ///// Parses the Line.Text into phrase texts, each phrase with a word count of aggregateSize, excepting
    ///// the final phrase which may have fewer words.
    ///// </summary>
    ///// <param name="aggregateSize">the number of words contained in each aggregate phrase text.</param>
    //public void PopulateAggregatePhraseTexts(int aggregateSize)
    //{
    //  if (aggregateSize == 0)
    //    throw new ArgumentOutOfRangeException("aggregateSize");

    //  AggregatePhraseTexts.Clear();

    //  var lineText = Line.Phrase.Text;
    //  //lets say lineText is 20 words long (long line).  our aggregateSize is 2.  
    //  //We will end up with 20/2 = 10 aggregate phrases.  
    //  //Now, say aggregate size is 7.  We will have 20 / 7 = 2 6/7, rounded up = 3 
    //  //phrases of lengths 7, 7, and 6.  So, in our for loop, we will iterate 
    //  //Count/AggregateSize rounded up number of times.

    //  var words = lineText.ParseIntoWords();
    //  var wordCount = words.Count;
    //  var aggregateCount = (wordCount - 1) / aggregateSize + 1; //equivalent to Count/AggregateSize rounded up

    //  //our first aggregate phrases up to the very last one will be of size aggregate size
    //  //but our last one is the remainder and may contain anywhere from 1 to aggregateSize
    //  //words in it.  so we must be mindful of this.
    //  for (int i = 0; i < aggregateCount; i++)
    //  {
    //    var aggregatePhraseText = "";
    //    for (int j = 0; j < aggregateSize; j++)
    //    {
    //      var wordIndex = (i * aggregateSize) + j;
    //      if (wordIndex >= words.Count)
    //        break;//we have reached the end of the last phrase
    //      var currentWord = words[wordIndex];

    //      if (string.IsNullOrEmpty(aggregatePhraseText))
    //        aggregatePhraseText = currentWord;
    //      else
    //        aggregatePhraseText += " " + currentWord;
    //    }

    //    if (!string.IsNullOrEmpty(aggregatePhraseText))
    //      AggregatePhraseTexts.Insert(i, aggregatePhraseText);

    //    //remove any phrases that are empty.  this shouldn't happen, but we'll log it if we find one.
    //    var count = AggregatePhraseTexts.Count;
    //    //counting down from bottom of index because we will be removing items if needed.
    //    for (int k = count - 1; k >= 0; k--)
    //    {
    //      if (string.IsNullOrEmpty(AggregatePhraseTexts[k]))
    //      {
    //        Services.Log(StudyResources.WarningMsgEmptyAggregatePhraseTextFound, 
    //                     LogPriority.Medium, 
    //                     LogCategory.Warning);
    //        AggregatePhraseTexts.RemoveAt(k);
    //      }
    //    }
    //  }
    //}

    ///// <summary>
    ///// Using this object's internal knowledge base, this will combine adjacent known aggregates 
    ///// into their largest sizes possible.  It will iterate through the phrase texts a maximum of
    ///// maxIterations.
    ///// </summary>
    //public void AggregateAdjacentKnownPhraseTexts(int maxIterations)
    //{
    //  //FIRST, GIVE US A FRESH START...I'M NOT ENTIRELY SURE THIS IS NECESSARY.
    //  PopulateAggregatePhraseTexts(AggregateSize);

    //  //SET UP A LOOP THAT ITERATES THROUGH THE LIST OF AGGREGATE PHRASES
    //  //
    //  //var maxIterations = 100; //escape for our while loop
    //  var iterations = 0;
    //  var stillLooking = true;
    //  while (stillLooking && (iterations < maxIterations))
    //  {
    //    //This is "starting" as in before our next for loop which may modify AggregatePhraseTexts.Count.
    //    var startingPhraseCount = AggregatePhraseTexts.Count;
    //    var foundNewAggregate = false;

    //    //We do this until count - 1, because we are indexing i and i+1.
    //    for (int i = 0; i < startingPhraseCount - 1; i++)
    //    {
    //      var phraseA = AggregatePhraseTexts[i];
    //      var phraseB = AggregatePhraseTexts[i + 1];
    //      if (IsPhraseKnown(phraseA) && IsPhraseKnown(phraseB))
    //      {
    //        var newAggregate = phraseA + " " + phraseB;

    //        AggregatePhraseTexts[i] = newAggregate;
    //        AggregatePhraseTexts.RemoveAt(i + 1);
    //        foundNewAggregate = true;
    //        break;
    //      }
    //    }

    //    //if we found a new aggregate, then we may yet find more in another iteration, 
    //    //so we are still looking.
    //    if (foundNewAggregate)
    //      stillLooking = true;
    //    else
    //      //if we didn't find a new aggregate, then we searched the entire list
    //      //of aggregate phrase texts and no adjacent phrase texts were known.
    //      //so, we have aggregated as much as we can, and we are no longer looking.
    //      stillLooking = false;

    //    //iteration counter for escaping out of this while loop.
    //    iterations++;
    //  }
    //}

    ///// <summary>
    ///// Searches within all KnownPhraseTexts for the given phraseText.  
    ///// Returns true if known, else false.
    ///// If you are asking about an aggregate phrase of which you know each of the pieces,
    ///// BUT we have NOT marked the entire aggregate phrase as known, this will return false.
    ///// Knowing the pieces is not necessarily knowing the whole.
    ///// </summary>
    //public bool IsPhraseKnown(string phraseText)
    //{
    //  var isKnown = (from knownPhraseText in KnownPhraseTexts
    //                 where knownPhraseText.Contains(phraseText)
    //                 select knownPhraseText).Count() > 0;

    //  return isKnown;
    //}

    ///// <summary>
    ///// Marks this phraseText internally as known.  This does not mark any outside knowledge base.
    ///// </summary>
    //public void MarkPhraseKnown(string phraseText)
    //{
    //  if (!IsPhraseKnown(phraseText))
    //    KnownPhraseTexts.Add(phraseText);
    //}

    ///// <summary>
    ///// Marks this phraseText internally as UNknown.  This does not mark any outside knowledge base.
    ///// </summary>
    //public void MarkPhraseUnknown(string phraseText)
    //{
    //  if (IsPhraseKnown(phraseText))
    //    KnownPhraseTexts.Remove(phraseText);
    //}

    ///// <summary>
    ///// Calculates the PercentKnown of this line.
    ///// </summary>
    ///// <returns></returns>
    //public double GetLinePercentKnown()
    //{
    //  var listKnown = new List<string>();
    //  var listUnknown = new List<string>();
    //  foreach (var phraseText in AggregatePhraseTexts)
    //  {
    //    var phraseTextDistinctWords = phraseText.ParseIntoWords().Distinct();
    //    if (IsPhraseKnown(phraseText))
    //      listKnown.AddRange(phraseTextDistinctWords);
    //    else
    //      listUnknown.AddRange(phraseTextDistinctWords);
    //  }

    //  int knownWordsCount = listKnown.Count;
    //  int unknownWordsCount = listUnknown.Count;
    //  int totalWordsCount = knownWordsCount + unknownWordsCount;
    //  if (totalWordsCount == 0)
    //    throw new Exception("knownWordsCount + unknownWordsCount == 0.  This means that we have an aggregate phrase of zero words in our LineAggregateInfo.  This shouldn't happen.");

    //  double percentKnown = (double)knownWordsCount / (double)(totalWordsCount);
    //  return percentKnown;
    //}

    #endregion
    
    
  }
}
