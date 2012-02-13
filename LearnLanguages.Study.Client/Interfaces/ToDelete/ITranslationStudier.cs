using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  public interface ITranslationStudier
  {
    void StudyTranslation(TranslationEdit translation, 
                          string nonNativeLanguageText, 
                          bool answerIsInNativeLanguage,
                          IOfferExchange offerExchange);
  }
}
