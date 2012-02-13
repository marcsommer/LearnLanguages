using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  public interface IMultiLineTextsStudier
  {
    //void StudyPhrases(PhraseList phrases, IEventAggregator eventAggregator);
    void StudyMultiLineTexts(MultiLineTextList multiLineTexts, IOfferExchange offerExchange);
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
