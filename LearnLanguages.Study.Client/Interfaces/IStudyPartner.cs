using System;
using System.Net;
//using Caliburn.Micro;
using LearnLanguages.Business;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudyPartner //: IStudier<IStudyJobInfo<MultiLineTextList>, MultiLineTextList>
  {
    IStudier<IStudyJobInfo<MultiLineTextList>, MultiLineTextList> MultiLineTextsStudier { get; }
    //void StudyPhrases(PhraseList phrases, IEventAggregator eventAggregator);
    //void StudyTranslations(TranslationList translations, IEventAggregator eventAggregator);
    //void Chug(IEventAggregator eventAggregator);
    //void ShowProgress(IEventAggregator eventAggregator);
    //double GetProgressAsOneNumber();
    //void GetProgressAsOneNumberAsync(Common.Delegates.AsyncCallback<double> callback);
    //double GetProgressAsOneNumber(TranslationList translations);
    //void GetProgressAsOneNumberAsync(TranslationList translations, Common.Delegates.AsyncCallback<double> callback);
    //double GetProgressAsOneNumber(MultiLineTextList multiLineTexts);
    //void GetProgressAsOneNumberAsync(MultiLineTextList multiLineTexts, Common.Delegates.AsyncCallback<double> callback);
  }
}
