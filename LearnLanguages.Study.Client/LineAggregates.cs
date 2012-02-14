using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Common;

namespace LearnLanguages.Study
{
  public class LineAggregates
  {
    #region Ctors and Init

    public LineAggregates(LineEdit line, int originalAggregateSize)
    {
      if (line == null)
        throw new ArgumentNullException("line");

      Line = line;
      OriginalAggregateSize = originalAggregateSize;
      AggregatePhraseTexts = new List<string>();
      KnownPhraseTexts = new List<string>();

      PopulateAggregatePhraseTexts(originalAggregateSize);
    }

    #endregion

    #region Properties

    public LineEdit Line { get; private set; }

    /// <summary>
    /// This AggregateSize this object was created with.
    /// </summary>
    public int OriginalAggregateSize { get; private set; }

    /// <summary>
    /// The Phrase's order in the list is their order in the line.
    /// </summary>
    public List<string> AggregatePhraseTexts { get; private set; }

    /// <summary>
    /// Contains list of known phrase texts according to this object.  This does not reference
    /// any outside source for knowledge.  It only is an internal reference.
    /// </summary>
    public List<string> KnownPhraseTexts { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Parses the Line.Text into phrase texts, each phrase with a word count of aggregateSize, excepting
    /// the final phrase which may have fewer words.
    /// </summary>
    /// <param name="aggregateSize">the number of words contained in each aggregate phrase text.</param>
    public void PopulateAggregatePhraseTexts(int aggregateSize)
    {
      if (aggregateSize == 0)
        throw new ArgumentOutOfRangeException("aggregateSize");

      AggregatePhraseTexts.Clear();

      var lineText = Line.Phrase.Text;
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
      }
    }

    /// <summary>
    /// Searches within all KnownPhraseTexts for the given phraseText.  
    /// Returns true if known, else false.
    /// If you are asking about an aggregate phrase of which you know each of the pieces,
    /// BUT we have NOT marked the entire aggregate phrase as known, this will return false.
    /// Knowing the pieces is not necessarily knowing the whole.
    /// </summary>
    public bool IsKnown(string phraseText)
    {
      var isKnown = (from knownPhraseText in KnownPhraseTexts
                     where knownPhraseText.Contains(phraseText)
                     select knownPhraseText).Count() > 0;

      return isKnown;
    }

    /// <summary>
    /// Marks this phraseText internally as known.  This does not mark any outside knowledge base.
    /// </summary>
    public void MarkKnown(string phraseText)
    {
      if (!IsKnown(phraseText))
        KnownPhraseTexts.Add(phraseText);
    }

    /// <summary>
    /// Marks this phraseText internally as UNknown.  This does not mark any outside knowledge base.
    /// </summary>
    public void MarkUnknown(string phraseText)
    {
      if (IsKnown(phraseText))
        KnownPhraseTexts.Remove(phraseText);
    }

    /// <summary>
    /// Using this object's internal knowledge base, this will combine adjacent known aggregates 
    /// into their largest sizes possible.
    /// </summary>
    public void AggregateAdjacentKnownPhraseTexts()
    {
      PopulateAggregatePhraseTexts(OriginalAggregateSize);

      var maxIterations = 100; //escape for our while loop
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
          if (IsKnown(phraseA) && IsKnown(phraseB))
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

    #endregion
  }
}
