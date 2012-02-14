using LearnLanguages.Common.Interfaces;

namespace LearnLanguages.Study.Interfaces
{
  public interface IStudier<T> 
  {
    void Study(T studyTarget, IOfferExchange offerExchange);
  }
}
