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
    protected virtual void Study(MultiLineTextList multiLineTexts, LanguageEdit language)
    {
      _CurrentMultiLineTexts = multiLineTexts;
      _CurrentLanguage = language;

      if (MultiLineTextsStudier == null)
        MultiLineTextsStudier = 
          (IDo<IJobInfo<MultiLineTextList>, MultiLineTextList>)(new DefaultMultiLineTextsStudier());

      DoImpl();
    }

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


    public void Handle(Common.Interfaces.IOpportunity message)
    {
      ///OKAY, HERE IS THE DEAL:
      ///WE LISTEN FOR JOB OPPORTUNITIES IN THE STUDY CATEGORY
      ///IF WE GET ONE, WE POST A NEW JOB OURSELVES THAT SOMEONE ELSE IS GOING TO CONSUME.
      ///IF WE GET A REQUEST TO STOP OUR JOB, THEN WE WILL REPOST A SIMILAR REQUEST CONCERNING OUR INTERNAL
      ///JOB.  THEN WE PASS THE BUCK FOR THE ORIGINALLY POSTED STOP REQUEST.
      ///WE ARE ESSENTIALLY A ROUTER OF STUDY JOBS, CONVEYING MESSAGES TO MLTs-STUDIERS

      if (!(message.Category == StudyResources.CategoryStudy))
        return;

      //we only know how to deal with study multilinetextlists at the moment
      if (!(
            message is Opportunity<MultiLineTextList> ||
            message is Cancelation
           ))
        return;

      var jobOpp = (Opportunity<MultiLineTextList>)message;

        _CurrentMultiLineTexts = multiLineTexts;
      _CurrentLanguage = language;

      if (MultiLineTextsStudier == null)
        MultiLineTextsStudier =
          (IDo<IJobInfo<MultiLineTextList>, MultiLineTextList>)(new DefaultMultiLineTextsStudier());

      DoImpl();
    }

    public void Handle(Opportunity<MultiLineTextList> message)
    {
      if (!(message.Category == StudyResources.CategoryStudy))
        return;

      //DISPATCH NEW MESSAGE MEANT TO HANDLE
    }

    public void Handle(Cancelation message)
    {
      throw new System.NotImplementedException();
    }
  }
}
