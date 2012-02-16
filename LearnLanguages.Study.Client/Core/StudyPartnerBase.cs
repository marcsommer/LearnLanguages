using LearnLanguages.Business;
using LearnLanguages.Study.Interfaces;
using LearnLanguages.Offer;
using LearnLanguages.Common.Interfaces;
using System;

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
      Exchange.Ton.SubscribeToOpportunities(this);
      Exchange.Ton.SubscribeToOfferResponses(this);
      Exchange.Ton.SubscribeToCancelations(this);
    }

    public Guid Id { get; protected set; }
    public Guid ConglomerateId { get; protected set; }

    //protected virtual void Study(MultiLineTextList multiLineTexts, LanguageEdit language)
    //{
    //  _CurrentMultiLineTexts = multiLineTexts;
    //  _CurrentLanguage = language;

    //  if (MultiLineTextsStudier == null)
    //    MultiLineTextsStudier = 
    //      (IDo<IJobInfo<MultiLineTextList>, MultiLineTextList>)(new DefaultMultiLineTextsStudier());

    //  DoImpl();
    //}

    /// <summary>
    /// Default behavior is to study the current MLTs, current language, for an indefinite period of time.
    /// </summary>
    protected virtual void DoImpl()
    {
      //max date == indefinite time.
      var dateNoExpiration = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
      var jobInfo = new StudyJobInfo<MultiLineTextList>(_CurrentMultiLineTexts,
                                                        _CurrentLanguage, 
                                                        dateNoExpiration, 
                                                        double.Parse(StudyResources.DefaultKnowledgeThreshold));

      MultiLineTextsStudier.Do(jobInfo);
    }

    public IDo<IJobInfo<MultiLineTextList>, MultiLineTextList> MultiLineTextsStudier { get; set; }


    public void Handle(Opportunity<MultiLineTextList> message)
    {
      if (message.Category != StudyResources.CategoryStudy)
        return;

      if (!(message.JobInfo is StudyJobInfo<MultiLineTextList>))
        return;

      StudyJobInfo<MultiLineTextList> studyJobInfo = (StudyJobInfo<MultiLineTextList>)message.JobInfo;

      if (!(studyJobInfo.Criteria is StudyJobCriteria))
        return;
      
      //Make an offer for the job.
      var offer = new Offer.Offer(message.OpportunityId, 
                                  this.Id, 
                                  this, 
                                  double.Parse(StudyResources.DefaultAmountDefaultMultiLineTextsStudier),
                                  StudyResources.CategoryStudy, 
                                  null);
      Exchange.Ton.Publish(offer);
        
      MultiLineTextsStudier = 
        (IDo<IJobInfo<MultiLineTextList>, MultiLineTextList>)(new DefaultMultiLineTextsStudier());
    }

    public void Handle(OfferResponse message)
    {
      throw new System.NotImplementedException();
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
