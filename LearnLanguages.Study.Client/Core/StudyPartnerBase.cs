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
      //_Studier =
      //  (IDo<IJobInfo<MultiLineTextList, IViewModelBase>, MultiLineTextList, IViewModelBase>)
      //  (new DefaultMultiLineTextsStudier());
      _Studier = new DefaultMultiLineTextsStudier();
      _OpenOffers = new List<IOffer<MultiLineTextList, IViewModelBase>>();
      _DeniedOffers = new List<IOffer<MultiLineTextList, IViewModelBase>>();
      _CurrentOffer = null;

      Exchange.Ton.SubscribeToOpportunities(this);
      Exchange.Ton.SubscribeToOfferResponses(this);
      Exchange.Ton.SubscribeToCancelations(this);
      Exchange.Ton.SubscribeToStatusUpdates(this);
    }

    public Guid Id { get; protected set; }
    public Guid ConglomerateId { get; protected set; }

    protected IDo<StudyJobInfo<MultiLineTextList, IViewModelBase>, MultiLineTextList, IViewModelBase> _Studier { get; set; }
    protected List<IOffer<MultiLineTextList, IViewModelBase>> _OpenOffers { get; set; }
    protected List<IOffer<MultiLineTextList, IViewModelBase>> _DeniedOffers { get; set; }
    protected IOffer<MultiLineTextList, IViewModelBase> _CurrentOffer { get; set; }

    protected void AbortCurrent()
    {
      if (_CurrentOffer == null)
        return;

      _Studier.PleaseStopDoing(_CurrentOffer.Opportunity.JobInfo.Id);
      _CurrentOffer = null;
    }

    public void Handle(IOpportunity<MultiLineTextList, IViewModelBase> message)
    {
      //WE ONLY CARE ABOUT STUDY OPPORTUNITIES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
        return;

      //WE ONLY CARE IF THE JOB INFO IS A STUDY JOB INFO<MLT>
      if (!(message.JobInfo is StudyJobInfo<MultiLineTextList, IViewModelBase>))
        return;

      //WE ONLY CARE IF WE UNDERSTAND THE STUDYJOBINFO.CRITERIA
      var studyJobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.JobInfo;
      if (!(studyJobInfo.Criteria is StudyJobCriteria))
        return;

      //WE HAVE A GENUINE OPPORTUNITY WE WOULD BE INTERESTED IN
      //MAKE THE OFFER FOR THE JOB
      var offer = new Offer<MultiLineTextList, IViewModelBase>(message, 
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

    public void Handle(IOfferResponse<MultiLineTextList, IViewModelBase> message)
    {
      //WE ONLY CARE ABOUT OFFERS IN THE STUDY CATEGORY
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
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

      //WE HAVE ALREADY CHECKED THIS TYPE CAST BEFORE MAKING OUR OFFER, IT SHOULD NOT HAVE CHANGED. 
      var jobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.Offer.Opportunity.JobInfo;
      _CurrentOffer = message.Offer;

      //PUBLISH STARTED UPDATE
      
      var startedUpdate =
        new StatusUpdate<MultiLineTextList, IViewModelBase>(CommonResources.StatusStarted,
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

    public void Handle(ICancelation message)
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// "Default" conglomerate messaging system.  This studier can keep in contact with the other default
    /// studiers through this message stream.
    /// </summary>
    /// <param name="message"></param>
    public void Handle(IConglomerateMessage message)
    {
      throw new NotImplementedException();
    }

    public void Handle(IStatusUpdate<MultiLineTextList, IViewModelBase> message)
    {
      //IF WE DON'T HAVE A CURRENT OFFER, THEN WE DON'T NEED TO LISTEN TO THIS MESSAGE.
      if (_CurrentOffer == null)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
        return;

      //WE ONLY CARE ABOUT STUDY MESSAGES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //NOT SURE IF THIS IS RIGHT
      if (_CurrentOffer.Opportunity.JobInfo.Id != message.JobInfo.Id)
        return;

      //THIS IS ONE OF THIS OBJECT'S UPDATES, SO BUBBLE IT BACK UP WITH THIS OFFER, OPPORTUNITY, ETC.

      //CREATE THE BUBBLING UP UPDATE
      var opportunity = _CurrentOffer.Opportunity;
      var offer = _CurrentOffer;
      var jobInfo = opportunity.JobInfo;
      var statusUpdate =
        new StatusUpdate<MultiLineTextList, IViewModelBase>(message.Status,
                                                            opportunity,
                                                            offer,
                                                            null,
                                                            jobInfo,
                                                            Id,
                                                            this,
                                                            StudyResources.CategoryStudy,
                                                            null);

      
      //BEFORE PUBLISHING UPDATE, CHECK FOR PRODUCT
      //IF THIS IS A COMPLETED STATUS UPDATE, THEN PRODUCT SHOULD BE SET.  SO, BUBBLE THIS ASPECT UP.
      if (message.Status == CommonResources.StatusCompleted)
      {
        if (message.JobInfo.Product != null && _CurrentOffer.Opportunity.JobInfo.Product != null)
        {
          jobInfo.Product = message.JobInfo.Product;
        }

        //WE'RE DONE WITH THE CURRENT OFFER
        _CurrentOffer = null;
      }

      //PUBLISH TO BUBBLE UP
      Exchange.Ton.Publish(statusUpdate);
    }

    
  }
}
