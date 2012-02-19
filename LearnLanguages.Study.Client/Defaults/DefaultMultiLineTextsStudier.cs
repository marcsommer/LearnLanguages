using System;
using System.ComponentModel.Composition;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;
using Caliburn.Micro;
using LearnLanguages.Offer;
using LearnLanguages.Common.Delegates;

namespace LearnLanguages.Study
{

  public class DefaultMultiLineTextsStudier //:
    //StudierBase<StudyJobInfo<MultiLineTextList, IViewModelBase>, MultiLineTextList, IViewModelBase>
    //IHandle<IStatusUpdate<MultiLineTextEdit, IViewModelBase>>
  {
    public void SetTarget(MultiLineTextList multiLineTexts)
    {
      _MeaningStudier = new DefaultMultiLineTextsMeaningStudier();
      _OrderStudier = new DefaultMultiLineTextsOrderStudier();
    }

    public void GetNextStudyItemViewModel(AsyncCallback<StudyItemViewModelArgs> callback)
    {

      //THIS LAYER OF STUDY DECIDES ON WHAT IS IMPORTANT: MEANING OR ORDER, THEN DELEGATES STUDY TO
      //THE CORRESPONDING STUDIER.

      //todo: analyze history events
      //for now, we will assume there is no history for the MLT, and thus we should focus on understanding
      //meaning of the song.
      //depending on history may ask user for info about MLT

      //update meaning and order percent knowns.  
      //These will affect the probability of choosing Meaning vs. Order practice.
      UpdatePercentKnowns();

      //if the randomDouble is below the threshold, then we go with meaning, else we go with order.
      if (ShouldStudyMeaning())
      {
        
        _MeaningStudier.Do(_StudyJobInfo);
        //_MeaningStudier.Do((StudyJobInfo<MultiLineTextList, IViewModelBase>)_StudyJobInfo);
      }
      else
      {
        //TODO: IMPLEMENT ORDER STUDIER DO AND REFERENCE THIS IN DEFAULT MLTS STUDIER
        //_MeaningStudier.Do(_StudyJobInfo);
//        _OrderStudier.Do(_StudyJobInfo);
        //_OrderStudier.Do((StudyJobInfo<MultiLineTextList, IViewModelBase>)_StudyJobInfo);
      }
    }

    private void UpdatePercentKnowns()
    {
      //todo: update percent knowns using history
      if (_MeaningStudier == null)
        MeaningPercentKnown = 0.0d;
      else
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

    //public void Handle(IStatusUpdate<MultiLineTextEdit, IViewModelBase> message)
    //{
    //  //WE ONLY CARE ABOUT STUDY MESSAGES
    //  if (message.Category != StudyResources.CategoryStudy)
    //    return;

    //  //WE ONLY CARE ABOUT MESSAGES PUBLISHED BY MLT MEANING STUDIERS
    //  if (
    //       (message.Publisher != null) &&
    //       !(message.Publisher is DefaultMultiLineTextsMeaningStudier)
    //     )
    //    return;

    //  ////WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
    //  //if (message.PublisherId == Id)
    //  //  return;

    //  //THIS IS ONE OF THIS OBJECT'S UPDATES, SO BUBBLE IT BACK UP WITH THIS JOB'S INFO

    //  //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
    //  if (message.Status == CommonResources.StatusCompleted && message.JobInfo.Product != null)
    //  {
    //    //_StudyJobInfo.Product = message.JobInfo.Product;
    //  }

    //  //CREATE THE BUBBLING UP UPDATE
    //  var statusUpdate = new StatusUpdate<MultiLineTextList, IViewModelBase>(message.Status, null, null,
    //    //null, _StudyJobInfo, Id, this, StudyResources.CategoryStudy, null);

    //  //PUBLISH TO BUBBLE UP
    //  Exchange.Ton.Publish(statusUpdate);
    //}
  }
}
