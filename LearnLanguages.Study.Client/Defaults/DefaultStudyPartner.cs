using System;
using System.Linq;
using System.ComponentModel.Composition;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Business;
using LearnLanguages.Offer;
using Caliburn.Micro;
using System.ComponentModel;

namespace LearnLanguages.Study
{
  /// <summary>
  /// 
  /// </summary>
  [Export(typeof(IStudyPartner))]
  public class DefaultStudyPartner : StudyPartnerBase
  {
    public DefaultStudyPartner()
    {
      _Studier = new DefaultMultiLineTextsStudier();
      _ViewModel = new ViewModels.DefaultStudyMultiLineTextsViewModel();
    }

    protected DefaultMultiLineTextsStudier _Studier { get; set; }

    #region protected DefaultStudyMultiLineTextsViewModel _ViewModel
    protected volatile object _ViewModelLock = new object();
    protected ViewModels.DefaultStudyMultiLineTextsViewModel _viewModel = null;
    /// <summary>
    /// This is the ViewModel that gets returned to the opportunity as the jobInfo.Product.
    /// This will house study item view models.
    /// </summary>
    protected ViewModels.DefaultStudyMultiLineTextsViewModel _ViewModel
    {
      get
      {
        lock (_ViewModelLock)
        {
          return _viewModel;
        }
      }
      set
      {
        lock (_ViewModelLock)
        {
          _viewModel = value;
        }
      }
    }
    #endregion

    protected IFeedbackViewModelBase _FeedbackViewModel { get; set; }

    /// <summary>
    /// Analyze opportunities published on Exchange.
    /// </summary>
    public override void Handle(IOpportunity<MultiLineTextList, IViewModelBase> message)
    {
      //WE ONLY CARE ABOUT STUDY OPPORTUNITIES
      if (message.Category != StudyResources.CategoryStudy)
        return;

      //WE DON'T CARE ABOUT MESSAGES WE PUBLISH OURSELVES
      if (message.PublisherId == Id)
        return;

      //CURRENTLY, WE ONLY CARE IF THE JOB INFO IS A STUDY JOB INFO<MLT>
      if (!(message.JobInfo is StudyJobInfo<MultiLineTextList, IViewModelBase>))
        return;

      //WE ONLY CARE IF WE UNDERSTAND THE STUDYJOBINFO.CRITERIA
      var studyJobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.JobInfo;
      if (!(studyJobInfo.Criteria is StudyJobCriteria))
        return;

      //WE HAVE A GENUINE OPPORTUNITY WE WOULD BE INTERESTED IN
      //MAKE THE OFFER FOR THE JOB
      var offer = 
        new Offer<MultiLineTextList, IViewModelBase>(message,
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
    /// <summary>
    /// Analyze OfferResponse's published on Exchange.
    /// </summary>
    public override void Handle(IOfferResponse<MultiLineTextList, IViewModelBase> message)
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

      ////WE HAVE AN ACCEPT RESPONSE WITH AN OPPORTUNITY THAT WE CARE ABOUT
      ////WHATEVER OPPORTUNITY WE ARE WORKING ON, THE NEW ONE SUPERCEDES IT
      //if (_CurrentOffer != null)
      //  AbortCurrent();

      //WE HAVE ALREADY CHECKED THIS TYPE CAST BEFORE MAKING OUR OFFER, IT SHOULD NOT HAVE CHANGED. 
      var jobInfo = (StudyJobInfo<MultiLineTextList, IViewModelBase>)message.Offer.Opportunity.JobInfo;
      _CurrentOffer = message.Offer;

      //SET THE PRODUCT...WE WILL KEEP A REFERENCE TO THIS VIEWMODEL, AND MAKE CHANGES AS WE STUDY.
      //BUT AS FOR THE JOB OPPORTUNITY IS CONCERNED, WE ARE ALREADY COMPLETE WITH JUST THE ASSIGNMENT!
      jobInfo.Product = _ViewModel;

      //PUBLISH COMPLETED UPDATE
      var completedUpdate =
        new StatusUpdate<MultiLineTextList, IViewModelBase>(CommonResources.StatusCompleted,
                                                            _CurrentOffer.Opportunity,
                                                            _CurrentOffer,
                                                            message,
                                                            jobInfo,
                                                            Id,
                                                            this,
                                                            StudyResources.CategoryStudy,
                                                            null);
      Exchange.Ton.Publish(completedUpdate);

      //Begin studying the multilinetexts
      BeginStudying(jobInfo.Target);
    }
    /// <summary>
    /// Handles cancelation messages.
    /// </summary>
    /// <param name="message"></param>
    public override void Handle(ICancelation message)
    {
      throw new System.NotImplementedException();
    }
    /// <summary>
    /// "Default" conglomerate messaging system.  This studier can keep in contact with the other default
    /// studiers through this message stream.
    /// </summary>
    /// <param name="message"></param>
    public override void Handle(IConglomerateMessage message)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Handles update messages.
    /// </summary>
    /// <param name="message"></param>
    public override void Handle(IStatusUpdate<MultiLineTextList, IViewModelBase> message)
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

    private void BeginStudying(MultiLineTextList multiLineTexts)
    {
      if (!_IsStudying)
      {
        InitializeForNewStudySession(multiLineTexts);
      }
    }

    private bool _IsStudying { get; set; }

    private void StudyNextItem()
    {
      _IsStudying = true;
      _FeedbackViewModel.IsEnabled = false;
      _Studier.GetNextStudyItemViewModel((s, r) =>
        {
          if (r.Error != null)
            throw r.Error;

          //HACK: MY CALLBACK GENERIC CAN'T CONTAIN AN INTERFACE, SO I CAN'T HAVE ISTUDYITEMVIEWMODELBASE
          //AS THE RESULTING OBJECT, SO I MADE A ARGS OBJECT WRAPPER.  I'M SURE THERE IS A BETTER SOLUTION,
          //BUT I NEED TO MOVE ON...I FORGOT, I HAVE RESULTARGS<IINTERFACE>...ARGH.  SHOULD FIX THIS SOMETIME.
          var result = r.Object;
          var studyItemViewModel = result.ViewModel;
          if (studyItemViewModel != null)
          {
            _ViewModel.StudyItemViewModel = studyItemViewModel;
          }
          else
            _ViewModel.StudyItemViewModel =
              Services.Container.GetExportedValue<ViewModels.StudySessionCompleteViewModel>();

          //SHOW THE STUDY ITEM VIEW MODEL.  CALLBACK HAPPENS WHEN THAT VIEWMODEL
          //SAYS THAT IT IS DONE, AND THIS WILL THROW THE EXCEPTION IF IT IS THERE.
          //ONCE THAT IS CHECKED FOR, WE SET OUR STUDYITEMVIEWMODEL TO NULL, WHICH
          //OUR HEARTBEAT WILL RECOGNIZE ON ITS NEXT PULSE.
          studyItemViewModel.Show((e) =>
          {
            if (e != null)
              throw e;

            //OUR VIEW MODEL IS DONE SHOWING.  ENABLE FEEDBACK AND GET FEEDBACK.
            _FeedbackViewModel.IsEnabled = true;
            _FeedbackViewModel.GetFeedbackAsync((s2, r2) =>
              {
                _ViewModel.StudyItemViewModel = null;
                _IsStudying = false;
              });
          });

        });
    }

    /// <summary>
    /// Initializes _Studier, _ViewModel, and whatever else needs to be initialized when starting 
    /// to study a new MultiLineTextList (new opportunity published).
    /// </summary>
    private void InitializeForNewStudySession(MultiLineTextList multiLineTexts)
    {
      //INITIALIZE STUDIER
      _Studier.InitializeForNewStudySession(multiLineTexts);

      //INITIALIZE STUDY HEARTBEAT
      //_StudyHeartbeat.DoWork += (s, r) =>
      //  {
      //    while (_ViewModel != null && !_AbortIsFlagged && !r.Cancel)
      //    {
      //      if (!_IsStudying && _ViewModel.StudyItemViewModel == null)
      //      {
      //        StudyNextItem();
      //      }
      //      System.Threading.Thread.Sleep(int.Parse(StudyResources.DefaultStudyHeartbeatTimeMilliseconds));
      //    }
      //  };
    }
  }
}
