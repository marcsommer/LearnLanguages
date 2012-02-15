using System;
using System.ComponentModel.Composition;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;

namespace LearnLanguages.Study
{

  public class DefaultMultiLineTextsStudier : StudierBase<StudyJobInfo<MultiLineTextList>, MultiLineTextList>
  {
    public DefaultMultiLineTextsStudier()
    {
      ////Initialize our weights.  These affect the probability of choosing Meaning vs. Order practice.
      ////If we don't know the meaning of something, then we are not likely to remember it and it would be 
      ////useless to study the order often times, so it should have a higher priority over order.
      //MeaningWeight = double.Parse(StudyResources.DefaultMultiLineTextsStudierMeaningWeight);
      //OrderWeight = double.Parse(StudyResources.DefaultMultiLineTextsStudierOrderWeight);
    }

    protected override void StudyImpl()
    {
      //THIS LAYER OF STUDY DECIDES ON WHAT IS IMPORTANT: MEANING OR ORDER, THEN DELEGATES STUDY TO
      //THE CORRESPONDING STUDIER.

      //analyzes history events
      //for now, we will assume there is no history for the MLT, and thus we should focus on understanding
      //meaning of the song.
      //depending on history may ask user for info about MLT

      //update meaning and order percent knowns.  
      //These will affect the probability of choosing Meaning vs. Order practice.
      UpdatePercentKnowns();
     
      //if the randomDouble is below the threshold, then we go with meaning, else we go with order.
      if (ShouldStudyMeaning())
      {
        _MeaningStudier.Study(_StudyJobInfo, _OfferExchange);
      }
      else
        _OrderStudier.Study(_StudyJobInfo, _OfferExchange);
    }

    private void UpdatePercentKnowns()
    {
      //todo: update percent knowns using history
      MeaningPercentKnown = _MeaningStudier.GetPercentKnown();
    }

    /// <summary>
    /// This only uses the percent of meaning known to calculate the odds and then run a probability
    /// to see if we should study meaning.
    /// </summary>
    private bool ShouldStudyMeaning()
    {
      //function: y = -((x-.1)^(6) + (x-.1)^4) + .9
      //x = percent of meaning known
      //y = probability should study meaning
      
      var random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute);
      var x = MeaningPercentKnown / 100.0d;
      var A = Math.Pow(x-0.1, 6); //1st term in above function, (x-0.1)^6
      var B = Math.Pow(x-0.1, 4); //2nd term in above function, (x-0.1)^4
      var probabilityChooseMeaning = -(A + B) + .9;

      //Now we have our probability.  We can think of this probability as a threshold between 0.0 and 1.0.
      //If we have a probability of 0.7, then if we choose a random double between 0.0 and 1.0 and it is 
      //below 0.7, then we can think of that as hitting the probability, otherwise we are not hitting it.
      //If our probability is 0.1, then we are very unlikely to choose a random double below 0.1.

      //So, choose our random double between 0.0 and 1.0
      var randomDouble = random.NextDouble();

      //Check to see if this is below our probability of choosing meaning.
      var randomDoubleIsBelowThreshold = (randomDouble < probabilityChooseMeaning);

      //if it is below, then we "hit" choose meaning, otherwise we will choose order.
      var chooseMeaning = randomDoubleIsBelowThreshold;

      //Overall, I don't like this wording, but I think it works.
      return chooseMeaning;
    }

    private DefaultMultiLineTextsOrderStudier _OrderStudier { get; set; }
    private DefaultMultiLineTextsMeaningStudier _MeaningStudier { get; set; }

    //private double MeaningWeight { get; set; }
    //private double OrderWeight { get; set; }
    private double MeaningPercentKnown { get; set; }
    //private double OrderPercentKnown { get; set; }
  }
}
