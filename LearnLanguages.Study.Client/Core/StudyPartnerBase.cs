using System;
using System.Linq;
using System.Collections.Generic;
using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Simple IStudyPartner base class.  You do not HAVE to descend from this, it just provides some 
  /// very basic plumbing.
  /// </summary>
  public abstract class StudyPartnerBase : IStudyPartner
  {
    public StudyPartnerBase()
    {
      Id = Guid.NewGuid();
      _Studier =
        (IDo<IJobInfo<MultiLineTextList>, MultiLineTextList>)(new DefaultMultiLineTextsStudier());
      _OpenOffers = new List<Offer<MultiLineTextList>>();
      _DeniedOffers = new List<Offer<MultiLineTextList>>();
      _CurrentOffer = null;

      Exchange.Ton.SubscribeToOpportunities(this);
      Exchange.Ton.SubscribeToOfferResponses(this);
      Exchange.Ton.SubscribeToCancelations(this);
    }

    public Guid Id { get; protected set; }
    public Guid ConglomerateId { get; protected set; }

    protected IDo<IJobInfo<MultiLineTextList>, MultiLineTextList> _Studier { get; set; }
    protected List<Offer<MultiLineTextList>> _OpenOffers { get; set; }
    protected List<Offer<MultiLineTextList>> _DeniedOffers { get; set; }
    protected IOffer<MultiLineTextList> _CurrentOffer { get; set; }

    protected void AbortCurrent()
    {
      if (_CurrentOffer == null)
        return;

      _Studier.PleaseStopDoing(_CurrentOffer.Opportunity.JobInfo.Id);
      _CurrentOffer = null;
    }

    public void Handle(Opportunity<MultiLineTextList> message)
    {
      //WE ONLY CARE ABOUT STUDY OPPORTUNITIES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE ONLY CARE IF THE JOB INFO IS A STUDY JOB INFO<MLT>
      if (!(message.JobInfo is StudyJobInfo<MultiLineTextList>))
        return;

      //WE ONLY CARE IF WE UNDERSTAND THE STUDYJOBINFO.CRITERIA
      StudyJobInfo<MultiLineTextList> studyJobInfo = (StudyJobInfo<MultiLineTextList>)message.JobInfo;
      if (!(studyJobInfo.Criteria is StudyJobCriteria))
        return;

      //WE HAVE A GENUINE OPPORTUNITY WE WOULD BE INTERESTED IN
      //MAKE THE OFFER FOR THE JOB
      var offer = new Offer<MultiLineTextList>(message, 
                                               this.Id,
                                               this, 
                                               double.Parse(StudyResources.DefaultAmountDefaultMultiLineTextsStudier),
                                               StudyResources.CategoryStudy, 
                                               null);

      //FIRST ADD THIS OFFER TO OUR LIST OF OFFERS
      _OpenOffers.Add(offer);

      //PUBLISH THE OFFER
      Exchange.Ton.Publish(offer);
    }

    public void Handle(OfferResponse<MultiLineTextList> message)
    {
      //WE ONLY CARE ABOUT OFFERS IN THE STUDY CATEGORY
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE ONLY CARE ABOUT RESPONSES TO OFFERS THAT WE MADE
      var results = (from offer in _OpenOffers
                     where offer.Id == message.Offer.Id
                     select offer);

      if (results.Count() != 1)
        return;

      var pertinentOffer = results.First();

      if (message.Response == OfferResources.OfferResponseDeny)
      {
        _OpenOffers.Remove(pertinentOffer);
        _DeniedOffers.Add(pertinentOffer);
        return;
      }
      else if (message.Response != OfferResources.OfferResponseAccept)
      {
        //INVALID OFFER RESPONSE THAT WE DON'T KNOW HOW TO HANDLE
        _OpenOffers.Remove(pertinentOffer);
        var msg = string.Format(StudyResources.WarningMsgUnknownOfferResponseResponse, message.Response);
        Services.Log(msg, LogPriority.High, LogCategory.Warning);
        return;
      }
       
      //WE HAVE AN ACCEPT RESPONSE WITH AN OPPORTUNITY THAT WE CARE ABOUT
      //WHATEVER OPPORTUNITY WE ARE WORKING ON, THE NEW ONE SUPERCEDES IT
      if (_CurrentOffer != null)
        AbortCurrent();

      var jobInfo = message.Offer.Opportunity.JobInfo;
      _CurrentOffer = message.Offer;
      //PUBLISH STARTED UPDATE
      var startedUpdate =
        new StatusUpdate<MultiLineTextList>(CommonResources.StatusStarted,
                                            _CurrentOffer.Opportunity,
                                            _CurrentOffer,
                                            message,
                                            jobInfo,
                                            Id,
                                            this,
                                            StudyResources.CategoryStudy,
                                            null);
      Exchange.Ton.Publish(startedUpdate);

      //DELEGATE JOB TO MLTs STUDIER
      _Studier.Do(jobInfo);
    }

    public void Handle(Cancelation message)
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// "Default" conglomerate messaging system.  This studier can keep in contact with the other default
    /// studiers through this message stream.
    /// </summary>
    /// <param name="message"></param>
    public void Handle(ConglomerateMessage message)
    {
      throw new NotImplementedException();
    }
  }
}
