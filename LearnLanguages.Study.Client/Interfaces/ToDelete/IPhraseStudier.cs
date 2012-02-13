using LearnLanguages.Business;
using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  public interface IPhraseStudier
  {
    void StudyPhrase(PhraseEdit phrase, 
                     string answerLanguageText,
                     IOfferExchange offerExchange);
  }
}
