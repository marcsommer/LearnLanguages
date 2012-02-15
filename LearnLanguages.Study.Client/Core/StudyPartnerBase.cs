using System;
using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using LearnLanguages.Business;
using LearnLanguages.Common.Delegates;
using LearnLanguages.Common.Interfaces;
using LearnLanguages.Study.Bases;
using LearnLanguages.Study.Interfaces;

namespace LearnLanguages.Study
{
  /// <summary>
  /// Simple IStudyPartner base class.  You do not HAVE to descend from this, it just provides some 
  /// very basic plumbing.
  /// </summary>
  public abstract class StudyPartnerBase : IStudyPartner
  {
    protected IExchange _OfferExchange { get; set; }
    protected MultiLineTextList _CurrentMultiLineTexts { get; set; }
    protected LanguageEdit _CurrentLanguage { get; set; }

    public virtual void Study(MultiLineTextList multiLineTexts, LanguageEdit language, IEventAggregator eventAggregator)
    {
      if (_OfferExchange == null)
      {
        SetOfferExchange(eventAggregator);
      }

      _CurrentMultiLineTexts = multiLineTexts;
      _CurrentLanguage = language;

      if (MultiLineTextsStudier == null)
        MultiLineTextsStudier = 
          (IStudier<IStudyJobInfo<MultiLineTextList>, MultiLineTextList>)(new DefaultMultiLineTextsStudier());

      StudyImpl();
    }

    private void SetOfferExchange(IEventAggregator eventAggregator)
    {
      _OfferExchange = new DefaultOfferExchange(new EventAggregator());
    }

    /// <summary>
    /// Default behavior is to study the current MLTs, current language, for an indefinite period of time.
    /// </summary>
    protected virtual void StudyImpl()
    {
      //max date == indefinite time.
      var dateNoExpiration = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime;
      var jobInfo = new StudyJobInfo<MultiLineTextList>(_CurrentMultiLineTexts,
                                                        _CurrentLanguage, 
                                                        _OfferExchange, 
                                                        dateNoExpiration, 
                                                        double.Parse(StudyResources.DefaultKnowledgeThreshold));

      MultiLineTextsStudier.Study(jobInfo, _OfferExchange);
    }

    public IStudier<IStudyJobInfo<MultiLineTextList>, MultiLineTextList> MultiLineTextsStudier { get; set; }

  }
}
