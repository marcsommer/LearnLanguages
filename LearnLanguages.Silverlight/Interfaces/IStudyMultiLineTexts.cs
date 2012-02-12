using System;
using System.Net;
using Caliburn.Micro;
using LearnLanguages.Business;

namespace LearnLanguages.Silverlight.Interfaces
{
  public interface IStudyMultiLineTexts
  {
    //void StudyPhrases(PhraseList phrases, IEventAggregator eventAggregator);
    void StudyMultiLineTexts(MultiLineTextList multiLineTexts, IEventAggregator eventAggregator);
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
